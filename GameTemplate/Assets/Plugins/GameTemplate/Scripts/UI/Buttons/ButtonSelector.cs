using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UIEvents))]
public class ButtonSelector : MonoBehaviour {
	[Header("Refs"), Space]
	[SerializeField] UIEvents events;

#if UNITY_EDITOR
	private void Reset() {
		events = GetComponent<UIEvents>();

		StartCoroutine(Init());

		IEnumerator Init() {
			yield return null;

			events.AddPersistentListener(ref events.onEnter, this, "OnEnter");
			events.AddPersistentListener(ref events.onClick, this, "OnClick");
			events.AddPersistentListener(ref events.onExit, this, "OnExit");
		}
	}
#endif

	void OnEnter() {
		if(!events.isOnlyForMouse)
			TemplateGameManager.Instance.uiinput.OnEnterButton(this);
	}

	void OnClick() {

	}

	void OnExit() {

	}
}
