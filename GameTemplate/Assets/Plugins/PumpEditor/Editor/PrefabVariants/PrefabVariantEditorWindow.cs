#if UNITY_2018_3_OR_NEWER

using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace PumpEditor
{
    // Implementation is based on SimpleTreeViewWindow example
    // from TreeView manual: https://docs.unity3d.com/Manual/TreeViewAPI.html
    public class PrefabVariantEditorWindow : EditorWindow
    {
        [SerializeField]
        private TreeViewState treeViewState;

        private PrefabVariantTreeView treeView;
        private SearchField searchField;

        [MenuItem("Window/Pump Editor/Prefab Variants")]
        private static void ShowWindow()
        {
            var window = GetWindow<PrefabVariantEditorWindow>();
            var icon = EditorGUIUtility.FindTexture("PrefabVariant Icon");
            window.titleContent = new GUIContent("Prefab Variants", icon);
            window.Show();
        }

        public void ReloadTreeView()
        {
            treeView.Reload();
        }

        private void DoToolbar()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Space(100);
            GUILayout.FlexibleSpace();
            treeView.searchString = searchField.OnToolbarGUI(treeView.searchString);
            GUILayout.EndHorizontal();
        }

        private void DoTreeView()
        {
            var rect = GUILayoutUtility.GetRect(0, 100000, 0, 100000);
            treeView.OnGUI(rect);
        }

        private void DoAssetInfo()
        {
            var selectedIDs = treeViewState.selectedIDs;
            if (selectedIDs.Count == 0)
            {
                return;
            }

            Debug.Assert(selectedIDs.Count == 1);

            var item = treeView.FindItem(selectedIDs[0]);

            // If asset is deleted, selectedIDs are not reset so need to
            // check that find item by id succeeded.
            if (item == null)
            {
                return;
            }

            var asset = item.PrefabAsset;

            GUILayout.BeginVertical(Styles.inspectorBigTitleInner);
            EditorGUILayout.Space();

            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUI.DisabledGroupScope(true))
                {
                    EditorGUILayout.ObjectField("Prefab Asset", asset, typeof(GameObject), false);
                }
            }

            var variantBase = PrefabUtility.GetCorrespondingObjectFromSource(asset);
            if (variantBase != null)
            {
                using (new EditorGUI.DisabledGroupScope(true))
                {
                    EditorGUILayout.ObjectField("Base", variantBase, typeof(GameObject), false);
                }
            }

            EditorGUILayout.Space();
            GUILayout.EndVertical();
        }

        private void OnEnable()
        {
            // Check if we already had a serialized view state (state
            // that survived assembly reloading)
            if (treeViewState == null)
            {
                treeViewState = new TreeViewState();
            }

            treeView = new PrefabVariantTreeView(treeViewState);
            searchField = new SearchField();
            searchField.downOrUpArrowKeyPressed += treeView.SetFocusAndEnsureSelectedItem;
        }

        private void OnGUI()
        {
            if (treeView == null)
                return;

            DoToolbar();
            DoTreeView();
            DoAssetInfo();
        }

        private static class Styles
        {
            public static readonly GUIStyle inspectorBigTitleInner = "IN BigTitle inner";
        }
    }
}

#endif
