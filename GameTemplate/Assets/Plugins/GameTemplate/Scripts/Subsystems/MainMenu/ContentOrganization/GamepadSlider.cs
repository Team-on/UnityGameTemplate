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
	[SerializeField] TextMeshProUGUI percentTextField;

	public void Init(string headerText, UnityEngine.Events.UnityAction<float> onValueChanged, float value) {
		slider.minValue = 0;
		slider.maxValue = 1;
		slider.SetValueWithoutNotify(value);

		percentTextField.text = Mathf.CeilToInt(value * 100).ToString();

		headerTextField.text = headerText;

		slider.onValueChanged.AddListener(onValueChanged);
		slider.onValueChanged.AddListener(OnValueChanged);
	}

	public void ForceUpdateVisuals(float value) {
		slider.SetValueWithoutNotify(value);

		percentTextField.text = Mathf.CeilToInt(value * 100).ToString();
	}

	void OnValueChanged(float value) {
		percentTextField.text = Mathf.CeilToInt(value * 100).ToString();
	}
}
