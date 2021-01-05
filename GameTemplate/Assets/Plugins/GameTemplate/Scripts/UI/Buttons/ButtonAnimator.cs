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

	Vector3 defaultScale;

	Color defaultColorImg;
	Sprite defaultSprite;

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
		defaultScale = transform.localScale;

		if (img) {
			defaultColorImg = img.color;
			defaultSprite = img.sprite;
		}

		if (text) {
			defaultColorText = text.color;
		}
	}

	private void OnDisable () {
		LeanTween.cancel(gameObject);

		transform.localScale = defaultScale;
		
		if (img) {
			img.color = defaultColorImg;
			img.sprite = defaultSprite;
		}

		if (text) {
			text.color = defaultColorText;
		}
	}

	void OnEnter() {
		LeanTween.scale(gameObject, hoverScale, time).setEase(hoverScaleTweenType);
		
		if (text) 
			LeanTweenEx.ChangeColor(text, hoverColor, time).setEase(hoverColorTweenType);

		if (img) {
			LeanTweenEx.ChangeColor(img, hoverColor, time).setEase(hoverColorTweenType);

			if (hoverSprite)
				img.sprite = hoverSprite;
		}
	}

	void OnClick() {

	}

	void OnExit() {
		LeanTween.scale(gameObject, defaultScale, time).setEase(hoverScaleTweenType);
		
		if (text) 
			LeanTweenEx.ChangeColor(text, defaultColorText, time).setEase(hoverColorTweenType);

		if (img) {
			LeanTweenEx.ChangeColor(img, defaultColorImg, time).setEase(hoverColorTweenType);
			img.sprite = defaultSprite;
		}
	}
}
