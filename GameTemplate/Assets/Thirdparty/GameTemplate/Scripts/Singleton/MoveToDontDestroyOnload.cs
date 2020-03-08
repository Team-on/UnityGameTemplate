using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToDontDestroyOnload : MonoBehaviour {
	void Awake() {
		DontDestroyOnLoad(gameObject);
		Destroy(this);
	}
}
