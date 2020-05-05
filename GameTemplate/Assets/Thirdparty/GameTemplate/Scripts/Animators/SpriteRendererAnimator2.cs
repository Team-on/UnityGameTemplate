using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteRendererAnimator2 : MonoBehaviour {
	public byte currSequence = 0;

	[SerializeField] bool startWithRandom = true;
	[SerializeField] float secondsForOneSprite = 0.35f;
	[SerializeField] [ReorderableList] Sprite[] sprites0 = null;
	[SerializeField] [ReorderableList] Sprite[] sprites1 = null;
	[ReadOnly] [SerializeField] SpriteRenderer sr = null;

	byte currSprite = 0;
	float time = 0;

#if UNITY_EDITOR
	private void OnValidate() {
		if (sr == null)
			sr = GetComponent<SpriteRenderer>();
	}
#endif

	private void Awake() {
		Sprite[] sprites = GetCurrSequence();

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
		if (time >= secondsForOneSprite) {
			Sprite[] sprites = GetCurrSequence();
			time -= secondsForOneSprite;
			++currSprite;
			if (currSprite == sprites.Length)
				currSprite = 0;
			sr.sprite = sprites[currSprite];
		}
	}

	public void SetSequenceForce(byte _currSequence) {
		currSequence = _currSequence;
		sr.sprite = GetCurrSequence()[currSprite];
	}

	Sprite[] GetCurrSequence() {
		switch (currSequence) {
			case 0:
				return sprites0;
			case 1:
				return sprites1;
		}
		return sprites0;
	}
}
