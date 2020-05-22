using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;

public class LoadingBar : MonoBehaviour {
	[SerializeField] List<string> tips;
	[Space]
	[SerializeField] TextMeshProUGUI tipText;
	[SerializeField] CanvasGroup canvasGroup;
	[SerializeField] Image loadingBar;
	[Space]
	[SerializeField] [MinMaxSlider(0.0f, 10.0f, false)] Vector2 additionalDelay = new Vector2(1.0f, 2.0f);

	int sceneId;
	AsyncOperation loader;
	Coroutine loadingBarRoutine;

	void Awake() {
		DisableCanvasGroup();

		EventManager.OnSceneLoadStart += OnSceneLoadStart;
		EventManager.OnSceneLoadEnd += OnSceneLoadEnd;
	}

	void OnDestroy() {
		EventManager.OnSceneLoadStart -= OnSceneLoadStart;
		EventManager.OnSceneLoadEnd -= OnSceneLoadEnd;
	}

	void OnSceneLoadStart(EventData data) {
		if(loader != null) {
			Debug.LogError("Cant display LoadingBar for 2 scenes");
			return;
		}

		bool needUI = (bool)data?["needUI"];
		if (!needUI)
			return;

		sceneId = (int)(data.Data?["id"] ?? -1);
		loader = data.Data?["loader"] as AsyncOperation;

		if(sceneId == -1 || loader == null) {
			Debug.LogError("LoadingBar data does not contains all necessary arguments");
			return;
		}

		bool needDelay = (bool)data?["uiNeedDelay"];

		loadingBarRoutine = StartCoroutine(needDelay ? LoadingBarUpdateWithDelay() : LoadingBarUpdate());

		tipText.text = "TIP: " + tips.Random();

		EnableCanvasGroup();
	}

	void OnSceneLoadEnd(EventData data) {
		if(loadingBarRoutine != null)
			StopCoroutine(loadingBarRoutine);
		DisableCanvasGroup();

		sceneId = -1;
		loader = null;
		loadingBarRoutine = null;
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
		canvasGroup.alpha = 0.0f;
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
	}

	void EnableCanvasGroup() {
		canvasGroup.alpha = 1.0f;
		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;
	}
}
