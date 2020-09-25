using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DisableNavigationOnMouseHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
	public void OnPointerDown(PointerEventData eventData) {
		TemplateGameManager.Instance.uiinput.OnPointerDownButton();
	}

	public void OnPointerEnter(PointerEventData eventData) {
		TemplateGameManager.Instance.uiinput.OnPointerEnterButton();
	}

	public void OnPointerExit(PointerEventData eventData) {
		TemplateGameManager.Instance.uiinput.OnPointerExitButton();
	}
}
