using UnityEngine;

public static class Vector2Ex {
	public static float Lerp(this Vector2 vector, float t) {
		return Mathf.Lerp(vector.x, vector.y, t);
	}

	public static float Lerp(this Vector2 vector, float numerator, float denominator) {
		return Lerp(vector, numerator / denominator);
	}

	public static int LerpInt(this Vector2 vector, float t) {
		return Mathf.RoundToInt(Mathf.Lerp((int)vector.x, (int)vector.y, t));
	}

	public static int LerpInt(this Vector2 vector, float numerator, float denominator) {
		return LerpInt(vector, numerator / denominator);
	}

	public static int Lerp(this Vector2Int vector, float t) {
		return Mathf.RoundToInt(Mathf.Lerp(vector.x, vector.y, t));
	}

	public static int Lerp(this Vector2Int vector, float numerator, float denominator) {
		return Lerp(vector, numerator / denominator);
	}



	public static float InverseLerp(this Vector2 vector, float value) {
		return Mathf.InverseLerp(vector.x, vector.y, value);
	}

	public static float InverseLerpInt(this Vector2 vector, int value) {
		return Mathf.InverseLerp((int)vector.x, (int)vector.y, value);
	}

	public static float InverseLerp(this Vector2Int vector, int value) {
		return Mathf.InverseLerp(vector.x, vector.y, value);
	}
}
