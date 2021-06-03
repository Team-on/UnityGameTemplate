using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarUI : MonoBehaviour {
	[Header("Refs"), Space]
	[SerializeField] Canvas canvas = null;
	[SerializeField] Slider barFirst = null;
	[SerializeField] Slider barSecond = null;
	[SerializeField] TextMeshProUGUI textField = null;

	[Header("Timing"), Space]
	[SerializeField] float firstBarTime = 0.2f;
	[SerializeField] float secondBarTime = 1.0f;

	float currValue, maxValue;

	public void Init(int _currValue, int _maxValue) {
		currValue = _currValue;
		maxValue = _maxValue;

		ReinitBars();
	}

	public void SetValue(int newValue) {
		currValue = newValue;
		UpdateBar();
	}

	public void ChangeValue(int change) {
		currValue += change;
		UpdateBar();
	}

	public void SetMaxValue(int newValue) {
		maxValue = newValue;
		ReinitBars();
		UpdateBar();
	}

	public void ChangeMaxValue(int change) {
		maxValue += change;
		ReinitBars();
		UpdateBar();
	}

	void ReinitBars() {
		barFirst.minValue = 0.0f;
		barFirst.maxValue = maxValue;
		barFirst.value = currValue;

		barSecond.minValue = 0.0f;
		barSecond.maxValue = maxValue;
		barSecond.value = currValue;

		if (textField != null)
			textField.text = $"{Mathf.RoundToInt(currValue)}/{Mathf.RoundToInt(maxValue)}";
	}


	void UpdateBar() {
		LeanTween.cancel(barFirst.gameObject);

		LeanTween.value(barFirst.gameObject, barFirst.value, currValue, firstBarTime)
		.setEase(LeanTweenType.linear)
		.setOnUpdate((float val) => {
			barFirst.value = val;
		});

		LeanTween.cancel(barSecond.gameObject);
		LeanTween.value(barSecond.gameObject, barSecond.value, currValue, secondBarTime)
		.setEase(LeanTweenType.easeInQuart)
		.setOnUpdate((float val) => {
			barSecond.value = val;
		});

		if (textField != null)
			textField.text = $"{Mathf.RoundToInt(currValue)}/{Mathf.RoundToInt(maxValue)}";
	}
}
