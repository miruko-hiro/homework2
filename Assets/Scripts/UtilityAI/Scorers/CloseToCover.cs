using Base.Game;
using UnityEngine;

namespace UtilityAI.Scorers
{
    public class CloseToCover: IScorer
    {
        private readonly LevelMap _mapCover;
        private readonly Transform _transformOwn;

        public CloseToCover(LevelMap mapCover, Transform transformOwn)
        {
            _mapCover = mapCover;
            _transformOwn = transformOwn;
        }

        public float GetScore()
        {
            foreach (var mapCoverPoint in _mapCover.Points)
            {
                if (Vector3.Distance(_transformOwn.position, mapCoverPoint) < 50)
                {
                    return 0.5f;
                }
            }

            return 0f;
        }
    }
}