using UnityEngine;

namespace Scenes.Game.Scripts.UtilityAI.Scorers
{
    public class DistanceToStaticPosition: Scorer
    {
        [SerializeField] private Transform transformOwn;
        [SerializeField] private Vector3 staticPosition;
        [SerializeField] private float minDistance;
        [SerializeField] private float maxDistance;
        [SerializeField] private float closeScore;
        [SerializeField] private float farScore;
        
        public float CloseScore => closeScore;
        public float FarScore => farScore;
        public float MinDistance => minDistance;
        public float MaxDistance => maxDistance;
        public Vector3 StaticPosition => staticPosition;
        
        public override float GetScore()
        {
            var distance =  Vector3.Distance(transformOwn.position, staticPosition);

            if (distance <= minDistance) return closeScore;
            if (distance >= maxDistance) return farScore;

            var y = (distance - minDistance) * (Mathf.Abs(closeScore - farScore) / (maxDistance - minDistance));
            return closeScore < farScore ? closeScore + y : closeScore - y;
        }
    }
}