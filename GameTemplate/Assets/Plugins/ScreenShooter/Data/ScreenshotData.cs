using System;
using UnityEngine;

[Serializable]
public class ScreenshotData {
	public bool isEnabled = true;
	public string name = "Name";
	public ScreenshooterTargetCamera targetCamera = ScreenshooterTargetCamera.GameView;
	public Vector2 resolution = new Vector2(1920, 1080);
	public float resolutionMultiplier = 1;
	public bool captureOverlayUI = true;

#if POLYGLOT
	public Polyglot.Language lang;  //Filled in script
#endif

	public ScreenshotData() {

	}

	public ScreenshotData(string name, Vector2 resolution) {
		this.name = name;
		this.resolution = resolution;
	}

	public ScreenshotData(string name, int x, int y) : this(name, new Vector2(x, y)) {
	}

	public ScreenshotData Clone() {
		return (ScreenshotData)MemberwiseClone();
	}
}
