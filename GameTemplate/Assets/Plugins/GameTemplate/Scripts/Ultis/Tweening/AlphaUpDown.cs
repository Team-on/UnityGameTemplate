using UnityEngine;
using UnityEngine.UI;

public class AlphaUpDown : MonoBehaviour {
	[Header("Params")]
	[SerializeField] bool startFromRandom = true;
	[Space]
	[SerializeField] float minAlpha = 0.75f;
	[SerializeField] float maxAlpha = 1.0f;
	[SerializeField] float maxTime = 0.5f;

	float currTime;
	bool isIncreaseAlpha = false;

	SpriteRenderer sr;
	Graphic graphic;

	void Awake() {
		sr = GetComponent<SpriteRenderer>();
		graphic = GetComponent<Graphic>();

		if (!sr && !graphic)
			Destroy(this);
	}

	void OnEnable() {
		if (startFromRandom) {
			isIncreaseAlpha = Random.Range(0, 2) == 1;
			currTime = Random.Range(0, maxTime);
		}

		if (graphic)
			graphic.color = graphic.color.SetA(Mathf.SmoothStep(minAlpha, maxAlpha, currTime / maxTime));
		else
			sr.color = sr.color.SetA(Mathf.SmoothStep(minAlpha, maxAlpha, currTime / maxTime));
	}

	void OnDisable() {
		if (graphic)
			graphic.color = graphic.color.SetA(Mathf.SmoothStep(minAlpha, maxAlpha, currTime / maxTime));
		else
			sr.color = sr.color.SetA(Mathf.SmoothStep(minAlpha, maxAlpha, currTime / maxTime));
	}

	void Update() {
		if (isIncreaseAlpha) {
			currTime += Time.deltaTime;
			if (currTime >= maxTime) {
				currTime = maxTime;
				isIncreaseAlpha = false;
			}
		}
		else {
			currTime -= Time.deltaTime;
			if (currTime <= 0) {
				currTime = 0;
				isIncreaseAlpha = true;
			}
		}

		if (graphic)
			graphic.color = graphic.color.SetA(Mathf.SmoothStep(minAlpha, maxAlpha, currTime / maxTime));
		else
			sr.color = sr.color.SetA(Mathf.SmoothStep(minAlpha, maxAlpha, currTime / maxTime));
	}


}
