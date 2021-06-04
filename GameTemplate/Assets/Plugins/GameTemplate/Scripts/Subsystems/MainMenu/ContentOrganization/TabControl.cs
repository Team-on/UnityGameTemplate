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

		TemplateGameManager.Instance.actions.UI.TabLeft.performed += OnTabLeft;
		TemplateGameManager.Instance.actions.UI.TabRight.performed += OnTabRight;
	}

	private void OnDisable() {
		TemplateGameManager.Instance.actions.UI.TabLeft.performed -= OnTabLeft;
		TemplateGameManager.Instance.actions.UI.TabRight.performed -= OnTabRight;
	}

	public void SetFirstSelected(GameObject go, Selectable first) {
		for(int i = 0; i < content.Length; ++i) {
			if(content[i] == go) {
				firstSelected[i] = first;
				return;
			}
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
			if (firstSelected[tabId] && !lastSelected[tabId])
				lastSelected[tabId] = firstSelected[tabId];

			if (lastSelected[tabId] != null) {
				if (TemplateGameManager.Instance.uiinput.isUseNavigation) {
					ButtonSounds bs = lastSelected[tabId].GetComponent<ButtonSounds>();
					if (bs)
						bs.isIgnoreNextEnter = true;
				}

				TemplateGameManager.Instance.uiinput.SetSelectedButton(lastSelected[tabId].gameObject);
			}

			buttonAnimators[tabId].OverrideDefaultStateBack();
			buttonAnimators[tabId].SetDefaultState();
		}
		else {
			if(TemplateGameManager.Instance.eventSystem.currentSelectedGameObject != null && TemplateGameManager.Instance.eventSystem.currentSelectedGameObject.transform.IsChildOf(content[tabId].transform))
				lastSelected[tabId] = TemplateGameManager.Instance.eventSystem.currentSelectedGameObject.GetComponent<Selectable>();

			buttonAnimators[tabId].OverrideDefaultState(disabledColor, disabledSprite);
			buttonAnimators[tabId].SetState(disabledColor, disabledSprite);
		}
	}

	void OnTabLeft(InputAction.CallbackContext context) {
		if (currTab != 0)
			buttons[currTab - 1].onClick?.Invoke();
	}

	void OnTabRight(InputAction.CallbackContext context) {
		if (currTab != content.Length - 1)
			buttons[currTab + 1].onClick?.Invoke();
	}
}
