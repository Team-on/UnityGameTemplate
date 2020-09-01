using UnityEngine;
using TMPro;


public class UIPopup : MonoBehaviour {
	[SerializeField] TextMeshProUGUI textField;

	public void SetText(string text) {
		textField.text = text;
	}

	public void PlayShowAnimation() {
		transform.localScale = Vector3.zero;
		LeanTween.value(gameObject, Vector3.zero, Vector3.one, 0.2f)
			.setOnUpdate((Vector3 size) => {
				transform.localScale = size;
			});
	}
}
