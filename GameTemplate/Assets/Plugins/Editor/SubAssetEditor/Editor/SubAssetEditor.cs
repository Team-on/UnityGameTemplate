using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;

namespace Mobcast.CoffeeEditor.SubAssetEditor
{
	/// <summary>内蔵アセットウィンドウ.</summary>
	class SubAssetEditor : EditorWindow
	{
		//---- ▼ GUIキャッシュ ▼ ----
		static GUIContent contentNoRef;
		static GUIContent contentAdd;
		static GUIContent contentDelete;
		static GUIContent contentImport;
		static GUIContent contentExport;
		static bool cached = false;
		const float ICON_SIZE = 20;

		/// <summary>GUIキャッシュ.</summary>
		static void CacheGUI()
		{
			if (cached)
				return;
			cached = true;

			contentNoRef = new GUIContent(EditorGUIUtility.FindTexture("console.warnicon.sml"), "Not referenced by main asset");
			contentAdd = new GUIContent(EditorGUIUtility.FindTexture("toolbar plus"), "Add to sub assets");
			contentDelete = new GUIContent(EditorGUIUtility.FindTexture("treeeditor.trash"), "Delete asset");
			contentImport = new GUIContent("Drag & Drop object to add as sub-asset.", EditorGUIUtility.FindTexture("toolbar plus"));
			contentExport = new GUIContent(EditorGUIUtility.FindTexture("saveactive"), "Export asset");
		}
		//---- ▲ GUIキャッシュ ▲ ----

		bool isLocked;
		bool isRenaming;
		bool hasSelectionChanged;

		Object current;
		Vector2 scrollPosition;

		List<Object> subAssets = new List<Object>();
		List<Object> referencingAssets = new List<Object>();

		[MenuItem("Window/Sub Asset Editor")]
		public static void OnOpenFromMenu()
		{
			EditorWindow.GetWindow<SubAssetEditor>("Sub Asset");
		}

		private void OnEnable()
		{
			Selection.selectionChanged += OnSelectionChanged;
		}

		private void OnDisable()
		{
			Selection.selectionChanged -= OnSelectionChanged;
		}

		void OnSelectionChanged()
		{
			// On select new asset.
			var active = Selection.activeObject;
			if (!isLocked && active && current != active && !(active is SceneAsset) && AssetDatabase.IsMainAsset(active))
			{
				OnSelectionChanged(active);
			}
		}

		void OnSelectionChanged(Object active)
		{
			current = active;

			// Find sub-assets.
			var assetPath = AssetDatabase.GetAssetPath(active);
			subAssets = AssetDatabase.LoadAllAssetsAtPath(assetPath)
				.Where(x => x != current && 0 == (x.hideFlags & HideFlags.HideInHierarchy))
					.Distinct()
					.ToList();

			// Find referencing assets.
			referencingAssets.Clear();
			foreach (var o in AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(active)))
			{
				var sp = new SerializedObject(o).GetIterator();
				sp.Next(true);

				// Search referencing asset in SerializedProperties.
				while (sp.Next(true))
				{
					if (sp.propertyType == SerializedPropertyType.ObjectReference && sp.objectReferenceValue)
					{
						var asset = sp.objectReferenceValue;
						if (active != asset && o != asset && 0 == (asset.hideFlags & HideFlags.HideInHierarchy) && !referencingAssets.Contains(asset))
						{
							referencingAssets.Add(asset);
						}
					}
				}
			}

			// Refresh GUI.
			Repaint();
		}

		void DeleteSubAsset(Object asset)
		{
			Object.DestroyImmediate(asset, true);
			hasSelectionChanged = true;
		}

		void AddSubAsset(Object asset)
		{
			AddSubAsset(new Object[]{ asset });
		}

		/// <summary>
		/// Replace the specified obj, oldAsset and newAsset.
		/// </summary>
		void AddSubAsset(IEnumerable<Object>  assets)
		{
			// 受け入れ可能オブジェクト
			assets = assets
				.Where(x => x != current && !(x is SceneAsset) && AssetDatabase.Contains(x) && !subAssets.Contains(x));

			// Replace targets
			SerializedObject[] replaceTargets = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(current))
				.Select(x => new SerializedObject(x))
				.ToArray();

			// 
			foreach (var asset in assets)
			{
				Object newInstance = Object.Instantiate(asset);
				newInstance.name = asset.name;
				AssetDatabase.AddObjectToAsset(newInstance, current);

				// Find referencing assets.
				foreach (SerializedObject so in replaceTargets)
				{
					var sp = so.GetIterator();

					// Search referencing asset in SerializedProperties.
					while (sp.Next(true))
					{
						// Replace to new object.
						if (sp.propertyType == SerializedPropertyType.ObjectReference && sp.objectReferenceValue == asset)
						{
							sp.objectReferenceValue = newInstance;
						}
					}
					sp.serializedObject.ApplyModifiedProperties();
				}
			}
			hasSelectionChanged = true;
		}

		void OnGUI()
		{
			CacheGUI();
			if (!current)
				return;
			using (new EditorGUILayout.HorizontalScope())
			{
                Rect rLabel = EditorGUILayout.GetControlRect(GUILayout.Width(80));
				GUI.Toggle(rLabel, true, "<b>Main Asset</b>", "IN Foldout");

				Rect rLock = EditorGUILayout.GetControlRect(GUILayout.Width(20));
				rLock.y += 2;
				if (GUI.Toggle(rLock, isLocked, GUIContent.none, "IN LockButton") != isLocked)
				{
					isLocked = !isLocked;
				}
				GUILayout.FlexibleSpace();
			}
			EditorGUI.indentLevel++;
			using (new EditorGUILayout.HorizontalScope())
			{
				Rect r = EditorGUILayout.GetControlRect(true);

				r.width -= 20;
				EditorGUI.ObjectField(r, current, current.GetType(), false);

				r.x += r.width;
				r.width = 20;
				r.height = 20;
				if (GUI.Button(r, contentDelete, EditorStyles.label))
				{
					DeleteSubAsset(current);
				}
			}
			EditorGUI.indentLevel--;

			GUILayout.Space(10);
			using (new EditorGUILayout.HorizontalScope())
			{
                Rect rLabel = EditorGUILayout.GetControlRect(GUILayout.Width(80));
                GUI.Toggle(rLabel, true, "<b>Sub Asset</b>", "IN Foldout");

                Rect rRename = EditorGUILayout.GetControlRect(GUILayout.Width(60));
				isRenaming = GUI.Toggle(rRename, isRenaming, "Rename", EditorStyles.miniButton);
				GUILayout.FlexibleSpace();
			}

			EditorGUI.indentLevel++;
			foreach (var asset in subAssets)
			{
				Rect r = EditorGUILayout.GetControlRect(true);

				r.width -= 60;
				Rect rField = new Rect(r);
				if (isRenaming)
				{
					rField.width = 12;
					rField.height = 12;
					//Draw icon of current object.
					EditorGUI.LabelField(r, new GUIContent(AssetPreview.GetMiniThumbnail(asset)));
					EditorGUI.BeginChangeCheck();

					rField.x += rField.width + 4;
					rField.width = r.width - rField.width;
					rField.height = r.height;
					asset.name = EditorGUI.DelayedTextField(rField, asset.name);
					if (EditorGUI.EndChangeCheck())
					{
						AssetDatabase.SaveAssets();
					}
				}
				else
				{
					EditorGUI.ObjectField(rField, asset, asset.GetType(), false);
				}

				r.x += r.width;
				r.width = 20;
				r.height = 20;
				if (!referencingAssets.Contains(asset))
				{
					GUI.Label(r, contentNoRef);
				}
				
				r.x += r.width;
				if (GetFileExtention(asset).Length != 0 && GUI.Button(r, contentExport, EditorStyles.label))
				{
					ExportSubAsset(asset);
				}
				
				r.x += r.width;
				if (GUI.Button(r, contentDelete, EditorStyles.label))
				{
					DeleteSubAsset(asset);
				}
			}
			EditorGUI.indentLevel--;

			GUILayout.Space(10);
			GUILayout.Toggle(true, "<b>Referencing Objects</b>", "IN Foldout");
			EditorGUI.indentLevel++;
			EditorGUILayout.HelpBox("Sub assets are excluded.", MessageType.None);
			foreach (var asset in referencingAssets.Except(subAssets))
			{
				Rect r = EditorGUILayout.GetControlRect();

				r.width -= 20;
				EditorGUI.ObjectField(r, asset, asset.GetType(), false);

				r.x += r.width;
				r.y -= 1;
				r.width = 20;
				r.height = 20;

				// Add object to sub asset.
				if (GUI.Button(r, contentAdd, EditorStyles.label))
				{
					var addAsset = asset;
					EditorApplication.delayCall += () => AddSubAsset(addAsset);
				}
			}
			EditorGUI.indentLevel--;


			DrawImportArea();


			if (hasSelectionChanged)
			{
				hasSelectionChanged = false;
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
				OnSelectionChanged(current);
			}
		}



		/// <summary>ドラッグ＆ドロップでアセットを追加可能なインポートエリアを描画します.</summary>
		void DrawImportArea()
		{
			GUILayout.Space(5);
			Rect dropArea = GUILayoutUtility.GetRect(0, 20, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
			GUI.Box(dropArea, contentImport, EditorStyles.helpBox);

			int id = GUIUtility.GetControlID(FocusType.Passive);
			Event evt = Event.current;
			switch (evt.type)
			{
				case EventType.DragUpdated:
				case EventType.DragPerform:
					if (!dropArea.Contains(evt.mousePosition))
						break;

					DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
					DragAndDrop.activeControlID = id;

					if (evt.type == EventType.DragPerform)
					{
						DragAndDrop.AcceptDrag();
						DragAndDrop.activeControlID = 0;

						AddSubAsset(DragAndDrop.objectReferences);
					}
					Event.current.Use();
					break;
			}
		}


		/// <summary>
		/// 内蔵アセットをエクスポートします.
		/// </summary>
		void ExportSubAsset(Object obj)
		{
			string exportDir = Path.GetDirectoryName(AssetDatabase.GetAssetPath(current));
			string exportName = obj.name + " (Exported)." + GetFileExtention(obj);
			string uniquePath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(exportDir, exportName));
			AssetDatabase.CreateAsset(Object.Instantiate(obj), uniquePath);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}


		/// <summary>アセットの型に合わせた拡張子を取得します.</summary>
		string GetFileExtention(Object obj)
		{
			if (obj is AnimationClip)
				return "anim";
			else if (obj is UnityEditor.Animations.AnimatorController)
				return "controller";
			else if (obj is AnimatorOverrideController)
				return "overrideController";
			else if (obj is Material)
				return "mat";
			else if (obj is Texture)
				return "png";
			else if (obj is ComputeShader)
				return "compute";
			else if (obj is Shader)
				return "shader";
			else if (obj is Cubemap)
				return "cubemap";
			else if (obj is Flare)
				return "flare";
			else if (obj is ShaderVariantCollection)
				return "shadervariants";
			else if (obj is LightmapParameters)
				return "giparams";
			else if (obj is GUISkin)
				return "guiskin";
			else if (obj is PhysicMaterial)
				return "physicMaterial";
			else if (obj is UnityEngine.Audio.AudioMixer)
				return "mixer";
			else if (obj is TextAsset)
				return "txt";
			else if (obj is GameObject)
				return "prefab";
			else if (obj is ScriptableObject)
				return "asset";
			return "";
		}
	}
}
