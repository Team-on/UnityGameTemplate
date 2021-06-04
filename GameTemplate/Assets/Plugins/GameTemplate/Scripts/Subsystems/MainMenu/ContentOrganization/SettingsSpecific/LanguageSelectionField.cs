using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Polyglot;

//TODO: need to remove that
[RequireComponent(typeof(GamepadSelectionField))]
public class LanguageSelectionField : MonoBehaviour {
	[Header("Refs"), Space]
	[SerializeField] GamepadSelectionField selection;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!selection)
			selection = GetComponent<GamepadSelectionField>();
	}
#endif

	private void Awake() {
		List<string> languageNames = Localization.Instance.EnglishLanguageNames;
		List<string> localizedLanguageNames = Localization.Instance.LocalizedLanguageNames;
		List<string> values = new List<string>(languageNames.Count);

		for (int index = 0; index < languageNames.Count; index++) {
			Language lang = (Language)index;

			string additionalInfo = Localization.Get("ADDITIONAL_INFO_IN_SELECT", lang);
			string text = $"{localizedLanguageNames[index]}";

			if (lang != Language.English)
				text += $" ({languageNames[index]})";
			if (!string.IsNullOrEmpty(additionalInfo))
				text += $" {additionalInfo}";

			if (lang == Localization.Instance.ConvertSystemLanguage(Application.systemLanguage))
				text = $"<b>{text}</b>";

			values.Add(text);
		}

        selection.Init(values, Localization.Instance.SelectedLanguageIndex);
        selection.AddCallback(OnSelectLanguage);
	}

	private void OnEnable() {
		selection.SetId(Localization.Instance.SelectedLanguageIndex);
	}

	void OnSelectLanguage(int id) {
		Localization.Instance.SelectLanguage(id);
	}
}
