using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DebugUIText : MonoBehaviour {
	[Header("Refs"), Space]
	[SerializeField] TextMeshProUGUI textField;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!textField)
			textField = GetComponent<TextMeshProUGUI>();
	}
#endif

	void Awake() {
		if (!textField)
			textField = GetComponent<TextMeshProUGUI>();

		TemplateGameManager.Instance.OnDebugModeChange += OnDebugModeChange;

		OnDebugModeChange(TemplateGameManager.Instance.IsDebugMode);
	}

	void OnDestroy() {
		TemplateGameManager.Instance.OnDebugModeChange -= OnDebugModeChange;
	}

	public void SetText(string text) {
		textField.text = text;
	}

	void OnDebugModeChange(bool mode) {
		textField.gameObject.SetActive(mode);
	}
}
