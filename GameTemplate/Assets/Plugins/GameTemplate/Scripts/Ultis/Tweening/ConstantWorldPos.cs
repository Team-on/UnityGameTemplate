using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class ConstantWorldPos : MonoBehaviour {
	Vector3 desiredWorldPos;

	void Start() {
        desiredWorldPos = transform.position;
	}

	void Update() {
        transform.position = desiredWorldPos;
    }
}
