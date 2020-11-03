using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateMainMenu : MenuBase {
	[SerializeField] string sceneToLoad = "SampleScene2D";

	public void Play() {
		SceneLoader.Instance.LoadScene(sceneToLoad, true, true);

	}

	public void Load() {
		SceneLoader.Instance.LoadScene(sceneToLoad, true, true);
	}
}
