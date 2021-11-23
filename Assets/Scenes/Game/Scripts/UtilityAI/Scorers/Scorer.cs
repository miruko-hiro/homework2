using UnityEngine;

namespace Scenes.Game.Scripts.UtilityAI.Scorers
{
    public abstract class Scorer: MonoBehaviour
    {
        public abstract float GetScore();
    }
}