using System;
using System.IO;
using System.Threading;
using UnityEngine;

namespace Scenes.LevelEditor.Scripts.Storage {
	public sealed class FileStorage : StorageBase{

		public string FilePath { get; }
		
		public FileStorage(string folderPath, string fileName)
		{
			FilePath = $"{folderPath}/{fileName}";
		}
		
		public FileStorage(string fileName) {
			var folder = "Saves";
			var folderPath = $"{Application.persistentDataPath}/{folder}";
			if (!Directory.Exists(folderPath)) 
				Directory.CreateDirectory(folderPath);

			FilePath = $"{folderPath}/{fileName}";
		}

		protected override void SaveInternal() {
			Debug.Log("File Path With Saving: " + FilePath);
			var file = File.Create(FilePath);
			Formatter.Serialize(file, Data);
			file.Close();
		}

		protected override void SaveAsyncInternal(Action callback = null) {
			var thread = new Thread(() => SaveDataTaskThreaded(callback));
			thread.Start();
		}
		
		private void SaveDataTaskThreaded(Action callback) {
			Save();
			callback?.Invoke();
		}
		
		protected override void LoadInternal() {
			if (!File.Exists(FilePath)) {
				var gameDataByDefault = new GameData();
				Data = gameDataByDefault;
				Save();
			}

			var file = File.Open(FilePath, FileMode.Open);
			Data = (GameData) Formatter.Deserialize(file);
			file.Close();
		}

		
		protected override void LoadAsyncInternal(Action<GameData> callback = null) {
			var thread = new Thread(() => LoadDataTaskThreaded(callback));
			thread.Start();
		}
		private void LoadDataTaskThreaded(Action<GameData> callback) {
			Load();
			callback?.Invoke(Data);
		}

		protected override void ClearInternal()
		{
			File.Exists(FilePath);
			var gameDataByDefault = new GameData();
			Data = gameDataByDefault;
		}

	}
}