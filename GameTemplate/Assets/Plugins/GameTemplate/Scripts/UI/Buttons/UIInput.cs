using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class UIInput : MonoBehaviour {
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
		selectedGo = go;

#if !UNITY_ANDROID
		if(TemplateGameManager.Instance.eventSystem.currentSelectedGameObject != null)
			eventSystem.SetSelectedGameObject(null);
		eventSystem.SetSelectedGameObject(selectedGo);
#endif
	}

	public void OnEnterButton(ButtonSelector selector) {
		if (selectedGo != selector.gameObject) {
			if (selectedGo)
				selectedGo.GetComponent<UIEvents>().DeselectOnOtherSelected();

			selectedGo = selector.gameObject;

#if !UNITY_ANDROID
			if (eventSystem.currentSelectedGameObject != selectedGo)
				eventSystem.SetSelectedGameObject(selectedGo);
#endif
		}
	}
}
