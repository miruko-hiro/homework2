using System;
using Base.Search;
using UnityEngine;
using UtilityAI.Score;
using UtilityAI.Scorers;
using Random = UnityEngine.Random;

namespace UtilityAI.Actions
{
    public class RunningAwayFromEnemy : BaseAction
    {
        public override bool IsEnabled { get; protected set; }
        
        [SerializeField] private Graph graph;
        [SerializeField] private Transform transformEnemy;
        [SerializeField] private Rigidbody _rigidbody;
        
        [Header("Parameters running away from enemy")]
        [SerializeField] private float speed = 9f;
        [SerializeField] private float randomPointRadius = 4f; 
        [SerializeField] private float additive = 6f;
        [SerializeField] private float gazeDistance = 2f;
            
        private bool _isMove = false;
        private Vector2Int _to;
        private Vector3 _randomPosition;
        private Vector3 _center;
        private Vector3 direction;
        private int _wallLayerIndex;

        private ScoreKeeper _scoreKeeper;

        private void Awake()
        {
            _wallLayerIndex = 1 << LayerMask.NameToLayer("Wall");
            _scoreKeeper = new ScoreKeeper(GetComponents<Scorer>());
            direction = (transform.position - transformEnemy.position).normalized;
        }

        public override float GetScores()
        {
            return _scoreKeeper.GetScores();
        }

        public override void Play()
        {
            
            if(_isMove) return;
            IsEnabled = true;

            var positionOwn = transform.position;
            // float cos = Mathf.Cos (Vector3.Angle(positionOwn, positionEnemy) * Mathf.Deg2Rad);
            // Quaternion quaternion = Quaternion.LookRotation(positionOwn.magnitude * cos * positionEnemy.normalized);
            bool correctPoint = false;

            
            //Проверка сохраненного направления на упирание в стену
            if (CheckWall(positionOwn + Vector3.up, direction))
            {
                //Направление от врага до этого объекта
                direction = (positionOwn - transformEnemy.position).normalized;
                //От застревания в углу, когда враг стоит позади, а этот объект упирается в стену, так как бежит всегда от врага
                direction = CheckWallInAllDirections(positionOwn + Vector3.up, direction);
            }
            
            //Выбор позиции, в которую убегать этому объекту
            do
            {
                //Рандомится позиция и проверяется существует ли она в нашем графе, если да, то выходим из цикла
                _center = positionOwn + direction * additive;
                Vector2 random = Random.insideUnitCircle.normalized;
                _randomPosition = new Vector3(random.x, 0f, random.y) * randomPointRadius + _center;
                _randomPosition.y = 0f;
                _to = graph.ToInt(_randomPosition);
                
                correctPoint = graph.IsPointCorrect(_to);
                
            } while (!correctPoint);
            
            _isMove = true;
        }

        public override void Stop()
        {
            _rigidbody.velocity = Vector3.zero;
            _isMove = false;
            IsEnabled = false;
        }

        private void FixedUpdate()
        {
            if (!_isMove) return;
            
            var positionOwn = transform.position;
            Debug.DrawLine(positionOwn + Vector3.up, _randomPosition + Vector3.up, Color.green);

            if (Vector3.Distance(positionOwn, _randomPosition) < 0.7f)
            {
                Stop();
                return;
            }
                
            //Поиск пути по А*
            var from = graph.ToInt(positionOwn);
            var path = AStarFromGoogle.FindPath(graph.Map, @from, _to);
            var nextPathPoint = path.Count >= 2 ? path[1] : _to;
            nextPathPoint = new Vector2Int(nextPathPoint.x - graph.DeltaX, nextPathPoint.y - graph.DeltaZ);
            Vector3 moveDirection = new Vector3(nextPathPoint.x, positionOwn.y, nextPathPoint.y) - positionOwn;

            _rigidbody.velocity = moveDirection.normalized * speed;
            //transform.rotation = Quaternion.LookRotation(_randomPosition - positionOwn);
        }

        private Vector3 CheckWallInAllDirections(Vector3 start, Vector3 dir)
        {
            Vector3 newDir = dir;
            int i = 0;
            
            while (true)
            {
                if (CheckWall(start, newDir))
                {
                    DrawRay(start, newDir * gazeDistance);
                    int degrees = CascadeTurn(i);
                    newDir = Turn(dir, degrees);
                    if (degrees == 180) return newDir;
                    i += 1;
                    continue;
                }

                return newDir;
            }
        }

        private int CascadeTurn(int i) => i switch 
        {
            0 => 45,
            1 => 315,
            2 => 90,
            3 => 270,
            4 => 225,
            5 => 135,
            6 => 180,
            _ => throw new ArgumentOutOfRangeException(nameof(i), i, null)
        };

        private Vector3 Turn(Vector3 dir, int degrees)
        {
            float sin;
            float cos;
            
            switch (degrees)
            {
                case 45:
                    sin = Mathf.Sqrt(2) / 2;
                    cos = sin;
                    return new Vector3(dir.x * cos - dir.z * sin, dir.y, dir.x * sin + dir.z * cos);
                case 90:
                    return new Vector3(dir.z, dir.y, -dir.x);
                case 135:
                    sin = Mathf.Sqrt(2) / 2;
                    cos = -sin;
                    return new Vector3(dir.x * cos - dir.z * sin, dir.y, dir.x * sin + dir.z * cos);
                case 180:
                    return new Vector3(-dir.x, dir.y, -dir.z);
                case 225:
                    sin = -(Mathf.Sqrt(2) / 2);
                    cos = sin;
                    return new Vector3(dir.x * cos - dir.z * sin, dir.y, dir.x * sin + dir.z * cos);
                case 270:
                    return new Vector3(-dir.z, dir.y, dir.x);
                case 315:
                    cos = Mathf.Sqrt(2) / 2;
                    sin = -cos;
                    return new Vector3(dir.x * cos - dir.z * sin, dir.y, dir.x * sin + dir.z * cos);
                default:
                    throw new ArgumentOutOfRangeException(nameof(degrees), degrees, null);
            }
        }

        private bool CheckWall(Vector3 start, Vector3 dir)
        {
            return Physics.Raycast(start, dir, gazeDistance, _wallLayerIndex);
        }

        private void DrawRay(Vector3 start, Vector3 dir)
        {
            Debug.DrawRay(start, dir, Color.red, 0.5f);
        }

        private void OnDrawGizmos()
        {
            if (_isMove)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(_randomPosition + Vector3.up, 0.5f);
                Gizmos.DrawWireSphere(_center, randomPointRadius);
            }
        }
    }
}