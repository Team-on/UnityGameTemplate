using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HelpGameObject : MonoBehaviour {
	[Header("Data"), Space]
	[SerializeField] int levelWhenShow = 0;

	[Header("Refs"), Space]
	[SerializeField] TextMeshProUGUI textField;
	[SerializeField] CanvasGroup cg;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!textField)
			textField = GetComponent<TextMeshProUGUI>();
		if (!cg)
			cg = GetComponent<CanvasGroup>();
	}
#endif

	void Awake() {
		TemplateGameManager.Instance.OnHelpModeChange += OnHelpModeChange;

		OnHelpModeChange(TemplateGameManager.Instance.HelpLevelMode);
	}

	void OnDestroy() {
		TemplateGameManager.Instance.OnHelpModeChange -= OnHelpModeChange;
	}

	void OnHelpModeChange(int mode) {
		LeanTween.cancel(gameObject, false);

		if (textField) {
			LeanTweenEx.ChangeAlpha(textField, mode == levelWhenShow ? 1.0f : 0.0f, 0.2f);
		}
		else if (cg) {
			LeanTweenEx.ChangeAlpha(cg, mode == levelWhenShow ? 1.0f : 0.0f, 0.2f);
		}
		else {
			gameObject.SetActive(mode == levelWhenShow);
		}
	}
}
