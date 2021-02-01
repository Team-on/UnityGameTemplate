using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteRendererAnimator3 : MonoBehaviour {
	public byte currSequence = 0;

	[SerializeField] bool startWithRandom = true;
	[SerializeField] float secondsForOneSprite = 0.35f;
	[SerializeField] [ReorderableList] Sprite[] sprites0 = null;
	[SerializeField] [ReorderableList] Sprite[] sprites1 = null;
	[SerializeField] [ReorderableList] Sprite[] sprites2 = null;
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
		Reinit();
	}

	void Update() {
		time += Time.deltaTime;
		if(time >= secondsForOneSprite) {
			Sprite[] sprites = GetCurrSequence();
			time -= secondsForOneSprite;
			++currSprite;
			if (currSprite == sprites.Length)
				currSprite = 0;
			sr.sprite = sprites[currSprite];
		}
	}

	public void SetSprites(Sprite[] _sprites, int id) {
		switch (id) {
			case 0:
				sprites0 = _sprites;
				break;
			case 1:
				sprites1 = _sprites;
				break;
			case 2:
				sprites2 = _sprites;
				break;
		}

		Reinit();
	}

	public void SetSpritesDublicateInner(Sprite[] _sprites, int id) {
		List<Sprite> list = new List<Sprite>(_sprites.Length * 2 - 2);
		list.AddRange(_sprites);
		for (int i = _sprites.Length - 2; i >= 1; --i)
			list.Add(_sprites[i]);

		switch (id) {
			case 0:
				sprites0 = list.ToArray();
				break;
			case 1:
				sprites1 = list.ToArray();
				break;
			case 2:
				sprites2 = list.ToArray();
				break;
		}

		Reinit();
	}

	void Reinit() {
		Sprite[] sprites = GetCurrSequence();

		if (startWithRandom) {
			currSprite = (byte)Random.Range(0, sprites.Length);
			time = Random.Range(0, secondsForOneSprite - Time.deltaTime);
		}

		sr.sprite = sprites[currSprite];
	}

	Sprite[] GetCurrSequence() {
		switch (currSequence) {
			case 0:
				return sprites0;
			case 1:
				return sprites1;
			case 2:
				return sprites2;
		}
		return sprites0;
	}
}
