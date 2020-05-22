using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotating : MonoBehaviour {
	[SerializeField] float rotationSpeed = 180f;

	private void Awake() {
		transform.Rotate(0, 0, Random.Range(0f, 360f));
	}

	void Update() {
		transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
	}
}
