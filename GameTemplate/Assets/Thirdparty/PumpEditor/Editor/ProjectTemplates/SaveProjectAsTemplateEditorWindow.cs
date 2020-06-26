#if UNITY_2018_1_OR_NEWER

using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace PumpEditor
{
    public class SaveProjectAsTemplateEditorWindow : EditorWindow
    {
        private static readonly string UnityEditorApplicationProjectTemplatesPath = Path.Combine(
            Path.GetDirectoryName(EditorApplication.applicationPath),
            "Data",
            "Resources",
            "PackageManager",
            "ProjectTemplates"
        );

        private static readonly string[] TemplateFolderNames = new string[]
        {
            "Assets",
            "Packages",
            "ProjectSettings",
        };

        private string targetPath;
        private string templateName;
        private string templateDisplayName;
        private string templateDescription;
        private string templateDefaultScene;
        private string templateVersion;
        private SceneAsset templateDefaultSceneAsset;
        private bool replaceTemplate;

        [MenuItem("Window/Pump Editor/Project Templates/Save Project As Template")]
        private static void ShowWindow()
        {
            var window = EditorWindow.GetWindow<SaveProjectAsTemplateEditorWindow>();
            var icon = EditorGUIUtility.Load("saveas@2x") as Texture2D;
            window.titleContent = new GUIContent("Save Project As Template", icon);
            window.Show();
        }

        private void DeleteTemplateFolders()
        {
            try
            {
                foreach (var tempateFolderName in TemplateFolderNames)
                {
                    var path = Path.Combine(targetPath, tempateFolderName);
                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path, true);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("Failed to delete template folders, exception: {0}", e.Message);
            }
        }

        // Use 2018.2 references as 2018.1 does not have this method (but it's called from 2018.1 UnityEditor.ProjectTemplateWindow).
        private void InvokeSaveProjectAsTemplate()
        {
            // Unity C# reference: https://github.com/Unity-Technologies/UnityCsReference/blob/4aea4dc4/Editor/Mono/EditorUtility.bindings.cs#L16
            Assembly editorAssembly = Assembly.GetAssembly(typeof(Editor));
            Type editorUtilityType = editorAssembly.GetType("UnityEditor.EditorUtility");

            // Unity C# reference: https://github.com/Unity-Technologies/UnityCsReference/blob/4aea4dc4/Editor/Mono/EditorUtility.bindings.cs#L172
            MethodInfo methodInfo = editorUtilityType.GetMethod("SaveProjectAsTemplate", BindingFlags.Static | BindingFlags.NonPublic);
            methodInfo.Invoke(editorUtilityType, new object[]{ targetPath, templateName, templateDisplayName, templateDescription, templateDefaultScene, templateVersion});
        }

        private void DeleteProjectVersionTxt()
        {
            var projectVersionTxtPath = Path.Combine(targetPath, "ProjectSettings", "ProjectVersion.txt");

            if (File.Exists(projectVersionTxtPath))
            {
                File.Delete(projectVersionTxtPath);
            }
            else
            {
                Debug.LogErrorFormat("File ProjectVersion.txt does not exist at path: {0}", projectVersionTxtPath);
            }
        }

        private void SetTemplateDataFromPackageJson()
        {
            var packageJsonPath = Path.Combine(targetPath, "package.json");
            if (File.Exists(packageJsonPath))
            {
                var packageJson = File.ReadAllText(packageJsonPath);
                var templateData = JsonUtility.FromJson<TemplateData>(packageJson);
                templateName = templateData.Name;
                templateDisplayName = templateData.DisplayName;
                templateDescription = templateData.Description;
                templateDefaultScene = templateData.DefaultScene;
                templateVersion = templateData.Version;
                SetDefaultSceneAssetFromPath();
            }
        }

        private void SetDefaultSceneAssetFromPath()
        {
            if (templateDefaultScene != String.Empty)
            {
                templateDefaultSceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(templateDefaultScene);
                Debug.AssertFormat(templateDefaultSceneAsset != null, "Failed to load scene asset at path from package.json, path: {0}", templateDefaultScene);
            }
            else
            {
                templateDefaultSceneAsset = null;
            }
        }

        private void DefaultSceneGUI()
        {
            templateDefaultSceneAsset = (SceneAsset)EditorGUILayout.ObjectField("Default scene asset:", templateDefaultSceneAsset, typeof(SceneAsset), false);
            if (templateDefaultSceneAsset != null)
            {
                templateDefaultScene = AssetDatabase.GetAssetPath(templateDefaultSceneAsset);
            }
            else
            {
                templateDefaultScene = null;
            }

            using (new EditorGUI.DisabledGroupScope(true))
            {
                EditorGUILayout.TextField("Default scene:", templateDefaultScene);
            }
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Select Target Folder"))
            {
                targetPath = EditorUtility.SaveFolderPanel("Choose target folder", UnityEditorApplicationProjectTemplatesPath, String.Empty);
                SetTemplateDataFromPackageJson();
            }

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                targetPath = EditorGUILayout.TextField("Path:", targetPath);
                if (check.changed)
                {
                    SetTemplateDataFromPackageJson();
                }
            }

            templateName = EditorGUILayout.TextField("Name:", templateName);
            templateDisplayName = EditorGUILayout.TextField("Display name:", templateDisplayName);
            templateDescription = EditorGUILayout.TextField("Description:", templateDescription);
            DefaultSceneGUI();
            templateVersion = EditorGUILayout.TextField("Version:", templateVersion);
            replaceTemplate = EditorGUILayout.Toggle("Replace template:", replaceTemplate);

            if (GUILayout.Button("Save"))
            {
                if (replaceTemplate)
                {
                    DeleteTemplateFolders();
                }
                AssetDatabase.SaveAssets();
                InvokeSaveProjectAsTemplate();
                DeleteProjectVersionTxt();
            }
        }

        [Serializable]
        private class TemplateData
        {
#pragma warning disable 0649
            [SerializeField]
            private string name;
            [SerializeField]
            private string displayName;
            [SerializeField]
            private string description;
            [SerializeField]
            private string defaultScene;
            [SerializeField]
            private string version;
#pragma warning restore 0649

            public string Name { get { return name; } }
            public string DisplayName { get { return displayName; } }
            public string Description { get { return description; } }
            public string DefaultScene { get { return defaultScene; } }
            public string Version { get { return version; } }
        }
    }
}

#endif
