using System.Collections;
using System.Collections.Generic;
using Base.Game;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UtilityAI.Scorers;

namespace Tests
{
    public class TestDistanceToDynamicPosition
    {
        private readonly Dictionary<ZombieComponent, Vector3> _zombieMap;

        public TestDistanceToDynamicPosition()
        {
            
            var zombieAIArray = Object.FindObjectsOfType<ZombieComponent>();
            _zombieMap = new Dictionary<ZombieComponent, Vector3>();
            
            foreach (var zombieAI in zombieAIArray)
            {
                _zombieMap.Add(zombieAI, zombieAI.transform.position);
            }
        }
        
        [UnityTest]
        public IEnumerator GetScoreGreaterOrEqualToCloseScoreIfCloseScoreLessFarScore()
        {
            yield return null;
            
            foreach (var pair in _zombieMap)
            {
                var distanceToDynamicPositionArray =
                    pair.Key.gameObject.GetComponentsInChildren<DistanceToDynamicPosition>();

                foreach (var distanceToDynamicPosition in distanceToDynamicPositionArray)
                {
                    if (distanceToDynamicPosition.CloseScore < distanceToDynamicPosition.FarScore)
                    {
                        Transform tf = pair.Key.transform;
                        tf.position += new Vector3(distanceToDynamicPosition.MaxDistance * 2, 0f, 0f);
                        Assert.GreaterOrEqual(distanceToDynamicPosition.GetScore(), distanceToDynamicPosition.CloseScore);

                        tf.position = distanceToDynamicPosition.DynamicPosition;
                        Assert.GreaterOrEqual(distanceToDynamicPosition.GetScore(), distanceToDynamicPosition.CloseScore);
                        
                        tf.position = pair.Value;
                        Assert.GreaterOrEqual(distanceToDynamicPosition.GetScore(), distanceToDynamicPosition.CloseScore);
                    }
                }
            }
        }

        [UnityTest]
        public IEnumerator GetScoreLessOrEqualToFarScoreIfCloseScoreLessFarScore()
        {
            yield return null;
            
            foreach (var pair in _zombieMap)
            {
                var distanceToDynamicPositionArray =
                    pair.Key.gameObject.GetComponentsInChildren<DistanceToDynamicPosition>();

                foreach (var distanceToDynamicPosition in distanceToDynamicPositionArray)
                {
                    if (distanceToDynamicPosition.CloseScore < distanceToDynamicPosition.FarScore)
                    {
                        Transform tf = pair.Key.transform;
                        tf.position += new Vector3(distanceToDynamicPosition.MaxDistance * 2, 0f, 0f);
                        Assert.LessOrEqual(distanceToDynamicPosition.GetScore(), distanceToDynamicPosition.FarScore);

                        tf.position = distanceToDynamicPosition.DynamicPosition;
                        Assert.LessOrEqual(distanceToDynamicPosition.GetScore(), distanceToDynamicPosition.FarScore);
                        
                        tf.position = pair.Value;
                        Assert.LessOrEqual(distanceToDynamicPosition.GetScore(), distanceToDynamicPosition.FarScore);
                    }
                }
            }
        }
        
        [UnityTest]
        public IEnumerator GetScoreLessOrEqualToCloseScoreIfCloseScoreCreatedFarScore()
        {
            yield return null;
            
            foreach (var pair in _zombieMap)
            {
                var distanceToDynamicPositionArray =
                    pair.Key.gameObject.GetComponentsInChildren<DistanceToDynamicPosition>();

                foreach (var distanceToDynamicPosition in distanceToDynamicPositionArray)
                {
                    if (distanceToDynamicPosition.CloseScore > distanceToDynamicPosition.FarScore)
                    {
                        Transform tf = pair.Key.transform;
                        tf.position += new Vector3(distanceToDynamicPosition.MaxDistance * 2, 0f, 0f);
                        Assert.LessOrEqual(distanceToDynamicPosition.GetScore(), distanceToDynamicPosition.CloseScore);

                        tf.position = distanceToDynamicPosition.DynamicPosition;
                        Assert.LessOrEqual(distanceToDynamicPosition.GetScore(), distanceToDynamicPosition.CloseScore);
                        
                        tf.position = pair.Value;
                        Assert.LessOrEqual(distanceToDynamicPosition.GetScore(), distanceToDynamicPosition.CloseScore);
                    }
                }
            }
        }

        [UnityTest]
        public IEnumerator GetScoreGreaterOrEqualToFarScoreIfCloseScoreCreatedFarScore()
        {
            yield return null;
            
            foreach (var pair in _zombieMap)
            {
                var distanceToDynamicPositionArray =
                    pair.Key.gameObject.GetComponentsInChildren<DistanceToDynamicPosition>();

                foreach (var distanceToDynamicPosition in distanceToDynamicPositionArray)
                {
                    if (distanceToDynamicPosition.CloseScore > distanceToDynamicPosition.FarScore)
                    {
                        Transform tf = pair.Key.transform;
                        tf.position += new Vector3(distanceToDynamicPosition.MaxDistance * 2, 0f, 0f);
                        Assert.GreaterOrEqual(distanceToDynamicPosition.GetScore(), distanceToDynamicPosition.FarScore);

                        tf.position = distanceToDynamicPosition.DynamicPosition;
                        Assert.GreaterOrEqual(distanceToDynamicPosition.GetScore(), distanceToDynamicPosition.FarScore);
                        
                        tf.position = pair.Value;
                        Assert.GreaterOrEqual(distanceToDynamicPosition.GetScore(), distanceToDynamicPosition.FarScore);
                    }
                }
            }
        }
    }
}
