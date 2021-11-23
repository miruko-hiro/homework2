using System.Collections;
using Scenes.Game.Scripts.UtilityAI.Score;
using Scenes.Game.Scripts.UtilityAI.Scorers;
using UnityEngine;

namespace Scenes.Game.Scripts.UtilityAI.Actions
{
    public class EatGrass : BaseAction
    {
        public override bool IsEnabled { get; protected set; }
        
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Vector3[] deltaPath;
        
        [Header("Physical parameters")] 
        [SerializeField] private float speed = 4f;

        private bool _isEat = false;
        private Vector3 _initPosition;
        private int _currentPoint = 0;
        
        private ScoreKeeper _scoreKeeper;
        private void Awake()
        {
            _scoreKeeper = new ScoreKeeper(GetComponents<Scorer>());
            _initPosition = transform.position;
        }

        public override float GetScores()
        {
            return _scoreKeeper.GetScores();
        }

        public override void Play()
        {
            if(_isEat) return;

            IsEnabled = true;
            _isEat = true;
            _initPosition = transform.position;

            StartCoroutine(Eat());
        }

        private IEnumerator Eat()
        {
            yield return new WaitForSeconds(1f);
            
            Stop();
        }

        private void FixedUpdate()
        {
            if (!_isEat) return;
            
            if (deltaPath == null || deltaPath.Length < 2)
                return;
            
            var direction = _initPosition + deltaPath[_currentPoint] - transform.position;
            _rigidbody.velocity = direction.normalized * speed;
            
            if (direction.magnitude <= 0.1f)
            {
                _currentPoint = (_currentPoint + 1) % deltaPath.Length;
            }
        }

        public override void Stop()
        {
            _isEat = false;
            _rigidbody.velocity = Vector3.zero;
            IsEnabled = false;
        }
    }
}