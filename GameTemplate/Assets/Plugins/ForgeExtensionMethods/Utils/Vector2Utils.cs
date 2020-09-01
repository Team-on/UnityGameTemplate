using UnityEngine;

public static class Vector2Utils {
	public static Vector2 SetX(this Vector2 vector, float value) {
		return new Vector2(value, vector.y);
	}

	public static Vector2 SetY(this Vector2 vector, float value) {
		return new Vector2(vector.x, value);
	}
}
