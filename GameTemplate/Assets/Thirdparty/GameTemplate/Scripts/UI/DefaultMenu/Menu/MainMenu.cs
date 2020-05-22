using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MenuBase {
	public void Play() {
		SceneLoader.Instance.LoadScene("SampleScene2D", true, true);

	}

	public void Load() {
		SceneLoader.Instance.LoadScene("SampleScene2D", true, false);
	}
}
