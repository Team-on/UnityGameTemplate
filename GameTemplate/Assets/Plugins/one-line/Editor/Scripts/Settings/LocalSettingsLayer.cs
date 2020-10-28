using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace OneLine.Settings {
    public class LocalSettingsLayer : ISettings {

        private Dictionary<string, TernaryBoolean> booleans = new Dictionary<string, TernaryBoolean>();

        private const string ENABLED_NAME = "ONE_LINE_SETTINGS_ENABLED";
        private const string DRAW_VERTICAL_SEPARATOR_NAME = "ONE_LINE_SETTINGS_DRAW_VERTICAL_SEPARATOR";
        private const string DRAW_HORIZONTAL_SEPARATOR_NAME = "ONE_LINE_SETTINGS_DRAW_HORIZONTAL_SEPARATOR";
        private const string EXPANDABLE_NAME = "ONE_LINE_SETTINGS_EXPANDABLE";
        private const string CUSTOM_DRAWER_NAME = "ONE_LINE_SETTINGS_CUSTOM_DRAWER";
        private const string CULLING_OPTIMIZATION_NAME = "ONE_LINE_SETTINGS_CULLING_OPTIMIZATION";
        private const string CACHE_OPTIMIZATION_NAME = "ONE_LINE_SETTINGS_CACHE_OPTIMIZATION";

        public TernaryBoolean Enabled { get { return getBool(ENABLED_NAME); } }
        public TernaryBoolean DrawVerticalSeparator { get { return getBool(DRAW_VERTICAL_SEPARATOR_NAME); } }
        public TernaryBoolean DrawHorizontalSeparator { get { return getBool(DRAW_HORIZONTAL_SEPARATOR_NAME); } }
        public TernaryBoolean Expandable { get { return getBool(EXPANDABLE_NAME); } }
        public TernaryBoolean CustomDrawer { get { return getBool(CUSTOM_DRAWER_NAME); } }
        public TernaryBoolean CullingOptimization { get { return getBool(CULLING_OPTIMIZATION_NAME); } }
        public TernaryBoolean CacheOptimization { get { return getBool(CACHE_OPTIMIZATION_NAME); } }

        private TernaryBoolean getBool(string key) {
#if ONE_LINE_DEFAULTS_ONLY
            return new TernaryBoolean(null);
#else
            TernaryBoolean result = null;
            if (!booleans.TryGetValue(key, out result)) {
                result = new TernaryBoolean((byte) EditorPrefs.GetInt(key, 0));
                booleans[key] = result;
            }
            return result;
#endif
        }

        public void Save() {
            foreach (var key in booleans.Keys) {
                Save(key, booleans[key]);
            }
        }

        private void Save(string key, TernaryBoolean value) {
            if (value != null) {
                EditorPrefs.SetInt(key, value.RawValue);
            }
        }
    }
}