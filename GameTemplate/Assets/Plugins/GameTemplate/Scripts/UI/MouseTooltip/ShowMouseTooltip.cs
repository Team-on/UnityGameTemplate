using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIEvents))]
public class ShowMouseTooltip : MonoBehaviour {
	[Header("Audio"), Space]
	[SerializeField] string textKey;

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
		TemplateGameManager.Instance.tooltip.SetText(Polyglot.Localization.Get(textKey));
		TemplateGameManager.Instance.tooltip.Show();
	}

	void OnClick() {
		TemplateGameManager.Instance.tooltip.Hide();
	}

	void OnExit() {
		TemplateGameManager.Instance.tooltip.Hide();
	}
}
