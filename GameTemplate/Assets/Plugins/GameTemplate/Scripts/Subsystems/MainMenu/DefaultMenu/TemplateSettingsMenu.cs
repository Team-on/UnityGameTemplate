using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TemplateSettingsMenu : PopupMenuBase {
	[Header("Video settings")]
	[SerializeField] ToggleGroup fullscreenToggles = null;
	[SerializeField] Toggle fullscreenOnToggle = null;
	[SerializeField] Toggle fullscreenOffToggle = null;
	[SerializeField] TMP_Dropdown resolutionDropdown = null;
	[SerializeField] TMP_Dropdown graphicsDropdown = null;
	VideoOptionsData videoOptionsData = new VideoOptionsData();

	bool isInit = false;

	void Init() {
		fullscreenOnToggle.onValueChanged.AddListener(OnToggleOnFullscreen);
		resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
		graphicsDropdown.onValueChanged.AddListener(OnGraphicsChanged);

		videoOptionsData.Load();
		resolutionDropdown.ClearOptions();
		resolutionDropdown.AddOptions(videoOptionsData.avaliableResolutionsStr);
	}

	internal override void Show(bool isForce) {
		Show(isForce);

		if (!isInit) {
			isInit = true;
			Init();
		}

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
}
