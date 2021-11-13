using System.Collections.Generic;
using System.Linq;
using Utility_AI.Scorers;

namespace Utility_AI
{
    public class ActionAI
    {
        private readonly List<IScorer> _scorers;
        public int Score { get; set; }

        public ActionAI(params IScorer[] scorers)
        {
            _scorers = scorers.ToList();
        }

        public void CountScores()
        {
            Score = 0;
            foreach (var scorer in _scorers)
            {
                Score += scorer.GetScore();
            }
        }
    }
}