using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour {
	[SerializeField] AudioClip sound;

	public void Play() {
		SoundManager.Instance.PlaySound(sound);
	}
}
