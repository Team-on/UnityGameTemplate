using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteRendererAnimator : MonoBehaviour {
	[SerializeField] bool startWithRandom = true;
	[SerializeField] float secondsForOneSprite = 0.35f;
	[SerializeField] [ReorderableList] Sprite[] sprites = null;
	[ReadOnly] [SerializeField] SpriteRenderer sr = null;

	byte currSprite = 0;
	float time = 0;

#if UNITY_EDITOR
	private void OnValidate() {
		if (sr == null)
			sr = GetComponent<SpriteRenderer>();
	}
#endif

	private void Start() {
		if (startWithRandom) {
			currSprite = (byte)Random.Range(0, sprites.Length);
			sr.sprite = sprites[currSprite];
			time = Random.Range(0, secondsForOneSprite - Time.deltaTime);
		}
		else {
			sr.sprite = sprites[currSprite];
		}
	}

	void Update() {
		time += Time.deltaTime;
		if(time >= secondsForOneSprite) {
			time -= secondsForOneSprite;
			++currSprite;
			if (currSprite == sprites.Length)
				currSprite = 0;
			if (sr)
				sr.sprite = sprites[currSprite];
		}
	}
}
