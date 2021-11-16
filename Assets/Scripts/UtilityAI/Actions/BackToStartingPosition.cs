using Base.Search;
using UnityEngine;
using UtilityAI.Score;
using UtilityAI.Scorers;

namespace UtilityAI.Actions
{
    public class BackToStartingPosition: BaseAction
    {
        public override bool IsEnabled { get; protected set; }
        
        [SerializeField] private Graph graph;
        [SerializeField] private Rigidbody _rigidbody;
        
        [Header("Physical parameters")] 
        [SerializeField] private float speed = 9f;
        [SerializeField] private Vector3 initPosition;
        
        private bool _isMove = false;
        private Vector2Int _to;
        
        private ScoreKeeper _scoreKeeper;
        
        private void Awake()
        {
            _scoreKeeper = new ScoreKeeper(GetComponents<Scorer>());
        }

        private void Start()
        {
            _to = graph.ToInt(new Vector3(initPosition.x, 0f, initPosition.z));
        }

        public override float GetScores()
        {
            return _scoreKeeper.GetScores();
        }

        public override void Play()
        {
            if(_isMove) return;

            IsEnabled = true;
            _isMove = true;
        }

        public override void Stop()
        {
            _isMove = false;
            _rigidbody.velocity = Vector3.zero;
            IsEnabled = false;
        }

        private void FixedUpdate()
        {
            if(!_isMove) return;
            
            var positionOwn = transform.position;
            var from = graph.ToInt(positionOwn);
            Debug.DrawLine(positionOwn + Vector3.up, initPosition + Vector3.up, Color.yellow);
            
            var path = AStarFromGoogle.FindPath(graph.Map, @from, _to);
            var nextPathPoint = path.Count >= 2 ? path[1] : _to;
            nextPathPoint = new Vector2Int(nextPathPoint.x - graph.DeltaX, nextPathPoint.y - graph.DeltaZ);
            Vector3 moveDirection = new Vector3(nextPathPoint.x, positionOwn.y, nextPathPoint.y) - positionOwn;

            _rigidbody.velocity = moveDirection.normalized * speed;
            transform.rotation = Quaternion.LookRotation(initPosition - positionOwn);
                
            if (Vector3.Distance(positionOwn, initPosition) < 2f)
            {
                Stop();
            }
        }
        
        private void OnDrawGizmos()
        {
            if (_isMove)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(initPosition + Vector3.up, 0.5f);
            }
        }
    }
}