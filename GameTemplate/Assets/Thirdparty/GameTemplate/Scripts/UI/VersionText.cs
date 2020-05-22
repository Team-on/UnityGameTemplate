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
			textField.text = GameManager.InstanceEditor.buildNameString;
		}
	}
#endif

	void Start() {
		textField.text = GameManager.Instance.buildNameString;
		Destroy(this);
	}
}
