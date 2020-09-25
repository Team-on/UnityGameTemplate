using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

namespace Hierarchy2
{
    [CustomEditor(typeof(HierarchyFolder))]
    internal class HierarchyFolderEditor : Editor
    {
        private void OnEnable()
        {

        }

        public override VisualElement CreateInspectorGUI()
        {
            var script = target as HierarchyFolder;

            var root = new VisualElement();

            IMGUIContainer imguiContainer = new IMGUIContainer(() =>
            {
                script.flattenMode = (HierarchyFolder.FlattenMode) EditorGUILayout.EnumPopup("Flatten Mode", script.flattenMode);
                if (script.flattenMode != HierarchyFolder.FlattenMode.None)
                {
                    script.flattenSpace = (HierarchyFolder.FlattenSpace) EditorGUILayout.EnumPopup("Flatten Space", script.flattenSpace);
                    script.destroyAfterFlatten = EditorGUILayout.Toggle("Destroy After Flatten", script.destroyAfterFlatten);
                }
            });
            root.Add(imguiContainer);
            
            return root;
        }

        [MenuItem("GameObject/Hierarchy Folder", priority = 0)]
        static void CreateInstance() => new GameObject("Folder", new Type[1] {typeof(HierarchyFolder)});
    }
}