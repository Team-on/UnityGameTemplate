using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using Polyglot;

public static class ScreenshotTakerEditor {
	public static bool isScreenshotQueueEmpty => queuedScreenshots.Count == 0;

	static object SizeHolder { get { return GetType("GameViewSizes").FetchProperty("instance").FetchProperty("currentGroup"); } }
	static EditorWindow GameView { get { return EditorWindow.GetWindow(GetType("GameView")); } }

	static Queue<ScreenshotData> queuedScreenshots = new Queue<ScreenshotData>();

	static int totalStages = 0;
	static int currentStage = 0;
	static bool isTakeScreenshot = false;
	static int originalIndex = 0;
	static int newIndex = 0;
	static int resolutionIndex = 0;
	static Language usedLanguage = Language.English;
	static string usedOutputFolder = "";

	public static void CaptureScreenshootQueueAllLanguages(List<ScreenshotData> data, string outputFolder) {
		usedLanguage = Localization.Instance.SelectedLanguage;
		usedOutputFolder = outputFolder;

		for (int i = 0; i < data.Count; i++) {
			if (data[i].isEnabled) {
				if (data[i].captureOverlayUI) {
					foreach (var lang in Localization.Instance.SupportedLanguages) {
						CaptureScreenshot(data[i], lang);
					}
				}
				else {
					CaptureScreenshot(data[i], usedLanguage);
				}
			}
		}

		EditorApplication.update -= CaptureQueuedScreenshots;
		isTakeScreenshot = false;
		originalIndex = -1;
		currentStage = 0;
		totalStages = queuedScreenshots.Count * 2;
		EditorApplication.update += CaptureQueuedScreenshots;
	}

	private static void CaptureScreenshot(ScreenshotData data, Language language) {
		int width = Mathf.RoundToInt(data.resolution.x * data.resolutionMultiplier);
		int height = Mathf.RoundToInt(data.resolution.y * data.resolutionMultiplier);

		if (width <= 0 || height <= 0) {
			Debug.LogWarning("Skipped resolution: " + data.resolution);
		}
		else {
			data = data.Clone();
			data.lang = language;
			queuedScreenshots.Enqueue(data);
		}
	}

	private static void CaptureQueuedScreenshots() {
		if (queuedScreenshots.Count == 0)
			return;

		ScreenshotData data = queuedScreenshots.Peek();

		int width = Mathf.RoundToInt(data.resolution.x * data.resolutionMultiplier);
		int height = Mathf.RoundToInt(data.resolution.y * data.resolutionMultiplier);
		EditorUtility.DisplayProgressBar("Making screenshots", $"{data.targetCamera} {width}x{height} {data.lang}", currentStage / (float)totalStages);
		++currentStage;

		if (!isTakeScreenshot) {
			isTakeScreenshot = true;

			if (originalIndex == -1)
				originalIndex = (int)GameView.FetchProperty("selectedSizeIndex");

			object customSize = GetFixedResolution(width, height);
			SizeHolder.CallMethod("AddCustomSize", customSize);
			newIndex = (int)SizeHolder.CallMethod("IndexOf", customSize) + (int)SizeHolder.CallMethod("GetBuiltinCount");
			resolutionIndex = newIndex;

			Localization.Instance.SelectedLanguage = data.lang;

			GameView.CallMethod("SizeSelectionCallback", resolutionIndex, null);
			GameView.Repaint();
		}
		else {
			isTakeScreenshot = false;

			if (!data.captureOverlayUI || data.targetCamera == ScreenshooterTargetCamera.SceneView)
				CaptureScreenshotWithoutUI(data);
			else
				CaptureScreenshotWithUI(data);

			SizeHolder.CallMethod("RemoveCustomSize", newIndex);

			queuedScreenshots.Dequeue();
			if (queuedScreenshots.Count == 0) {
				resolutionIndex = originalIndex;
				GameView.CallMethod("SizeSelectionCallback", resolutionIndex, null);

				Localization.Instance.SelectedLanguage = usedLanguage;

				EditorApplication.update -= CaptureQueuedScreenshots;

				Debug.Log("<b>Saved screenshots:</b> " + usedOutputFolder);
				EditorUtility.ClearProgressBar();
			}
		}
	}

	private static void CaptureScreenshotWithoutUI(ScreenshotData data) {
		Camera camera = data.targetCamera == ScreenshooterTargetCamera.GameView ? Camera.main : SceneView.lastActiveSceneView.camera;

		RenderTexture temp = RenderTexture.active;
		RenderTexture temp2 = camera.targetTexture;

		int width = Mathf.RoundToInt(data.resolution.x * data.resolutionMultiplier);
		int height = Mathf.RoundToInt(data.resolution.y * data.resolutionMultiplier);
		RenderTexture renderTex = RenderTexture.GetTemporary(width, height, 24);
		Texture2D screenshot = null;

		try {
			RenderTexture.active = renderTex;

			camera.targetTexture = renderTex;
			camera.Render();

			screenshot = new Texture2D(renderTex.width, renderTex.height, TextureFormat.RGBA32, false);
			screenshot.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0, false);
			screenshot.Apply(false, false);

			File.WriteAllBytes(ScreenshotTaker.GetUniqueFilePath(renderTex.width, renderTex.height, data.targetCamera == ScreenshooterTargetCamera.SceneView, false, data.lang.ToString(), usedOutputFolder, "jpeg"), screenshot.EncodeToJPG(100));
		}
		finally {
			camera.targetTexture = temp2;

			RenderTexture.active = temp;
			RenderTexture.ReleaseTemporary(renderTex);

			if (screenshot != null)
				GameObject.DestroyImmediate(screenshot);
		}
	}

	private static void CaptureScreenshotWithUI(ScreenshotData data) {
		RenderTexture temp = RenderTexture.active;

		RenderTexture renderTex = (RenderTexture)GameView.FetchField("m_TargetTexture");
		Texture2D screenshot = null;

		int width = renderTex.width;
		int height = renderTex.height;

		try {
			RenderTexture.active = renderTex;

			screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
			screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0, false);

			if (SystemInfo.graphicsUVStartsAtTop) {
				Color32[] pixels = screenshot.GetPixels32();
				for (int i = 0; i < height / 2; i++) {
					int startIndex0 = i * width;
					int startIndex1 = (height - i - 1) * width;
					for (int x = 0; x < width; x++) {
						Color32 color = pixels[startIndex0 + x];
						pixels[startIndex0 + x] = pixels[startIndex1 + x];
						pixels[startIndex1 + x] = color;
					}
				}

				screenshot.SetPixels32(pixels);
			}

			screenshot.Apply(false, false);

			File.WriteAllBytes(ScreenshotTaker.GetUniqueFilePath(width, height, data.targetCamera == ScreenshooterTargetCamera.SceneView, true, data.lang.ToString(), usedOutputFolder, "jpeg"), screenshot.EncodeToJPG(100));
		}
		finally {
			RenderTexture.active = temp;

			if (screenshot != null)
				GameObject.DestroyImmediate(screenshot);
		}
	}

	private static object GetFixedResolution(int width, int height) {
		object sizeType = Enum.Parse(GetType("GameViewSizeType"), "FixedResolution");
		return GetType("GameViewSize").CreateInstance(sizeType, width, height, "MSC_temp");
	}

	private static Type GetType(string type) {
		return typeof(EditorWindow).Assembly.GetType("UnityEditor." + type);
	}
}
