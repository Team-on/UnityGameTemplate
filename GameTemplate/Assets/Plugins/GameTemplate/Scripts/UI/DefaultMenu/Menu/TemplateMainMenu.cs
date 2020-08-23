using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateMainMenu : MenuBase {
	public void Play() {
		SceneLoader.Instance.LoadScene("SampleScene2D", true, true);

	}

	public void Load() {
		SceneLoader.Instance.LoadScene("SampleScene2D", true, false);
	}
}
