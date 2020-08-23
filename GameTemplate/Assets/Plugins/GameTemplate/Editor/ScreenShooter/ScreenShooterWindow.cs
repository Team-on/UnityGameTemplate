using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;
using Polyglot;

public class ScreenShooterWindow : EditorWindow {
	const string SAVE_DATA_PATH = "Plugins/GameTemplate/Editor/ScreenShooter/SaveData.json";
	static object SizeHolder { get { return GetType("GameViewSizes").FetchProperty("instance").FetchProperty("currentGroup"); } }
	static EditorWindow GameView { get { return GetWindow(GetType("GameView")); } }

	ScreenshooterSaveData data;
	Queue<ScreenshootData> queuedScreenshots = new Queue<ScreenshootData>();

	Vector2 scrollPos;

	int totalStages = 0;
	int currentStage = 0;
	bool isTakeScreenshot;
	int originalIndex = 0;
	int newIndex = 0;
	int resolutionIndex = 0;
	Language usedLanguage;

	bool isPaused = false;
	float prevTimescale = 1.0f;

	[MenuItem("Window/Custom/Screen Shooter &s")]
	public static void ShowWindow() {
		GetWindow(typeof(ScreenShooterWindow), false, "Screen Shooter", true);
	}

	private void Awake() {
		LoadSettings();
	}

	private void OnDestroy() {
		SaveSettings();
	}

	private void OnGUI() {
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

		for (int i = 0; i < data.screenshoots.Count; i++) {
			GUILayout.BeginHorizontal();
			ScreenshootData curr = data.screenshoots[i];

			GUI.enabled = true;
			curr.isEnabled = EditorGUILayout.Toggle(GUIContent.none, curr.isEnabled, GUILayout.Width(25f));

			GUI.enabled = curr.isEnabled;
			curr.targetCamera = (TargetCamera)EditorGUILayout.EnumPopup(curr.targetCamera, GUILayout.Width(150));
			curr.resolution = EditorGUILayout.Vector2Field(GUIContent.none, curr.resolution, GUILayout.Width(200));
			EditorGUILayout.LabelField("Multiplier", GUILayout.Width(70));
			curr.resolutionMultiplier = EditorGUILayout.FloatField(curr.resolutionMultiplier, GUILayout.Width(50));

			if (curr.targetCamera == TargetCamera.GameView) {
				GUI.enabled = true;
				EditorGUILayout.LabelField("UI", GUILayout.Width(20));
				curr.captureOverlayUI = EditorGUILayout.Toggle(curr.captureOverlayUI, GUILayout.Width(15));
			}
			else {
				GUI.enabled = false;
				EditorGUILayout.LabelField("UI", GUILayout.Width(20));
				curr.captureOverlayUI = EditorGUILayout.Toggle(false, GUILayout.Width(15));
			}


			GUI.enabled = true;
			if (GUILayout.Button("+", GUILayout.Width(25f))) {
				data.screenshoots.Insert(i + 1, new ScreenshootData());
			}

			GUI.enabled = data.screenshoots.Count != 1;
			if (GUILayout.Button("-", GUILayout.Width(25f))) {
				data.screenshoots.RemoveAt(i);
				--i;
			}

			GUILayout.EndHorizontal();
		}


		GUI.enabled = true;
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		if (data.outputFolder == "-----")
			data.outputFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ScreenshotsUnity", PlayerSettings.productName);
		data.outputFolder = PathField("Save to:", data.outputFolder);


		EditorGUILayout.Space();
		EditorGUILayout.Space();
		if (GUILayout.Button(isPaused ? "Play" : "Pause")) {
			Pause();
		}
		if (GUILayout.Button("Next Frame")) {
			WaitDelayAsync();
		}


		EditorGUILayout.Space();
		EditorGUILayout.Space();
		GUI.enabled = queuedScreenshots.Count == 0;
		if (GUILayout.Button("Capture Screenshots Series")) {
			usedLanguage = Localization.Instance.SelectedLanguage;

			foreach (var lang in Localization.Instance.SupportedLanguages) {
				for (int i = 0; i < data.screenshoots.Count; i++) {
					if (data.screenshoots[i].isEnabled)
						CaptureScreenshot(data.screenshoots[i], lang);
				}
			}


			EditorApplication.update -= CaptureQueuedScreenshots;
			isTakeScreenshot = false;
			originalIndex = -1;
			currentStage = 0;
			totalStages = queuedScreenshots.Count * 2;
			EditorApplication.update += CaptureQueuedScreenshots;
		}

		if (GUILayout.Button("Take Game View Screenshot")) {
			TakeScreenshot();
		}

		GUI.enabled = true;
		EditorGUILayout.EndScrollView();
	}

	private void CaptureScreenshot(ScreenshootData data, Language language) {
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

	private void CaptureQueuedScreenshots() {
		if (queuedScreenshots.Count == 0)
			return;

		ScreenshootData data = queuedScreenshots.Peek();

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

			if (!data.captureOverlayUI || data.targetCamera == TargetCamera.SceneView) 
				CaptureScreenshotWithoutUI(data);
			else 
				CaptureScreenshotWithUI(data);

			SizeHolder.CallMethod("RemoveCustomSize", newIndex);

			queuedScreenshots.Dequeue();
			if (queuedScreenshots.Count == 0) {
				resolutionIndex = originalIndex;
				GameView.CallMethod("SizeSelectionCallback", resolutionIndex, null);

				Localization.Instance.SelectedLanguage = usedLanguage;

				Repaint();
				EditorApplication.update -= CaptureQueuedScreenshots;

				Debug.Log("<b>Saved screenshots:</b> " + this.data.outputFolder);
				EditorUtility.ClearProgressBar();
			}
		}
	}

	private void CaptureScreenshotWithoutUI(ScreenshootData data) {
		Camera camera = data.targetCamera == TargetCamera.GameView ? Camera.main : SceneView.lastActiveSceneView.camera;

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

			File.WriteAllBytes(GetUniqueFilePath(renderTex.width, renderTex.height, data.targetCamera == TargetCamera.SceneView, data.lang.ToString()), screenshot.EncodeToJPG(100));
		}
		finally {
			camera.targetTexture = temp2;

			RenderTexture.active = temp;
			RenderTexture.ReleaseTemporary(renderTex);

			if (screenshot != null)
				DestroyImmediate(screenshot);
		}
	}

	private void CaptureScreenshotWithUI(ScreenshootData data) {
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

			File.WriteAllBytes(GetUniqueFilePath(width, height, data.targetCamera == TargetCamera.SceneView, data.lang.ToString()), screenshot.EncodeToJPG(100));
		}
		finally {
			RenderTexture.active = temp;

			if (screenshot != null)
				DestroyImmediate(screenshot);
		}
	}

	private string GetUniqueFilePath(int width, int height, bool isSceneView, string lang) {
		string filename = string.Format("{3}_{4}_{0}x{1}_{5}_{2}", width, height, DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss"), PlayerSettings.productName.Replace(" ", "_"), isSceneView ? "Scene" : "Game", lang);

		int fileIndex = 0;
		string path;
		do {
			path = Path.Combine(data.outputFolder, string.Format(filename + "_{0}.jpeg", ++fileIndex));
		} while (File.Exists(path));

		return path;
	}

	private static object GetFixedResolution(int width, int height) {
		object sizeType = Enum.Parse(GetType("GameViewSizeType"), "FixedResolution");
		return GetType("GameViewSize").CreateInstance(sizeType, width, height, "MSC_temp");
	}

	private static Type GetType(string type) {
		return typeof(EditorWindow).Assembly.GetType("UnityEditor." + type);
	}

	private void SaveSettings() {
		string savePath = Path.Combine(Application.dataPath, SAVE_DATA_PATH);
		string json = JsonUtility.ToJson(data, true);

		if (!File.Exists(savePath)) {
			FileInfo file = new FileInfo(savePath);
			file.Directory.Create();
		}

		File.WriteAllText(savePath, json);
	}

	private void LoadSettings() {
		string savePath = Path.Combine(Application.dataPath, SAVE_DATA_PATH);

		if (!File.Exists(savePath)) {
			data = new ScreenshooterSaveData();
		}
		else {
			string json = File.ReadAllText(savePath);
			data = JsonUtility.FromJson<ScreenshooterSaveData>(json);
		}
	}

	private string PathField(string label, string path) {
		GUILayout.BeginHorizontal();
		path = EditorGUILayout.TextField(label, path);
		if (GUILayout.Button("...", GUILayout.Width(25f))) {
			string selectedPath = EditorUtility.OpenFolderPanel("Choose output directory", "", "");
			if (!string.IsNullOrEmpty(selectedPath))
				path = selectedPath;

			GUIUtility.keyboardControl = 0; // Remove focus from active text field
		}
		if (GUILayout.Button("Open", GUILayout.Width(100))) {
			var file = Directory.EnumerateFiles(data.outputFolder).FirstOrDefault();
			if (!string.IsNullOrEmpty(file))
				EditorUtility.RevealInFinder(Path.Combine(data.outputFolder, file));
			else
				EditorUtility.RevealInFinder(data.outputFolder);
		}
		GUILayout.EndHorizontal();

		return path;
	}

	private async Task WaitDelayAsync() {
		Pause();
		await Task.Delay(1);
		Pause();
	}

	private void TakeScreenshot() {
		if (!Directory.Exists(data.outputFolder))
			Directory.CreateDirectory(data.outputFolder);

		Vector2Int screenSize = new Vector2Int(1920, 1080);

		var screenshotName = GetUniqueFilePath(screenSize.x, screenSize.y, false, "Single");
		string path = Path.Combine(data.outputFolder, screenshotName);
		ScreenCapture.CaptureScreenshot(path);

		Debug.Log("Save single screenshoot:" + path);
	}

	private void Pause() {
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
}

public enum TargetCamera : byte { GameView = 0, SceneView = 1 };

[Serializable]
public class ScreenshooterSaveData {
	public string outputFolder = "-----";
	public List<ScreenshootData> screenshoots = new List<ScreenshootData>(1) { new ScreenshootData() };
}

[Serializable]
public class ScreenshootData {
	public bool isEnabled = true;
	public TargetCamera targetCamera = TargetCamera.GameView;
	public Vector2 resolution = new Vector2(1920, 1080);
	public float resolutionMultiplier = 1;
	public bool captureOverlayUI = true;

	public Polyglot.Language lang;  //Filled in script

	public ScreenshootData Clone() {
		return (ScreenshootData)MemberwiseClone();
	}
}