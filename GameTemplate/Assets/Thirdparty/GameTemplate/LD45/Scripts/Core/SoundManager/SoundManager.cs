using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager> {
	const float musicChangeTime = 1.0f;

	public float MasterVolume {
		get {
			return _masterVolume;
		}
		set {
			_masterVolume = value;
			musicSource.volume = _musicVolume * MasterVolume;
			soundSource.volume = _soundVolume * MasterVolume;
		}
	}
	public float MusicVolume {
		get {
			return _musicVolume;
		}
		set {
			_musicVolume = value;
			musicSource.volume = _musicVolume * MasterVolume;
		}
	}
	public float SoundVolume {
		get {
			return _soundVolume;
		}
		set {
			_soundVolume = value;
			soundSource.volume = _soundVolume * MasterVolume;
		}
	}
	public float _masterVolume;
	public float _musicVolume;
	public float _soundVolume;

	AudioSource musicSource;
	AudioSource soundSource;
	Coroutine changeMusicCoroutine;

	protected SoundManager() { }

	void Awake() {
		//TODO: save/load this values
		_masterVolume = _musicVolume = _soundVolume = 1.0f;

		soundSource = CreateAS("soundSource");
		soundSource.volume = SoundVolume * MasterVolume;

		musicSource = CreateAS("musicSource");
		musicSource.volume = MusicVolume * MasterVolume;

		AudioSource CreateAS(string name) {
			GameObject soundgo = new GameObject();
			soundgo.transform.parent = transform;
			soundgo.name = name;
			return soundgo.AddComponent<AudioSource>();
		}
	}

	public void PlaySound(AudioClip clip) {
		soundSource.PlayOneShot(clip);
	}

	public void PlayMusic(AudioClip music) {
		changeMusicCoroutine = StartCoroutine(ChangeMusic());

		IEnumerator ChangeMusic() {
			float timer = musicChangeTime;
			float startMusicVolume = musicSource.volume;

			while ((timer -= Time.deltaTime) > 0) {
				musicSource.volume = startMusicVolume * timer / musicChangeTime;
				yield return null;
			}

			yield return null;
			musicSource.Stop();
			yield return null;
			musicSource.volume = MusicVolume * MasterVolume;
			musicSource.clip = music;
			musicSource.Play();
		}
	}
}
