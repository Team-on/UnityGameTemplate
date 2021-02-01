using UnityEngine;

public static class TransformEx {
	/// <summary>
	/// Rotates the transform so the forward vector points from target's current position.
	/// </summary>
	/// <param name="target">Object to point away from.</param>
	public static void LookFrom(this Transform transform, Transform target) {
		transform.LookFrom(target.position);
	}

	/// <summary>
	/// Rotates the transform so the forward vector points from target's current position.
	/// </summary>
	/// <param name="target">Object to point away from.</param>
	/// <param name="worldUp">Vector specifying the upward direction.</param>
	public static void LookFrom(this Transform transform, Transform target, Vector3 worldUp) {
		transform.LookFrom(target.position, worldUp);
	}

	/// <summary>
	/// Rotates the transform so the forward vector points from worldPosition.
	/// </summary>
	/// <param name="worldPosition">Point to look at.</param>
	public static void LookFrom(this Transform transform, Vector3 worldPosition) {
		transform.rotation = Quaternion.LookRotation(transform.position - worldPosition);
	}

	/// <summary>
	/// Rotates the transform so the forward vector points from worldPosition.
	/// </summary>
	/// <param name="worldPosition">Point to look at.</param>
	/// <param name="worldUp">Vector specifying the upward direction.</param>
	public static void LookFrom(this Transform transform, Vector3 worldPosition, Vector3 worldUp) {
		transform.rotation = Quaternion.LookRotation(transform.position - worldPosition, worldUp);
	}

	public static void SetX(this Transform transform, float value) {
		transform.position = transform.position.SetX(value);
	}

	public static void SetY(this Transform transform, float value) {
		transform.position = transform.position.SetY(value);
	}

	public static void SetZ(this Transform transform, float value) {
		transform.position = transform.position.SetZ(value);
	}

	public static void SetLocalX(this Transform transform, float value) {
		transform.localPosition = transform.localPosition.SetX(value);
	}

	public static void SetLocalY(this Transform transform, float value) {
		transform.localPosition = transform.localPosition.SetY(value);
	}

	public static void SetLocalZ(this Transform transform, float value) {
		transform.localPosition = transform.localPosition.SetZ(value);
	}

	public static void ChangeX(this Transform transform, float value) {
		transform.position = transform.position.SetX(transform.position.x + value);
	}

	public static void ChangeY(this Transform transform, float value) {
		transform.position = transform.position.SetY(transform.position.y + value);
	}

	public static void ChangeZ(this Transform transform, float value) {
		transform.position = transform.position.SetZ(transform.position.z + value);
	}

	public static void ChangeLocalX(this Transform transform, float value) {
		transform.localPosition = transform.localPosition.SetX(transform.localPosition.x + value);
	}

	public static void ChangeLocalY(this Transform transform, float value) {
		transform.localPosition = transform.localPosition.SetY(transform.localPosition.y + value);
	}

	public static void ChangeLocalZ(this Transform transform, float value) {
		transform.localPosition = transform.localPosition.SetZ(transform.localPosition.z + value);
	}

	public static void AddChild(this Transform transform, GameObject childGameObject) {
		childGameObject.transform.SetParent(transform, false);
	}

	public static void AddChild(this Transform transform, GameObject childGameObject, bool worldPositionStays) {
		childGameObject.transform.SetParent(transform, worldPositionStays);
	}

	public static void AddChild(this Transform transform, Transform childTransform) {
		childTransform.SetParent(transform, false);
	}

	public static void AddChild(this Transform transform, Transform childTransform, bool worldPositionStays) {
		childTransform.SetParent(transform, worldPositionStays);
	}

	public static Transform DestroyAllChildrens(this Transform transform) {
		foreach (Transform child in transform)
			GameObject.Destroy(child.gameObject);

		transform.DetachChildren();

		return transform;
	}

	public static bool IsChildOf(this Transform t, Transform wantedParent) {
		Transform selectedParent = t.parent;

		while (selectedParent != null) {
			if (selectedParent == wantedParent)
				return true;

			selectedParent = selectedParent.parent;
		}

		return false;
	}
}
