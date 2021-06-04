using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GamepadSelectionFieldSector : MonoBehaviour {
	public float Width => textField.textBounds.size.x;

	[Header("Refs"), Space]
	[SerializeField] TextMeshProUGUI textField;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!textField)
			textField = GetComponent<TextMeshProUGUI>();
	}
#endif

	public void SetText(string text) {
		textField.text = text;
		textField.ForceMeshUpdate(true, true);
	}

	public void SetFontSize(float fontSize) {
		textField.fontSize = fontSize;
		textField.ForceMeshUpdate(true, true);
	}
}
