using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace PumpEditor {
	public class SceneOpenBuildEditorWindow : EditorWindow {
        private Vector2 windowScrollPosition;
       
        [MenuItem("Window/Custom/Scenes/Build list")]
        private static void Init() {
            var window = EditorWindow.GetWindow(typeof(SceneOpenBuildEditorWindow), false, "Scenes build", true);
            window.titleContent = new GUIContent("Scenes build", EditorGUIUtility.Load("buildsettings.editor.small") as Texture2D);
        }

        private void OnGUI() {
            EditorGUILayout.BeginVertical();
            ScenesInBuildSettingsGUI();
            EditorGUILayout.EndVertical();
        }

        protected void ScenesInBuildSettingsGUI() {
            EditorGUILayout.LabelField("Scenes In Build Settings", EditorStyles.boldLabel);
            windowScrollPosition = EditorGUILayout.BeginScrollView(windowScrollPosition);

            // Though Unity documentations states that EditorBuildSettingsScene
            // path property returns file path as listed in build settings window,
            // this is not true. In build settings scene path is listed without
            // Assets folder at path start and without .unity extension. But path
            // property returns full project path like Assets/Scenes/MyScene.unity
            var scenePaths = EditorBuildSettings.scenes.Select(s => s.path);
            OpenSceneButtonsGUI(scenePaths);

            EditorGUILayout.EndScrollView();
        }

        private static void OpenSceneButtonsGUI(IEnumerable<string> scenePaths) {
            if (scenePaths.Count() == 0) {
                EditorGUILayout.HelpBox("No scenes to open.", MessageType.Info);
                return;
            }

            foreach (var scenePath in scenePaths) {
                if (GUILayout.Button(scenePath, PumpEditorStyles.ButtonTextMiddleLeft)) {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
                        EditorSceneManager.OpenScene(scenePath);
                    }
                }
            }
        }
    }
}
