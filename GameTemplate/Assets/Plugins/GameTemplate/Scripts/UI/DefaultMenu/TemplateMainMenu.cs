using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateMainMenu : MenuBase {
	[SerializeField] int sceneIdToLoad = 1;

	public void Play() {
		SceneLoader.Instance.LoadScene(sceneIdToLoad, true, true);

	}

	public void Load() {
		SceneLoader.Instance.LoadScene(sceneIdToLoad, false, false);
	}
}
