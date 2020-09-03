using UnityEngine;

public static class ColorEx {
	public static Color SetR(this Color color, float value) {
		return new Color(value, color.g, color.b, color.a);
	}

	public static Color SetG(this Color color, float value) {
		return new Color(color.r, value, color.b, color.a);
	}

	public static Color SetB(this Color color, float value) {
		return new Color(color.r, color.g, value, color.a);
	}

	public static Color SetA(this Color color, float value) {
		return new Color(color.r, color.g, color.b, value);
	}
}
