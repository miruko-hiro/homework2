﻿using UnityEngine;

namespace UtilityAI.Scorers
{
    public class DistanceToStaticPosition: Scorer
    {
        [SerializeField] private Transform transformOwn;
        [SerializeField] private Vector3 staticPosition;
        [SerializeField] private float minDistance;
        [SerializeField] private float maxDistance;
        [SerializeField] private float closeScore;
        [SerializeField] private float farScore;
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