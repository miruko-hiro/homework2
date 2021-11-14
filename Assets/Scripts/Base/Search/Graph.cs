using System.Linq;
using Base.Game;
using UnityEngine;

namespace Base.Search
{
    public class Graph : MonoBehaviour
    {
        [SerializeField] private LevelMap _levelMap;
        
        public int[,] Map { get; private set; }
        public int DeltaX { get; private set; }
        public int DeltaZ { get; private set; }
        
        private void Awake()
        {
            var maxX = _levelMap.Points.Max(p => Mathf.RoundToInt(p.x));
            var minX = _levelMap.Points.Min(p => Mathf.RoundToInt(p.x));
            
            var maxZ = _levelMap.Points.Max(p => Mathf.RoundToInt(p.z));
            var minZ = _levelMap.Points.Min(p => Mathf.RoundToInt(p.z));

            DeltaX = minX < 0 ? -minX : 0;
            DeltaZ = minZ < 0 ? -minZ : 0;
            
            Map = new int[maxX + DeltaX + 1, maxZ + DeltaZ + 1];
            
            foreach (var point in _levelMap.Points)
            {
                Map[DeltaX + Mathf.RoundToInt(point.x), DeltaZ + Mathf.RoundToInt(point.z)] = -1;
            }
        }

        public Vector2Int ToInt(Vector3 vector3) =>
            new Vector2Int(DeltaX + Mathf.RoundToInt(vector3.x), DeltaZ + Mathf.RoundToInt(vector3.z));

        public bool IsPointCorrect(Vector2Int point)
        {
            // Проверяем, что не вышли за границы карты.
            if (point.x < 0 || point.x >= Map.GetLength(0))
                return false;
            if (point.y < 0 || point.y >= Map.GetLength(1))
                return false;
            // Проверяем, что по клетке можно ходить.
            if ((Map[point.x, point.y] != 0) && (Map[point.x, point.y] != 1))
                return false;
            
            return true;
        }
    }
}