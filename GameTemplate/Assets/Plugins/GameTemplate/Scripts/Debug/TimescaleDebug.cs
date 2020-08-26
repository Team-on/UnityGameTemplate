using UnityEngine;

public class TimescaleDebug : MonoBehaviour {
	[Header("Timescale keys")]
	[Space]
	[SerializeField] float minTimeScale = 0.0f;
	[SerializeField] float maxTimeScale = 2.0f;
	[SerializeField] float stepDown = 0.1f;
	[SerializeField] float stepUp = 0.1f;

	[Header("Timescale keys")]
	[Space]
	[SerializeField] KeyCode pauseKey = KeyCode.F9;
	[SerializeField] KeyCode defaultTimeKey = KeyCode.F10;
	[SerializeField] KeyCode slowDownTimeKey = KeyCode.F11;
	[SerializeField] KeyCode speedUpTimeKey = KeyCode.F12;

	float defaultScale;

	private void Start() {
		defaultScale = Time.timeScale;
	}

	void Update() {
		if (Input.GetKeyDown(pauseKey)) {
			Time.timeScale = Time.timeScale == 0 ? defaultScale : 0.0f;
		}
		else if (Input.GetKeyDown(defaultTimeKey)) {
			Time.timeScale = defaultScale;
		}
		else if (Input.GetKeyDown(slowDownTimeKey)) {
			Time.timeScale = Mathf.Clamp(Time.timeScale - stepDown, minTimeScale, maxTimeScale);
		}
		else if (Input.GetKeyDown(speedUpTimeKey)) {
			Time.timeScale = Mathf.Clamp(Time.timeScale + stepUp, minTimeScale, maxTimeScale);
		}
	}
}
