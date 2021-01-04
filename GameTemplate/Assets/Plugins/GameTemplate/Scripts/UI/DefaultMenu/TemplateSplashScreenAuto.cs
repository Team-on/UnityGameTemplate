using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateSplashScreenAuto : MenuBase {
	[Header("Timings"), Space]
	[SerializeField] float timeToShow = 2.0f;

	[Header("Refs"), Space]
	[SerializeField] MenuBase nextMenu;

	internal override void Hide(bool isForce) {
		enabled = false;
		base.Hide(isForce);
	}

	internal override void Show(bool isForce) {
		enabled = true;
		base.Show(isForce);

		LeanTween.delayedCall(timeToShow, () => { 
			MenuManager.Show(nextMenu);
		});
	}
}
