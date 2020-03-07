using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public abstract class MenuBase : MonoBehaviour {
	[NonSerialized] public MenuManager MenuManager;

	protected CanvasGroup canvasGroup;

	protected virtual void Awake() {
		canvasGroup = GetComponent<CanvasGroup>();
	}

	public void Show() {
		canvasGroup.interactable = canvasGroup.blocksRaycasts = true;
		canvasGroup.alpha = 1.0f;
		OnEnter();
	}

	public void Hide() {
		canvasGroup.interactable = canvasGroup.blocksRaycasts = false;
		canvasGroup.alpha = 0.0f;
		OnExit();
	}

	protected virtual void OnEnter() { }
	protected virtual void OnExit() { }
}
