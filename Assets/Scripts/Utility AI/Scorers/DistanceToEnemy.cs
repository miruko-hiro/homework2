using UnityEngine;

namespace Utility_AI.Scorers
{
    public class DistanceToEnemy: IScorer
    {
        private readonly Transform _transformOwn;
        private readonly Transform _transformEnemy;

        public DistanceToEnemy(Transform transformOwn, Transform transformEnemy)
        {
            _transformOwn = transformOwn;
            _transformEnemy = transformEnemy;
        }

        public int GetScore()
        {
            var distance = (int) Vector3.Distance(_transformOwn.position, _transformEnemy.position);
            if (distance <= 0) return 0;
            if (distance >= 100) return 100;
            return distance;
        }
    }
}