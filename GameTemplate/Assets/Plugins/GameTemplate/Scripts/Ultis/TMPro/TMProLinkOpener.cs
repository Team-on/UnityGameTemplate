using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TMP_Text))]
public class TMProLinkOpener : MonoBehaviour, IPointerClickHandler {
	public void OnPointerClick(PointerEventData eventData) {
		TMP_Text pTextMeshPro = GetComponent<TMP_Text>();
		int linkIndex = TMP_TextUtilities.FindIntersectingLink(pTextMeshPro, eventData.position, null);
		if (linkIndex != -1) {
			TMP_LinkInfo linkInfo = pTextMeshPro.textInfo.linkInfo[linkIndex];
#if UNITY_WEBGL
			openWindow(linkInfo.GetLinkID());
#else
			Application.OpenURL(linkInfo.GetLinkID());
#endif
		}
	}

#if UNITY_WEBGL
	[DllImport("__Internal")]
	private static extern void openWindow(string url);
#endif
}
