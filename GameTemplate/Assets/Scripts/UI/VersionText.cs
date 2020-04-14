using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using TMPro;

public class VersionText : MonoBehaviour {
	[SerializeField] TextMeshProUGUI textField;

#if UNITY_EDITOR
	private void OnValidate() {
		if (textField == null)
			textField = GetComponent<TextMeshProUGUI>();

		if(textField != null) {
			textField.text = $"{PlayerSettings.bundleVersion} - {ChangelogData.localizedUpdate}";
		}
	}
#endif

	void Start() {
		textField.text = $"{PlayerSettings.bundleVersion} - {ChangelogData.localizedUpdate}";
		Destroy(this);
	}
}
