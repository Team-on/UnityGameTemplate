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
	public UnityEvent onClick;
	public UnityEvent onEnter;
	public UnityEvent onExit;

	int enterCount = 0;

	public void OnPointerClick(PointerEventData eventData) {
		Click();
	}

	public void OnSubmit(BaseEventData eventData) {
		Click();
	}

	public void OnPointerEnter(PointerEventData eventData) {
		Enter();
	}

	public void OnSelect(BaseEventData eventData) {
		Enter();
	}

	public void OnPointerExit(PointerEventData eventData) {
		Exit();
	}

	public void OnDeselect(BaseEventData eventData) {
		Exit();
	}

	public void DeselectOnOtherSelected() {
		ForceExit();
	}

	void Enter() {
		++enterCount;
		if (enterCount == 1)
			onEnter?.Invoke();
	}

	void Click() {
		onClick?.Invoke();
	}

	void Exit() {
		if (enterCount <= 0)
			return;

		--enterCount;
		if (enterCount == 0)
			onExit?.Invoke();
	}

	void ForceExit() {
		if (enterCount != 0) {
			enterCount = 0;
			onExit?.Invoke();
		}
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
