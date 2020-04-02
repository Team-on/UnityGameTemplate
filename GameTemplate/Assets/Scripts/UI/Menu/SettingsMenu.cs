using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : PopupMenuBase {
	[Header("Audio settings")]
	[SerializeField] ToggleGroup enableToggles;
	[SerializeField] Toggle enableOnToggle;
	[SerializeField] Toggle enableOffToggle;
	[SerializeField] Slider mainVolumeSlider;
	[SerializeField] Slider musicVolumeSlider;
	[SerializeField] Slider soundVolumeSlider;

	private new void Awake() {
		base.Awake();

		enableOffToggle.isOn = !AudioManager.Instance.IsEnabled;
		enableOnToggle.isOn = AudioManager.Instance.IsEnabled;
		mainVolumeSlider.value = AudioManager.Instance.GetVolume(AudioManager.AudioChannel.Master);
		musicVolumeSlider.value = AudioManager.Instance.GetVolume(AudioManager.AudioChannel.Music);
		soundVolumeSlider.value = AudioManager.Instance.GetVolume(AudioManager.AudioChannel.Sound);

		enableOnToggle.onValueChanged.AddListener(OnToggleOnAudio);
		mainVolumeSlider.onValueChanged.AddListener(OnMainVolumeSliderChange);
		musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeSliderChange);
		soundVolumeSlider.onValueChanged.AddListener(OnSoundVolumeSliderChange);
	}

	#region Audio Settings
	public void OnMainVolumeSliderChange(float value) {
		AudioManager.Instance.SetVolume(AudioManager.AudioChannel.Master, value);
	}

	public void OnMusicVolumeSliderChange(float value) {
		AudioManager.Instance.SetVolume(AudioManager.AudioChannel.Music, value);
	}

	public void OnSoundVolumeSliderChange(float value) {
		AudioManager.Instance.SetVolume(AudioManager.AudioChannel.Sound, value);
	}

	public void OnToggleOnAudio(bool value) {
		AudioManager.Instance.IsEnabled = value;
	}
	#endregion

	#region Video

	#endregion

	#region Key bindings

	#endregion

	#region Gameplay

	#endregion

	//TODO: Am I need this? Maybe move it to `general tab` with some other settings
	#region Language	

	#endregion

	#region Mods

	#endregion

}