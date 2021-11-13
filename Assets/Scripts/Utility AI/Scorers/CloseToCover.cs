using Base.Game;
using UnityEngine;

namespace Utility_AI.Scorers
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

        public int GetScore()
        {
            foreach (var mapCoverPoint in _mapCover.Points)
            {
                if (Vector3.Distance(_transformOwn.position, mapCoverPoint) < 50)
                {
                    return 50;
                }
            }

            return 0;
        }
    }
}