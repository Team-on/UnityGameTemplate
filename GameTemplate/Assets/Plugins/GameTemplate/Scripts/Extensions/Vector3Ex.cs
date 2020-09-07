using UnityEngine;

public static class Vector3Ex {
	public static Vector3 With(this Vector3 original, float? x = null, float? y = null, float? z = null) {
		return new Vector3(x ?? original.x, y ?? original.y, z ?? original.z);
	}

	public static Vector3 ToVector3(this Vector3 v) {
		return new Vector3(v.x, v.y, 0.0f);
	}

	public static Vector3 SetX(this Vector3 v, float x) {
		return new Vector3(x, v.y, v.z);
	}

	public static Vector3 SetY(this Vector3 v, float y) {
		return new Vector3(v.x, y, v.z);
	}

	public static Vector3 SetZ(this Vector3 v, float z) {
		return new Vector3(v.x, v.y, z);
	}

	public static Vector3 SetZ(this Vector2 v, float z) {
		return new Vector3(v.x, v.y, z);
	}

	public static Vector3 ChangeX(this Vector3 v, float x) {
		return new Vector3(v.x + x, v.y, v.z);
	}

	public static Vector3 ChangeY(this Vector3 v, float y) {
		return new Vector3(v.x, v.y + y, v.z);
	}

	public static Vector3 ChangeZ(this Vector3 v, float z) {
		return new Vector3(v.x, v.y, v.z + z);
	}

	public static Vector3 ChangeZ(this Vector2 v, float z) {
		return new Vector3(v.x, v.y, z);
	}
}
