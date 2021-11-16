using System.Collections.Generic;
using System.Linq;
using UtilityAI.Scorers;

namespace UtilityAI.Score
{
    public class ScoreKeeper
    {
        private readonly List<Scorer> _scorers;

        public ScoreKeeper(params Scorer[] scorers)
        {
            _scorers = scorers.ToList();
        }

        public float GetScores()
        {
            float score = 0f;
            
            foreach (var scorer in _scorers)
            {
                score += scorer.GetScore();
            }

            return score;
        }
    }
}