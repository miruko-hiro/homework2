using UnityEngine;

namespace UtilityAI.Actions
{
    public abstract class BaseAction: MonoBehaviour
    {
        public abstract float GetScores();
        public abstract void Play();
        public abstract void Stop();
    }
}