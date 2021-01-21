using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class ConstantWorldScale : MonoBehaviour {
	Vector3 desiredGlobalScale;

    void Start() {
        desiredGlobalScale = transform.lossyScale;
    }

    void Update() {
        Vector3 scaleFactor = GetGlobalToLocalScaleFactor(transform);
        Vector3 newLocalScale = new Vector3(desiredGlobalScale.x / scaleFactor.x, desiredGlobalScale.y / scaleFactor.y,  desiredGlobalScale.z / scaleFactor.z);
        transform.localScale = newLocalScale; 
    }

    public static Vector3 GetGlobalToLocalScaleFactor(Transform t) {
        Vector3 factor = Vector3.one;

        while (true) {
            Transform tParent = t.parent;

            if (tParent != null) {
                factor.x *= tParent.localScale.x;
                factor.y *= tParent.localScale.y;
                factor.z *= tParent.localScale.z;

                t = tParent;
            }
            else {
                return factor;
            }
        }
    }
}
