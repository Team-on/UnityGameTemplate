using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Trigger2DEvents : MonoBehaviour {
	[SerializeField] bool useLayer = false;
	[SerializeField] [Layer] int layer;

	public Collider2DEvent onTriggerEnter;
	public Collider2DEvent onTriggerStay;
	public Collider2DEvent onTriggerExit;

	private void OnTriggerEnter2D(Collider2D other) {
		if (IsCallTrigger(other.gameObject))
			onTriggerEnter?.Invoke(other);
	}

	private void OnTriggerStay2D(Collider2D other) {
		if (IsCallTrigger(other.gameObject))
			onTriggerStay?.Invoke(other);
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (IsCallTrigger(other.gameObject))
			onTriggerExit?.Invoke(other);
	}

	bool IsCallTrigger(GameObject go) => !useLayer || layer == go.layer;
}
