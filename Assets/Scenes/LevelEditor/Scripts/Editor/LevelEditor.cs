using System.Collections.Generic;
using System.IO;
using Scenes.LevelEditor.Scripts.Storage;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Scenes.LevelEditor.Scripts.Editor
{
    public class LevelEditor : EditorWindow
    {
	    private GameObject _parent;
	    private Transform _transformParent;
	    private string _layerName = "Default";
	    private string _prefabName = "";
	    private string _pathPrefab = "";
	    private Vector3 _position = Vector3.zero;
	    private StorageBase _fileStorage;

	    [MenuItem("Window/Level Editor")]
        private static void ShowWindow()
        {
            var window = GetWindow<LevelEditor>();
            window.titleContent = new GUIContent("Level Editor");
            window.Show();
        }

        private void OnEnable()
        {
	        VisualTreeAsset original =
		        AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
			        "Assets/Scenes/LevelEditor/Scripts/Editor/LevelEditorUI.uxml");
	        TemplateContainer treeAsset = original.CloneTree();
	        rootVisualElement.Add(treeAsset);

	        StyleSheet styleSheet =
		        AssetDatabase.LoadAssetAtPath<StyleSheet>(
			        "Assets/Scenes/LevelEditor/Scripts/Editor/LevelEditorStyle.uss");
	        rootVisualElement.styleSheets.Add(styleSheet);

	        var folderPath = "Assets/Scenes/LevelEditor/Scripts/Editor";
	        var fileName = "Level.data";
	        _fileStorage = new FileStorage(folderPath, fileName);
        }

        private void OnGUI()
        {
	        GUILayout.Label("Create an Instance of Prefab", EditorStyles.boldLabel);

	        DrawParentGameObject();
            DrawSelectorLayers();
            DrawSelectorPrefabs();
            DrawPosition();
            DrawButtonForAddingGameObject();
            
            EditorGUILayout.Space(20f);
            GUILayout.Label("Save Prefabs Positions From Parent", EditorStyles.boldLabel);
            DrawButtonForSavePrefabPositions();
            EditorGUILayout.Space(20f);
            GUILayout.Label("Load Prefabs Into Parent", EditorStyles.boldLabel);
            DrawButtonForLoadPrefabPositions();
        }
        
        private void DrawParentGameObject()
        {
	        EditorGUILayout.Space();
	        EditorGUILayout.BeginHorizontal();	
	        EditorGUILayout.PrefixLabel(new GUIContent("Parent"));

	        _parent = (GameObject) EditorGUILayout.ObjectField(_parent, typeof(GameObject), true);

	        EditorGUILayout.EndHorizontal();	

	        if (_parent == null)
	        {
		        WarningMessageBox();
		        _transformParent = null;
	        }
	        else
		        _transformParent = _parent.transform;
        }

        private void WarningMessageBox()
        {
	        EditorGUILayout.Space();
	        EditorGUILayout.HelpBox("Without a parent selected, you will not be able to use adding a prefab to the scene, saving and loading.", MessageType.Warning);
        }
        
        private void DrawSelectorLayers()
        {
	        EditorGUILayout.Space();
	        EditorGUILayout.BeginHorizontal();	
	        EditorGUILayout.PrefixLabel(new GUIContent("Layer"));
	        
	        List<string> layerNames = new List<string>();
	        for(int i = 0; i < 32; i++)
	        {
		        var layer = LayerMask.LayerToName(i);
		        if (layer.Length > 0)
		        {
			        layerNames.Add(layer);
		        }
	        }

	        var layerNameSelected = _layerName;
	        var classNameSelectedIndex = layerNames.IndexOf(layerNameSelected);
	        classNameSelectedIndex = Mathf.Clamp(classNameSelectedIndex, 0, layerNames.Count - 1);
	        classNameSelectedIndex = EditorGUILayout.Popup(classNameSelectedIndex, layerNames.ToArray());

	        _layerName = layerNames[classNameSelectedIndex];
			
	        EditorGUILayout.EndHorizontal();	
        }

		private void DrawSelectorPrefabs() {
			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();	
			EditorGUILayout.PrefixLabel(new GUIContent("Prefab"));
			
			string[] allPrefabs = GetAllPrefabs();
			List<string> listNameResult = new List<string>();
			List<string> listResult = new List<string>();
			
			foreach (string prefab in allPrefabs) {
				Object o = AssetDatabase.LoadMainAssetAtPath(prefab);
				GameObject go;
				try {
					go = (GameObject) o;

					if (go.layer == LayerMask.NameToLayer(_layerName))
					{
						listResult.Add(prefab);
						listNameResult.Add(Path.GetFileName(prefab));
					}
				} catch {
					Debug.Log( "For some reason, prefab " + prefab + " won't cast to GameObject" );
				}
			}

			if (listNameResult.Count == 0)
			{
				EditorGUILayout.Popup(0, new string[] {""});
				EditorGUILayout.EndHorizontal();
				_pathPrefab = "";
				_prefabName = "";
				return;
			}

			var sceneNameSelected = _prefabName;
			var classNameSelectedIndex = listNameResult.IndexOf(sceneNameSelected);
			classNameSelectedIndex = Mathf.Clamp(classNameSelectedIndex, 0, listNameResult.Count - 1);
			classNameSelectedIndex = EditorGUILayout.Popup(classNameSelectedIndex, listNameResult.ToArray());

			_pathPrefab = listResult[classNameSelectedIndex];
			_prefabName = listNameResult[classNameSelectedIndex];

			
			LoadPreviewPrefab(GetPrefabPreview(_pathPrefab));
			
			EditorGUILayout.EndHorizontal();	
		}
		
		private void DrawPosition()
		{
			EditorGUILayout.Space(60f);
			EditorGUILayout.BeginHorizontal();	

			_position = EditorGUILayout.Vector3Field("Position", _position);

			EditorGUILayout.EndHorizontal();	
		}

		private void DrawButtonForAddingGameObject() {
			EditorGUILayout.Space(20f);
			if (GUILayout.Button("Add Prefab To Scene", GUILayout.Height(30f))) {
				if(_pathPrefab.Length < 0 || _parent == null) return;
				
				Object o = AssetDatabase.LoadMainAssetAtPath(_pathPrefab);
				GameObject go;
				try {
					go = (GameObject) o;
					GameObject newGo = (GameObject) PrefabUtility.InstantiatePrefab(go);
					if (_transformParent != null)
						newGo.transform.parent = _transformParent;
					newGo.transform.position = _position;

				} catch {
					Debug.Log( "For some reason, prefab " + _pathPrefab + " won't cast to GameObject" );
				}
			}
		}

		private void DrawButtonForSavePrefabPositions()
		{
			EditorGUILayout.Space(20f);
			if (GUILayout.Button("Save prefabs positions", GUILayout.Height(30f))) {
				if(_parent == null) return;

				_fileStorage.Clear();
				
				var childCount = _parent.transform.childCount;
				for (int i = 0; i < childCount; i++)
				{
					var child = _parent.transform.GetChild(i);
					string path = AssetDatabase.GetAssetPath(PrefabUtility.GetCorrespondingObjectFromSource(child.gameObject));
					if(path.Length <= 0) continue;
					path = JsonUtility.ToJson((i, path));
					_fileStorage.Set(path, child.position);
				}
				
				_fileStorage.Save();
			}
		}

		private void DrawButtonForLoadPrefabPositions()
		{
			EditorGUILayout.Space(20f);
			if (GUILayout.Button("Load prefabs", GUILayout.Height(30f))) {
				if(_parent == null) return;

				_fileStorage.Load();
				foreach (var pair in _fileStorage.Data.DataMap)
				{ 
					var (i, path) = JsonUtility.FromJson<(int, string)>(pair.Key);
					Object o = AssetDatabase.LoadMainAssetAtPath(path);
					GameObject go;
					try {
						go = (GameObject) o;
						var newGo = (GameObject) PrefabUtility.InstantiatePrefab(go);
						newGo.transform.parent = _transformParent;
						newGo.transform.position = (Vector3) pair.Value;
					} catch {
						Debug.Log( "For some reason, prefab " + path + " won't cast to GameObject" );
					}
				}
			}
		}
		
		private string[] GetAllPrefabs () {
			string[] temp = AssetDatabase.GetAllAssetPaths();
			List<string> result = new List<string>();
			foreach ( string s in temp ) {
				if ( s.Contains( ".prefab" ) ) result.Add( s );
			}
			return result.ToArray();
		}

		private void LoadPreviewPrefab(Texture texture)
		{
			var windowPreview = rootVisualElement.Query<Image>("preview").First();
			windowPreview.image = texture;
		}
		
		private Texture2D GetPrefabPreview(string path)
		{
			GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
			var editor = UnityEditor.Editor.CreateEditor(prefab);
			Texture2D tex = editor.RenderStaticPreview(path, null, 50, 50);
			DestroyImmediate(editor);
			return tex;
		}
    }
}