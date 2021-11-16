using UnityEngine;

namespace UtilityAI.Scorers
{
    public abstract class Scorer: MonoBehaviour
    {
        public abstract float GetScore();
    }
}