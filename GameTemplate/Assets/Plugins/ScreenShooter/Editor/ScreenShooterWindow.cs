using System;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public class ScreenShooterWindow : EditorWindow {
	ScreenshooterSaveData data;

	Vector2 scrollPos;
	ReorderableList _list;

	[MenuItem("Window/Custom/Screen Shooter &s")]
	public static void ShowWindow() {
		GetWindow(typeof(ScreenShooterWindow), false, "Screen Shooter", true);
	}

	private void Awake() {
		data = ScreenshooterSaveData.LoadSettings();
	}

	private void OnDestroy() {
		ScreenshooterSaveData.SaveSettings(data);
	}

	private void OnGUI() {
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

		_list = _list ?? ScreenShooterConfigList.Create(data.screenshoots, OnMenuItemAdd);
		_list.DoLayoutList();

		GUI.enabled = true;
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		if (data.outputFolder == "-----")
			data.outputFolder = ScreenshotTaker.GetDefaultScreenshotPath();
		data.outputFolder = PathField("Save to:", data.outputFolder);


		EditorGUILayout.Space();
		EditorGUILayout.Space();
		if (GUILayout.Button(ScreenshotTaker.IsPaused ? "Play" : "Pause")) {
			ScreenshotTaker.Pause();
		}
		if (GUILayout.Button("Next Frame")) {
			ScreenshotTaker.WaitOneFrame();
		}

		EditorGUILayout.Space();
		EditorGUILayout.Space();
		GUI.enabled = ScreenshotTakerEditor.isScreenshotQueueEmpty;
		if (GUILayout.Button("Capture Screenshots Series")) {
			ScreenshotTakerEditor.CaptureScreenshootQueueAllLanguages(data.screenshoots, data.outputFolder);
		}

		if (GUILayout.Button("Take Game View Screenshot")) {
			ScreenshotTaker.TakeScreenshot(data.outputFolder);
		}

		GUI.enabled = true;
		EditorGUILayout.EndScrollView();
	}

	private void OnMenuItemAdd(object target) {
		data.screenshoots.Add(target as ScreenshotData);
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
}
