using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using NaughtyAttributes;

public class LoadingBar : MonoBehaviour {
	[Header("Tips")]
	[Space]
	[SerializeField] List<string> tips;
	string tipBeggining = "TIP:";

	[Header("Audio")]
	[Space]
	[SerializeField] AudioClip onLoadingEndClip;
	[SerializeField] AudioClip onAnyKeyPressClip;

	[Header("Timings")]
	[Space]
	[SerializeField] [MinMaxSlider(0.0f, 10.0f)] Vector2 additionalDelay = new Vector2(1.0f, 2.0f);

	[Header("Refs")]
	[Space]
	[SerializeField] TextMeshProUGUI tipText;
	[SerializeField] CanvasGroup canvasGroup;
	[SerializeField] Image loadingBar;
	[SerializeField] Image background;
	[Space]
	[SerializeField] TextMeshProUGUI pressAnyKeyText;
	[SerializeField] AlphaUpDown pressAnyKeyTextAlphaLerp;
	[SerializeField] ScaleUpDown pressAnyKeyTextSizeLerp;


	int sceneId;
	AsyncOperation loader;
	Coroutine loadingBarRoutine;
	
	bool isLoadingEnd;

	void Awake() {
		DisableCanvasGroup();

		EventManager.OnSceneLoadStart += OnSceneLoadStart;
		EventManager.OnSceneLoadEnd += OnSceneLoadEnd;
	}

	private void Start() {
		background.color = TransitionManager.Instance.defaultEffectColor;
		enabled = false;
	}

	void OnDestroy() {
		EventManager.OnSceneLoadStart -= OnSceneLoadStart;
		EventManager.OnSceneLoadEnd -= OnSceneLoadEnd;
	}

	private void Update() {
		if (isLoadingEnd && InputEx.IsAnyKeyPressedThisFrame()) 
			OnAnyKeyPress();
	}

	void OnSceneLoadStart(EventData data) {
		if(loader != null) {
			Debug.LogError("Cant display LoadingBar for 2 scenes");
			return;
		}

		AudioManager.Instance.MuteMusicAndDelete(0.0f);

		bool needUI = (bool)data?["needUI"];
		if (!needUI)
			return;

		isLoadingEnd = false;
		sceneId = (int)(data.Data?["id"] ?? -1);
		loader = data.Data?["loader"] as AsyncOperation;

		if (sceneId == -1 || loader == null) {
			Debug.LogError("LoadingBar data does not contains all necessary arguments");
			return;
		}

		enabled = true;
		bool needDelay = (bool)data?["uiNeedDelay"];


		loadingBarRoutine = StartCoroutine(needDelay ? LoadingBarUpdateWithDelay() : LoadingBarUpdate());

		tipText.text = tipBeggining + " " + tips.Random();
		EnableCanvasGroup();
	}

	void OnSceneLoadEnd(EventData data) {
		if (data == null)
			return;

		bool needUI = (bool)data?["needUI"];
		if (!needUI)
			return;

		if (loadingBarRoutine != null)
			StopCoroutine(loadingBarRoutine);

		if (onLoadingEndClip)
			AudioManager.Instance.Play(onLoadingEndClip);
		
		LeanTweenEx.ChangeAlpha(pressAnyKeyText, 1.0f, 0.5f).setIgnoreTimeScale(true)
		.setOnComplete(()=> {
			pressAnyKeyTextAlphaLerp.enabled = true;
			pressAnyKeyTextSizeLerp.enabled = true;
		});
		loadingBar.fillAmount = 1.0f;

		isLoadingEnd = true;
	}

	IEnumerator LoadingBarUpdate() {
		while(!loader.isDone) {
			loadingBar.fillAmount = loader.progress / 0.9f;

			yield return null;
		}
	}

	IEnumerator LoadingBarUpdateWithDelay() {
		float delayMax = additionalDelay.GetRandomValueFloat();
		float delayCurr = 0;
		loader.allowSceneActivation = false;

		while (loader.progress < 0.9f || delayCurr <= delayMax) {
			delayCurr += Time.deltaTime;

			loadingBar.fillAmount = Mathf.Min(loader.progress / 0.9f, delayCurr / delayMax);

			yield return null;
		}

		loader.allowSceneActivation = true;
	}

	void DisableCanvasGroup() {
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
		canvasGroup.alpha = 0.0f;

		pressAnyKeyText.color = pressAnyKeyText.color.SetA(0.0f);
		pressAnyKeyTextAlphaLerp.enabled = false;
		pressAnyKeyTextSizeLerp.enabled = false;
	}

	void EnableCanvasGroup() {
		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;
		canvasGroup.alpha = 1.0f;

		pressAnyKeyText.color = pressAnyKeyText.color.SetA(0.0f);
		pressAnyKeyTextAlphaLerp.enabled = false;
		pressAnyKeyTextSizeLerp.enabled = false;
	}

	void OnAnyKeyPress() {
		isLoadingEnd = false;

		if (onAnyKeyPressClip)
			AudioManager.Instance.Play(onAnyKeyPressClip);

		LeanTweenEx.ChangeAlpha(pressAnyKeyText, 0.0f, 0.2f).setIgnoreTimeScale(true);

		StartCoroutine(OnKeyPressRoutine());

		IEnumerator OnKeyPressRoutine() {
			loader.allowSceneActivation = true;
			yield return null;

			DisableCanvasGroup();
			yield return null;

			TransitionManager.Instance.StartTransitonEffectOut();

			sceneId = -1;
			loader = null;
			loadingBarRoutine = null;
			enabled = false;
		}
	}
}
