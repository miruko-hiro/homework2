using Base.Game;

namespace UtilityAI.Scorers
{
    public class IsInCover: IScorer
    {
        private readonly ZombieComponent _zombie;

        public IsInCover(ZombieComponent zombie)
        {
            _zombie = zombie;
        }
        
        public float GetScore()
        {
            return _zombie.IsInCover ? 0f : 0.5f;
        }
    }
}