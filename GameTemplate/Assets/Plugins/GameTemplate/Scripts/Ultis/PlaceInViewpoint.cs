using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceInViewpoint : MonoBehaviour {
	[SerializeField] Vector2 viewpointPos = new Vector2(0.5f, 0.5f);

	private void Start() {
		transform.position = Camera.main.ViewportToWorldPoint(viewpointPos);
		Destroy(this);
	}
}
