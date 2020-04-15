using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : PopupMenuBase {
	[Header("Audio settings")]
	[SerializeField] ToggleGroup enableAudioToggles = null;
	[SerializeField] Toggle enableAudioOnToggle = null;
	[SerializeField] Toggle enableAudioOffToggle = null;
	[SerializeField] Slider mainVolumeSlider = null;
	[SerializeField] Slider musicVolumeSlider = null;
	[SerializeField] Slider soundVolumeSlider = null;

	[Header("Video settings")]
	[SerializeField] ToggleGroup fullscreenToggles = null;
	[SerializeField] Toggle fullscreenOnToggle = null;
	[SerializeField] Toggle fullscreenOffToggle = null;
	[SerializeField] TMP_Dropdown resolutionDropdown = null;
	[SerializeField] TMP_Dropdown graphicsDropdown = null;
	VideoOptionsData videoOptionsData = new VideoOptionsData();

	private new void Awake() {
		base.Awake();

		enableAudioOnToggle.onValueChanged.AddListener(OnToggleOnAudio);
		mainVolumeSlider.onValueChanged.AddListener(OnMainVolumeSliderChange);
		musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeSliderChange);
		soundVolumeSlider.onValueChanged.AddListener(OnSoundVolumeSliderChange);

		fullscreenOnToggle.onValueChanged.AddListener(OnToggleOnFullscreen);
		resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
		graphicsDropdown.onValueChanged.AddListener(OnGraphicsChanged);

		videoOptionsData.Load();
		resolutionDropdown.ClearOptions();
		resolutionDropdown.AddOptions(videoOptionsData.avaliableResolutionsStr);
	}

	internal override void Show(bool isForce) {
		Show(isForce);

		enableAudioOnToggle.SetIsOnWithoutNotify(AudioManager.Instance.IsEnabled);
		mainVolumeSlider.SetValueWithoutNotify(AudioManager.Instance.GetVolume(AudioManager.AudioChannel.Master));
		musicVolumeSlider.SetValueWithoutNotify(AudioManager.Instance.GetVolume(AudioManager.AudioChannel.Music));
		soundVolumeSlider.SetValueWithoutNotify(AudioManager.Instance.GetVolume(AudioManager.AudioChannel.Sound));

		videoOptionsData.Load();

		fullscreenOnToggle.SetIsOnWithoutNotify(videoOptionsData.isFullscreen);
		for(ushort i = 0; i < videoOptionsData.avaliableResolutions.Length; ++i) {
			if (Screen.currentResolution.height == videoOptionsData.avaliableResolutions[i].height &&
				Screen.currentResolution.width == videoOptionsData.avaliableResolutions[i].width) {
				resolutionDropdown.SetValueWithoutNotify(i);
				videoOptionsData.resolution = videoOptionsData.avaliableResolutions[i];
				break;
			}
		}
		
	}

	internal override void Hide(bool isForce) {
		Hide(isForce);
		videoOptionsData.Save();
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
	public void OnToggleOnFullscreen(bool value) {
		videoOptionsData.isFullscreen = value;
		videoOptionsData.ApplyResolutionSettings();
	}
	public void OnResolutionChanged(int id) {
		videoOptionsData.resolution = videoOptionsData.avaliableResolutions[id];
		videoOptionsData.ApplyResolutionSettings();
	}

	public void OnGraphicsChanged(int id) {

	}
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