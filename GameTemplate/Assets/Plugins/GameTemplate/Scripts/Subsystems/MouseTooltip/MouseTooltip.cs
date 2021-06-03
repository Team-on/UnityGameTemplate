using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class MouseTooltip : MonoBehaviour {
	public static int canvasSortOrder = 100;
	public const int charsToMaxWidth = 24;

	public bool IsShowed => isShowed;

	[Header("Refs"), Space]
	[SerializeField] RectTransform rt;
	[SerializeField] Canvas canvas;
	[SerializeField] CanvasGroup cg;
	[SerializeField] TextMeshProUGUI textField;
	[SerializeField] LayoutElement childLayoutElement;

	RectTransform selected;

	string neededText = "";

	bool isShowed = false;
	bool isNeedShow = false;
	bool isCompleteHide = true;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!rt)
			rt = GetComponent<RectTransform>();
		if (!canvas)
			canvas = GetComponent<Canvas>();
		if (!cg)
			cg = GetComponent<CanvasGroup>();
	}
#endif

	private void Awake() {
		cg.alpha = 0.0f;
	}

	private void Start() {
		enabled = false;

		TemplateGameManager.Instance.tooltip = this;
	}

	private void Update() {
		if (!isShowed)
			return;

		rt.anchorMin = new Vector2(1.0f, 0.0f);
		rt.anchorMax = new Vector2(1.0f, 0.0f);
		rt.pivot = new Vector2(1.0f, 0.0f);

		//TODO: move tooltip to not be overlaped by cursor

		if (TemplateGameManager.Instance.uiinput.isUseNavigation) {
			rt.position = selected.position +
				new Vector3((selected.pivot.x) * selected.rect.width, (1.0f - selected.pivot.y) * selected.rect.height) + 
				new Vector3(-16, 16);

			if (rt.position.x - rt.rect.width <= 0) {
				rt.anchoredPosition += new Vector2(rt.rect.width, 0) + new Vector2(16 + 32, 0) + new Vector2((1.0f - selected.pivot.x) * -selected.rect.width, 0);
			}

			if (rt.position.y + rt.rect.height >= Screen.height) {
				rt.anchoredPosition += new Vector2(0, -rt.rect.height) + new Vector2(0, -16 + -32) + new Vector2(0, -selected.rect.height);
			}
		}
		else {
			rt.position = UnityEngine.InputSystem.Mouse.current.position.ReadValue() + new Vector2(-16, 16);

			if (rt.position.x - rt.rect.width <= 0) {
				rt.anchoredPosition += new Vector2(rt.rect.width, 0) + new Vector2(16 + 32, 0);
			}

			if (rt.position.y + rt.rect.height >= Screen.height) {
				rt.anchoredPosition += new Vector2(0, -rt.rect.height) + new Vector2(0, -16 + -32);
			}
		}
	}

	public void SetText(string text) {
		neededText = text;
	}

	public void Show() {
		if(TemplateGameManager.Instance.eventSystem.currentSelectedGameObject)
			selected = TemplateGameManager.Instance.eventSystem.currentSelectedGameObject.GetComponent<RectTransform>();

		if (isCompleteHide) {
			TryShow();
		}
		else {
			isNeedShow = true;
		}
	}

	public void Hide() {
		isCompleteHide = false;
		isShowed = false;
		isNeedShow = false;

		--canvasSortOrder;

		LeanTween.cancel(gameObject, false);
		LeanTweenEx.ChangeAlpha(cg, 0.0f, 0.1f)
		.setEase(LeanTweenType.easeInOutQuad)
		.setOnComplete(() => {
			enabled = false;
			isCompleteHide = true;

			if (isNeedShow) {
				TryShow();
			}
		});
	}

	void TryShow() {
		enabled = isShowed = true;

		canvas.sortingOrder = canvasSortOrder++;

		textField.text = neededText;

		bool needMaxWidth = false;
		int charsInThisLine = 0;

		for (int i = 0; i < neededText.Length; ++i) {
			if (neededText[i] == '\n') {
				charsInThisLine = 0;
			}
			else {
				++charsInThisLine;
				if (charsInThisLine > charsToMaxWidth) {
					needMaxWidth = true;
					break;
				}
			}
		}

		childLayoutElement.enabled = needMaxWidth;

		LeanTween.cancel(gameObject, false);
		LeanTweenEx.ChangeAlpha(cg, 1.0f, 0.1f)
			.setEase(LeanTweenType.easeInOutQuad)
			.setDelay(0.5f);
	}
}
