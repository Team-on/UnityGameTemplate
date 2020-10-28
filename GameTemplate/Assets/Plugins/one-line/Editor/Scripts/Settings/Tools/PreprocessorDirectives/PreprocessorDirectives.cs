using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace OneLine.Settings {
    public class PreprocessorDirectives {
        private Dictionary<string, Nullable<bool>> items = 
            new Dictionary<string, Nullable<bool>>();

        public void add(string key, Nullable<bool> get) {
            if (string.IsNullOrEmpty(key) || get == null) {
                throw new ArgumentNullException();
            } 
            if (items.ContainsKey(key)) {
                throw new ArgumentException(string.Format("Key {0} is already registered", key));
            }
            items[key] = get;
        }

        public void add(string key, TernaryBoolean get) {
            if (get == null) {
                throw new ArgumentNullException();
            }
            items[key] = get.BoolValue;
        }

        public void DefineForCurrentBuildTarget() {
            BuildTargetGroup target = EditorUserBuildSettings.selectedBuildTargetGroup;
            if (target == BuildTargetGroup.Unknown) {
                Debug.LogError("OneLine Settings Error: can not determine current BuildTargetGroup");
                return;
            }

            var currentDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);
            var resultDefines = new List<string>();
            foreach (var define in currentDefines.Split(';')) {
                if (!string.IsNullOrEmpty(define)) {
                    if (!items.ContainsKey(define)){
                        resultDefines.Add(define);
                    }
                }
            }

            foreach (var key in items.Keys) {
                var value = items[key];
                if (value.HasValue && value.Value) {
                    resultDefines.Add(key);
                }
            }

            PlayerSettings.SetScriptingDefineSymbolsForGroup(target, string.Join(";", resultDefines.ToArray()));
        }
    }
}