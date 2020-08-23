#if UNITY_2018_3_OR_NEWER

using System;
using UnityEditor;
using UnityEngine;

namespace PumpEditor
{
    public class PrefabAssetPostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssetPaths, string[] deletedAssetPaths, string[] movedAssetPaths, string[] movedFromAssetPaths)
        {
            var importedPrefabIndex = Array.FindIndex(importedAssetPaths, IsPrefab);

            // TODO: [rfadeev] - For now any deleted asset reloads prefab variants
            // window, find out how to use OnWillDeleteAsset method instead.
            if (importedPrefabIndex != -1 || deletedAssetPaths.Length != 0)
            {
                var windows = Resources.FindObjectsOfTypeAll<PrefabVariantEditorWindow>();
                if (windows != null)
                {
                    Debug.Assert(windows.Length == 1);
                    foreach (var window in windows)
                    {
                        window.ReloadTreeView();
                    }
                }
            }
        }

        private static bool IsPrefab(string assetPath)
        {
            var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
            var prefabAssetType = PrefabUtility.GetPrefabAssetType(asset);
            return prefabAssetType == PrefabAssetType.Model
                || prefabAssetType == PrefabAssetType.Regular
                || prefabAssetType == PrefabAssetType.Variant;
        }
    }
}

#endif
