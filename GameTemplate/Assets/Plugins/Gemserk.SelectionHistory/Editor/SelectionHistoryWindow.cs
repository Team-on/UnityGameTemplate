using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

namespace Gemserk
{
	[InitializeOnLoad]
	public static class SelectionHistoryInitialized
	{
		static SelectionHistoryInitialized()
		{
			SelectionHistoryWindow.RegisterSelectionListener ();
		}
	}

	public class SelectionHistoryWindow : EditorWindow {
		const float buttonsWidth = 120f;

		public static readonly string HistorySizePrefKey = "Gemserk.SelectionHistory.HistorySize";
		public static readonly string HistoryAutomaticRemoveDeletedPrefKey = "Gemserk.SelectionHistory.AutomaticRemoveDeleted";
		public static readonly string HistoryAllowDuplicatedEntriesPrefKey = "Gemserk.SelectionHistory.AllowDuplicatedEntries";
	    public static readonly string HistoryShowHierarchyObjectsPrefKey = "Gemserk.SelectionHistory.ShowHierarchyObjects";
	    public static readonly string HistoryShowProjectViewObjectsPrefKey = "Gemserk.SelectionHistory.ShowProjectViewObjects";
	    public static readonly string HistoryFavoritesPrefKey = "Gemserk.SelectionHistory.Favorites";

        static SelectionHistory selectionHistory = new SelectionHistory();

		static readonly bool debugEnabled = false;

		public static bool shouldReloadPreferences = true;

	    private static Color hierarchyElementColor = new Color(0.7f, 1.0f, 0.7f);
	    private static Color selectedElementColor = new Color(0.2f, 170.0f / 255.0f, 1.0f, 1.0f);

        [MenuItem ("Window/Custom/Gemserk/Selection History %#h")]
		static void Init () {
			// Get existing open window or if none, make a new one:
			var window = EditorWindow.GetWindow<SelectionHistoryWindow> ();

			window.titleContent.text = "History";
			window.Show();
		}

		static void SelectionRecorder ()
		{
			if (Selection.activeObject != null) {
				if (debugEnabled) {
					Debug.Log ("Recording new selection: " + Selection.activeObject.name);
				}

				selectionHistory = EditorTemporaryMemory.Instance.selectionHistory;
				selectionHistory.UpdateSelection (Selection.activeObject);
			} 
		}

		public static void RegisterSelectionListener()
		{
			Selection.selectionChanged += SelectionRecorder;
		}

		public GUISkin windowSkin;

		MethodInfo openPreferencesWindow;

		void OnEnable()
		{
			automaticRemoveDeleted = EditorPrefs.GetBool (HistoryAutomaticRemoveDeletedPrefKey, true);

			selectionHistory = EditorTemporaryMemory.Instance.selectionHistory;
			selectionHistory.HistorySize = EditorPrefs.GetInt (HistorySizePrefKey, 10);

			Selection.selectionChanged += delegate {

				if (selectionHistory.IsSelected(selectionHistory.GetHistoryCount() - 1)) {
					_historyScrollPosition.y = float.MaxValue;
				}

				Repaint();
			};

			try {
				var asm = Assembly.GetAssembly (typeof(EditorWindow));
				var t = asm.GetType ("UnityEditor.PreferencesWindow");
				openPreferencesWindow = t.GetMethod ("ShowPreferencesWindow", BindingFlags.NonPublic | BindingFlags.Static);
			} catch {
				// couldnt get preferences window...
				openPreferencesWindow = null;
			}
		}

		void UpdateSelection(Object obj)
		{
		    selectionHistory.SetSelection(obj);
            Selection.activeObject = obj;
            // Selection.activeObject = selectionHistory.UpdateSelection(currentIndex);
		}

	    private Vector2 _favoritesScrollPosition;
		private Vector2 _historyScrollPosition;

		bool automaticRemoveDeleted;
		bool allowDuplicatedEntries;

	    bool showHierarchyViewObjects;
	    bool showProjectViewObjects;

        void OnGUI () {

			if (shouldReloadPreferences) {
				selectionHistory.HistorySize = EditorPrefs.GetInt (SelectionHistoryWindow.HistorySizePrefKey, 10);
				automaticRemoveDeleted = EditorPrefs.GetBool (SelectionHistoryWindow.HistoryAutomaticRemoveDeletedPrefKey, true);
				allowDuplicatedEntries = EditorPrefs.GetBool (SelectionHistoryWindow.HistoryAllowDuplicatedEntriesPrefKey, false);

			    showHierarchyViewObjects = EditorPrefs.GetBool(SelectionHistoryWindow.HistoryShowHierarchyObjectsPrefKey, true);
			    showProjectViewObjects = EditorPrefs.GetBool(SelectionHistoryWindow.HistoryShowProjectViewObjectsPrefKey, true);

                shouldReloadPreferences = false;
			}

			if (automaticRemoveDeleted)
				selectionHistory.ClearDeleted ();

			if (!allowDuplicatedEntries)
				selectionHistory.RemoveDuplicated ();

            var favoritesEnabled = EditorPrefs.GetBool(HistoryFavoritesPrefKey, true);
            if (favoritesEnabled && selectionHistory.Favorites.Count > 0)
            {
                _favoritesScrollPosition = EditorGUILayout.BeginScrollView(_favoritesScrollPosition);
                DrawFavorites();
                EditorGUILayout.EndScrollView();
                EditorGUILayout.Separator();
            }
        
            bool changedBefore = GUI.changed;

			_historyScrollPosition = EditorGUILayout.BeginScrollView(_historyScrollPosition);

			bool changedAfter = GUI.changed;

			if (!changedBefore && changedAfter) {
				Debug.Log ("changed");
			}

			DrawHistory();

			EditorGUILayout.EndScrollView();

			if (GUILayout.Button("Clear")) {
				selectionHistory.Clear();
				Repaint();
			}

			if (!automaticRemoveDeleted) {
				if (GUILayout.Button ("Remove Deleted")) {
					selectionHistory.ClearDeleted ();
					Repaint ();
				}
			} 

			if (allowDuplicatedEntries) {
				if (GUILayout.Button ("Remove Duplciated")) {
					selectionHistory.RemoveDuplicated ();
					Repaint ();
				}
			} 

			DrawSettingsButton ();
		}

		void DrawSettingsButton()
		{
			if (openPreferencesWindow == null)
				return;
			
			if (GUILayout.Button ("Preferences")) {
				openPreferencesWindow.Invoke(null, null);
			}
		}
			
		[MenuItem("Window/Custom/Gemserk/Previous selection %#,")]
		public static void PreviousSelection()
		{
			selectionHistory.Previous ();
			Selection.activeObject = selectionHistory.GetSelection ();
		}

		[MenuItem("Window/Custom/Gemserk/Next selection %#.")]
		public static void Nextelection()
		{
			selectionHistory.Next();
			Selection.activeObject = selectionHistory.GetSelection ();
		}

	    void DrawElement(Object obj, int i, Color originalColor)
	    {
	        var buttonStyle = windowSkin.GetStyle("SelectionButton");
			buttonStyle.fixedWidth = position.width - buttonsWidth;
			var nonSelectedColor = originalColor;

            if (!EditorUtility.IsPersistent(obj))
            {
                if (!showHierarchyViewObjects)
                    return;
                nonSelectedColor = hierarchyElementColor;
            }
            else
            {
                if (!showProjectViewObjects)
                    return;
            }

            if (selectionHistory.IsSelected(obj))
            {
                GUI.contentColor = selectedElementColor;
            }
            else
            {
                GUI.contentColor = nonSelectedColor;
            }

            var rect = EditorGUILayout.BeginHorizontal();

            if (obj == null)
            {
                GUILayout.Label("Deleted", buttonStyle);
            }
            else
            {
                var icon = AssetPreview.GetMiniThumbnail(obj);

                GUIContent content = new GUIContent();

                content.image = icon;
                content.text = obj.name;

                // chnanged to label to be able to handle events for drag
                GUILayout.Label(content, buttonStyle);

                GUI.contentColor = originalColor;

                if (GUILayout.Button("Ping", windowSkin.button))
                {
                    EditorGUIUtility.PingObject(obj);
                }

                var favoritesEnabled = EditorPrefs.GetBool(HistoryFavoritesPrefKey, true);

                if (favoritesEnabled)
                {
                    var pinString = "Pin";
                    var isFavorite = selectionHistory.IsFavorite(obj);

                    if (isFavorite)
                    {
                        pinString = "Unpin";
                    }

                    if (GUILayout.Button(pinString, windowSkin.button))
                    {
                        selectionHistory.ToggleFavorite(obj);
                        Repaint();
                    }
                }

            }

            EditorGUILayout.EndHorizontal();

            ButtonLogic(rect, obj);
        }

	    void DrawFavorites()
	    {
	        var originalColor = GUI.contentColor;

	        var favorites = selectionHistory.Favorites;

	        var buttonStyle = windowSkin.GetStyle("SelectionButton");

	        for (int i = 0; i < favorites.Count; i++)
	        {
	            var favorite = favorites[i];
                DrawElement(favorite, i, originalColor);
	        }

	        GUI.contentColor = originalColor;
        }

		void DrawHistory()
		{
			var originalColor = GUI.contentColor;

			var history = selectionHistory.History;

			var buttonStyle = windowSkin.GetStyle("SelectionButton");

		    var favoritesEnabled = EditorPrefs.GetBool(HistoryFavoritesPrefKey, true);

            for (int i = 0; i < history.Count; i++) {
				var historyElement = history [i];
                if (selectionHistory.IsFavorite(historyElement) && favoritesEnabled)
                    continue;
			    DrawElement(historyElement, i, originalColor);
            }

			GUI.contentColor = originalColor;
		}

		void ButtonLogic(Rect rect, Object currentObject)
		{
			var currentEvent = Event.current;

			if (currentEvent == null)
				return;

			if (!rect.Contains (currentEvent.mousePosition))
				return;
			
//			Debug.Log (string.Format("event:{0}", currentEvent.ToString()));

			var eventType = currentEvent.type;

			if (eventType == EventType.MouseDrag && currentEvent.button == 0) {

				if (currentObject != null) {
					DragAndDrop.PrepareStartDrag ();

					DragAndDrop.StartDrag (currentObject.name);

					DragAndDrop.objectReferences = new Object[] { currentObject };

//					if (ProjectWindowUtil.IsFolder(currentObject.GetInstanceID())) {

					// fixed to use IsPersistent to work with all assets with paths.
					if (EditorUtility.IsPersistent(currentObject)) {

						// added DragAndDrop.path in case we are dragging a folder.

						DragAndDrop.paths = new string[] {
							AssetDatabase.GetAssetPath(currentObject)
						};

						// previous test with setting generic data by looking at
						// decompiled Unity code.

						// DragAndDrop.SetGenericData ("IsFolder", "isFolder");
					}
				}

				Event.current.Use ();

			} else if (eventType == EventType.MouseUp) {

				if (currentObject != null) {
					if (Event.current.button == 0) {
						UpdateSelection (currentObject);
					} else {
						EditorGUIUtility.PingObject (currentObject);
					}
				}

				Event.current.Use ();
			}

		}

	}
}