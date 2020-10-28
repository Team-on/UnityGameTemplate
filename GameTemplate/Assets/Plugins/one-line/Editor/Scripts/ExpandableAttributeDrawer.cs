using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using OneLine;
using RectEx;
using System;

namespace OneLine {
#if ! ONE_LINE_EXPANDABLE_DISABLE
    [CustomPropertyDrawer(typeof(ExpandableAttribute), true)]
#endif
    public class ExpandableAttributeDrawer : PropertyDrawer {

        private static readonly float FOLDOUT_WIDTH = 14;

        private new ExpandableAttribute attribute { get {return base.attribute as ExpandableAttribute; } }

        private GUIStyle foldoutStyle;
    
        public override void OnGUI (Rect rect, SerializedProperty property, GUIContent label) {
            rect = EditorGUI.PrefixLabel(rect, label);

            var rects = rect.CutFromLeft(FOLDOUT_WIDTH, 0);
    
            EditorGUI.ObjectField(rects[1], property, GUIContent.none);
    
            if (property.objectReferenceValue == null) return;
        
            DrawFoldout(rects[0], rects[1], property);
        }

        private void DrawFoldout(Rect foldoutRect, Rect propertyRect, SerializedProperty property){
            if (foldoutStyle == null) {
                foldoutStyle = new GUIStyle(EditorStyles.foldout);
                foldoutStyle.active = foldoutStyle.normal;
            }

            if (GUI.Button(foldoutRect, "", foldoutStyle)){
                var content = new ExpandedObjectWindow(propertyRect, property, attribute.ReadOnly);
                PopupWindow.Show(propertyRect.MoveDownFor(0), content);
            }
        }

        public class ExpandedObjectWindow : PopupWindowContent {

            private static readonly float MIN_WINDOW_WIDTH = 400;
            private float max_window_height;

            private SerializedProperty property;
            private bool isReadOnly;

            private Rect contentRect;
            private Vector2 scrollPosition;
            private Rect windowRect;

            public ExpandedObjectWindow(Rect rect, SerializedProperty property, bool isReadOnly){
                this.property = property;
                this.isReadOnly = isReadOnly;

                this.scrollPosition = Vector2.zero;
                this.windowRect = new Rect(0,0, Math.Max(rect.width, MIN_WINDOW_WIDTH), 100);
                this.contentRect = new Rect(0, 0, rect.width - 20, 0);

                this.max_window_height = Screen.height;
            }

            public override Vector2 GetWindowSize() {
                return windowRect.size;
            }

            public override void OnGUI(Rect rect) {
                EditorGUI.DrawRect(rect, Color.gray);

                Editor editor = null;
                Editor.CreateCachedEditor(property.objectReferenceValue, null, ref editor);

                EditorGUI.BeginDisabledGroup(isReadOnly);
                if (editor != null){
                    DrawCustomEditor(editor);
                }
                else {
                    var message = "Can not find editor for type {0}. Drawing manually.\nPlease create an issue at https://github.com/slavniyteo/one-line and we will repair it.";
                    Debug.LogWarning(String.Format(message, property.objectReferenceValue.GetType()));

                    DrawExpandedObject(rect, new SerializedObject(property.objectReferenceValue));
                }
                EditorGUI.EndDisabledGroup();
            }

            #region Draw Custom Editor

            private void DrawCustomEditor(Editor editor) {
                scrollPosition = GUILayout.BeginScrollView(scrollPosition);

                var layoutRect = EditorGUILayout.BeginVertical();
                EditorGUI.BeginChangeCheck();
                editor.OnInspectorGUI();
                if (EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(property.objectReferenceValue);
                EditorGUILayout.EndVertical();

                GUILayout.EndScrollView();

                if (Event.current.type != EventType.Layout) {
                    windowRect.height = layoutRect.height + 4;
                }
            }

            #endregion

            #region Draw Manually

            private void DrawExpandedObject(Rect rect, SerializedObject target) {
                if (target == null) return; 

                scrollPosition = GUI.BeginScrollView(windowRect, scrollPosition, contentRect);
                if (contentRect.height > windowRect.height){
                    rect = rect.CutFromRight(10)[0];
                }

                rect = rect.Intend(5).FirstLine();
                DrawChildren(rect, target);

                GUI.EndScrollView();
        
                target.ApplyModifiedProperties();
            }

            private void DrawChildren(Rect rect, SerializedObject target){
                var property = target.GetIterator();
                property.NextVisible (true);

                if (property.name == "m_Script"){
                    DrawScriptReference(rect, property);
                    rect = rect.MoveDown();
                    property.NextVisible(false);
                }
                
                do {
                    rect.height = EditorGUI.GetPropertyHeight(property, GUIContent.none, true);
                    EditorGUI.PropertyField (rect, property, true);
                    rect.y += rect.height;
                }
                while (property.NextVisible (false));

                contentRect.height = rect.y + 16;
                windowRect.height = Math.Min(contentRect.height, max_window_height);
            }

            private static void DrawScriptReference(Rect rect, SerializedProperty property){
                EditorGUI.BeginDisabledGroup (true);
                EditorGUI.PropertyField (rect, property, false);
                EditorGUI.EndDisabledGroup ();
            }

            #endregion
        }
    }
}
