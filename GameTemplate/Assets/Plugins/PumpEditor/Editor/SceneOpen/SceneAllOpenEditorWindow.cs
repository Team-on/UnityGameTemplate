using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace PumpEditor {
	public class SceneOpenAllEditorWindow : EditorWindow {
        private Vector2 windowScrollPosition;
       
        [MenuItem("Window/Custom/Scenes/All list")]
        private static void Init() {
            var window = EditorWindow.GetWindow<SceneOpenAllEditorWindow>();
            var icon = EditorGUIUtility.Load("buildsettings.editor.small") as Texture2D;
            window.titleContent = new GUIContent("Scenes all", icon);
            window.Show();
        }

        private void OnGUI() {
            EditorGUILayout.BeginVertical();
            ScenesInProjectGUI();
            EditorGUILayout.EndVertical();
        }

        protected void ScenesInProjectGUI() {
            EditorGUILayout.LabelField("Scenes In Project", EditorStyles.boldLabel);
            windowScrollPosition = EditorGUILayout.BeginScrollView(windowScrollPosition);

            var sceneAssetGuids = AssetDatabase.FindAssets("t:scene");
            var scenePaths = sceneAssetGuids.Select(sceneAssetGuid => {
                return AssetDatabase.GUIDToAssetPath(sceneAssetGuid);
            });
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
