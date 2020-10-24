using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace ProjectWindowDetail.Details {
	[Serializable]
	public class ProjectWindowDetailsData {
		const string SAVE_FILE_NOREZ =  "ProjectWindowSettings";
		const string SAVE_FILE =        "ProjectWindowSettings.json";
		const string SAVE_FILE_EDITORPREFS = "ProjectWindowDetail.Save";
		const string SAVE_FILE_EDITORPREFS_DEFAULT = "Plugins/Editor/ProjectWindowDetail/Data/ProjectWindowSettings.json";

		public List<ProjectWindowDetailData> detailsData = new List<ProjectWindowDetailData>();

		public bool GetVisible(string name) {
			foreach (var detail in detailsData)
				if (detail.name == name)
					return detail.visible;

			return false;
		}

		public void SetValueOrCreateNew(string name, bool visible) {
			bool isFind = false;
			for(int i = 0; i < detailsData.Count; ++i) {
				if (detailsData[i].name == name) {
					detailsData[i].visible = visible;
					isFind = true;
					break;
				}
			}

			if (!isFind) {
				detailsData.Add(new ProjectWindowDetailData(name, visible));
			}
		}

		public static void SaveSettings(ProjectWindowDetailsData data) {
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

		public static ProjectWindowDetailsData LoadSettings() {
			if (!PlayerPrefs.HasKey(SAVE_FILE_EDITORPREFS)) {
				string[] allPath = AssetDatabase.FindAssets(SAVE_FILE_NOREZ);
				if (allPath.Length != 0)
					PlayerPrefs.SetString(SAVE_FILE_EDITORPREFS, AssetDatabase.GUIDToAssetPath(allPath[0]).Replace("Assets/", ""));
			}

			string savePath = Path.Combine(Application.dataPath, PlayerPrefs.GetString(SAVE_FILE_EDITORPREFS, SAVE_FILE_EDITORPREFS_DEFAULT));

			if (!File.Exists(savePath)) {
				return new ProjectWindowDetailsData();
			}
			else {
				string json = File.ReadAllText(savePath);
				return JsonUtility.FromJson<ProjectWindowDetailsData>(json);
			}
		}

		[Serializable]
		public class ProjectWindowDetailData {
			public string name;
			public bool visible;

			public ProjectWindowDetailData(string name, bool visible) {
				this.name = name;
				this.visible = visible;
			}
		}
	}
}
