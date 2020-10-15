using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PrefabAssetUtility.Editor
{
    internal static class PrefabAssetSettings
    {
        public const string STORE_CACHE_ON_PREFAB_CHANGE = "StoreCacheOnPrefabChange";
        public const string LAZY_LOAD_PREFAB_CACHE = "LazyLoadPrefabCache";

        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            SettingsProvider provider = new SettingsProvider("Preferences/Prefab Asset Cache", SettingsScope.User)
            {
                label = "Prefab Asset Cache",
                guiHandler = searchContext =>
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Lazy load cache on first use:");
                    EditorPrefs.SetBool(LAZY_LOAD_PREFAB_CACHE,
                        EditorGUILayout.Toggle(EditorPrefs.GetBool(LAZY_LOAD_PREFAB_CACHE, true)));
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Save cache on prefab change:");
                    EditorPrefs.SetBool(STORE_CACHE_ON_PREFAB_CHANGE,
                        EditorGUILayout.Toggle(EditorPrefs.GetBool(STORE_CACHE_ON_PREFAB_CHANGE)));
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.LabelField("Cache will always be stored when recompiling or exiting Unity");
                    if (GUILayout.Button("Force refresh cache"))
                    {
                        PrefabUtils.RefreshPrefabCache();
                    }
                },
                keywords = new HashSet<string>(new[] {"prefab", "asset", "cache", "component", "guid"})
            };

            return provider;
        }
    }
}
