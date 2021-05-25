using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class UIInput : MonoBehaviour {
	[NonSerialized] public bool isUseNavigation = false;

	[Header("This refs")]
	[Space]
	[SerializeField] EventSystem eventSystem;
	[SerializeField] InputSystemUIInputModule inputSystem;

	GameObject selectedGo = null;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!eventSystem)
			eventSystem = GetComponent<EventSystem>();
		if (!inputSystem)
			inputSystem = GetComponent<InputSystemUIInputModule>();
	}
#endif

	void Awake() {
		if (!TemplateGameManager.Instance.inputSystem)
			TemplateGameManager.Instance.inputSystem = inputSystem;
		if (!TemplateGameManager.Instance.eventSystem)
			TemplateGameManager.Instance.eventSystem = eventSystem;
		if (!TemplateGameManager.Instance.uiinput)
			TemplateGameManager.Instance.uiinput = this;
	}

	public void SetSelectedButton(GameObject go) {
		if (TemplateGameManager.Instance.eventSystem.currentSelectedGameObject != null) {
			bool isUseNavigation = TemplateGameManager.Instance.uiinput.isUseNavigation;
			eventSystem.SetSelectedGameObject(null);
			TemplateGameManager.Instance.uiinput.isUseNavigation = isUseNavigation;
			if (selectedGo)
				selectedGo.GetComponent<UIEvents>().DeselectOnOtherSelected();
		}

		selectedGo = null;

		if (TemplateGameManager.Instance.uiinput.isUseNavigation) {
			selectedGo = go;
			eventSystem.SetSelectedGameObject(selectedGo);
		}
	}

	public void OnEnterButton(ButtonSelector selector) {
		selectedGo = selector.gameObject;

		if (eventSystem.currentSelectedGameObject != selectedGo) {
			bool isUseNavigation = TemplateGameManager.Instance.uiinput.isUseNavigation;
			eventSystem.SetSelectedGameObject(selectedGo);
			TemplateGameManager.Instance.uiinput.isUseNavigation = isUseNavigation;
		}
	}

	public void OnExitButton(ButtonSelector selector) {

	}
}
