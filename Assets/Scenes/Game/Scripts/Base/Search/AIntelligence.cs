using UnityEngine;

namespace Scenes.Game.Scripts.Base.Search
{
    public abstract class AIntelligence: MonoBehaviour
    {
        public abstract bool IsEnable { get; set; }
    }
}