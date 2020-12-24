using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public abstract class MenuBase : MonoBehaviour {
	public bool IsCanReturnToMenu => isCanReturnToMenu;
	
	[NonSerialized] public MenuManager MenuManager;

	[Header("Base menu values")]
	[SerializeField] protected float animTime = 0.2f;
	[SerializeField] protected bool isCanReturnToMenu = true;

	[Header("Refs")]
	[NaughtyAttributes.ReadOnly] public CanvasGroup cg;
	[NaughtyAttributes.ReadOnly] public RectTransform rt;

	[Header("Buttons")]
	[Space]
	[SerializeField] protected Button firstButton;
	protected GameObject lastSelectedButton = null;

	Selectable[] selectables;

#if UNITY_EDITOR
	private void OnValidate() {
		if(!cg)
			cg = GetComponent<CanvasGroup>();
		if (!rt)
			rt = GetComponent<RectTransform>();
	}
#endif

	protected virtual void Awake() {
		selectables = GetComponentsInChildren<Selectable>(true);
	}

	internal virtual void Show(bool isForce) {
		enabled = true;
		gameObject.SetActive(true);

		LeanTween.cancel(cg.gameObject);
		cg.interactable = cg.blocksRaycasts = true;

		EnableAllSelectable();
		SelectButton();
		if (firstButton)
			lastSelectedButton = firstButton.gameObject;

		if (isForce)
			cg.alpha = 1.0f;
		else
			LeanTweenEx.ChangeAlpha(cg, 1.0f, animTime);
	}

	internal virtual void Hide(bool isForce) {
		LeanTween.cancel(cg.gameObject);
		cg.interactable = cg.blocksRaycasts = false;

		SaveLastButton();

		if (isForce) {
			cg.alpha = 0.0f;
			gameObject.SetActive(false);
			enabled = false;
		}
		else {
			LeanTweenEx.ChangeAlpha(cg, 0.0f, animTime)
			.setOnComplete(() => {
				gameObject.SetActive(false);
				enabled = false;
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
		if(lastSelectedButton || firstButton)
			TemplateGameManager.Instance.uiinput.SetFirstButton(lastSelectedButton ? lastSelectedButton : firstButton.gameObject);
	}

	public void SaveLastButton() {
		lastSelectedButton = TemplateGameManager.Instance.eventSystem.currentSelectedGameObject;
	}
}
