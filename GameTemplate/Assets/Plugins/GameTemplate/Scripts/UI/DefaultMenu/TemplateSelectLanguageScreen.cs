using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class TemplateSelectLanguageScreen : MenuBase {
	const string isSelectLanguageKey = "TemplateSelectLanguageScreen.isSelectLanguage";

	[Header("Refs"), Space]
	[SerializeField] MenuBase nextMenu;

	bool isSelectLanguageBefore;

	protected override void Awake() {
		base.Awake();

		isSelectLanguageBefore = PlayerPrefsX.GetBool(isSelectLanguageKey, false);
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

	public void OnSelectRu() {
		Polyglot.Localization.Instance.SelectedLanguage = Polyglot.Language.Russian;
	}

	public void OnSelectEng() {
		Polyglot.Localization.Instance.SelectedLanguage = Polyglot.Language.English;
	}

	public void OnSelectAnyLanguage() {
		PlayerPrefsX.SetBool(isSelectLanguageKey, true);
		MenuManager.Show(nextMenu);
	}
}
