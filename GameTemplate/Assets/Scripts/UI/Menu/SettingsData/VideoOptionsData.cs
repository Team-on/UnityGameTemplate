using System.Collections.Generic;
using UnityEngine;

public struct VideoOptionsData {
	const string VIDEO_OPTIONS_SAVE_KEY = "VideoOptionsData";

	public bool isFullscreen;
	public Resolution resolution;

	public Resolution[] avaliableResolutions;
	public List<string> avaliableResolutionsStr;

	public void Load() {
		string json = PlayerPrefs.GetString(VIDEO_OPTIONS_SAVE_KEY, "");
		if (!string.IsNullOrEmpty(json)) {
			VideoOptionsData loadedData = JsonUtility.FromJson<VideoOptionsData>(json);
			this = loadedData;
		}

		avaliableResolutions = Screen.resolutions;
		avaliableResolutionsStr = new List<string>(avaliableResolutions.Length);
		for(ushort i = 0; i < avaliableResolutions.Length; ++i)
			avaliableResolutionsStr.Add($"{avaliableResolutions[i].width} X {avaliableResolutions[i].height} ({avaliableResolutions[i].refreshRate} Hz)");

		if (string.IsNullOrEmpty(json)) {
			isFullscreen = true;
			resolution = avaliableResolutions[avaliableResolutions.Length - 1];
			Debug.LogWarning("No saved Video options data. Use default");
		}
	}

	public void Save() {
		string json = JsonUtility.ToJson(this);
		PlayerPrefs.SetString(VIDEO_OPTIONS_SAVE_KEY, json);
	}

	public void ApplyResolutionSettings() {
		Screen.SetResolution(resolution.width, resolution.height, isFullscreen);
		GameManager.Instance.Events.CallOnScreenResolutionChange();
	}
}
