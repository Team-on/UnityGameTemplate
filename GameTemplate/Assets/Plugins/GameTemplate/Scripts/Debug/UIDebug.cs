using UnityEngine;

public class UIDebug : MonoBehaviour {
	[Header("Timescale keys")]
	[Space]
	[SerializeField] KeyCode toggleDebugUI = KeyCode.F5;
	[SerializeField] KeyCode toggleAllUI = KeyCode.F6;

	[Header("Refs")]
	[Space]
	[SerializeField] GameObject debugParent;

	bool isDebugUIOn = true;
	bool isUIOn = true;

	void Update() {
		if (Input.GetKeyDown(toggleDebugUI)) {
			isDebugUIOn = !isDebugUIOn;

			Canvas[] allCanvases = debugParent.GetComponentsInChildren<Canvas>(true);
			foreach (var c in allCanvases) {
				c.enabled = isDebugUIOn;
			}
		}
		else if (Input.GetKeyDown(toggleAllUI)) {
			isUIOn = !isUIOn;
			Canvas[] allCanvases = GameObject.FindObjectsOfType<Canvas>();
			foreach (var c in allCanvases) {
				c.enabled = isUIOn;
			}
		}
	}
}
