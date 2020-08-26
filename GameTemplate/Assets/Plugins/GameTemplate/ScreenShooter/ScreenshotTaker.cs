using System;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Polyglot;

public static class ScreenshotTaker {
	public static bool IsPaused => isPaused;

	static bool isPaused = false;
	static float prevTimescale = 1.0f;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	static void Init() {
		isPaused = false;
		prevTimescale = 1.0f;
	}

	public static void TakeScreenshot(string outputFolder) {
		if (!Directory.Exists(outputFolder))
			Directory.CreateDirectory(outputFolder);

		Vector2Int screenSize = new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height);

		var screenshotName = GetUniqueFilePath(screenSize.x, screenSize.y, false, Localization.Instance.SelectedLanguage.ToString(), outputFolder);
		string path = Path.Combine(outputFolder, screenshotName);
		ScreenCapture.CaptureScreenshot(path);

		Debug.Log("Save single screenshoot:" + path);
	}

	public static async Task WaitOneFrame() {
		Pause();
		await Task.Delay(1);
		Pause();
	}

	public static void Pause() {
		isPaused = !isPaused;
		if (isPaused) {
			prevTimescale = Time.timeScale;
			if (prevTimescale == 0)
				prevTimescale = 0.05f;
			Time.timeScale = 0;
		}
		else {
			Time.timeScale = prevTimescale;
		}
	}

	public static string GetUniqueFilePath(int width, int height, bool isSceneView, string lang, string folder) {
		string filename = string.Format("{3}{4}_{0}x{1}{5}_{2}", 
			width,
			height, 
			DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"),
#if UNITY_EDITOR
			PlayerSettings.productName.Replace(" ", "_"),
#else
			TemplateGameManager.Instance.productName.Replace(" ", "_"),
#endif
			isSceneView ? "_Scene" : "",
			isSceneView ? "" : $"_{lang}"
		);

		int fileIndex = 0;
		string path;
		do {
			path = Path.Combine(folder, string.Format(filename + "_{0}.jpeg", ++fileIndex));
		} while (File.Exists(path));

		return path;
	}

	public static string GetDefaultScreenshotPath() {
		return Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
			"ScreenshotsUnity",
#if UNITY_EDITOR
			PlayerSettings.productName.Replace(" ", "_")
#else
			TemplateGameManager.Instance.productName.Replace(" ", "_")
#endif
		);
	}
}
