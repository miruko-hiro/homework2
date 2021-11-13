using Base.Game;

namespace Utility_AI.Scorers
{
    public class IsInCover: IScorer
    {
        private readonly ZombieComponent _zombie;

        public IsInCover(ZombieComponent zombie)
        {
            _zombie = zombie;
        }
        
        public int GetScore()
        {
            return _zombie.IsInCover ? 0 : 50;
        }
    }
}