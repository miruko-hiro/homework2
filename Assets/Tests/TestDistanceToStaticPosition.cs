using System.Collections;
using System.Collections.Generic;
using Base.Game;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UtilityAI.Scorers;
using Object = UnityEngine.Object;

namespace Tests
{
    public class TestDistanceToStaticPosition
    {
        private readonly Dictionary<ZombieComponent, Vector3> _zombieMap;

        public TestDistanceToStaticPosition()
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
                var distanceToStaticPositionArray =
                    pair.Key.gameObject.GetComponentsInChildren<DistanceToStaticPosition>();

                foreach (var distanceToStaticPosition in distanceToStaticPositionArray)
                {
                    if (distanceToStaticPosition.CloseScore < distanceToStaticPosition.FarScore)
                    {
                        Transform tf = pair.Key.transform;
                        tf.position += new Vector3(distanceToStaticPosition.MaxDistance * 2, 0f, 0f);
                        Assert.GreaterOrEqual(distanceToStaticPosition.GetScore(), distanceToStaticPosition.CloseScore);

                        tf.position = distanceToStaticPosition.StaticPosition;
                        Assert.GreaterOrEqual(distanceToStaticPosition.GetScore(), distanceToStaticPosition.CloseScore);
                        
                        tf.position = pair.Value;
                        Assert.GreaterOrEqual(distanceToStaticPosition.GetScore(), distanceToStaticPosition.CloseScore);
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
                var distanceToStaticPositionArray =
                    pair.Key.gameObject.GetComponentsInChildren<DistanceToStaticPosition>();

                foreach (var distanceToStaticPosition in distanceToStaticPositionArray)
                {
                    if (distanceToStaticPosition.CloseScore < distanceToStaticPosition.FarScore)
                    {
                        Transform tf = pair.Key.transform;
                        tf.position += new Vector3(distanceToStaticPosition.MaxDistance * 2, 0f, 0f);
                        Assert.LessOrEqual(distanceToStaticPosition.GetScore(), distanceToStaticPosition.FarScore);

                        tf.position = distanceToStaticPosition.StaticPosition;
                        Assert.LessOrEqual(distanceToStaticPosition.GetScore(), distanceToStaticPosition.FarScore);
                        
                        tf.position = pair.Value;
                        Assert.LessOrEqual(distanceToStaticPosition.GetScore(), distanceToStaticPosition.FarScore);
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
                var distanceToStaticPositionArray =
                    pair.Key.gameObject.GetComponentsInChildren<DistanceToStaticPosition>();

                foreach (var distanceToStaticPosition in distanceToStaticPositionArray)
                {
                    if(distanceToStaticPosition.CloseScore > distanceToStaticPosition.FarScore)
                    {
                        Transform tf = pair.Key.transform;
                        tf.position += new Vector3(distanceToStaticPosition.MaxDistance * 2, 0f, 0f);
                        Assert.LessOrEqual(distanceToStaticPosition.GetScore(), distanceToStaticPosition.CloseScore);

                        tf.position = distanceToStaticPosition.StaticPosition;
                        Assert.LessOrEqual(distanceToStaticPosition.GetScore(), distanceToStaticPosition.CloseScore);
                        
                        tf.position = pair.Value;
                        Assert.LessOrEqual(distanceToStaticPosition.GetScore(), distanceToStaticPosition.CloseScore);
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
                var distanceToStaticPositionArray =
                    pair.Key.gameObject.GetComponentsInChildren<DistanceToStaticPosition>();

                foreach (var distanceToStaticPosition in distanceToStaticPositionArray)
                {
                    if(distanceToStaticPosition.CloseScore > distanceToStaticPosition.FarScore)
                    {
                        Transform tf = pair.Key.transform;
                        tf.position += new Vector3(distanceToStaticPosition.MaxDistance * 2, 0f, 0f);
                        Assert.GreaterOrEqual(distanceToStaticPosition.GetScore(), distanceToStaticPosition.FarScore);

                        tf.position = distanceToStaticPosition.StaticPosition;
                        Assert.GreaterOrEqual(distanceToStaticPosition.GetScore(), distanceToStaticPosition.FarScore);
                        
                        tf.position = pair.Value;
                        Assert.GreaterOrEqual(distanceToStaticPosition.GetScore(), distanceToStaticPosition.FarScore);
                    }
                }
            }
        }
    }
}
