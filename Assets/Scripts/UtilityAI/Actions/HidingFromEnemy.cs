using Base.Game;
using UnityEngine;
using UtilityAI.Score;
using UtilityAI.Scorers;

namespace UtilityAI.Actions
{
    public class HidingFromEnemy : BaseAction
    {
        [SerializeField] private LevelMap levelMap;

        private ScoreKeeper _scoreKeeper;

        private void Awake()
        {
            _scoreKeeper = new ScoreKeeper(
                new CloseToCover(levelMap, transform),
                new IsInCover(GetComponent<ZombieComponent>())
                );
        }

        public override float GetScores()
        {
            return _scoreKeeper.GetScores();
        }

        public override void Play()
        {
            Debug.Log("Hidding");
        }

        public override void Stop()
        {
            
        }
    }
}