using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuBase : MonoBehaviour {
	[NonSerialized] public MenuManager MenuManager;

	[Header("Base menu values")]
	[SerializeField] protected float animTime = 0.2f;
	protected CanvasGroup canvasGroup;

	protected virtual void Awake() {
		canvasGroup = GetComponent<CanvasGroup>();
	}

	internal virtual void Show(bool isForce) {
		gameObject.SetActive(true);
		
		if (isForce) 
			canvasGroup.alpha = 1.0f;
		else 
			LeanTweenEx.ChangeCanvasGroupAlpha(canvasGroup, 1.0f, animTime);
	}

	internal virtual void Hide(bool isForce) {
		if (isForce) {
			canvasGroup.alpha = 0.0f;
			gameObject.SetActive(false);
		}
		else {
			LeanTweenEx.ChangeCanvasGroupAlpha(canvasGroup, 0.0f, animTime)
			.setOnComplete(() => {
				gameObject.SetActive(false);
			});
		}
	}
}
