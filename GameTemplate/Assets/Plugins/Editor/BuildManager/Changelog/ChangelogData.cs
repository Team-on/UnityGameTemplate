using System;
using System.IO;
using UnityEngine;
using UnityEditor;

[Serializable]
public class ChangelogData {
	public string updateName;

	#region Serialization
	const string SAVE_FILE_NOREZ = "ChangelogSettings";
	const string SAVE_FILE = "ChangelogSettings.json";
	const string SAVE_FILE_EDITORPREFS = "ChangelogSettings.Save";
	const string SAVE_FILE_EDITORPREFS_DEFAULT = "Plugins/Editor/BuildManager/Changelog/ChangelogSettings.json";

	public static void SaveChangelog(ChangelogData data) {
		if (!PlayerPrefs.HasKey(SAVE_FILE_EDITORPREFS)) {
			string[] allPath = AssetDatabase.FindAssets(SAVE_FILE_NOREZ);
			if (allPath.Length != 0)
				PlayerPrefs.SetString(SAVE_FILE_EDITORPREFS, AssetDatabase.GUIDToAssetPath(allPath[0]).Replace("Assets/", ""));
		}

		string savePath = Path.Combine(Application.dataPath, PlayerPrefs.GetString(SAVE_FILE_EDITORPREFS, SAVE_FILE_EDITORPREFS_DEFAULT));

		string json = JsonUtility.ToJson(data, true);

		if (!File.Exists(savePath)) {
			FileInfo file = new FileInfo(savePath);
			file.Directory.Create();
		}

		File.WriteAllText(savePath, json);
	}

	public static ChangelogData LoadChangelog() {
		if (!PlayerPrefs.HasKey(SAVE_FILE_EDITORPREFS)) {
			string[] allPath = AssetDatabase.FindAssets(SAVE_FILE_NOREZ);
			if (allPath.Length != 0)
				PlayerPrefs.SetString(SAVE_FILE_EDITORPREFS, AssetDatabase.GUIDToAssetPath(allPath[0]).Replace("Assets/", ""));
		}

		string savePath = Path.Combine(Application.dataPath, PlayerPrefs.GetString(SAVE_FILE_EDITORPREFS, SAVE_FILE_EDITORPREFS_DEFAULT));

		if (!File.Exists(savePath)) {
			return new ChangelogData();
		}
		else {
			string json = File.ReadAllText(savePath);
			return JsonUtility.FromJson<ChangelogData>(json);
		}
	}
	#endregion
}
