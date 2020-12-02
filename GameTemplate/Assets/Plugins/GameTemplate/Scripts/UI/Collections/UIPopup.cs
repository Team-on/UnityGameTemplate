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
	[SerializeField] protected TextMeshProUGUI[] textFields;

	Vector3 startTextFieldLocalScale;

	private void Awake() {
		startTextFieldLocalScale = textFields[0].transform.localScale;
	}

	public void SetText(string text) {
		textFields[0].text = text;
	}

	public float PlayShowAnimation() {
		transform.localScale = startScale;
		textFields[0].transform.localScale = new Vector3(startTextFieldLocalScale.x / startScale.x, startTextFieldLocalScale.y / startScale.y, startTextFieldLocalScale.z);

		LeanTween.value(gameObject, startScale, endScale, showTime)
		.setOnUpdate((Vector3 size) => {
			transform.localScale = size;
			textFields[0].transform.localScale = new Vector3(startTextFieldLocalScale.x / size.x, startTextFieldLocalScale.y / size.y, startTextFieldLocalScale.z);
		});

		StartCoroutine(ShowTextCharacters());

		return showTime + textFields[0].text.Length / lettersPerSecond;
	}

	IEnumerator ShowTextCharacters() {
		int len = textFields[0].text.Length;
		float showTime = len / lettersPerSecond;
		float currTime = 0.0f;

		int i = 0;
		for(; i < textFields.Length; ++i)
			textFields[i].maxVisibleCharacters = 0;

		yield return null;

		i = 0;
		int overflowChars = 0;
		while (currTime <= showTime) {
			currTime += Time.deltaTime;

			textFields[i].maxVisibleCharacters = Mathf.RoundToInt(Mathf.Lerp(0, len, currTime / showTime));

			if(textFields[i].firstOverflowCharacterIndex != -1 && textFields[i].firstOverflowCharacterIndex <= textFields[i].maxVisibleCharacters) {
				overflowChars = textFields[i].firstOverflowCharacterIndex - 1;
				textFields[i].maxVisibleCharacters = 99999;
				++i;
			}

			yield return null;
		}
	}
}
