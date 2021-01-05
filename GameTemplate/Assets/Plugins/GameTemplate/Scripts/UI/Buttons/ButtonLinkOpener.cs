using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

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
		if (!string.IsNullOrEmpty(link)) {
#if UNITY_WEBGL
		openWindow(link);
#else
		Application.OpenURL(link);
#endif
		}
	}

#if UNITY_WEBGL
	[DllImport("__Internal")]
	private static extern void openWindow(string url);
#endif
}
