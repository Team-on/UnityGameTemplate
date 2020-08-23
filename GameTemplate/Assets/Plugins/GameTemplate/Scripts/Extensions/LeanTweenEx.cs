using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public static class LeanTweenEx
{
	public static LTDescr ChangeCanvasGroupAlpha(CanvasGroup canvasGroup, float alpha, float animTime)
	{
		return LeanTween.value(canvasGroup.gameObject, canvasGroup.alpha, alpha, animTime)
			.setOnUpdate((float a) => {
				canvasGroup.alpha = a;
			});
	}
	
	public static LTDescr ChangeTextAlpha(TextMeshProUGUI text, float alpha, float animTime)
	{
		return LeanTween.value(text.gameObject, text.alpha, alpha, animTime)
			.setOnUpdate((float a) => {
				text.alpha = a;
			});
	}

	public static void FadeImage(Image imageOrig, Sprite newSprite, float time)
	{
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

	public static void InvokeNextFrame(GameObject go, Action action)
	{
		go.GetComponent<MonoBehaviour>().StartCoroutine(InvokeNextFrameInner(action));
	}

	static IEnumerator InvokeNextFrameInner(Action action)
	{
		yield return null;
		action?.Invoke();
	}
}