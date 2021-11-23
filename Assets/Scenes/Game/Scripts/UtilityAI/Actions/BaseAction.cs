using UnityEngine;

namespace Scenes.Game.Scripts.UtilityAI.Actions
{
    public abstract class BaseAction: MonoBehaviour
    {
        public abstract bool IsEnabled { get; protected set; }
        public abstract float GetScores();
        public abstract void Play();
        public abstract void Stop();
    }
}