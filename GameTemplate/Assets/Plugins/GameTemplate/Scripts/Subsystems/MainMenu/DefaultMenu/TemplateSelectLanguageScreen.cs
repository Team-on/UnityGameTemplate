using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using Polyglot;

public class TemplateSelectLanguageScreen : MenuBase {
	const string isSelectLanguageKey = "TemplateSelectLanguageScreen.isSelectLanguage";

	[Header("Refs"), Space]
	[SerializeField] MenuBase nextMenu;
	[SerializeField] GridLayoutGroup grid;
	[SerializeField] GameObject buttonPrefab;

	bool isSelectLanguageBefore;

	protected override void Awake() {
		base.Awake();

		isSelectLanguageBefore = PlayerPrefsX.GetBool(isSelectLanguageKey, false);

		if (!isSelectLanguageBefore) {
			var languageNames = Localization.Instance.EnglishLanguageNames;
			var localizedLanguageNames = Localization.Instance.LocalizedLanguageNames;

			for (int index = 0; index < languageNames.Count; index++) {
				Language lang = (Language)index;

				UIEvents button = Instantiate(buttonPrefab, grid.transform).GetComponent<UIEvents>();
				TextMeshProUGUI textField = button.GetComponentInChildren<TextMeshProUGUI>();

				string additionalInfo = Localization.Get("ADDITIONAL_INFO_IN_SELECT", lang);
				textField.text = $"{localizedLanguageNames[index]}";
				if(lang != Language.English) 
					textField.text += $" ({languageNames[index]})";
				if (!string.IsNullOrEmpty(additionalInfo))
					textField.text += $" {additionalInfo}";

				button.onClick.AddListener(() => {
					Localization.Instance.SelectedLanguage = lang;
					OnSelectAnyLanguage();
				});

				if (lang == Localization.Instance.ConvertSystemLanguage(Application.systemLanguage)) 
					textField.text = $"<b>{textField.text}</b>";

				if(index == 0) 
					firstSelected = button.GetComponent<ButtonSelector>();
			}
		}
	}

	internal override void Hide(bool isForce) {
		enabled = false;
		base.Hide(isForce);
	}

	internal override void Show(bool isForce) {
		enabled = true;
		base.Show(isForce);

		if(isSelectLanguageBefore)
			OnSelectAnyLanguage();
	}

	void OnSelectAnyLanguage() {
		PlayerPrefsX.SetBool(isSelectLanguageKey, true);
		MenuManager.Show(nextMenu);
	}
}
