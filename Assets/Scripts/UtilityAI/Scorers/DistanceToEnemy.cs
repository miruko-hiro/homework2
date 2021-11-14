using UnityEngine;

namespace UtilityAI.Scorers
{
    public class DistanceToEnemy: IScorer
    {
        private readonly Transform _transformOwn;
        private readonly Transform _transformEnemy;
        private readonly float _minDistance;
        private readonly float _maxDistance;
        private readonly float _minScore = 0f;
        private readonly float _maxScore = 1f;
        private readonly bool _isInverted;

        public DistanceToEnemy(Transform transformOwn, Transform transformEnemy, float minDistance, float maxDistance, bool isInverted)
        {
            _transformOwn = transformOwn;
            _transformEnemy = transformEnemy;
            _minDistance = minDistance;
            _maxDistance = maxDistance;
            _isInverted = isInverted;
        }

        public float GetScore()
        {
            var distance = (int) Vector3.Distance(_transformOwn.position, _transformEnemy.position);

            if (!_isInverted)
            {
                if (distance <= _minDistance) return _maxScore;
                if (distance >= _maxDistance) return _minScore;
                return (_maxDistance - distance) * (_maxScore / _maxDistance);
            }
            else
            {
                if (distance <= _minDistance) return _minScore;
                if (distance >= _maxDistance) return _maxScore;
                return (_maxDistance - (_maxDistance - distance)) * (_maxScore / _maxDistance);
            }
        }
    }
}