using UnityEngine;

public class RandomSprite : MonoBehaviour {
	[SerializeField] Sprite[] sprites; 

	[SerializeField] SpriteRenderer sr;

#if UNITY_EDITOR
	private void OnValidate() {
		if (sr == null)
			sr = GetComponent<SpriteRenderer>();
	}
#endif

	private void Awake() {
		sr.sprite = sprites.Random();
		Destroy(this);
	}
}
