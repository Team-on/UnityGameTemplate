using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(UIEvents))]
public class ButtonLinkOpener : MonoBehaviour {
	[Header("Link"), Space]
	[SerializeField] string link;

	[Header("Refs"), Space]
	[SerializeField] UIEvents events;

#if UNITY_EDITOR
	private void Reset() {
		events = GetComponent<UIEvents>();

		StartCoroutine(Init());

		IEnumerator Init() {
			yield return null;

			events.AddPersistentListener(ref events.onClick, this, "OnClick");
		}
	}
#endif

	void OnClick() {
		if (!string.IsNullOrEmpty(link))
			Application.OpenURL(link);
	}
}
