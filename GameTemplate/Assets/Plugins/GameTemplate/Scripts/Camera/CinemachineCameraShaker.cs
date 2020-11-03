using Cinemachine;
using System.Collections;
using UnityEngine;

public class CinemachineCameraShaker : MonoBehaviour {
	float shakeDuration;
	float shakeAmplitude;
	float shakeFrequency;
	float currShakeTime;

	bool isRunning = false;

	protected CinemachineVirtualCamera _virtualCamera;
	protected CinemachineBasicMultiChannelPerlin noice;

	NoiseSettings.TransformNoiseParams[] shakeX = new NoiseSettings.TransformNoiseParams[4] {
				new NoiseSettings.TransformNoiseParams(){
					X = new NoiseSettings.NoiseParams(){ Frequency = 3.2f, Amplitude = 0.4f, Constant = false},
					Y = new NoiseSettings.NoiseParams(){ Frequency = 0, Amplitude = 0, Constant = false},
					Z = new NoiseSettings.NoiseParams(){ Frequency = 0, Amplitude = 0, Constant = false},
				},
				new NoiseSettings.TransformNoiseParams(){
					X = new NoiseSettings.NoiseParams(){ Frequency = 7.7f, Amplitude = 0.24f, Constant = false},
					Y = new NoiseSettings.NoiseParams(){ Frequency = 0, Amplitude = 0, Constant = false},
					Z = new NoiseSettings.NoiseParams(){ Frequency = 0, Amplitude = 0, Constant = false},
				},
				new NoiseSettings.TransformNoiseParams(){
					X = new NoiseSettings.NoiseParams(){ Frequency = 42f, Amplitude = 0.44f, Constant = false},
					Y = new NoiseSettings.NoiseParams(){ Frequency = 0, Amplitude = 0, Constant = false},
					Z = new NoiseSettings.NoiseParams(){ Frequency = 0, Amplitude = 0, Constant = false},
				},
				new NoiseSettings.TransformNoiseParams(){
					X = new NoiseSettings.NoiseParams(){ Frequency = 123f, Amplitude = 0.45f, Constant = false},
					Y = new NoiseSettings.NoiseParams(){ Frequency = 0, Amplitude = 0, Constant = false},
					Z = new NoiseSettings.NoiseParams(){ Frequency = 0, Amplitude = 0, Constant = false},
				},
			};
	NoiseSettings.TransformNoiseParams[] shakeY = new NoiseSettings.TransformNoiseParams[4] {
				new NoiseSettings.TransformNoiseParams(){
					X = new NoiseSettings.NoiseParams(){ Frequency = 0, Amplitude = 0, Constant = false},
					Y = new NoiseSettings.NoiseParams(){ Frequency =  5.2f, Amplitude = 0.22f, Constant = false},
					Z = new NoiseSettings.NoiseParams(){ Frequency = 0, Amplitude = 0, Constant = false},
				},
				new NoiseSettings.TransformNoiseParams(){
					X = new NoiseSettings.NoiseParams(){ Frequency = 0, Amplitude = 0, Constant = false},
					Y = new NoiseSettings.NoiseParams(){ Frequency = 11.45f, Amplitude = 0.26f, Constant = false},
					Z = new NoiseSettings.NoiseParams(){ Frequency = 0, Amplitude = 0, Constant = false},
				},
				new NoiseSettings.TransformNoiseParams(){
					X = new NoiseSettings.NoiseParams(){ Frequency = 0, Amplitude = 0, Constant = false},
					Y = new NoiseSettings.NoiseParams(){ Frequency = 54.21f, Amplitude = 0.45f, Constant = false},
					Z = new NoiseSettings.NoiseParams(){ Frequency = 0, Amplitude = 0, Constant = false},
				},
				new NoiseSettings.TransformNoiseParams(){
					X = new NoiseSettings.NoiseParams(){ Frequency = 0, Amplitude = 0, Constant = false},
					Y = new NoiseSettings.NoiseParams(){ Frequency = 95.2f, Amplitude = 0.39f, Constant = false},
					Z = new NoiseSettings.NoiseParams(){ Frequency = 0, Amplitude = 0, Constant = false},
				},
			};
	NoiseSettings.TransformNoiseParams[] shakeOmni = new NoiseSettings.TransformNoiseParams[4] {
				new NoiseSettings.TransformNoiseParams(){
					X = new NoiseSettings.NoiseParams(){ Frequency = 3.2f, Amplitude = 0.4f, Constant = false},
					Y = new NoiseSettings.NoiseParams(){ Frequency =  5.2f, Amplitude = 0.22f, Constant = false},
					Z = new NoiseSettings.NoiseParams(){ Frequency = 0, Amplitude = 0, Constant = false},
				},
				new NoiseSettings.TransformNoiseParams(){
					X = new NoiseSettings.NoiseParams(){ Frequency = 7.7f, Amplitude = 0.24f, Constant = false},
					Y = new NoiseSettings.NoiseParams(){ Frequency = 11.45f, Amplitude = 0.26f, Constant = false},
					Z = new NoiseSettings.NoiseParams(){ Frequency = 0, Amplitude = 0, Constant = false},
				},
				new NoiseSettings.TransformNoiseParams(){
					X = new NoiseSettings.NoiseParams(){ Frequency = 42f, Amplitude = 0.44f, Constant = false},
					Y = new NoiseSettings.NoiseParams(){ Frequency = 54.21f, Amplitude = 0.45f, Constant = false},
					Z = new NoiseSettings.NoiseParams(){ Frequency = 0, Amplitude = 0, Constant = false},
				},
				new NoiseSettings.TransformNoiseParams(){
					X = new NoiseSettings.NoiseParams(){ Frequency = 123f, Amplitude = 0.45f, Constant = false},
					Y = new NoiseSettings.NoiseParams(){ Frequency = 95.2f, Amplitude = 0.39f, Constant = false},
					Z = new NoiseSettings.NoiseParams(){ Frequency = 0, Amplitude = 0, Constant = false},
				},
			};

	protected virtual void Awake() {
		_virtualCamera = GameObject.FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
		noice = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

		noice.m_NoiseProfile.PositionNoise = shakeOmni;
	}


	public void ShakeCameraX(float duration, float amplitude, float frequency) {
		if (duration == 0 || amplitude == 0 || frequency == 0)
			return;

		noice.m_NoiseProfile.PositionNoise = shakeX;
		ShakeCamera(duration, amplitude, frequency);
	}

	public void ShakeCameraY(float duration, float amplitude, float frequency) {
		if (duration == 0 || amplitude == 0 || frequency == 0)
			return;

		noice.m_NoiseProfile.PositionNoise = shakeY;
		ShakeCamera(duration, amplitude, frequency);
	}

	public void ShakeCameraOnmi(float duration, float amplitude, float frequency) {
		if (duration == 0 || amplitude == 0 || frequency == 0)
			return;

		noice.m_NoiseProfile.PositionNoise = shakeOmni;
		ShakeCamera(duration, amplitude, frequency);
	}

	void ShakeCamera(float duration, float amplitude, float frequency) {
		if (duration == 0 || amplitude == 0 || frequency == 0)
			return;

		shakeAmplitude = amplitude;
		shakeFrequency = frequency;
		shakeDuration = duration;

		if (!isRunning)
			StartCoroutine(Shake());
	}

	IEnumerator Shake() {
		isRunning = true;

		do {
			noice.m_AmplitudeGain = Mathf.Lerp(shakeAmplitude, 0.0f, currShakeTime / shakeDuration);
			noice.m_FrequencyGain = shakeFrequency;

			yield return null;
		} while ((currShakeTime += Time.deltaTime) < shakeDuration);

		noice.m_AmplitudeGain = shakeAmplitude = 0.0f;
		noice.m_FrequencyGain = shakeFrequency = 0.0f;

		shakeDuration = 0.0f;
		currShakeTime = 0.0f;

		isRunning = false;
	}
}
