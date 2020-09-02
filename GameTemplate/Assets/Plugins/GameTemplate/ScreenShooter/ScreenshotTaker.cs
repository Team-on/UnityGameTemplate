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

	public static string TakeScreenshot(string outputFolder) {
		if (!Directory.Exists(outputFolder))
			Directory.CreateDirectory(outputFolder);

		Vector2Int screenSize = new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height);

		var screenshotName = GetUniqueFilePath(screenSize.x, screenSize.y, false, true, Localization.Instance.SelectedLanguage.ToString(), outputFolder, "jpeg");
		string path = Path.Combine(outputFolder, screenshotName);
		ScreenCapture.CaptureScreenshot(path);

		Debug.Log("Save single screenshoot:" + path);
		return path;
	}

	public static Texture2D TakeScreenshotTexture2D() {
		return ScreenCapture.CaptureScreenshotAsTexture();
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

	public static string GetUniqueFilePath(int width, int height, bool isSceneView, bool isUI, string lang, string folder, string extension) {
		string filename = string.Format("{3}{4}_{0}x{1}{5}{6}_{2}", 
			width,
			height, 
			DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"),
			Application.productName.Replace(" ", "_"),
			isSceneView ? "_Scene" : "",
			isSceneView || !isUI ? "" : $"_{lang}",
			isUI ? "" : $"_noUI"
		);

		int fileIndex = 0;
		string path;
		do {
			path = Path.Combine(folder, $"{filename}_{++fileIndex}.{extension}");
		} while (File.Exists(path));

		return path;
	}

	public static string GetDefaultScreenshotPath() {
		return Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
			"ScreenshotsUnity",
			Application.productName.Replace(" ", "_")
		);
	}
}
