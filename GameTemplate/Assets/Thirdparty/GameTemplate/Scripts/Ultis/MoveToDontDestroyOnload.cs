using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MoveToDontDestroyOnload : MonoBehaviour {
	static List<string> placed = new List<string>();

#if UNITY_EDITOR	
	static MoveToDontDestroyOnload() {
	EditorApplication.playModeStateChanged += (PlayModeStateChange state) => {
			if(state == PlayModeStateChange.ExitingPlayMode) {
				placed.Clear();
			}
		};
	}
#endif

	void Awake() {
		if (placed.Contains(name)) {
			Destroy(gameObject);
			return;
		}

		placed.Add(name);
		DontDestroyOnLoad(gameObject);
		Destroy(this);
	}
}
