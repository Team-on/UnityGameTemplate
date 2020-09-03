using UnityEngine;

public static class SpriteRendererEx {
    public static void SetColorR(this SpriteRenderer spriteRenderer, float value) {
		spriteRenderer.color = spriteRenderer.color.SetR(value);
	}

	public static void SetG(this SpriteRenderer spriteRenderer, float value) {
		spriteRenderer.color = spriteRenderer.color.SetG(value);
	}

	public static void SetB(this SpriteRenderer spriteRenderer, float value) {
		spriteRenderer.color = spriteRenderer.color.SetB(value);
	}

	public static void SetA(this SpriteRenderer spriteRenderer, float value) {
		spriteRenderer.color = spriteRenderer.color.SetA(value);
	}
}