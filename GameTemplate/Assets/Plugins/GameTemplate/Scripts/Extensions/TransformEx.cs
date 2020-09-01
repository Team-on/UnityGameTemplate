using UnityEngine;

public static class TransformEx {
    public static Transform DestroyAllChildrens(this Transform transform) {
        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }

        transform.DetachChildren();

        return transform;
    }
}
