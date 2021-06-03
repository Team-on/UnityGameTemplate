using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(UIEvents))]
public class ButtonAnimator : MonoBehaviour {
	[Header("Tweens"), Space]
	[SerializeField] Vector3 hoverScale = new Vector3(1.1f, 1.1f, 1.0f);
	[SerializeField] LeanTweenType scaleTweenType = LeanTweenType.easeOutBack;
	[Space]
	[SerializeField] Color hoverColor = Color.yellow;
	[SerializeField] LeanTweenType colorTweenType = LeanTweenType.easeInOutQuad;
	[Space]
	[SerializeField] Sprite hoverSprite = null;
	[Space]
	[SerializeField] float time = 0.2f;

	[Header("Refs"), Space]
	[SerializeField] UIEvents events;
	[SerializeField] Image img;
	[SerializeField] TMPro.TextMeshProUGUI text;

	Vector3 defaultScale, defaultScaleSaved;

	Color defaultColorImg, defaultColorImgSaved;
	Sprite defaultSprite, defaultSpriteSaved;

	Color defaultColorText, defaultColorTextSaved;

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
		defaultScaleSaved = defaultScale = transform.localScale;

		if (img) {
			defaultColorImgSaved = defaultColorImg = img.color;
			defaultSpriteSaved = defaultSprite = img.sprite;
		}

		if (text) {
			defaultColorTextSaved = defaultColorText = text.color;
		}
	}

	private void OnDisable () {
		LeanTween.cancel(gameObject);
		if(img)
			LeanTween.cancel(img.gameObject);
		if(text)
			LeanTween.cancel(text.gameObject);

		transform.localScale = defaultScale;
		
		if (img) {
			img.color = defaultColorImg;
			img.sprite = defaultSprite;
		}

		if (text) {
			text.color = defaultColorText;
		}
	}

	public void SetState(Color color, Sprite sprite = null) {
		SetState(defaultScale, color, defaultColorText, sprite);
	}

	public void SetState(Color color, Color colorText, Sprite sprite = null) {
		SetState(defaultScale, color, colorText, sprite);
	}

	public void SetState(Vector3 scale, Color color, Sprite sprite = null) {
		SetState(scale, color, defaultColorText, sprite);
	}

	public void SetState(Vector3 scale, Color color, Color colorText, Sprite sprite = null) {
		LeanTween.scale(gameObject, scale, time).setEase(scaleTweenType);

		if (text)
			LeanTweenEx.ChangeColor(text, colorText, time).setEase(colorTweenType);

		if (img) {
			LeanTweenEx.ChangeColor(img, color, time).setEase(colorTweenType);

			if (sprite)
				img.sprite = sprite;
		}
	}

	public void OverrideDefaultState(Color color, Sprite sprite = null) {
		OverrideDefaultState(defaultScale, color, defaultColorText, sprite);
	}

	public void OverrideDefaultState(Color color, Color colorText, Sprite sprite = null) {
		OverrideDefaultState(defaultScale, color, colorText, sprite);
	}

	public void OverrideDefaultState(Vector3 scale, Color color, Sprite sprite = null) {
		OverrideDefaultState(scale, color, defaultColorText, sprite);
	}

	public void OverrideDefaultState(Vector3 scale, Color color, Color colorText, Sprite sprite = null) {
		defaultScale = scale;
		defaultColorImg = color;
		defaultColorText = colorText;
		defaultSprite = sprite;
	}


	public void OverrideDefaultStateBack() {
		defaultScale = defaultScaleSaved ;
		defaultColorImg = defaultColorImgSaved;
		defaultColorText = defaultColorTextSaved;
		defaultSprite = defaultSpriteSaved;
	}
	public void SetHoverState() {
		OnEnter();
	}

	public void SetDefaultState() {
		OnExit();
	}

	void OnEnter() {
		SetState(hoverScale, hoverColor, hoverColor, hoverSprite);
	}

	void OnClick() {

	}

	void OnExit() {
		SetState(defaultScale, defaultColorImg, defaultColorText, defaultSprite);
	}
}
