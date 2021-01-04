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

	Vector3 defaultScale;
	Color defaultColor;
	Sprite defaultSprite;

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
		defaultColor = img.color;
		defaultSprite = img.sprite;
	}

	void OnEnter() {
		img.color = hoverColor;
	}

	void OnClick() {

	}

	void OnExit() {
		img.color = defaultColor;
	}
}
