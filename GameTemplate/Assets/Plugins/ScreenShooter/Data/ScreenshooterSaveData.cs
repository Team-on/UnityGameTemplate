using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;

[Serializable]
public class ScreenshooterSaveData {
	const string SAVE_FILE_NOREZ = "ScreenShooterSettings";
	const string SAVE_FILE = "ScreenShooterSettings.json";
	const string SAVE_FILE_EDITORPREFS = "ScreenShooterSettings.Save";
	const string SAVE_FILE_EDITORPREFS_DEFAULT = "Plugins/ScreenShooter/Data/ScreenShooterSettings.json";

	public string outputFolder = "-----";
	public List<ScreenshotData> screenshoots = new List<ScreenshotData>(1) { new ScreenshotData() };

	public static void SaveSettings(ScreenshooterSaveData data) {
		if (!PlayerPrefs.HasKey(SAVE_FILE_EDITORPREFS)) {
#if UNITY_EDITOR
			string[] allPath = AssetDatabase.FindAssets(SAVE_FILE_NOREZ);
			if (allPath.Length != 0)
				PlayerPrefs.SetString(SAVE_FILE_EDITORPREFS, AssetDatabase.GUIDToAssetPath(allPath[0]).Replace("Assets/", ""));
#endif
		}

		string savePath = Path.Combine(Application.dataPath, PlayerPrefs.GetString(SAVE_FILE_EDITORPREFS, SAVE_FILE_EDITORPREFS_DEFAULT));

		string json = JsonUtility.ToJson(data, true);

		if (!File.Exists(savePath)) {
			FileInfo file = new FileInfo(savePath);
			file.Directory.Create();
		}

		File.WriteAllText(savePath, json);
	}

	public static ScreenshooterSaveData LoadSettings() {
		if (!PlayerPrefs.HasKey(SAVE_FILE_EDITORPREFS)) {
#if UNITY_EDITOR
			string[] allPath = AssetDatabase.FindAssets(SAVE_FILE_NOREZ);
			if (allPath.Length != 0)
				PlayerPrefs.SetString(SAVE_FILE_EDITORPREFS, AssetDatabase.GUIDToAssetPath(allPath[0]).Replace("Assets/", ""));
#endif
		}

		string savePath = Path.Combine(Application.dataPath, PlayerPrefs.GetString(SAVE_FILE_EDITORPREFS, SAVE_FILE_EDITORPREFS_DEFAULT));

		if (!File.Exists(savePath)) {
			return new ScreenshooterSaveData();
		}
		else {
			string json = File.ReadAllText(savePath);
			return JsonUtility.FromJson<ScreenshooterSaveData>(json);
		}
	}
}
