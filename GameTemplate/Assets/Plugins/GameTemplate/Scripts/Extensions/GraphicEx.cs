using UnityEngine.UI;

public static class GraphicEx {
	public static void SetR(this Graphic graphic, float value) {
		graphic.color = graphic.color.SetR(value);
	}

	public static void SetG(this Graphic graphic, float value) {
		graphic.color = graphic.color.SetG(value);
	}

	public static void SetB(this Graphic graphic, float value) {
		graphic.color = graphic.color.SetB(value);
	}

	public static void SetA(this Graphic graphic, float value) {
		graphic.color = graphic.color.SetA(value);
	}
}
