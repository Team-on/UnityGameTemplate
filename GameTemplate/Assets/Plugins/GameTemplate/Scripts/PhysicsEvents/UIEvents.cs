using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Events;
#endif

public class UIEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISelectHandler, IDeselectHandler, ISubmitHandler {
	public UnityEvent onClick;
	public UnityEvent onEnter;
	public UnityEvent onExit;

	public void OnPointerClick(PointerEventData eventData) {
		onClick?.Invoke();
	}

	public void OnSubmit(BaseEventData eventData) {
		onClick?.Invoke();
	}

	public void OnPointerEnter(PointerEventData eventData) {
		onEnter?.Invoke();
	}

	public void OnSelect(BaseEventData eventData) {
		onEnter?.Invoke();
	}

	public void OnPointerExit(PointerEventData eventData) {
		onExit?.Invoke();
	}

	public void OnDeselect(BaseEventData eventData) {
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
