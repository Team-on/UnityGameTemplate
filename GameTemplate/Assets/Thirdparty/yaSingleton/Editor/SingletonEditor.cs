using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using yaSingleton.Utility;

namespace yaSingleton.Editor {
    /// <summary>
    /// Fallback Singleton Inspector - validates the singleton location. Extend this class if you want the validation in your singletons.
    /// Note: The editor targets ScriptableObjects due BaseSingleton being abstract (no custom inspectors for abstract types).
    /// </summary>
    [CustomEditor(typeof(BaseSingleton), true, isFallback = true)]
    public class SingletonEditor : UnityEditor.Editor {
        protected bool isSingletonEditor;

        /// <summary>
        /// Disable in inherited editors to draw the validation at any point of your code.
        /// </summary>
        protected bool autoDrawSingletonValidation = true;

        protected ScriptableObject[] duplicateSingletons = null;

        protected bool HasDuplicates {
            get { return duplicateSingletons != null && duplicateSingletons.Length > 1; }
        }

        protected virtual void OnEnable() {
            isSingletonEditor = target && target.GetType().IsSubclassOf(typeof(BaseSingleton));

            if(!isSingletonEditor) {
                return;
            }

            duplicateSingletons = GetAllScriptableObjects(target.GetType());
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            if(autoDrawSingletonValidation) {
                DrawSingletonValidation();
            }
        }

        protected void DrawSingletonValidation() {
            if(!isSingletonEditor) {
                return;
            }

            if(HasDuplicates) {
                EditorGUILayout.HelpBox(
                    "There are duplicate " + target.GetType().Name +
                    " (Singleton) instances. This can lead to unexpected results.",
                    MessageType.Warning, true);
                EditorGUILayout.LabelField("Duplicate " + target.GetType().Name + " Singletons:");
                GUI.enabled = false;
                for(var i = 0; i < duplicateSingletons.Length; ++i) {
                    var duplicateSingleton = duplicateSingletons[i];
                    EditorGUILayout.ObjectField(duplicateSingleton.name, duplicateSingleton, target.GetType(), false);
                }
                GUI.enabled = true;
            }
        }

        public static ScriptableObject[] GetAllScriptableObjects(Type scriptableObjectType) {
            string[] assets = AssetDatabase.FindAssets("t:" + scriptableObjectType.Name);

            return assets.Select(id => AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetDatabase.GUIDToAssetPath(id))).ToArray();
        }
    }
}