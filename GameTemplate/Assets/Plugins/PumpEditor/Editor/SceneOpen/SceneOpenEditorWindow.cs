using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace PumpEditor
{
    public class SceneOpenEditorWindow : EditorWindow
    {
        private const int ALL_SCENES_TOOLBAR_INDEX = 0;
        private const int BUILD_SCENES_TOOLBAR_INDEX = 1;

        private static readonly string[] TOOLBAR_STRINGS = new string[2]
        {
            "All Scenes",
            "Build Scenes",
        };

        private bool showAdvancedMode;
        private int toolbarIndex;
        private Vector2 windowScrollPosition;

        [MenuItem("Window/Pump Editor/Scene Open")]
        private static void Init()
        {
            var window = EditorWindow.GetWindow<SceneOpenEditorWindow>();
            var icon = EditorGUIUtility.Load("buildsettings.editor.small") as Texture2D;
            window.titleContent = new GUIContent("Scenes", icon);
            window.Show();
        }

        private static void OpenSceneButtonsGUI(IEnumerable<string> scenePaths)
        {
            if (scenePaths.Count() == 0)
            {
                EditorGUILayout.HelpBox("No scenes to open.", MessageType.Info);
                return;
            }

            foreach (var scenePath in scenePaths)
            {
                if (GUILayout.Button(scenePath, PumpEditorStyles.ButtonTextMiddleLeft))
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(scenePath);
                    }
                }
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.Space();
            showAdvancedMode = EditorGUILayout.Toggle("Show Advanced Mode", showAdvancedMode);
            EditorGUILayout.Space();

            if (showAdvancedMode)
            {
                toolbarIndex = GUILayout.Toolbar(toolbarIndex, TOOLBAR_STRINGS);
                switch (toolbarIndex)
                {
                    case ALL_SCENES_TOOLBAR_INDEX:
                        ScenesInProjectGUI();
                        break;
                    case BUILD_SCENES_TOOLBAR_INDEX:
                        ScenesInBuildSettingsGUI();
                        break;
                }
            }
            else
            {
                ScenesInProjectGUI();
            }

            EditorGUILayout.EndVertical();
        }

        private void ScenesInProjectGUI()
        {
            EditorGUILayout.LabelField("Scenes In Project", EditorStyles.boldLabel);
            windowScrollPosition = EditorGUILayout.BeginScrollView(windowScrollPosition);

            var sceneAssetGuids = AssetDatabase.FindAssets("t:scene");
            var scenePaths = sceneAssetGuids.Select(sceneAssetGuid =>
            {
                return AssetDatabase.GUIDToAssetPath(sceneAssetGuid);
            });
            OpenSceneButtonsGUI(scenePaths);

            EditorGUILayout.EndScrollView();
        }

        private void ScenesInBuildSettingsGUI()
        {
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
    }
}
