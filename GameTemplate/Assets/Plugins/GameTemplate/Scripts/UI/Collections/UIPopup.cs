using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;



public class UIPopup : MonoBehaviour {
	[Header("Show animation")]
	[SerializeField] float showTime = 0.5f;
	[SerializeField] Vector3 startScale = Vector3.one / 2;
	[SerializeField] Vector3 endScale = Vector3.one;
	[SerializeField] float lettersPerSecond = 120;

	[Header("Child refs")]
	[Space]
	[SerializeField] protected TextMeshProUGUI textField;

	Vector3 startTextFieldLocalScale;

	private void Awake() {
		startTextFieldLocalScale = textField.transform.localScale;
	}

	public void SetText(string text) {
		textField.text = text;
	}

	public float PlayShowAnimation() {
		transform.localScale = startScale;
		textField.transform.localScale = new Vector3(startTextFieldLocalScale.x / startScale.x, startTextFieldLocalScale.y / startScale.y, startTextFieldLocalScale.z);

		LeanTween.value(gameObject, startScale, endScale, showTime)
		.setOnUpdate((Vector3 size) => {
			transform.localScale = size;
			textField.transform.localScale = new Vector3(startTextFieldLocalScale.x / size.x, startTextFieldLocalScale.y / size.y, startTextFieldLocalScale.z);
		});

		StartCoroutine(ShowTextCharacters());

		return showTime + textField.text.Length / lettersPerSecond;
	}

	IEnumerator ShowTextCharacters() {
		int len = textField.text.Length;
		float showTime = len / lettersPerSecond;
		float currTime = 0.0f;

		textField.maxVisibleCharacters = 0;

		yield return null;

		while (currTime <= showTime) {
			currTime += Time.deltaTime;

			textField.maxVisibleCharacters = Mathf.RoundToInt(Mathf.Lerp(0, len, currTime / showTime));
			yield return null;
		}
	}
}
