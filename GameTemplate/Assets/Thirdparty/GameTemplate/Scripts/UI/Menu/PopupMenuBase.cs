using System;
using UnityEngine;
using UnityEngine.UI;

public class PopupMenuBase : MenuBase {
	[Header("Popup values")]
	[SerializeField] protected RectTransform popupTransform;

	[SerializeField] protected LeanTweenType easeIn = LeanTweenType.easeOutBack;
	[SerializeField] protected LeanTweenType easeOut = LeanTweenType.easeInBack;
	[SerializeField] protected Vector2 hidePosViewpoint = new Vector2(0.5f, 1.0f);
	[SerializeField] protected Vector2 showPosViewpoint = new Vector2(0.5f, 0.5f);

	protected Vector2 hidePos;
	protected Vector2 showPos;
	protected override void Awake() {
		base.Awake();

		hidePos = GameManager.Instance.Camera.ViewportToScreenPoint(hidePosViewpoint) + new Vector3(0, popupTransform.sizeDelta.y * 0.6f, 0);
		showPos = GameManager.Instance.Camera.ViewportToScreenPoint(showPosViewpoint);
		popupTransform.localPosition = hidePos;
	}

	internal override void Show(bool isForce) {
		Show(isForce, null, null);
	}

	internal override void Hide(bool isForce) {
		Hide(isForce, null, null);
	}

	internal void Show(bool isForce, Action CallBefore = null, Action CallAfter = null) {
		gameObject.SetActive(true);
	
		if (isForce) {
			CallBefore?.Invoke();
			popupTransform.position = hidePos;
			CallAfter?.Invoke();
		}
		else {
			CallBefore?.Invoke();
			LeanTween.move(popupTransform.gameObject, showPos, animTime)
				.setEase(easeIn)
				.setOnComplete(CallAfter)
				.setDelay(Time.deltaTime);
		}
	}

	internal void Hide(bool isForce, Action CallBefore = null, Action CallAfter = null) {
		if (isForce) {
			CallBefore?.Invoke();
			popupTransform.position = hidePos;
			gameObject.SetActive(false);
			CallAfter?.Invoke();
		}
		else {
			CallBefore?.Invoke();
			LeanTween.move(popupTransform.gameObject, hidePos, animTime)
				.setEase(easeOut)
				.setOnComplete(()=> {
					gameObject.SetActive(false);
					CallAfter?.Invoke();
				});
		}
	}
}
