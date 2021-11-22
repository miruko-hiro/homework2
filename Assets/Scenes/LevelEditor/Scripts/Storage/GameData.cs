using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes.LevelEditor.Scripts.Storage {
	[Serializable]
	public sealed class GameData : ISerializationCallbackReceiver {


		[SerializeField] private List<string> _keys;
		[SerializeField] private List<object> _values;

		public Dictionary<string, object> DataMap { get; private set; } = new Dictionary<string, object>();


		public void OnBeforeSerialize() {
			_keys.Clear();
			_values.Clear();

			foreach (var item in DataMap) {
				_keys.Add(item.Key);
				_values.Add(item.Value);
			}
		}

		public void OnAfterDeserialize()
		{
			DataMap = new Dictionary<string, object>();

			for (int i = 0; i != Math.Min(_keys.Count, _values.Count); i++)
				DataMap.Add(_keys[i], _values[i]);
		}
		

		public T Get<T>(string key) {
			DataMap.TryGetValue(key, out var foundValue);
			if (foundValue != null)
				return (T) foundValue;
			return default;
		}
		
		public T Get<T>(string key, T valueByDefault) {
			DataMap.TryGetValue(key, out var value);
			if (value != null)
				return (T) value;

			Set(key, valueByDefault);
			return valueByDefault;
		}

		public void Set<T>(string key, T newValue) {
			DataMap[key] = newValue;
		}

		public override string ToString() {
			var line = "";
			foreach (var pair in DataMap) 
				line += $"Pair: {pair.Key} - {pair.Value}\n";
			return line;
		}
		
	}
}