using UnityEngine;

public static class GameObjectEx {
	/// <summary>
	/// Set the layer for game object and all its childs recursively.
	/// </summary>
	/// <param name="layer">The layer for game objects.</param>
	public static void SetLayerRecursively(this GameObject gameObject, int layer) {
		gameObject.layer = layer;
		foreach (Transform child in gameObject.transform)
			child.gameObject.SetLayerRecursively(layer);
	}

	/// <summary>
	/// Set the layer for game object and all its childs recursively.
	/// </summary>
	/// <param name="layer">The layer for game objects.</param>
	public static void SetLayerRecursively(this GameObject gameObject, string layerName) {
		int layer = LayerMask.NameToLayer(layerName);
		gameObject.layer = layer;
		foreach (Transform child in gameObject.transform)
			child.gameObject.SetLayerRecursively(layer);
	}

	/// <summary>
	/// Set the tag for game object and all its childs recursively.
	/// </summary>
	/// <param name="tag">The tag for game objects.</param>
	public static void SetTagRecursively(this GameObject gameObject, string tag) {
		gameObject.tag = tag;
		foreach (Transform child in gameObject.transform)
			child.gameObject.SetTagRecursively(tag);
	}

	public static void AddChild(this GameObject gameObject, GameObject childGameObject) {
		childGameObject.transform.SetParent(gameObject.transform, false);
	}

	public static void AddChild(this GameObject gameObject, GameObject childGameObject, bool worldPositionStays) {
		childGameObject.transform.SetParent(gameObject.transform, worldPositionStays);
	}

	public static void AddChild(this GameObject gameObject, Transform childTransform) {
		childTransform.SetParent(gameObject.transform, false);
	}

	public static void AddChild(this GameObject gameObject, Transform childTransform, bool worldPositionStays) {
		childTransform.SetParent(gameObject.transform, worldPositionStays);
	}

	public static void DestroyAllChildrens(this GameObject gameObject) {
		gameObject.transform.DestroyAllChildrens();
	}
}
