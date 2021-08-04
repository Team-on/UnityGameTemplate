using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSettingsData {
	const string saveKey = "GameSettingsData";

	//Language get saved by Polyglot

	//Audio settings
	public AudioSettigns audioSettigns = new AudioSettigns();

	public void ApplyAllSettings() {
		ApplyAudioSettings();
	}

	public void ApplyAudioSettings() {
		AudioManager.Instance.ApplySettings(audioSettigns);
	}

	public static GameSettingsData Load() {
		if (!PlayerPrefs.HasKey(saveKey)) {
			Debug.Log($"Create new game settings");
			return new GameSettingsData();
		}

		string json = PlayerPrefs.GetString(saveKey);
		GameSettingsData data = JsonUtility.FromJson<GameSettingsData>(json);

		Debug.Log($"Load game settings");

		return data;
	}

	public static void Save(GameSettingsData data) {
		string json = JsonUtility.ToJson(data, true);

		PlayerPrefs.SetString(saveKey, json);

		Debug.Log($"Successfully saved game settings");
	}
}

[Serializable]
public class AudioSettigns {
	public bool isEnabled = true;
	public List<float> volumes = new List<float> { 0.5f, 1.0f, 1.0f };
}