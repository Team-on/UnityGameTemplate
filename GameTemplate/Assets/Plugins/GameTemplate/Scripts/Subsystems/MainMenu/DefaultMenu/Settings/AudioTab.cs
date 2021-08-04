using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using Polyglot;

public class AudioTab : MonoBehaviour {
	[Header("Refs - general"), Space]
	[SerializeField] TabControl tabControl;
	[Header("Refs - toggle"), Space]
	[Header("Refs - slider"), Space]
	[SerializeField] GridLayoutGroup volumeSliderGrid;
	[SerializeField] GameObject volumeSliderPrefab;

	List<GamepadSlider> sliders; 

	private void Awake() {
		sliders = new List<GamepadSlider>(AudioManager.Instance.audioGroups.Length);

		for (int i = 0; i < AudioManager.Instance.audioGroups.Length; ++i) {
			AudioManager.AudioChannel currentChannel = (AudioManager.AudioChannel)i;
			GamepadSlider slider = Instantiate(volumeSliderPrefab, volumeSliderGrid.transform).GetComponent<GamepadSlider>();

			slider.Init(
				Localization.Get(AudioManager.Instance.audioGroups[i].translatedStringForSettings),
				(float value) => {
					AudioManager.Instance.SetVolume(currentChannel, value);
				},
				AudioManager.Instance.GetVolume(currentChannel)
			);

			if (i == 0)
				tabControl.SetFirstSelected(gameObject, slider.GetComponent<Selectable>());

			sliders.Add(slider);
		}

		//enableAudioOnToggle.onValueChanged.AddListener(OnToggleOnAudio);
		//enableAudioOnToggle.SetIsOnWithoutNotify(AudioManager.Instance.IsEnabled);
	}

	private void OnEnable() {
		UpdateVisuals();
	}

	public void UpdateVisuals() {
		for (int i = 0; i < AudioManager.Instance.audioGroups.Length; ++i) {
			AudioManager.AudioChannel currentChannel = (AudioManager.AudioChannel)i;
			
			sliders[i].ForceUpdateVisuals(AudioManager.Instance.GetVolume(currentChannel));
		}
	}
}
