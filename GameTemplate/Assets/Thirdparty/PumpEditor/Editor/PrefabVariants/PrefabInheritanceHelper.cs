#if UNITY_2018_3_OR_NEWER

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PumpEditor
{
    public static class PrefabInheritanceHelper
    {
        public static List<List<Object>> GetInheritanceChains()
        {
            var inheritanceChains = new List<List<Object>>();

            var guids = AssetDatabase.FindAssets("t:Prefab");
            foreach (var guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);

                var inheritanceChain = new List<Object>();
                while (PrefabUtility.GetPrefabAssetType(asset) == PrefabAssetType.Variant)
                {
                    inheritanceChain.Add(asset);
                    asset = PrefabUtility.GetCorrespondingObjectFromSource(asset);
                }

                inheritanceChain.Add(asset);
                inheritanceChains.Add(inheritanceChain);
            }

            return inheritanceChains;
        }
    }
}

#endif
