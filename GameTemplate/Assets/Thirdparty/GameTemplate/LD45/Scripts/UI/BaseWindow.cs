using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWindow : MonoBehaviour {
	[SerializeField] LeanTweenType Ease = LeanTweenType.easeOutBack;
	[SerializeField] float MoveTime = 0.5f;
	[SerializeField] Vector2 HidePos = new Vector2(0, 1000);
	[SerializeField] Vector2 ShowPos = new Vector2(0, 0);

	protected void Awake() {
		transform.localPosition = HidePos;
	}

	public virtual void Show(bool isForce) {
		Move(isForce, ShowPos);
	}

	public virtual void Hide(bool isForce) {
		Move(isForce, HidePos);
	}

	void Move(bool isForce, Vector2 pos) {
		if (isForce) {
			transform.position = HidePos;
		}
		else {
			LeanTween.moveLocal(gameObject, pos, MoveTime)
				.setEase(Ease);
		}
	}
}
