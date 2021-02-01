using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class LeanTweenEx {
	public static void InterriptPrev(GameObject go) {
		LeanTween.cancel(go);
	}

	public static void InterriptPrev(MonoBehaviour mb) {
		LeanTween.cancel(mb.gameObject);
	}

	public static LTDescr ChangeAlpha(CanvasGroup canvasGroup, float alpha, float animTime) {
		return LeanTween.value(canvasGroup.gameObject, canvasGroup.alpha, alpha, animTime)
			.setOnUpdate((float a) => {
				canvasGroup.alpha = a;
			});
	}

	public static LTDescr ChangeAlpha(Graphic sr, float alpha, float animTime) {
		return LeanTween.value(sr.gameObject, sr.color.a, alpha, animTime)
			.setOnUpdate((float a) => {
				sr.color = sr.color.SetA(a);
			});
	}

	public static LTDescr ChangeAlpha(SpriteRenderer sr, float alpha, float animTime) {
		return LeanTween.value(sr.gameObject, sr.color.a, alpha, animTime)
			.setOnUpdate((float a) => {
				sr.color = sr.color.SetA(a);
			});
	}

	public static LTDescr ChangeColor(SpriteRenderer sr, Color color, float animTime) {
		return LeanTween.value(sr.gameObject, sr.color, color, animTime)
			.setOnUpdate((Color c) => {
				sr.color = c;
			});
	}

	public static LTDescr ChangeColor(Image img, Color color, float animTime) {
		return LeanTween.value(img.gameObject, img.color, color, animTime)
			.setOnUpdate((Color c) => {
				img.color = c;
			});
	}

	public static LTDescr ChangeColor(Graphic img, Color color, float animTime) {
		return LeanTween.value(img.gameObject, img.color, color, animTime)
			.setOnUpdate((Color c) => {
				img.color = c;
			});
	}

	public static LTDescr StayWorldPos(GameObject obj, float time) {
		Vector3 worldPos = obj.transform.position;

		return LeanTween.value(0, 1, time)
		.setOnUpdate((float t) => {
			obj.transform.position = worldPos;
		});
	}

	public static LTDescr StayWorldPosAndMoveUp(GameObject obj, float time, float yMove) {
		Vector3 worldPos = obj.transform.position;

		return LeanTween.value(obj, 0, 1, time)
		.setOnUpdate((float t) => {
			obj.transform.position = worldPos + Vector3.up * yMove * t;
		});
	}

	public static void FadeImage(Image imageOrig, Sprite newSprite, float time) {
		GameObject fadedImage = new GameObject("fadedImage");

		Image image = fadedImage.AddComponent<Image>();
		image.sprite = newSprite;
		image.color = new Color(1, 1, 1, 0);

		RectTransform trans = fadedImage.GetComponent<RectTransform>();
		trans.transform.SetParent(imageOrig.transform);
		trans.transform.SetAsFirstSibling();
		trans.localScale = imageOrig.rectTransform.localScale;
		trans.localPosition = Vector3.zero;
		trans.sizeDelta = new Vector2(imageOrig.rectTransform.rect.width, imageOrig.rectTransform.rect.height);

		LeanTween.alpha(trans, 1.0f, time)
			.setOnComplete(() => {
				GameObject.Destroy(fadedImage);
				imageOrig.sprite = newSprite;
			});
	}

	public static void InvokeNextFrame(GameObject go, Action action) {
		go.GetComponent<MonoBehaviour>().StartCoroutine(InvokeNextFrameInner(action));
	}

	public static void InvokeNextFrames(GameObject go, int frames, Action action) {
		go.GetComponent<MonoBehaviour>().StartCoroutine(InvokeNextFramesInner(action, frames));
	}

	static IEnumerator InvokeNextFrameInner(Action action) {
		yield return new WaitForFixedUpdate();
		yield return new WaitForEndOfFrame();
		action?.Invoke();
	}

	static IEnumerator InvokeNextFramesInner(Action action, int frames) {
		while (frames-- != 0) {
			yield return new WaitForFixedUpdate();
			yield return new WaitForEndOfFrame();
		}
		action?.Invoke();
	}
}
