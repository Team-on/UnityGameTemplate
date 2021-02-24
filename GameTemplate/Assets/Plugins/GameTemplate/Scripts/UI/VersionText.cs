using System;
using System.IO;
using UnityEngine;
using TMPro;

public class VersionText : MonoBehaviour {
	[SerializeField] TextMeshProUGUI textField;

#if UNITY_EDITOR
	private void OnValidate() {
		if (textField == null)
			textField = GetComponent<TextMeshProUGUI>();

		if(textField != null) {
			if(textField.text != TemplateGameManager.Instance.buildNameString) {
				textField.text = TemplateGameManager.Instance.buildNameString;
				UnityEditor.EditorUtility.SetDirty(textField.gameObject);
			}
		}
	}
#endif

	void Start() {
		textField.text = TemplateGameManager.Instance.buildNameString;
		Destroy(this);
	}
}
