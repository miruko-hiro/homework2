using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Scenes.LevelEditor.Scripts.Editor
{
    public class LevelEditor : EditorWindow
    {
	    private Transform _transformParent;
	    private string _layerName = "Default";
	    private string _prefabName = "";
	    private string _pathPrefab = "";
	    private Vector3 _position = Vector3.zero;

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
        }

        private void OnGUI()
        {
	        GUILayout.Label("Create New Game Object", EditorStyles.boldLabel);

	        DrawParentGameObject();
            DrawSelectorLayers();
            DrawSelectorPrefabs();
            DrawPosition();
            DrawButtonForAddingGameObject();
        }
        
        private void DrawParentGameObject()
        {
	        EditorGUILayout.Space();
	        EditorGUILayout.BeginHorizontal();	
	        EditorGUILayout.PrefixLabel(new GUIContent("Parent"));

	        _transformParent = (Transform) EditorGUILayout.ObjectField(_transformParent, typeof(Transform), true);
			
	        EditorGUILayout.EndHorizontal();	
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
			EditorGUILayout.Space(30f);
			if (GUILayout.Button("Add Prefab To Scene", GUILayout.Height(30f))) {
				if(_pathPrefab.Length < 0) return;
				
				Object o = AssetDatabase.LoadMainAssetAtPath(_pathPrefab);
				GameObject go;
				try {
					go = (GameObject) o;
					if (_transformParent == null)
						Instantiate(go, _position, Quaternion.identity);
					else
						Instantiate(go, _position, Quaternion.identity, _transformParent);
					
				} catch {
					Debug.Log( "For some reason, prefab " + _pathPrefab + " won't cast to GameObject" );
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