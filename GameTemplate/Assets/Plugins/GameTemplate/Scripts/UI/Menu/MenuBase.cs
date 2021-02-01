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

	[Header("Audio")]
	[SerializeField] protected bool playOnForce = false;
	[SerializeField] protected AudioClip openClip;
	[SerializeField] protected AudioClip closeClip;
	[Space]
	[SerializeField] protected AudioClip ambient;

	[Header("Refs")]
	[NaughtyAttributes.ReadOnly] public CanvasGroup cg;
	[NaughtyAttributes.ReadOnly] public RectTransform rt;

	[Header("Buttons")]
	[Space]
	[SerializeField] protected ButtonSelector firstSelected;
	protected GameObject lastSelectedGO;

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

		if ((!isForce || playOnForce) && openClip)
				AudioManager.Instance.Play(openClip);

		if ((!isForce || playOnForce) && ambient)
			AudioManager.Instance.PlayMusic(ambient);

		if (isForce) {
			cg.alpha = 1.0f;
		}
		else {
			LeanTweenEx.ChangeAlpha(cg, 1.0f, animTime);
		}
	}

	internal virtual void Hide(bool isForce) {
		LeanTween.cancel(cg.gameObject);
		cg.interactable = cg.blocksRaycasts = false;

		if ((!isForce || playOnForce) && closeClip)
			AudioManager.Instance.Play(closeClip);

		if (isForce) {
			cg.alpha = 0.0f;
			gameObject.SetActive(false);
			enabled = false;
		}
		else {
			SaveLastButton();

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
		if (firstSelected && !lastSelectedGO)
			lastSelectedGO = firstSelected.gameObject;

		if (lastSelectedGO) {
			ButtonSounds bs = lastSelectedGO.GetComponent<ButtonSounds>();
			if (bs) {
				bs.isIgnoreNextEnter = true;
			}

			TemplateGameManager.Instance.uiinput.SetSelectedButton(lastSelectedGO);
		}
	}

	public void SaveLastButton() {
		if(TemplateGameManager.Instance.eventSystem.currentSelectedGameObject != null && TemplateGameManager.Instance.eventSystem.currentSelectedGameObject.transform.IsChildOf(gameObject.transform))
			lastSelectedGO = TemplateGameManager.Instance.eventSystem.currentSelectedGameObject;
	}
}
