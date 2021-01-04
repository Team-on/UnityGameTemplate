using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotating : MonoBehaviour {
	[SerializeField] Vector3 rotationSpeed = new Vector3(180, 180, 180);

	private void Awake() {
		transform.Rotate(
			rotationSpeed.x == 0 ? 0 : Random.Range(0f, 360f),
			rotationSpeed.y == 0 ? 0 : Random.Range(0f, 360f),
			rotationSpeed.z == 0 ? 0 : Random.Range(0f, 360f)
		);
	}

	void Update() {
		transform.Rotate(rotationSpeed * Time.deltaTime);
	}
}
