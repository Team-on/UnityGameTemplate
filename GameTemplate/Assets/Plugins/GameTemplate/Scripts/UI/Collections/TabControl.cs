using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TabControl : MonoBehaviour {
	[Header("Data"), Space]
	[SerializeField] Color disabledColor = new Color(0.33f, 0.33f, 0.33f);
	[SerializeField] Sprite disabledSprite = null;

	[Header("Refs"), Space]
	[SerializeField] UIEvents[] buttons;
	[SerializeField] GameObject[] content;
	[SerializeField] Selectable[] firstSelected;
	Selectable[] lastSelected;
	ButtonAnimator[] buttonAnimators;

	byte currTab = 0;

	private void Awake() {
		buttonAnimators = new ButtonAnimator[content.Length];
		lastSelected = new Selectable[firstSelected.Length];

		for (byte i = 0; i < content.Length; ++i) {
			byte id = i;
			buttonAnimators[i] = buttons[i].GetComponent<ButtonAnimator>();
			buttons[i].onClick.AddListener(()=> {
				OnTabClick(id);
			});
		}
	}

	private void OnEnable() {
		for (byte i = 0; i < content.Length; ++i) {
			SetSelection(i, i == currTab);
			content[i].SetActive(i == currTab);
		}
	}

	private void Update() {
		if (Gamepad.current != null) {
			if (Gamepad.current.leftShoulder.wasPressedThisFrame && currTab != 0)
				buttons[currTab - 1].onClick?.Invoke();
			else if (Gamepad.current.rightShoulder.wasPressedThisFrame && currTab != content.Length - 1)
				buttons[currTab + 1].onClick?.Invoke();
		}

		if (Keyboard.current != null) {
			if (Keyboard.current.qKey.wasPressedThisFrame && currTab != 0)
				buttons[currTab - 1].onClick?.Invoke();
			else if (Keyboard.current.eKey.wasPressedThisFrame && currTab != content.Length - 1)
				buttons[currTab + 1].onClick?.Invoke();
		}
	}

	void OnTabClick(byte tabId) {
		content[currTab].SetActive(false);
		SetSelection(currTab, false);

		content[currTab = tabId].SetActive(true);
		SetSelection(currTab, true);
	}

	void SetSelection(int tabId, bool isSelected) {
		buttons[tabId].enabled = !isSelected;

		if (isSelected) {
			Selectable toSelect;
			if(lastSelected[tabId] == null)
				toSelect = firstSelected[tabId];
			else
				toSelect = lastSelected[tabId];
			if(toSelect != null)
				TemplateGameManager.Instance.eventSystem.SetSelectedGameObject(toSelect.gameObject);

			buttonAnimators[tabId].OverrideDefaultStateBack();
			buttonAnimators[tabId].SetDefaultState();
		}
		else {
			lastSelected[tabId] = TemplateGameManager.Instance.eventSystem.currentSelectedGameObject.GetComponent<Selectable>();

			buttonAnimators[tabId].OverrideDefaultState(disabledColor, disabledSprite);
			buttonAnimators[tabId].SetState(disabledColor, disabledSprite);
		}
	}
}
