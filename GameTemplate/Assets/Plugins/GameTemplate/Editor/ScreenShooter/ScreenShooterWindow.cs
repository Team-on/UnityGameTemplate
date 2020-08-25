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

	UnityEditorInternal.ReorderableList _list;

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

		_list = _list ?? ScreenShooterConfigList.Create(data.screenshoots, MenuItemHandler);
	}

	private void OnDestroy() {
		SaveSettings();
	}

	private void OnGUI() {
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

		_list.DoLayoutList();

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

	private void MenuItemHandler(object target) {
		data.screenshoots.Add(target as ScreenshootData);
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
	public string name = "Name";
	public TargetCamera targetCamera = TargetCamera.GameView;
	public Vector2 resolution = new Vector2(1920, 1080);
	public float resolutionMultiplier = 1;
	public bool captureOverlayUI = true;

	public Polyglot.Language lang;  //Filled in script

	public ScreenshootData() {

	}

	public ScreenshootData(string name, Vector2 resolution) {
		this.name = name;
		this.resolution = resolution;
	}

	public ScreenshootData(string name, int x, int y) : this(name, new Vector2(x, y)) {
	}

	public ScreenshootData Clone() {
		return (ScreenshootData)MemberwiseClone();
	}
}

public static class ScreenShooterConfigList {
	public static UnityEditorInternal.ReorderableList Create(List<ScreenshootData> configsList, GenericMenu.MenuFunction2 menuItemHandler) {
		var reorderableList = new UnityEditorInternal.ReorderableList(configsList, typeof(ScreenshootData), true, false, true, true);

		reorderableList.elementHeight = EditorGUIUtility.singleLineHeight + 4;
		reorderableList.drawElementCallback = (position, index, isActive, isFocused) => {
			const float enabledWidth = 15f;
			const float cameraWidth = 100f;
			const float textWidth = 10f;
			const float sizeWidth = 100f;
			const float multWidth = 30f;
			const float uiWidth = 15f;
			const float space = 10f;
			const float minNameWidth = 200f;

			const float singleWidth = 10;


			var config = configsList[index];
			var nameWidth = position.width - space * 6 - enabledWidth - cameraWidth - textWidth - sizeWidth * 2 - multWidth - uiWidth - singleWidth * 21;
			if (nameWidth < minNameWidth)
				nameWidth = minNameWidth;

			position.y += 2;
			position.height -= 4;

			position.x += space;
			position.width = enabledWidth;
			config.isEnabled = EditorGUI.Toggle(position, config.isEnabled);

			position.x += position.width + space;
			position.width = cameraWidth;
			config.targetCamera = (TargetCamera)EditorGUI.EnumPopup(position, config.targetCamera);

			position.x += position.width + space;
			position.width = nameWidth;
			config.name = EditorGUI.TextField(position, config.name);

			position.x += position.width + space;
			position.width = singleWidth * 4;
			EditorGUI.LabelField(position, "Size");

			position.x += position.width;
			position.width = sizeWidth;
			config.resolution.x = EditorGUI.IntField(position, (int)config.resolution.x);

			position.x += position.width;
			position.width = textWidth;
			EditorGUI.LabelField(position, "x");

			position.x += position.width;
			position.width = sizeWidth;
			config.resolution.y = EditorGUI.IntField(position, (int)config.resolution.y);

			position.x += position.width + space;
			position.width = singleWidth * 9;
			EditorGUI.LabelField(position, "Size Multiplier");

			position.x += position.width;
			position.width = multWidth;
			config.resolutionMultiplier = EditorGUI.FloatField(position, config.resolutionMultiplier);

			position.x += position.width + space;
			position.width = singleWidth * 2;
			EditorGUI.LabelField(position, "UI");

			position.x += position.width;
			position.width = uiWidth;
			if (config.targetCamera == TargetCamera.GameView) {
				config.captureOverlayUI = EditorGUI.Toggle(position, config.captureOverlayUI);
			}
			else {
				config.captureOverlayUI = EditorGUI.Toggle(position, false);
			}
		};

		reorderableList.onAddDropdownCallback = (buttonRect, list) => {
			var menu = new GenericMenu();

			menu.AddItem(new GUIContent("Custom"), false, menuItemHandler, new ScreenshootData("Custom", 1920, 1080));
			menu.AddSeparator("");

			foreach (var config in PredefinedConfigs.Android) {
				var label = "Android/" + config.name + " (" + config.resolution.x + "x" + config.resolution.y + ")";
				menu.AddItem(new GUIContent(label), false, menuItemHandler, config);
			}

			foreach (var config in PredefinedConfigs.iOS) {
				var label = "iOS/" + config.name + " (" + config.resolution.x + "x" + config.resolution.y + ")";
				menu.AddItem(new GUIContent(label), false, menuItemHandler, config);
			}

			foreach (var config in PredefinedConfigs.Standalone) {
				var label = "Standalone/" + config.name + " (" + config.resolution.x + "x" + config.resolution.y + ")";
				menu.AddItem(new GUIContent(label), false, menuItemHandler, config);
			}

			menu.ShowAsContext();
		};

		return reorderableList;
	}
}

public class PredefinedConfigs {
	public static ScreenshootData[] Android =
	{
			new ScreenshootData("Nexus 4 Portrait", 768, 1280),
			new ScreenshootData("Nexus 4 Landscape", 1280, 768),

			new ScreenshootData("Nexus 5 Portrait", 1080, 1920),
			new ScreenshootData("Nexus 5 Landscape", 1920, 1080),

			new ScreenshootData("Nexus 6 Portrait", 1440, 2560),
			new ScreenshootData("Nexus 6 Landscape", 2560, 1440),

			new ScreenshootData("Nexus 7 Portrait", 800, 1280),
			new ScreenshootData("Nexus 7 Landscape", 1280, 800),

			new ScreenshootData("Nexus 7 (2013) Portrait", 1200, 1920),
			new ScreenshootData("Nexus 7 (2013) Landscape", 1920, 1200),

			new ScreenshootData("Nexus 10 Portrait", 1600, 2560),
			new ScreenshootData("Nexus 10 Landscape", 2560, 1600),
		};

	public static ScreenshootData[] iOS =
	{
			new ScreenshootData("iPhone 3.5-Inch Portrait", 640, 960),
			new ScreenshootData("iPhone 3.5-Inch Landscape", 960, 640),

			new ScreenshootData("iPhone 4-Inch Portrait", 640, 1136),
			new ScreenshootData("iPhone 4-Inch Landscape", 1136, 640),

			new ScreenshootData("iPhone 4.7-Inch Portrait", 750, 1334),
			new ScreenshootData("iPhone 4.7-Inch Landscape", 1334, 750),

			new ScreenshootData("iPhone 5.5-Inch Portrait", 1242, 2208),
			new ScreenshootData("iPhone 5.5-Inch Landscape", 2208, 1242),

			new ScreenshootData("iPad Portrait", 768, 1024),
			new ScreenshootData("iPad Landscape", 1024, 768),

			new ScreenshootData("iPad Hi-Res Portrait", 1536, 2048),
			new ScreenshootData("iPad Hi-Res Landscape", 2048, 1536),

			new ScreenshootData("iPad Pro Portrait", 2048, 2732),
			new ScreenshootData("iPad Pro Landscape", 2732, 2048)
		};

	public static ScreenshootData[] Standalone =
	{
			new ScreenshootData("XGA", 1024, 768),
			new ScreenshootData("SXGA", 1280, 1024),
			new ScreenshootData("WXGA", 1280, 800),
			new ScreenshootData("WXGA+", 1440, 900),
			new ScreenshootData("WSXGA+", 1680, 1050),
			new ScreenshootData("HD", 1366, 768),
			new ScreenshootData("HD+", 1600, 900),
			new ScreenshootData("Full HD", 1920, 1080),
			new ScreenshootData("Quad HD", 2560, 1440),
			new ScreenshootData("4K UHD", 3840, 2160)
		};
}

