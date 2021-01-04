using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAsSceneChild : MonoBehaviour {
	void Awake() {
		transform.SetParent(null);
		Destroy(this);
	}
}
