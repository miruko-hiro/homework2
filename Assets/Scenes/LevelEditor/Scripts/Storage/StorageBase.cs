using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Scenes.LevelEditor.Scripts.Storage.Surrogates;
using UnityEngine;

namespace Scenes.LevelEditor.Scripts.Storage {
	public abstract class StorageBase {

		// public event Action OnStorageSaveStartedEvent;
		// public event Action OnStorageSaveCompleteEvent;
		// public event Action<GameData> OnStorageLoadedEvent;

		public static BinaryFormatter Formatter {
			get {
				if (_formatter == null)
					_formatter = CreateBinaryFormatter();
				return _formatter;
			}
		}
		private static BinaryFormatter _formatter;

		public GameData Data { get; protected set; }

		private static BinaryFormatter CreateBinaryFormatter() {
			var createdFormatter = new BinaryFormatter();
			var selector = new SurrogateSelector();
			
			var vector3Surrogate = new Vector3SerializationSurrogate();
			
			selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3Surrogate);
			
			createdFormatter.SurrogateSelector = selector;

			return createdFormatter;
		}

		public void Save() {
			//OnStorageSaveStartedEvent?.Invoke();
			SaveInternal();
			//OnStorageSaveCompleteEvent?.Invoke();
		}

		protected abstract void SaveInternal();

		public void SaveAsync(Action callback = null) {
			//OnStorageSaveStartedEvent?.Invoke();
			SaveAsyncInternal(callback);
			//OnStorageSaveCompleteEvent?.Invoke();
		}

		protected abstract void SaveAsyncInternal(Action callback = null);

		public void Load() {
			LoadInternal();
			//OnStorageLoadedEvent?.Invoke(Data);
		}

		protected abstract void LoadInternal();

		public void LoadAsync(Action<GameData> callback = null) {
			LoadAsyncInternal(loadedData => {
				callback?.Invoke(Data);
				//OnStorageLoadedEvent?.Invoke(Data);
			});
		}

		protected abstract void LoadAsyncInternal(Action<GameData> callback = null);

		public void Clear()
		{
			ClearInternal();
		}

		protected abstract void ClearInternal();
		

		public T Get<T>(string key) {
			return Data.Get<T>(key);
		}
		
		public T Get<T>(string key, T valueByDefault) {
			return Data.Get(key, valueByDefault);
		}

		public void Set<T>(string key, T value) {
			Data.Set(key, value);
		}

		public override string ToString() {
			return Data.ToString();
		}
		
		
	}
}