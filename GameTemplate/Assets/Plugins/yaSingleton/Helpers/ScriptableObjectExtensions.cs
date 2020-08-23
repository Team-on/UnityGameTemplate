using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace yaSingleton.Helpers {
    public static class ScriptableObjectExtensions {
        [Conditional("UNITY_EDITOR")]
        public static void AddToPreloadedAssets(this ScriptableObject scriptableObject) {
#if UNITY_EDITOR
            var preloadedAssets = UnityEditor.PlayerSettings.GetPreloadedAssets().ToList();
            
            if(preloadedAssets.Any(preloadedAsset => preloadedAsset && preloadedAsset.GetInstanceID() == scriptableObject.GetInstanceID())) {
                // Already being preloaded
                return; 
            }
            
            preloadedAssets.Add(scriptableObject);
            
            UnityEditor.PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
#endif
        }

        [Conditional("UNITY_EDITOR")]
        public static void RemoveEmptyPreloadedAssets() {
#if UNITY_EDITOR
            var preloadedAssets = UnityEditor.PlayerSettings.GetPreloadedAssets().ToList();

            var nonEmptyPreloadedAssets = preloadedAssets.Where(asset => asset).ToArray();
                
            UnityEditor.PlayerSettings.SetPreloadedAssets(nonEmptyPreloadedAssets);
#endif
        }
    }
}