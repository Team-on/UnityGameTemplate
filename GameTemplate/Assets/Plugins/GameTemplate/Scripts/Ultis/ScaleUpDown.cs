using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScaleUpDown : MonoBehaviour {
	[Header("Params")]
	[SerializeField] float minScale = 0.75f;
	[SerializeField] float maxScale = 1.0f;
	[SerializeField] float maxTime = 0.5f;

	float currTime;
	bool isIncreaseScale = false;

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

		gameObject.transform.localScale = Vector3.one * Mathf.SmoothStep(0.66f, 1.0f, currTime / maxTime);
	}
}
