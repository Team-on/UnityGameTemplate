using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TemplateSplashScreenAuto : MenuBase {
	[Header("Timings"), Space]
	[SerializeField] float timeToShow = 2.0f;

	[Header("Animation"), Space]
	[SerializeField] float startTextSize = 72;
	[SerializeField] float endTextSize = 1500;
	[SerializeField] float startAlpha = 1.0f;
	[SerializeField] float endAlpha = 0.0f;


	[Header("Refs"), Space]
	[SerializeField] MenuBase nextMenu;
	[SerializeField] TextMeshProUGUI logoText;

	protected override void Awake() {
		base.Awake();
		logoText.fontSize = startTextSize;
		logoText.alpha = startAlpha;
	}

	internal override void Hide(bool isForce) {
		enabled = false;
		base.Hide(isForce);
	}

	internal override void Show(bool isForce) {
		enabled = true;
		base.Show(isForce);

		LeanTween.value(gameObject, 0, 1, timeToShow)
		.setOnUpdate((float t) => {
			logoText.fontSize = Mathf.Lerp(startTextSize, endTextSize, t);
			logoText.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
		})
		.setOnComplete(() => {
			MenuManager.Show(nextMenu);
		});
	}
}
