using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;
using Polyglot;

[Serializable]
public class ScreenshooterSaveData {
	const string SAVE_DATA_PATH = "Plugins/GameTemplate/ScreenShooter/Data/SaveData.json";

	public string outputFolder = "-----";
	public List<ScreenshotData> screenshoots = new List<ScreenshotData>(1) { new ScreenshotData() };

	public static void SaveSettings(ScreenshooterSaveData data) {
		string savePath = Path.Combine(Application.dataPath, SAVE_DATA_PATH);
		string json = JsonUtility.ToJson(data, true);

		if (!File.Exists(savePath)) {
			FileInfo file = new FileInfo(savePath);
			file.Directory.Create();
		}

		File.WriteAllText(savePath, json);
	}

	public static ScreenshooterSaveData LoadSettings() {
		string savePath = Path.Combine(Application.dataPath, SAVE_DATA_PATH);

		if (!File.Exists(savePath)) {
			return new ScreenshooterSaveData();
		}
		else {
			string json = File.ReadAllText(savePath);
			return JsonUtility.FromJson<ScreenshooterSaveData>(json);
		}
	}
}
