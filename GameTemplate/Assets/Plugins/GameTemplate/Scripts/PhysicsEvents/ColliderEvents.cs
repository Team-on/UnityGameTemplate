using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ColliderEvents : MonoBehaviour {
	[SerializeField] bool useLayer = false;
	[SerializeField] [Layer] int layer;

	public CollisionEvent onTriggerEnter;
	public CollisionEvent onTriggerStay;
	public CollisionEvent onTriggerExit;

	private void OnCollisionEnter(Collision other) {
		if (IsCallTrigger(other.gameObject))
			onTriggerEnter?.Invoke(other);
	}

	private void OnCollisionStay(Collision other) {
		if (IsCallTrigger(other.gameObject))
			onTriggerStay?.Invoke(other);
	}

	private void OnCollisionExit(Collision other) {
		if (IsCallTrigger(other.gameObject))
			onTriggerExit?.Invoke(other);
	}

	bool IsCallTrigger(GameObject go) => !useLayer || layer == go.layer;
}
