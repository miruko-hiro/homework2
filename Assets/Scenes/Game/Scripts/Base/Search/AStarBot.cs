using System.Linq;
using Scenes.Game.Scripts.Base.Game;
using Scenes.Game.Scripts.Base.Input;
using UnityEngine;

namespace Scenes.Game.Scripts.Base.Search
{
    public class AStarBot : PlayerInput
    {
        [SerializeField] private Graph graph;
        [SerializeField] private ZombieMap _zombieMap;
        [SerializeField] private Transform _player;
        [SerializeField] private float _fireDistance;

        public override (Vector3 moveDirection, Quaternion viewDirection, bool shoot) CurrentInput()
        {
            var alivePositions = _zombieMap.AlivePositions();
            if (alivePositions.Count == 0)
            {
                return (Vector3.zero, Quaternion.identity, false);
            }

            var targetPosition = alivePositions.First();
            var playerPosition = _player.position;
            var from = graph.ToInt(playerPosition);
            var to = graph.ToInt(targetPosition);

            var path = AStarFromGoogle.FindPath(graph.Map, @from, to);
            var nextPathPoint = path.Count >= 2 ? path[1] : to;
            nextPathPoint = new Vector2Int(nextPathPoint.x - graph.DeltaX, nextPathPoint.y - graph.DeltaZ);

            var moveDirection = new Vector3(nextPathPoint.x, playerPosition.y, nextPathPoint.y) - playerPosition;
            var directVector = targetPosition - playerPosition;
            
            Debug.DrawLine(playerPosition + Vector3.up, targetPosition + Vector3.up, Color.yellow);
            return (moveDirection, Quaternion.LookRotation(directVector), directVector.magnitude <= _fireDistance);
        }
    }
}