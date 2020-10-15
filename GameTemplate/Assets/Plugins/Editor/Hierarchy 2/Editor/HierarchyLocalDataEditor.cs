using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Hierarchy2
{
    [CustomEditor(typeof(HierarchyLocalData))]
    public class HierarchyLocalDataEditor : Editor
    {
        private HierarchyLocalData hld;
        
        private void OnEnable()
        {
            hld = target as HierarchyLocalData;
        }

        public override void OnInspectorGUI()
        {
            if (!hld.gameObject.CompareTag("EditorOnly"))
                hld.gameObject.tag = "EditorOnly";
            
            EditorGUILayout.HelpBox("Holding reference custom data of row item on hierarchy\nExclude in build.", MessageType.Info);
            EditorGUILayout.BeginVertical("box");
            base.OnInspectorGUI();
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("ClearNullRef"))
            {
                hld.ClearNullRef();
            }
        }
    }
}

