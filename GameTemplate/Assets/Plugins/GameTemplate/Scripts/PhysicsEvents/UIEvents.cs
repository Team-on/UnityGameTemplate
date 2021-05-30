using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Events;
#endif

[RequireComponent(typeof(Selectable))]
public class UIEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISelectHandler, IDeselectHandler, ISubmitHandler {
	[Header("Events")]
	public UnityEvent onClick;
	public UnityEvent onEnter;
	public UnityEvent onExit;

	[Header("Refs"), Space]
	public Selectable selectable;

#if UNITY_EDITOR
	private void Reset() {
		selectable = GetComponent<Selectable>();
	}
#endif

	private void Awake() {

	}

	private void OnDisable() {

	}

	public void OnPointerClick(PointerEventData eventData) {
		TemplateGameManager.Instance.uiinput.isUseNavigation = false;

		Click();
	}

	public void OnSubmit(BaseEventData eventData) {
		if (!TemplateGameManager.Instance.uiinput.isUseNavigation)
			return;

		Click();
	}

	public void OnPointerEnter(PointerEventData eventData) {
		TemplateGameManager.Instance.uiinput.isUseNavigation = false;

		Enter();
	}

	public void OnSelect(BaseEventData eventData) {
		TemplateGameManager.Instance.uiinput.isUseNavigation = true;

		Enter();
	}

	public void OnPointerExit(PointerEventData eventData) {
		TemplateGameManager.Instance.uiinput.isUseNavigation = false;

		Exit();
	}

	public void OnDeselect(BaseEventData eventData) {
		TemplateGameManager.Instance.uiinput.isUseNavigation = true;

		Exit();
	}

	public void DeselectOnOtherSelected() {
		Exit();
	}

	void Enter() {
		onEnter?.Invoke();
	}

	void Click() {
		onClick?.Invoke();
	}

	void Exit() {
		onExit?.Invoke();
	}

#if UNITY_EDITOR
	public void AddPersistentListener(ref UnityEvent @event, MonoBehaviour mb, string methodName) {
		var targetinfo = UnityEvent.GetValidMethodInfo(mb, methodName, new Type[] { });

		UnityAction action = Delegate.CreateDelegate(typeof(UnityAction), mb, targetinfo, false) as UnityAction;
		UnityEventTools.AddPersistentListener(@event, action);

		EditorUtility.SetDirty(mb.gameObject);
	}
#endif
}
