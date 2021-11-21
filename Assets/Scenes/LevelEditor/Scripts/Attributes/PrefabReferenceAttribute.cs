using System;
using UnityEngine;

namespace Scenes.LevelEditor.Scripts.Attributes
{
    [Serializable]
    public class PrefabReferenceAttribute: PropertyAttribute
    {
        public string LayerName { get; set; }

        public PrefabReferenceAttribute(string layerName)
        {
            LayerName = layerName;
        }
    }
}