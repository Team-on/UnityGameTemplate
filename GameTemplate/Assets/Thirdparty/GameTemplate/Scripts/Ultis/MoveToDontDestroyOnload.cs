using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToDontDestroyOnload : MonoBehaviour {
	static List<string> placed = new List<string>();

	void Awake() {
		if (placed.Contains(name)) {
			Destroy(gameObject);
			return;
		}

		placed.Add(name);
		DontDestroyOnLoad(gameObject);
		Destroy(this);
	}
}
