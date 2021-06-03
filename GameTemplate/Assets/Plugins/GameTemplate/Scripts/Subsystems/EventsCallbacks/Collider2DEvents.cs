using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Collider2DEvents : MonoBehaviour {
	[SerializeField] bool useLayer = false;
	[SerializeField] [Layer] int layer;

	public Collision2DEvent onTriggerEnter;
	public Collision2DEvent onTriggerStay;
	public Collision2DEvent onTriggerExit;

	private void OnCollisionEnter2D(Collision2D other) {
		if (IsCallTrigger(other.gameObject))
			onTriggerEnter?.Invoke(other);
	}

	private void OnCollisionStay2D(Collision2D other) {
		if (IsCallTrigger(other.gameObject))
			onTriggerStay?.Invoke(other);
	}

	private void OnCollisionExit2D(Collision2D other) {
		if (IsCallTrigger(other.gameObject))
			onTriggerExit?.Invoke(other);
	}

	bool IsCallTrigger(GameObject go) => !useLayer || layer == go.layer;
}
