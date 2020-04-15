using System;
using UnityEngine;
using UnityEngine.UI;

public class PopupMenuBase : MenuBase {
	[Header("Popup values")]
	[SerializeField] protected RectTransform popupTransform = null;

	[SerializeField] protected LeanTweenType easeIn = LeanTweenType.easeOutBack;
	[SerializeField] protected LeanTweenType easeOut = LeanTweenType.easeInBack;
	[SerializeField] protected Transform openPos = null;
	[SerializeField] protected Transform closePos = null;

	protected bool isShowed = true;

	private void Start() {
		RecalcPos();

		EventManager.OnScreenResolutionChange += RecalcPos;
	}

	private void OnDestroy() {
		EventManager.OnScreenResolutionChange -= RecalcPos;
	}

	internal override void Show(bool isForce) {
		Show(isForce, null, null);
	}

	internal override void Hide(bool isForce) {
		Hide(isForce, null, null);
	}

	internal void Show(bool isForce, Action CallBefore = null, Action CallAfter = null) {
		if (isShowed)
			return;
		isShowed = true;
		gameObject.SetActive(true);

		if (isForce) {
			CallBefore?.Invoke();
			popupTransform.position = openPos.position;
			CallAfter?.Invoke();
		}
		else {
			CallBefore?.Invoke();
			LeanTween.move(popupTransform.gameObject, openPos.position, animTime)
				.setEase(easeIn)
				.setOnComplete(CallAfter)
				.setDelay(Time.deltaTime);
		}
	}

	internal void Hide(bool isForce, Action CallBefore = null, Action CallAfter = null) {
		if (!isShowed)
			return;
		isShowed = false;

		if (isForce) {
			CallBefore?.Invoke();
			popupTransform.position = closePos.position;
			gameObject.SetActive(false);
			CallAfter?.Invoke();
		}
		else {
			CallBefore?.Invoke();
			LeanTween.move(popupTransform.gameObject, closePos.position, animTime)
				.setEase(easeOut)
				.setOnComplete(()=> {
					gameObject.SetActive(false);
					CallAfter?.Invoke();
				});
		}
	}

	void RecalcPos(EventData ob = null) {
		if (isShowed) {
			popupTransform.position = openPos.position;
		}
		else {
			popupTransform.position = closePos.position;
		}
	}
}
