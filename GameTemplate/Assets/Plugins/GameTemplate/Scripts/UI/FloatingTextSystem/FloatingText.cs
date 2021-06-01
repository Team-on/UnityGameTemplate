using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour {
	[Header("Refs"), Space]
	[SerializeField] TextMeshProUGUI textField;
	[SerializeField] RectTransform textRect;

#if UNITY_EDITOR
	private void OnValidate() {
		if (textField == null)
			textField = gameObject.GetComponentInChildren<TextMeshProUGUI>();
		if (textRect == null)
			textRect = textField.GetComponent<RectTransform>();
	}
#endif

	public void Play(string textKey, Color color) {
		textField.text = Polyglot.Localization.Get(textKey);
		textField.color = color;

		textField.color = textField.color.SetA(0.0f);
		textField.transform.localScale = Vector3.one;

		StayWorldPosAndMoveUp(textField.gameObject, 1.2f, 0.8f, Vector3.zero)
			.setOnComplete(() => {
				LeanTween.cancel(textField.gameObject, false);
				Destroy(gameObject);
			});

		LeanTweenEx.ChangeAlpha(textField, 1.0f, 0.1f);
		LeanTween.scale(textField.gameObject, Vector3.one * 1.4f, 0.2f)
		.setOnComplete(() => {
			LeanTween.scale(textField.gameObject, Vector3.one, 0.2f).setDelay(0.1f);
			LeanTweenEx.ChangeAlpha(textField, 0.0f, 0.3f).setDelay(0.6f);
		});
	}

	public void Play(string textKey) {
		Play(textKey, Color.white);
	}

	public static FloatingText PlayFloatingText(Vector3 worldPos, string textKey, Color color) {
		FloatingText floatingText = Instantiate(TemplateGameManager.Instance.floatingTextDefaultPrefab, worldPos, Quaternion.identity, null).GetComponent<FloatingText>();

		floatingText.Play(textKey, color);

		return floatingText;
	}

	public static FloatingText PlayFloatingText(Vector3 worldPos, string textKey) {
		return PlayFloatingText(worldPos, textKey, Color.white);
	}

	static LTDescr StayWorldPos(GameObject obj, float time, Vector3 localPosReturn) {
		obj.transform.localPosition = localPosReturn;
		Vector3 worldPos = obj.transform.position;

		return LeanTween.value(0, 1, time)
		.setOnUpdate((float t) => {
			obj.transform.position = worldPos;
		})
		.setOnComplete(() => {
			obj.transform.localPosition = localPosReturn;
		});
	}

	static LTDescr StayWorldPosAndMoveUp(GameObject obj, float time, float yMove, Vector3 localPosReturn) {
		obj.transform.localPosition = localPosReturn;
		Vector3 worldPos = obj.transform.position;

		return LeanTween.value(obj, 0, 1, time)
		.setOnUpdate((float t) => {
			obj.transform.position = worldPos + Vector3.up * yMove * t;
		})
		.setOnComplete(() => {
			obj.transform.localPosition = localPosReturn;
		});
	}
}
