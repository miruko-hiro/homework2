using UnityEngine;
using UtilityAI.Score;
using UtilityAI.Scorers;

namespace UtilityAI.Actions
{
    public class DodgingEnemy : BaseAction
    {
        private ScoreKeeper _scoreKeeper;

        private void Awake()
        {
            _scoreKeeper = new ScoreKeeper(new EnemyNotSee());
        }

        public override float GetScores()
        {
            return _scoreKeeper.GetScores();
        }

        public override void Play()
        {
            Debug.Log("Dodging");
        }

        public override void Stop()
        {
            
        }
    }
}