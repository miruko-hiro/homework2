using Base.Search;
using UnityEngine;
using UtilityAI.Score;
using UtilityAI.Scorers;
using Random = UnityEngine.Random;

namespace UtilityAI.Actions
{
    public class RunningAwayFromEnemy : BaseAction
    {
        [SerializeField] private Graph graph;
        [SerializeField] private Transform transformEnemy;
        [SerializeField] private Rigidbody _rigidbody;

        private const float RandomPointRadius = 9f; 
        private const float Speed = 9f;
        
        private bool _isMove = false;
        private Vector2Int _to;
        private Vector3 _randomPosition;

        private ScoreKeeper _scoreKeeper;

        private void Awake()
        {
            _scoreKeeper = new ScoreKeeper(
                new DistanceToEnemy(transform, transformEnemy, 0f, 20f, false),
                new EnemyNotSee()
            );
        }

        public override float GetScores()
        {
            return _scoreKeeper.GetScores();
        }

        public override void Play()
        {
            //Debug.Log("Running");
            
            if(_isMove) return;

            
            var positionOwn = transform.position;
            
            // float cos = Mathf.Cos (Vector3.Angle(positionOwn, positionEnemy) * Mathf.Deg2Rad);
            // Quaternion quaternion = Quaternion.LookRotation(positionOwn.magnitude * cos * positionEnemy.normalized);

            bool correctPoint = false;

            do
            {
                _randomPosition = Random.insideUnitSphere.normalized * RandomPointRadius + positionOwn;
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
                
            var from = graph.ToInt(positionOwn);
            
            var path = AStarFromGoogle.FindPath(graph.Map, @from, _to);
            var nextPathPoint = path.Count >= 2 ? path[1] : _to;
            
            nextPathPoint = new Vector2Int(nextPathPoint.x - graph.DeltaX, nextPathPoint.y - graph.DeltaZ);
            
            Vector3 moveDirection = new Vector3(nextPathPoint.x, positionOwn.y, nextPathPoint.y) - positionOwn;

            _rigidbody.velocity = moveDirection.normalized * Speed;
            transform.rotation = Quaternion.LookRotation(_randomPosition - positionOwn);
        }

        private void OnDrawGizmos()
        {
            if (_isMove)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(_randomPosition + Vector3.up, 0.5f);
            }
        }
    }
}