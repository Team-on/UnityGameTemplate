using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class TemplateSplashScreenAnyKey : MenuBase {
	[Header("Timings"), Space]
	[SerializeField] float timeToShowAnyKeyText = 2.0f;
	[SerializeField] float timeToCanSkip = 0.5f;
	float timePassed = 0.0f;

	[Header("Refs"), Space]
	[SerializeField] MenuBase nextMenu;
	[SerializeField] TextMeshProUGUI anyKeyText;
	[SerializeField] AlphaUpDown anyKeyTextAlphaLerp;
	[SerializeField] ScaleUpDown anyKeyTextSizeLerp;

	private void Start() {
		anyKeyText.color = anyKeyText.color.SetA(0.0f);
		anyKeyTextAlphaLerp.enabled = anyKeyTextSizeLerp.enabled = false;
	}

	internal override void Hide(bool isForce) {
		enabled = false;
		base.Hide(isForce);
	}

	internal override void Show(bool isForce) {
		enabled = true;
		base.Show(isForce);
	}

	private void Update() {
		if (timePassed < timeToShowAnyKeyText) {
			timePassed += Time.deltaTime;
			if (timePassed >= timeToShowAnyKeyText) {
				LeanTweenEx.ChangeAlpha(anyKeyText, 1.0f, 0.2f)
					.setOnComplete(()=> { 
						anyKeyTextAlphaLerp.enabled = anyKeyTextSizeLerp.enabled = true;
					});
			}
		}
		
		if (timePassed >= timeToCanSkip && InputEx.IsAnyKeyPressedThisFrame()) {
			OnAnyKeyPress();
		}
	}

	void OnAnyKeyPress() {
		MenuManager.Show(nextMenu);
	}
}
