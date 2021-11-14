using System;
using System.Collections;
using Base.Search;
using UnityEngine;
using UtilityAI.Score;
using UtilityAI.Scorers;

namespace UtilityAI.Actions
{
    public class EatGrass : BaseAction
    {
        [SerializeField] private Graph graph;
        [SerializeField] private Transform transformEnemy;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Vector3[] deltaPath;
        private const float Speed = 5f;
        private bool _isEat = false;
        private bool _isMove = false;
        private int _currentPoint = 0;
        private Vector3 _initPosition;
        private Vector2Int _to;
        
        private ScoreKeeper _scoreKeeper;
        private void Awake()
        {
            _scoreKeeper = new ScoreKeeper(
                new DistanceToEnemy(transform, transformEnemy, 0f, 20f, true)
            );
            _initPosition = transform.position;
        }

        private void Start()
        {
            _to = graph.ToInt(new Vector3(_initPosition.x, 0f, _initPosition.z));
        }

        public override float GetScores()
        {
            return _scoreKeeper.GetScores();
        }

        public override void Play()
        {
            if(_isEat || _isMove) return;

            StartCoroutine(Vector3.Distance(_initPosition, transform.position) < 1f ? Eat() : GoToFeedingPoint());
        }

        private IEnumerator Eat()
        {
            _isEat = true;
            
            while (_isEat)
            {
                if (!_isEat) yield break;
            
                if (deltaPath == null || deltaPath.Length < 2)
                    yield break;
            
                var direction = _initPosition + deltaPath[_currentPoint] - transform.position;
                _rigidbody.velocity = direction.normalized * Speed;
            
                if (direction.magnitude <= 0.1f)
                {
                    _currentPoint = (_currentPoint + 1) % deltaPath.Length;
                }
                
                yield return new WaitForFixedUpdate();
            }
        }

        private IEnumerator GoToFeedingPoint()
        {
            _isMove = true;
            
            while (_isMove)
            {
                var positionOwn = transform.position;
                var from = graph.ToInt(positionOwn);
                Debug.DrawLine(positionOwn + Vector3.up, _initPosition + Vector3.up, Color.yellow);
                Debug.Log("Рисую " + Time.time);
            
                var path = AStarFromGoogle.FindPath(graph.Map, @from, _to);
                var nextPathPoint = path.Count >= 2 ? path[1] : _to;
            
                nextPathPoint = new Vector2Int(nextPathPoint.x - graph.DeltaX, nextPathPoint.y - graph.DeltaZ);
            
                Vector3 moveDirection = new Vector3(nextPathPoint.x, positionOwn.y, nextPathPoint.y) - positionOwn;

                _rigidbody.velocity = moveDirection.normalized * Speed;
                transform.rotation = Quaternion.LookRotation(_initPosition - positionOwn);
                
                if (Vector3.Distance(positionOwn, _initPosition) < 1f)
                {
                    Stop();
                    yield break;
                }
                
                yield return new WaitForFixedUpdate();
            }
        }

        public override void Stop()
        {
            _isEat = false;
            _isMove = false;
            StopAllCoroutines();
            _rigidbody.velocity = Vector3.zero;
        }

        private void OnDrawGizmos()
        {
            if (_isMove)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(_initPosition + Vector3.up, 0.5f);
            }
        }
    }
}