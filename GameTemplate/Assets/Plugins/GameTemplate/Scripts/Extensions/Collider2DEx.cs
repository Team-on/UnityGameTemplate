using UnityEngine;

public static class Collider2DEx {
	public static Vector2 GetRandomWorldPointInsideCollider(this Collider2D collider) {
		var bounds = collider.bounds;
		var center = bounds.center;
		float minX = center.x - bounds.extents.x;
		float maxX = center.x + bounds.extents.x;
		float minY = center.y - bounds.extents.y;
		float maxY = center.y + bounds.extents.y;

		Vector2 rez;
		int tries = 100;
		do {
			rez = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
		} while (!collider.OverlapPoint(rez) && --tries >= 0);

		return rez;
	}

	public static Vector2 GetRandomLocalPointInsideCollider(this Collider2D collider) {
		return GetRandomWorldPointInsideCollider(collider) - (Vector2)collider.transform.position;
	}
}
