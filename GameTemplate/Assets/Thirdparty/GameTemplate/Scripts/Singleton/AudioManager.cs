using UnityEngine;
using UnityEngine.Audio;
using yaSingleton;

//TODO: add audio source pooling 
[CreateAssetMenu(fileName = "Audio Manager", menuName = "Singletons/AudioManager")]
public class AudioManager : Singleton<AudioManager> {
	const string SAVE_KEY_MASTER = "AudioManager.MasterVolume";
	const string SAVE_KEY_MUSIC = "AudioManager.MusicVolume";
	const string SAVE_KEY_SOUND = "AudioManager.SoundVolume";

	public enum AudioChannel : byte { None, Master, Music, Sound, LastChannel }

	public AudioMixer masterMixer;
	public AudioMixerGroup masterGroup;
	public AudioMixerGroup musicGroup;
	public AudioMixerGroup soundGroup;

	public int lowestDeciblesBeforeMute = -20;

	protected override void Initialize() {
		base.Initialize();

		SetVolume(AudioChannel.Master, PlayerPrefs.GetInt(SAVE_KEY_MASTER, 100));
		SetVolume(AudioChannel.Music, PlayerPrefs.GetInt(SAVE_KEY_MUSIC, 100));
		SetVolume(AudioChannel.Sound, PlayerPrefs.GetInt(SAVE_KEY_SOUND, 100));
	}

	/// <param name="volume">[0, 100]</param>
	public void SetVolume(AudioChannel channel, int volume) {
		float adjustedVolume = volume <= 0 ? 0.0001f : Mathf.Log10(volume) * 20;

		switch (channel) {
			case AudioChannel.Master:
				masterMixer.SetFloat("MasterVolume", adjustedVolume);
				PlayerPrefs.SetInt(SAVE_KEY_MASTER, volume);
				break;
			case AudioChannel.Music:
				masterMixer.SetFloat("MusicVolume", adjustedVolume);
				PlayerPrefs.SetInt(SAVE_KEY_MUSIC, volume);
				break;
			case AudioChannel.Sound:
				masterMixer.SetFloat("SoundsVolume", adjustedVolume);
				PlayerPrefs.SetInt(SAVE_KEY_SOUND, volume);
				break;

		}
	}

	public AudioMixerGroup GetAudioMixer(AudioChannel channel) {
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

	public AudioSource Play(AudioClip clip, Transform emitter, float volume = 1.0f, float pitch = 1.0f, AudioChannel channel = AudioChannel.None) {
		AudioSource source = CreatePlaySource(clip, emitter, volume, pitch, channel);
		Destroy(source.gameObject, clip.length + 1.0f);
		return source;
	}

	public AudioSource Play(AudioClip clip, Vector3 point, float volume = 1.0f, float pitch = 1.0f, AudioChannel channel = AudioChannel.None) {
		AudioSource source = CreatePlaySource(clip, point, volume, pitch, channel);
		Destroy(source.gameObject, clip.length + 1.0f);
		return source;
	}

	public AudioSource Play(AudioClip clip, float volume = 1.0f, float pitch = 1.0f, AudioChannel channel = AudioChannel.None) {
		AudioSource source = CreatePlaySource(clip, Vector3.zero, volume, pitch, channel);
		Destroy(source.gameObject, clip.length + 1.0f);
		return source;
	}

	public AudioSource PlayLoop(AudioClip clip, Transform emitter, float volume = 1.0f, float pitch = 1.0f, AudioChannel channel = AudioChannel.None) {
		AudioSource source = CreatePlaySource(clip, emitter, volume, pitch, channel);
		source.loop = true;
		return source;
	}

	public AudioSource PlayLoop(AudioClip clip, Vector3 point, float volume = 1.0f, float pitch = 1.0f, AudioChannel channel = AudioChannel.None) {
		AudioSource source = CreatePlaySource(clip, point, volume, pitch, channel);
		source.loop = true;
		return source;
	}

	public AudioSource PlayLoop(AudioClip clip, float volume = 1.0f, float pitch = 1.0f, AudioChannel channel = AudioChannel.None) {
		AudioSource source = CreatePlaySource(clip, Vector3.zero, volume, pitch, channel);
		source.loop = true;
		return source;
	}

	AudioSource CreatePlaySource(AudioClip clip, Transform emitter, float volume, float pitch, AudioChannel channel) {
		GameObject go = new GameObject("Audio: " + clip.name);
		go.transform.position = emitter.position;
		go.transform.parent = emitter;

		AudioSource source = AddAudioSource(go, clip, volume, pitch, channel);

		source.Play();
		return source;
	}

	AudioSource CreatePlaySource(AudioClip clip, Vector3 point, float volume, float pitch, AudioChannel channel) {
		GameObject go = new GameObject("Audio: " + clip.name);
		go.transform.position = point;

		AudioSource source = AddAudioSource(go, clip, volume, pitch, channel);

		source.Play();
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
}
