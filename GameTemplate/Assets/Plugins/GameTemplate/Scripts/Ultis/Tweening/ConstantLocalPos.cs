using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class ConstantLocalPos : MonoBehaviour {
	Vector3 localPos;

	void Start() {
        localPos = transform.localPosition;
	}

	void Update() {
        transform.localPosition = new Vector3(localPos.x / Mathf.Abs(transform.parent.lossyScale.x), localPos.y / Mathf.Abs(transform.parent.lossyScale.y), localPos.z / Mathf.Abs(transform.parent.lossyScale.z));
    }
}
