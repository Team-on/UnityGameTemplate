using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class MenuBase : MonoBehaviour {
	[NonSerialized] public MenuManager MenuManager;

	[Header("Base menu values")]
	[SerializeField] protected float animTime = 0.2f;
	protected CanvasGroup canvasGroup;

	[Header("Buttons")]
	[Space]
	[SerializeField] protected Button firstButton;
	protected GameObject lastSelectedButton = null;

	Selectable[] selectables;

	protected virtual void Awake() {
		canvasGroup = GetComponent<CanvasGroup>();
		selectables = GetComponentsInChildren<Selectable>(true);
	}

	internal virtual void Show(bool isForce) {
		LeanTween.cancel(canvasGroup.gameObject);
		gameObject.SetActive(true);
		EnableAllSelectable();

		SelectButton();
		lastSelectedButton = firstButton.gameObject;

		if (isForce)
			canvasGroup.alpha = 1.0f;
		else
			LeanTweenEx.ChangeAlpha(canvasGroup, 1.0f, animTime);
	}

	internal virtual void Hide(bool isForce) {
		LeanTween.cancel(canvasGroup.gameObject);
		SaveLastButton();

		if (isForce) {
			canvasGroup.alpha = 0.0f;
			gameObject.SetActive(false);
		}
		else {
			LeanTweenEx.ChangeAlpha(canvasGroup, 0.0f, animTime)
			.setOnComplete(() => {
				gameObject.SetActive(false);
			});
		}
	}

	public void EnableAllSelectable() {
		foreach (var selectable in selectables)
			selectable.interactable = true;
	}

	public void DisableAllSelectable() {
		foreach (var selectable in selectables) 
			selectable.interactable = false;
	}

	public void SelectButton() {
		TemplateGameManager.Instance.uiinput.SetFirstButton(lastSelectedButton ? lastSelectedButton : firstButton.gameObject);
	}

	public void SaveLastButton() {
		lastSelectedButton = TemplateGameManager.Instance.eventSystem.currentSelectedGameObject;
	}
}
