using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GamepadSlider : MonoBehaviour {
	[Header("Refs"), Space]
	[SerializeField] Slider slider;
	[SerializeField] TextMeshProUGUI headerTextField;

	public void Init(string headerText, UnityEngine.Events.UnityAction<float> onValueChanged, float value) {
		slider.minValue = 0;
		slider.maxValue = 0;
		slider.SetValueWithoutNotify(value);

		headerTextField.text = headerText;

		slider.onValueChanged.AddListener(onValueChanged);
	}

	public void ForceUpdateVisuals(float value) {
		slider.SetValueWithoutNotify(value);
	}
}
