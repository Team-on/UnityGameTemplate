using UnityEngine;
using yaSingleton.Helpers;

namespace yaSingleton.Utility {
    /// <inheritdoc />
    /// <summary>
    /// ScriptableObject that automagically adds itself to Unity's preloaded assets.
    /// </summary>
    public abstract class PreloadedScriptableObject : ScriptableObject {
        protected virtual void OnEnable() {
#if UNITY_EDITOR
            if(UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) {
                return;
            }
#endif
            
            this.AddToPreloadedAssets();
        }

        protected virtual void OnDisable() {
#if UNITY_EDITOR
            if(UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) {
                return;
            }
#endif
            ScriptableObjectExtensions.RemoveEmptyPreloadedAssets();
        }
    }
}