using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using Polyglot;

public class SelectLanguageTab : MonoBehaviour {
	[Header("Data"), Space]
	[SerializeField] Color currentlySelectedColor = Color.yellow;


	[Header("Refs"), Space]
	[SerializeField] TabControl tabControl;
	[SerializeField] GridLayoutGroup grid;
	[SerializeField] GameObject buttonPrefab;

	List<TextMeshProUGUI> buttonTextFields;
	List<Color> defaultColors;

	private void Awake() {
		var languageNames = Localization.Instance.EnglishLanguageNames;
		var localizedLanguageNames = Localization.Instance.LocalizedLanguageNames;
		buttonTextFields = new List<TextMeshProUGUI>(localizedLanguageNames.Count);
		defaultColors = new List<Color>(localizedLanguageNames.Count);

		for (int index = 0; index < languageNames.Count; index++) {
			Language lang = (Language)index;

			UIEvents button = Instantiate(buttonPrefab, grid.transform).GetComponent<UIEvents>();
			TextMeshProUGUI textField = button.GetComponentInChildren<TextMeshProUGUI>();

			buttonTextFields.Add(textField);
			defaultColors.Add(textField.color);

			string additionalInfo = Localization.Get("ADDITIONAL_INFO_IN_SELECT", lang);
			textField.text = $"{localizedLanguageNames[index]}";
			if (lang != Language.English)
				textField.text += $" ({languageNames[index]})";
			if (!string.IsNullOrEmpty(additionalInfo))
				textField.text += $"\n{additionalInfo}";

			button.onClick.AddListener(() => {
				Localization.Instance.SelectedLanguage = lang;
			OnLocalize();
			});

			if (lang == Localization.Instance.ConvertSystemLanguage(Application.systemLanguage))
				textField.text = $"<b>{textField.text}</b>";


			if (index == 0)
				tabControl.SetFirstSelected(gameObject, button.GetComponent<Selectable>());
		}
	}

	private void OnEnable() {
		OnLocalize();
	}

	public void OnLocalize() {
		for (int index = 0; index < buttonTextFields.Count; index++) {
			if (index == Localization.Instance.SelectedLanguageIndex)
				buttonTextFields[index].color = currentlySelectedColor;
			else
				buttonTextFields[index].color = defaultColors[index];
		}
	}
}
