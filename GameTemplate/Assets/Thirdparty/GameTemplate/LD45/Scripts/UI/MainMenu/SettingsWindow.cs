using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsWindow : BaseWindow {
	[SerializeField] TextMeshProUGUI MasterVolumeValue;
	[SerializeField] TextMeshProUGUI MusicVolumeValue;
	[SerializeField] TextMeshProUGUI SoundVolumeValue;

	[SerializeField] Slider MasterVolumeSlider;
	[SerializeField] Slider MusicVolumeSlider;
	[SerializeField] Slider SoundVolumeSlider;

	new void Awake() {
		base.Awake();
	}

	public override void Show(bool isForce) {
		MasterVolumeValue.text = ((MasterVolumeSlider.value = SoundManager.Instance.MasterVolume) * 100).ToString("0");
		MusicVolumeValue.text = ((MusicVolumeSlider.value = SoundManager.Instance.MusicVolume) * 100).ToString("0");
		SoundVolumeValue.text = ((SoundVolumeSlider.value = SoundManager.Instance.SoundVolume) * 100).ToString("0");

		base.Show(isForce);
	}

	public void OnMasterVolumeSliderUpdate() {
		SoundManager.Instance.MasterVolume = MasterVolumeSlider.value;
		MasterVolumeValue.text = (MasterVolumeSlider.value * 100).ToString("0");
	}

	public void OnMusicVolumeSliderUpdate() {
		SoundManager.Instance.MusicVolume = MusicVolumeSlider.value;
		MusicVolumeValue.text = (MusicVolumeSlider.value * 100).ToString("0");
	}

	public void OnSoundVolumeSliderUpdate() {
		SoundManager.Instance.SoundVolume = SoundVolumeSlider.value;
		SoundVolumeValue.text = (SoundVolumeSlider.value * 100).ToString("0");
	}
}
