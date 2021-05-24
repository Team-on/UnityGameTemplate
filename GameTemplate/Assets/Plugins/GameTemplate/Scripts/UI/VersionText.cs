using System;
using System.IO;
using UnityEngine;
using TMPro;

public class VersionText : MonoBehaviour {
	const string placeholderText = "Version - Update header here (date)";

	[SerializeField] TextMeshProUGUI textField;

#if UNITY_EDITOR
	private void OnValidate() {
		if (textField == null)
			textField = GetComponent<TextMeshProUGUI>();

		if (textField != null && textField.text != placeholderText) {
			textField.text = placeholderText;
			UnityEditor.EditorUtility.SetDirty(textField.gameObject);
		}
	}
#endif

	void Start() {
		textField.text = TemplateGameManager.Instance.buildNameString;
		Destroy(this);
	}
}
