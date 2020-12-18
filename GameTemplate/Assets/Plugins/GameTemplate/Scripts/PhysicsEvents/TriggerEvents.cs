using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TriggerEvents : MonoBehaviour {
	[SerializeField] bool useLayer = false;
	[SerializeField] [Layer] int layer;

	public ColliderEvent onTriggerEnter;
	public ColliderEvent onTriggerStay;
	public ColliderEvent onTriggerExit;

	private void OnTriggerEnter(Collider other) {
		if (IsCallTrigger(other.gameObject))
			onTriggerEnter?.Invoke(other);
	}

	private void OnTriggerStay(Collider other) {
		if (IsCallTrigger(other.gameObject))
			onTriggerStay?.Invoke(other);
	}

	private void OnTriggerExit(Collider other) {
		if (IsCallTrigger(other.gameObject))
			onTriggerExit?.Invoke(other);
	}

	bool IsCallTrigger(GameObject go) => !useLayer || layer == go.layer;
}
