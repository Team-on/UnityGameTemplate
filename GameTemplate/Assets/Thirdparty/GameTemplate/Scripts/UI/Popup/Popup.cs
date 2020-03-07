using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour {
	[SerializeField] protected LeanTweenType Ease = LeanTweenType.easeOutBack;
	[SerializeField] protected float MoveTime = 0.5f;
	[SerializeField] protected Vector2 HidePosViewpoint = new Vector2(0, 1);
	[SerializeField] protected Vector2 ShowPosViewpoint = new Vector2(0, 0);

	protected Vector2 HidePos;
	protected Vector2 ShowPos;

	protected void Awake() {
		Camera cam = Camera.main;
		HidePos = cam.ViewportToScreenPoint(HidePosViewpoint) + new Vector3(0, GetComponent<Image>().rectTransform.sizeDelta.y, 0);
		ShowPos = cam.ViewportToScreenPoint(ShowPosViewpoint);

		transform.localPosition = HidePos;
	}

	public virtual void Show(bool isForce) {
		Show(isForce, null, null);
	}

	public virtual void Hide(bool isForce) {
		Hide(isForce, null, null);
	}

	public void Show(bool isForce, Action CallBefore = null, Action CallAfter = null) {
		if (isForce) {
			CallBefore?.Invoke();
			transform.position = HidePos;
			CallAfter?.Invoke();
		}
		else {
			CallBefore?.Invoke();
			LeanTween.moveLocal(gameObject, ShowPos, MoveTime)
				.setEase(Ease)
				.setOnComplete(CallAfter);
		}
	}

	public void Hide(bool isForce, Action CallBefore = null, Action CallAfter = null) {
		if (isForce) {
			CallBefore?.Invoke();
			transform.position = HidePos;
			CallAfter?.Invoke();
		}
		else {
			CallBefore?.Invoke();
			LeanTween.moveLocal(gameObject, HidePos, MoveTime)
				.setEase(Ease)
				.setOnComplete(CallAfter);
		}
	}
}
