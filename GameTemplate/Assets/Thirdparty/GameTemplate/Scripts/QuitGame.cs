using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour {
#if UNITY_WEBGL		//Destroy quit button, cuz you cant quit from game in webgl (only reload page workaround)
	private void Awake() {
		Destroy(gameObject);
	}
#endif

	public void Quit() {
		QuitApp();
	}

	public static void QuitApp() {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
#else
		Application.Quit();
#endif
	}
}
