﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using yaSingleton;

//TODO: add audio source pooling 
[CreateAssetMenu(fileName = "Audio Manager", menuName = "Singletons/AudioManager")]
public class AudioManager : Singleton<AudioManager> {
	public enum AudioChannel : byte { Master, Music, Sound, LastChannel }

	const string SAVE_KEY_MASTER = "AudioManager.MasterVolume";
	const string SAVE_KEY_MUSIC = "AudioManager.MusicVolume";
	const string SAVE_KEY_SOUND = "AudioManager.SoundVolume";
	const string SAVE_KEY_ENABLED = "AudioManager.Enabled";

	public bool IsEnabled {
		get {
			return isEnabled;
		}
		set {
			bool prevState = isEnabled;
			isEnabled = value;
			PlayerPrefsX.SetBool(SAVE_KEY_ENABLED, isEnabled);

			if (value)
				masterMixer.SetFloat("MasterVolume", GetAdjustedVolume(PlayerPrefs.GetFloat(SAVE_KEY_MASTER, defaultMasterVolume)));
			else
				masterMixer.SetFloat("MasterVolume", GetAdjustedVolume(-80.0f));
		}
	}
	bool isEnabled;

	[Header("Audio mixer refs")]
	public AudioMixer masterMixer;
	public AudioMixerGroup masterGroup;
	public AudioMixerGroup musicGroup;
	public AudioMixerGroup soundGroup;

	[Header("3D sound settings")]
	public bool is3DGame = false;
	public float maxSoundDistance = 20.0f;
	public AnimationCurve volumeRolloff;
	public AnimationCurve spread;
	
	[Header("Music settings")]
	public float crossfadeTime = 5.0f;

	[Header("All mixers settings")]
	public int lowestDeciblesBeforeMute = -20;

	[Header("Default settings")]
	public float defaultMasterVolume = 0.75f;
	public float defaultMusicVolume = 1;
	public float defaultSoundVolume = 1;
	public bool defaultEnabled = true;

	AudioClip musicClip;
	AudioSource musicSource;

	protected override void Initialize() {
		base.Initialize();

		SceneManager.sceneLoaded += InitAfterLoad;

		void InitAfterLoad(Scene c, LoadSceneMode mode) {
			SceneManager.sceneLoaded -= InitAfterLoad;
			LeanTween.delayedCall(0.1f, () => {
				IsEnabled = PlayerPrefsX.GetBool(SAVE_KEY_ENABLED, defaultEnabled);
				SetVolume(AudioChannel.Master, PlayerPrefs.GetFloat(SAVE_KEY_MASTER, defaultMasterVolume));
				SetVolume(AudioChannel.Music, PlayerPrefs.GetFloat(SAVE_KEY_MUSIC, defaultMusicVolume));
				SetVolume(AudioChannel.Sound, PlayerPrefs.GetFloat(SAVE_KEY_SOUND, defaultSoundVolume));
			});
		}
	}

	/// <param name="volume">[0..1]</param>
	public void SetVolume(AudioChannel channel, float volume) {
		float adjustedVolume = GetAdjustedVolume(volume);

		switch (channel) {
			case AudioChannel.Master:
				if(isEnabled)
					masterMixer.SetFloat("MasterVolume", adjustedVolume);
				PlayerPrefs.SetFloat(SAVE_KEY_MASTER, volume);
				break;
			case AudioChannel.Music:
				masterMixer.SetFloat("MusicVolume", adjustedVolume);
				PlayerPrefs.SetFloat(SAVE_KEY_MUSIC, volume);
				break;
			case AudioChannel.Sound:
				masterMixer.SetFloat("SoundsVolume", adjustedVolume);
				PlayerPrefs.SetFloat(SAVE_KEY_SOUND, volume);
				break;
		}
	}

	public float GetVolume(AudioChannel channel) {
		switch (channel) {
			case AudioChannel.Master:
				return PlayerPrefs.GetFloat(SAVE_KEY_MASTER, defaultMasterVolume);
			case AudioChannel.Music:
				return PlayerPrefs.GetFloat(SAVE_KEY_MUSIC, defaultMusicVolume);
			case AudioChannel.Sound:
				return PlayerPrefs.GetFloat(SAVE_KEY_SOUND, defaultSoundVolume);
		}

		return 0.0f;
	}

	public void PlayMusic(AudioClip clip, float volume = 1.0f) {
		if(clip != musicClip || musicSource == null) {
			AudioSource oldSource = musicSource;
			musicClip = clip;
			musicSource = PlayLoop(musicClip, volume, playDelay: crossfadeTime);
			DontDestroyOnLoad(musicSource.gameObject);

			if (oldSource != null) {
				LeanTween.cancel(oldSource.gameObject, false);
				LeanTween.value(oldSource.gameObject, oldSource.volume, 0.0f, crossfadeTime)
				.setOnUpdate((float v) => {
					oldSource.volume = v;
				})
				.setOnComplete(() => {
					Destroy(oldSource.gameObject);
				});
			}
		}
	}

	public AudioSource Play3D(AudioClip clip, Transform emitter, float volume = 1.0f, float pitch = 1.0f, float playDelay = 0.0f, AudioChannel channel = AudioChannel.Sound) {
		if (!IsEnabled && channel != AudioChannel.Music)
			return null;
		AudioSource source = CreatePlaySource3D(clip, emitter, volume, pitch, playDelay, channel);
		Destroy(source.gameObject, clip.length + 1.0f);
		return source;
	}

	public AudioSource Play3D(AudioClip clip, Vector3 point, float volume = 1.0f, float pitch = 1.0f, float playDelay = 0.0f, AudioChannel channel = AudioChannel.Sound) {
		if (!IsEnabled && channel != AudioChannel.Music)
			return null;
		AudioSource source = CreatePlaySource3D(clip, point, volume, pitch, playDelay, channel);
		Destroy(source.gameObject, clip.length + 1.0f);
		return source;
	}

	public AudioSource Play(AudioClip clip, Transform emitter, float volume = 1.0f, float pitch = 1.0f, float playDelay = 0.0f, AudioChannel channel = AudioChannel.Sound) {
		if (!IsEnabled && channel != AudioChannel.Music)
			return null;
		AudioSource source = CreatePlaySource(clip, emitter, volume, pitch, playDelay, channel);
		Destroy(source.gameObject, clip.length + 1.0f);
		return source;
	}

	public AudioSource Play(AudioClip clip, Vector3 point, float volume = 1.0f, float pitch = 1.0f, float playDelay = 0.0f, AudioChannel channel = AudioChannel.Sound) {
		if (!IsEnabled && channel != AudioChannel.Music)
			return null;
		AudioSource source = CreatePlaySource(clip, point, volume, pitch, playDelay, channel);
		Destroy(source.gameObject, clip.length + 1.0f);
		return source;
	}

	public AudioSource Play(AudioClip clip, float volume = 1.0f, float pitch = 1.0f, float playDelay = 0.0f, AudioChannel channel = AudioChannel.Sound) {
		if (!IsEnabled && channel != AudioChannel.Music)
			return null;
		AudioSource source = CreatePlaySource(clip, Vector3.zero, volume, pitch, playDelay, channel);
		Destroy(source.gameObject, clip.length + 1.0f);
		return source;
	}

	public AudioSource PlayLoop(AudioClip clip, Transform emitter, float volume = 1.0f, float pitch = 1.0f, float playDelay = 0.0f, AudioChannel channel = AudioChannel.Music) {
		AudioSource source = CreatePlaySource(clip, emitter, volume, pitch, playDelay, channel);
		source.loop = true;
		return source;
	}

	public AudioSource PlayLoop(AudioClip clip, Vector3 point, float volume = 1.0f, float pitch = 1.0f, float playDelay = 0.0f, AudioChannel channel = AudioChannel.Music) {
		AudioSource source = CreatePlaySource(clip, point, volume, pitch, playDelay, channel);
		source.loop = true;
		return source;
	}

	public AudioSource PlayLoop(AudioClip clip, float volume = 1.0f, float pitch = 1.0f, float playDelay = 0.0f, AudioChannel channel = AudioChannel.Music) {
		AudioSource source = CreatePlaySource(clip, Vector3.zero, volume, pitch, playDelay, channel);
		source.loop = true;
		return source;
	}

	AudioSource CreatePlaySource(AudioClip clip, Transform emitter, float volume, float pitch, float playDelay, AudioChannel channel) {
		GameObject go = new GameObject("Audio: " + clip.name);
		go.transform.position = emitter.position;
		go.transform.parent = emitter;

		AudioSource source = AddAudioSource(go, clip, volume, pitch, channel);

		PlayDelayed(source, playDelay);
		return source;
	}

	AudioSource CreatePlaySource(AudioClip clip, Vector3 point, float volume, float pitch, float playDelay, AudioChannel channel) {
		GameObject go = new GameObject("Audio: " + clip.name);
		go.transform.position = point;

		AudioSource source = AddAudioSource(go, clip, volume, pitch, channel);

		PlayDelayed(source, playDelay);
		return source;
	}

	AudioSource AddAudioSource(GameObject go, AudioClip clip, float volume, float pitch, AudioChannel channel) {
		AudioSource source = go.AddComponent<AudioSource>();
		source.clip = clip;
		source.volume = volume;
		source.pitch = pitch;
		source.outputAudioMixerGroup = GetAudioMixer(channel);

		return source;
	}

	AudioSource CreatePlaySource3D(AudioClip clip, Transform emitter, float volume, float pitch, float playDelay, AudioChannel channel) {
		GameObject go = new GameObject("Audio: " + clip.name);
		go.transform.parent = emitter;
		if(is3DGame)
			go.transform.position = emitter.position;
		else
			go.transform.position = new Vector3(emitter.position.x, emitter.position.y, TemplateGameManager.Instance.Camera.transform.position.z);

		AudioSource source = AddAudioSource3D(go, clip, volume, pitch, channel);

		PlayDelayed(source, playDelay);
		return source;
	}

	AudioSource CreatePlaySource3D(AudioClip clip, Vector3 point, float volume, float pitch, float playDelay, AudioChannel channel) {
		GameObject go = new GameObject("Audio: " + clip.name);
		if(is3DGame)
			go.transform.position = point;
		else
			go.transform.position = new Vector3(point.x, point.y, TemplateGameManager.Instance.Camera.transform.position.z);

		AudioSource source = AddAudioSource3D(go, clip, volume, pitch, channel);

		PlayDelayed(source, playDelay);
		return source;
	}

	AudioSource AddAudioSource3D(GameObject go, AudioClip clip, float volume, float pitch, AudioChannel channel) {
		AudioSource source = go.AddComponent<AudioSource>();
		source.clip = clip;
		source.pitch = pitch;
		source.outputAudioMixerGroup = GetAudioMixer(channel);
		
		source.minDistance = 0.0f;
		source.maxDistance = maxSoundDistance;
		source.spatialBlend = 1.0f;

		source.rolloffMode = AudioRolloffMode.Custom;
		source.SetCustomCurve(AudioSourceCurveType.CustomRolloff, volumeRolloff);
		source.SetCustomCurve(AudioSourceCurveType.Spread, spread);

		return source;
	}

	float GetAdjustedVolume(float volume) {
		return volume <= 0.0001f ? -80f : Mathf.Log10(volume) * 20;
	}

	void PlayDelayed(AudioSource source, float delay) {
		if (delay == 0)
			source.Play();
		else {
			float savedVolume = source.volume;
			source.volume = 0.0f;
			source.Play();
			LeanTween.cancel(source.gameObject, false);
			LeanTween.value(source.gameObject, source.volume, savedVolume, delay)
				.setOnUpdate((float v)=> { 
					source.volume = v;
				});
		}
	}

	AudioMixerGroup GetAudioMixer(AudioChannel channel) {
		switch (channel) {
			case AudioChannel.Master:
				return masterGroup;
			case AudioChannel.Music:
				return musicGroup;
			case AudioChannel.Sound:
				return soundGroup;
			default:
				return null;
		}
	}
}