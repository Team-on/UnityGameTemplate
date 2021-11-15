using UnityEngine;

public static class Vector2Ex {
	public static Vector2 With(this Vector2 original, float? x = null, float? y = null) {
		return new Vector2(x ?? original.x, y ?? original.y);
	}

	public static Vector2 ToVector2(this Vector3 v) {
		return new Vector2(v.x, v.y);
	}

	public static Vector2 SetX(this Vector2 v, float x) {
		return new Vector2(x, v.y);
	}

	public static Vector2 SetY(this Vector2 v, float y) {
		return new Vector2(v.x, y);
	}

	public static Vector2 ChangeX(this Vector2 v, float x) {
		return new Vector2(v.x + x, v.y);
	}

	public static Vector2 ChangeY(this Vector2 v, float y) {
		return new Vector2(v.x, v.y + y);
	}

	public static string MinMaxValueToString(this Vector2 value, int numbersAfterComma = 0) {
		return MinMaxValueToString(value.x, value.y, numbersAfterComma);
	}

	public static string MinMaxValueToString(float minValue, float maxValue, int numbersAfterComma) {
		if (minValue > maxValue) {
			float temp = maxValue;
			maxValue = minValue;
			minValue = temp;
		}

		if (numbersAfterComma == 0) {
			if (minValue == maxValue)
				return Mathf.RoundToInt(minValue).ToString();
			return Mathf.RoundToInt(minValue).ToString() + " - " + Mathf.RoundToInt(maxValue).ToString();
		}
		else {
			if (minValue == maxValue)
				return minValue.ToString();

			string precisionString = "0." + new string('#', numbersAfterComma);
			;
			return minValue.ToString(precisionString) + " - " + maxValue.ToString(precisionString);
		}

	}
}
