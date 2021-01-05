using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(UIEvents))]
public class ButtonAnimator : MonoBehaviour {
	[Header("Tweens"), Space]
	[SerializeField] Vector3 hoverScale = new Vector3(1.1f, 1.1f, 1.0f);
	[SerializeField] LeanTweenType hoverScaleTweenType = LeanTweenType.easeOutBack;
	[Space]
	[SerializeField] Color hoverColor = Color.yellow;
	[SerializeField] LeanTweenType hoverColorTweenType = LeanTweenType.easeInOutQuad;
	[Space]
	[SerializeField] Sprite hoverSprite = null;
	[Space]
	[SerializeField] float time = 0.2f;

	[Header("Refs"), Space]
	[SerializeField] UIEvents events;
	[SerializeField] Image img;
	[SerializeField] TMPro.TextMeshProUGUI text;

	Vector3 defaultScaleImg;
	Color defaultColorImg;
	Sprite defaultSprite;

	Vector3 defaultScaleText;
	Color defaultColorText;

#if UNITY_EDITOR
	private void Reset() {
		img = GetComponent<Image>();
		events = GetComponent<UIEvents>();

		StartCoroutine(Init());

		IEnumerator Init() {
			yield return null;

			events.AddPersistentListener(ref events.onEnter, this, "OnEnter");
			events.AddPersistentListener(ref events.onClick, this, "OnClick");
			events.AddPersistentListener(ref events.onExit, this, "OnExit");
		}
	}
#endif

	private void Awake() {
		defaultScaleImg = transform.localScale;
		defaultColorImg = img.color;
		defaultSprite = img.sprite;

		defaultScaleText = text.transform.localScale;
		defaultColorText= text.color;
	}

	private void OnDisable () {
		LeanTween.cancel(gameObject);

		transform.localScale = defaultScaleImg;
		img.color = defaultColorImg;
		img.sprite = defaultSprite;
	}

	void OnEnter() {
		if (text) {
			LeanTween.scale(text.gameObject, hoverScale, time).setEase(hoverScaleTweenType);
			LeanTweenEx.ChangeColor(text, hoverColor, time).setEase(hoverColorTweenType);
		}

		LeanTween.scale(img.gameObject, hoverScale, time).setEase(hoverScaleTweenType);
		LeanTweenEx.ChangeColor(img, hoverColor, time).setEase(hoverColorTweenType);

		if (hoverSprite)
			img.sprite = hoverSprite;
	}

	void OnClick() {

	}

	void OnExit() {
		if (text) {
			LeanTween.scale(text.gameObject, defaultScaleText, time).setEase(hoverScaleTweenType);
			LeanTweenEx.ChangeColor(text, defaultColorText, time).setEase(hoverColorTweenType);
		}

		LeanTween.scale(img.gameObject, defaultScaleImg, time).setEase(hoverScaleTweenType);
		LeanTweenEx.ChangeColor(img, defaultColorImg, time).setEase(hoverColorTweenType);

		img.sprite = defaultSprite;
	}
}
