using UnityEngine;

public class ScaleUpDown : MonoBehaviour {
	[Header("Params")]
	[SerializeField] bool startFromRandom = true;
	[Space] 
	[SerializeField] float minScale = 0.75f;
	[SerializeField] float maxScale = 1.0f;
	[SerializeField] float maxTime = 0.5f;

	float currTime;
	bool isIncreaseScale = false;

	void OnEnable() {
		if (startFromRandom) {
			isIncreaseScale = Random.Range(0, 2) == 1;
			currTime = Random.Range(0, maxTime);
		}

		gameObject.transform.localScale = Vector3.one * Mathf.SmoothStep(minScale, maxScale, currTime / maxTime);
	}

	void OnDisable() {
		gameObject.transform.localScale = Vector3.one;
	}

	void Update() {
		if (isIncreaseScale) {
			currTime += Time.deltaTime;
			if (currTime >= maxTime) {
				currTime = maxTime;
				isIncreaseScale = false;
			}
		}
		else {
			currTime -= Time.deltaTime;
			if (currTime <= 0) {
				currTime = 0;
				isIncreaseScale = true;
			}
		}

		gameObject.transform.localScale = Vector3.one * Mathf.SmoothStep(minScale, maxScale, currTime / maxTime);
	}
}
