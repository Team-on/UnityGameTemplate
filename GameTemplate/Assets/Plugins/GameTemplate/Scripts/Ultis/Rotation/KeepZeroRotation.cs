using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepZeroRotation : MonoBehaviour
{
    static Quaternion zeroQuaterion = Quaternion.Euler(Vector3.zero);

    private void LateUpdate() {
        transform.rotation = zeroQuaterion;
    }
}
