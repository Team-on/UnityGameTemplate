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
		public static TSingleton Instance {
			get {
				if (!instance)
					Create();

				var res = instance;
#if UNITY_EDITOR
				if (!UnityEditor.EditorApplication.isPlaying) {
					instance = null;
				}
#endif

				return res;
			}
			private set {
				instance = value;
			}
		}
		static TSingleton instance;

		internal override void CreateInstance() {
			if (instance != null) {
				return;
			}

#if UNITY_EDITOR
			if (UnityEditor.EditorApplication.isPlaying) {
#endif
				instance = GetOrCreate<TSingleton>();
#if UNITY_EDITOR
			}
			else {
				if (instance == null) {
					instance = UnityEditor.AssetDatabase.LoadAssetAtPath<TSingleton>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("t:" + typeof(TSingleton).Name)[0]));
				}
			}
#endif
		}

		static void Create() {
			if (instance != null) {
				return;
			}

#if UNITY_EDITOR
			if (UnityEditor.EditorApplication.isPlaying) {
#endif
				instance = GetOrCreate<TSingleton>();
#if UNITY_EDITOR
			}
			else {
				if (instance == null) {
					instance = UnityEditor.AssetDatabase.LoadAssetAtPath<TSingleton>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets($"t:scriptableobject {typeof(TSingleton).Name}")[0]));
				}
			}
#endif
		}
	}
}