using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using yaSingleton.Helpers;

namespace yaSingleton.Editor {
    [CustomEditor(typeof(SingletonUpdater), true)]
    public class SingletonUpdaterEditor : UnityEditor.Editor {
        
        private Dictionary<int, UnityEditor.Editor> _editors;

        private Dictionary<int, bool> _foldouts;
        
        private static GUIStyle _boldLabel;
        
        public static GUIStyle BoldLabel {
            get {
                if(_boldLabel == null) {
                    _boldLabel = new GUIStyle("BoldLabel");
                }
                
                return _boldLabel;
            }
        }

        public void OnEnable() {
            _editors = new Dictionary<int, UnityEditor.Editor>();
            
            _foldouts = new Dictionary<int, bool>();
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            
            EditorGUILayout.LabelField("Singletons", BoldLabel);
            
            for(int i = 0; i < BaseSingleton.AllSingletons.Count; ++i) {
                if(!_editors.ContainsKey(i)) {
                    _editors.Add(i, null);
                }

                if(!_foldouts.ContainsKey(i)) {
                    _foldouts.Add(i, true);
                }
                
                var singleton = BaseSingleton.AllSingletons[i];

                _foldouts[i] = EditorGUILayout.InspectorTitlebar(_foldouts[i], singleton);
                
                if(_foldouts[i]) {
                    var editor = _editors[i];

                    CreateCachedEditor(singleton, null, ref editor);

                    _editors[i] = editor;
                
                    EditorGUI.indentLevel += 1;
                    editor.OnInspectorGUI();
                    
                    EditorGUILayout.Space();
                    
                    if(AssetDatabase.Contains(singleton)) {
                        EditorGUILayout.ObjectField("Reference", singleton, singleton.GetType(), false);
                    } else {
                        EditorGUILayout.HelpBox("Runtime generated", MessageType.Info);
                    }
                    
                    EditorGUI.indentLevel -= 1;
                }
            }
            
            
        }
    }
}