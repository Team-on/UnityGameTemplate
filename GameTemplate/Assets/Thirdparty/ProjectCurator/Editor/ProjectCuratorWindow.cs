using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Nanolabo
{
    public class ProjectCuratorWindow : EditorWindow, IHasCustomMenu
    {

        [MenuItem("Window/Project Curator")]
        static void Init()
        {
            GetWindow<ProjectCuratorWindow>("Project Curator");
        }

        public ProjectCuratorWindow()
        {
            Selection.selectionChanged += OnSelectionChanged;
        }

        private void OnSelectionChanged()
        {
            Repaint();
        }

        private Vector2 scroll;

        private bool dependenciesOpen = true;
        private bool referencesOpen = true;

        private static GUIStyle titleStyle;
        private static GUIStyle TitleStyle => titleStyle ?? (titleStyle = new GUIStyle(EditorStyles.label) { fontSize = 13 });

        private static GUIStyle itemStyle;
        private static GUIStyle ItemStyle => itemStyle ?? (itemStyle = new GUIStyle(EditorStyles.label) { margin = new RectOffset(32, 0, 0, 0) });

        private void OnGUI()
        {
            string selectedPath = AssetDatabase.GetAssetPath(UnityEditor.Selection.activeObject);
            if (string.IsNullOrEmpty(selectedPath))
                return;

            // Standard spacing to mimic Unity's Inspector header
            GUILayout.Space(2);

            Rect rect;

            GUILayout.BeginHorizontal("In BigTitle");
            GUILayout.Label(AssetDatabase.GetCachedIcon(selectedPath), GUILayout.Width(36), GUILayout.Height(36));
            GUILayout.BeginVertical();
            GUILayout.Label(Path.GetFileName(selectedPath), TitleStyle);
            // Display directory (without "Assets/" prefix)
            GUILayout.Label(Regex.Match(Path.GetDirectoryName(selectedPath), "(\\\\.*)$").Value);
            rect = GUILayoutUtility.GetLastRect();
            GUILayout.EndVertical();
            GUILayout.Space(44);
            GUILayout.EndHorizontal();

            if (Directory.Exists(selectedPath))
                return;

            AssetInfo selectedAssetInfo = ProjectCurator.GetAsset(selectedPath);
            if (selectedAssetInfo == null) {
                bool rebuildClicked = HelpBoxWithButton(new GUIContent("You must rebuild database to obtain information on this asset", EditorGUIUtility.IconContent("console.warnicon").image), new GUIContent("Rebuild Database"));
                if (rebuildClicked) {
                    ProjectCurator.RebuildDatabase();
                }
                return;
            }

            var content = new GUIContent(selectedAssetInfo.IsIncludedInBuild ? ProjectIcons.LinkBlue : ProjectIcons.LinkBlack, selectedAssetInfo.IncludedStatus.ToString());
            GUI.Label(new Rect(position.width - 20, rect.y + 1, 16, 16), content);

            scroll = GUILayout.BeginScrollView(scroll);

            dependenciesOpen = EditorGUILayout.Foldout(dependenciesOpen, $"Dependencies ({selectedAssetInfo.dependencies.Count})");
            if (dependenciesOpen) {
                foreach (var dependency in selectedAssetInfo.dependencies) {
                    if (GUILayout.Button(Path.GetFileName(dependency), ItemStyle)) {
                        UnityEditor.Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(dependency);
                    }
                    rect = GUILayoutUtility.GetLastRect();
                    GUI.DrawTexture(new Rect(rect.x - 16, rect.y, rect.height, rect.height), AssetDatabase.GetCachedIcon(dependency));
                    AssetInfo depInfo = ProjectCurator.GetAsset(dependency);
                    content = new GUIContent(depInfo.IsIncludedInBuild ? ProjectIcons.LinkBlue : ProjectIcons.LinkBlack, depInfo.IncludedStatus.ToString());
                    GUI.Label(new Rect(rect.width + rect.x - 20, rect.y + 1, 16, 16), content);
                }
            }

            GUILayout.Space(6);

            referencesOpen = EditorGUILayout.Foldout(referencesOpen, $"Referencers ({selectedAssetInfo.referencers.Count})");
            if (referencesOpen) {
                foreach (var referencer in selectedAssetInfo.referencers) {
                    if (GUILayout.Button(Path.GetFileName(referencer), ItemStyle)) {
                        UnityEditor.Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(referencer);
                    }
                    rect = GUILayoutUtility.GetLastRect();
                    GUI.DrawTexture(new Rect(rect.x - 16, rect.y, rect.height, rect.height), AssetDatabase.GetCachedIcon(referencer));
                    AssetInfo refInfo = ProjectCurator.GetAsset(referencer);
                    content = new GUIContent(refInfo.IsIncludedInBuild ? ProjectIcons.LinkBlue : ProjectIcons.LinkBlack, refInfo.IncludedStatus.ToString());
                    GUI.Label(new Rect(rect.width + rect.x - 20, rect.y + 1, 16, 16), content);
                }
            }

            GUILayout.Space(5);

            GUILayout.EndScrollView();

            if (!selectedAssetInfo.IsIncludedInBuild) {
                bool deleteClicked = HelpBoxWithButton(new GUIContent("This asset is not referenced and never used. Would you like to delete it ?", EditorGUIUtility.IconContent("console.warnicon").image), new GUIContent("Delete Asset"));
                if (deleteClicked) {
                    File.Delete(selectedPath);
                    AssetDatabase.Refresh();
                    ProjectCurator.RemoveAssetFromDatabase(selectedPath);
                }
            }
        }

        void IHasCustomMenu.AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(new GUIContent("Rebuild Database"), false, ProjectCurator.RebuildDatabase);
            menu.AddItem(new GUIContent("Clear Database"), false, ProjectCurator.ClearDatabase);
            menu.AddItem(new GUIContent("Project Overlay"), ProjectWindowOverlay.Enabled, () => { ProjectWindowOverlay.Enabled = !ProjectWindowOverlay.Enabled; });
        }

        public bool HelpBoxWithButton(GUIContent messageContent, GUIContent buttonContent)
        {
            float buttonWidth = buttonContent.text.Length * 8;
            const float buttonSpacing = 5f;
            const float buttonHeight = 18f;

            // Reserve size of wrapped text
            Rect contentRect = GUILayoutUtility.GetRect(messageContent, EditorStyles.helpBox);
            // Reserve size of button
            GUILayoutUtility.GetRect(1, buttonHeight + buttonSpacing);

            // Render background box with text at full height
            contentRect.height += buttonHeight + buttonSpacing;
            GUI.Label(contentRect, messageContent, EditorStyles.helpBox);

            // Button (align lower right)
            Rect buttonRect = new Rect(contentRect.xMax - buttonWidth - 4f, contentRect.yMax - buttonHeight - 4f, buttonWidth, buttonHeight);
            return GUI.Button(buttonRect, buttonContent);
        }
    }
}