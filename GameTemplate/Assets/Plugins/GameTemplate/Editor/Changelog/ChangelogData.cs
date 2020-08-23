using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using Polyglot;

[Serializable]
public class ChangelogData {
	public string updateName;

	#region Serialization
	public static ChangelogData LoadChangelogFromFile(string usedFileName) {
		ChangelogData data;
		string path = Path.Combine(Application.dataPath, usedFileName);

		if (File.Exists(path)) {
			string jsonString = null;

			using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
				using (StreamReader sr = new StreamReader(file))
					jsonString = sr.ReadToEnd();

			data = JsonUtility.FromJson<ChangelogData>(jsonString);
		}
		else {
			data = null;
		}

		return data;
	}

	public static void SaveChangelogToFile(string usedFileName, ChangelogData data) {
		string path = Path.Combine(Application.dataPath, usedFileName);

		string jsonString = JsonUtility.ToJson(data, true);

		if (!File.Exists(path))
			File.Create(path);

		using (FileStream file = new FileStream(path, FileMode.Truncate, FileAccess.Write))
			using (StreamWriter sw = new StreamWriter(file))
				sw.Write(jsonString);
	}
	#endregion
}
