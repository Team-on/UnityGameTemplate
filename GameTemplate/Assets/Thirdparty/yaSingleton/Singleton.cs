using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using yaSingleton.Helpers;
using yaSingleton.Utility;

namespace yaSingleton {
    /// <summary>
    /// Singleton class. It'll be initialized before the Awake method of all other MonoBehaviours.
    /// Inherit by passing the inherited type (e.g. class GameManager : Singleton&lt;GameManager&gt;)
    /// </summary>
    /// <typeparam name="TSingleton">The Inherited Singleton's Type</typeparam>
    [Serializable]
    public abstract class Singleton<TSingleton> : BaseSingleton where TSingleton : BaseSingleton {
        public static TSingleton Instance { get; private set; }

#if UNITY_EDITOR
        public static TSingleton InstanceEditor {
            get {
                var preloadedAssets = UnityEditor.PlayerSettings.GetPreloadedAssets().ToList();

                foreach (var preloadedAsset in preloadedAssets) {
                    if (preloadedAsset && preloadedAsset is TSingleton) {
                        return preloadedAsset as TSingleton;
                    }
                }

                return null;
            }
        }
#endif


        internal override void CreateInstance() {
            if(Instance != null) {
                return;
            }

            Instance = GetOrCreate<TSingleton>();
        }
    }
}