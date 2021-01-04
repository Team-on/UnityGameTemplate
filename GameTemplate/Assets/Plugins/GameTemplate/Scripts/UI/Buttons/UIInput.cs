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

	bool isUseNavigationInput = false;
	GameObject firstButton = null;

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

	private void Start() {
		inputSystem.move.action.performed += OnNavigate;
	}

	private void OnDestroy() {
		inputSystem.move.action.performed -= OnNavigate;
	}

	public void SetFirstButton(GameObject go) {
		firstButton = go;

		eventSystem.SetSelectedGameObject(null);
		eventSystem.SetSelectedGameObject(firstButton);

		isUseNavigationInput = true;
	}

	public void OnPointerEnterButton() {
		if (isUseNavigationInput) {
			eventSystem.SetSelectedGameObject(null);
		}
		isUseNavigationInput = false;
	}

	public void OnPointerDownButton() {
		if (!isUseNavigationInput) {
			eventSystem.SetSelectedGameObject(null);
		}
	}

	public void OnPointerExitButton() {
		eventSystem.SetSelectedGameObject(null);
		isUseNavigationInput = false;
	}

	void OnNavigate(InputAction.CallbackContext context) {
		Vector2 value = context.ReadValue<Vector2>();

		if (!isUseNavigationInput || eventSystem.currentSelectedGameObject == null || !eventSystem.currentSelectedGameObject.gameObject.activeInHierarchy) {
			if (firstButton && value.y <= -0.5f) {
				Selectable upSelectable = firstButton.GetComponent<Selectable>().navigation.selectOnUp;
				if(upSelectable)
					eventSystem.SetSelectedGameObject(upSelectable.gameObject);
				else
					eventSystem.SetSelectedGameObject(firstButton);
			}
			else {
				eventSystem.SetSelectedGameObject(firstButton);
			}
		}

		isUseNavigationInput = true;
	}
}
