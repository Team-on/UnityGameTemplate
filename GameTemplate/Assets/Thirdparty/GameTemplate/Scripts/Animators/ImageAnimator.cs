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
		if (startWithRandom) {
			currSprite = (byte)Random.Range(0, sprites.Length);
			img.sprite = sprites[currSprite];
			time = Random.Range(0, secondsForOneSprite - Time.deltaTime);
		}
		else {
			img.sprite = sprites[currSprite];
		}
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
}
