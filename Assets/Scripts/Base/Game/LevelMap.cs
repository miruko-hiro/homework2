using System.Collections.Generic;
using UnityEngine;

namespace Base.Game
{
    public class LevelMap : MonoBehaviour
    {
        //[SerializeField] private GameObject _prefab;

        [SerializeField] private Transform _root;

        private List<Vector3> _points;

        public IReadOnlyList<Vector3> Points => _points;

        private void Awake()
        {
            _points = new List<Vector3>();
            var count = _root.childCount;
            for (var i = 0; i < count; i++)
            {
                _points.Add(_root.GetChild(i).position);
            }
        }

        // [MenuItem("CONTEXT/LevelMap/Instantiate Points")]
        // private static void InstantiatePoints(MenuCommand command)
        // {
        //     Clear(command);
        //     
        //     var levelMap = command.context as LevelMap;
        //     if (levelMap == null)
        //         return;
        //
        //     foreach (var p in levelMap._points.Distinct())
        //     {
        //         var prefab = PrefabUtility.InstantiatePrefab(levelMap._prefab, levelMap._root) as GameObject;
        //         prefab.transform.position = p;
        //     }
        // }
        
        // [MenuItem("CONTEXT/LevelMap/Clear Points")]
        // private static void Clear(MenuCommand command)
        // {
        //     var levelMap = command.context as LevelMap;
        //     if (levelMap == null)
        //         return;
        //     
        //     var count = levelMap._root.childCount;
        //     for (var i = count - 1; i >= 0; i--)
        //     {
        //         DestroyImmediate(levelMap._root.GetChild(i).gameObject);
        //     }
        // }
    }
}