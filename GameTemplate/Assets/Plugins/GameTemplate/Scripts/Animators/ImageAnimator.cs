using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

[RequireComponent(typeof(Image))]
public class ImageAnimator : MonoBehaviour {
	[SerializeField] bool startWithRandom = true;
	[SerializeField] bool stopAfterAnim = false;
	[SerializeField] float secondsForOneSprite = 0.35f;
	[SerializeField] [ReorderableList] Sprite[] sprites = null;
	[ReadOnly] [SerializeField] Image img = null;

	byte currSprite = 0;
	float time = 0;

#if UNITY_EDITOR
	private void OnValidate() {
		if (img == null)
			img = GetComponent<Image>();
	}
#endif

	private void Awake() {
		Reinit();
	}

	void Update() {
		time += Time.deltaTime;
		if(time >= secondsForOneSprite) {
			time -= secondsForOneSprite;
			++currSprite;
			if (currSprite == sprites.Length) {
				if (stopAfterAnim) {
					enabled = false;
					return;
				}
				currSprite = 0;
			}
			img.sprite = sprites[currSprite];
		}
	}

	public float GetDuration() {
		return secondsForOneSprite * sprites.Length + secondsForOneSprite / 2;
	}

	public void SetSprites(Sprite[] _sprites) {
		sprites = _sprites;

		Reinit();
	}

	public void SetSpritesDublicateInner(Sprite[] _sprites) {
		List<Sprite> list = new List<Sprite>(_sprites.Length * 2 - 2);
		list.AddRange(_sprites);
		for (int i = _sprites.Length - 2; i >= 1; --i)
			list.Add(_sprites[i]);

		sprites = list.ToArray();

		Reinit();
	}

	void Reinit() {
		if (startWithRandom) {
			currSprite = (byte)Random.Range(0, sprites.Length);
			time = Random.Range(0, secondsForOneSprite - Time.deltaTime);
		}
		
		img.sprite = sprites[currSprite];
	}
}
