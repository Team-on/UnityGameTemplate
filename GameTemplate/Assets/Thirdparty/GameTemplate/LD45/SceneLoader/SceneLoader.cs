using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using yaSingleton;

[CreateAssetMenu(fileName = "Scene Loader", menuName = "Singletons/SceneLoader")]
public class SceneLoader : Singleton<SceneLoader> {
	AsyncOperation loader;

	protected override void Initialize() {
		base.Initialize();

		EventManager.OnSceneNeedLoad += OnSceneNeedLoad;

	}

	protected override void Deinitialize() {
		base.Deinitialize();

		EventManager.OnSceneNeedLoad -= OnSceneNeedLoad;

	}

	void OnSceneNeedLoad(EventData data) {
		if (!data.Data.ContainsKey("id")) {
			Debug.LogError("OnSceneNeedLoad no [id] field");
			return;
		}

		int sceneId = -1;
		bool needUI = false, uiNeedDelay = false;

		if (data["id"] is int) {
			sceneId = (int)(data["id"]);
		}
		else if (data["id"] is string) {
			sceneId = SceneUtility.GetBuildIndexByScenePath((string)(data["id"]));
		}
		else {
			Debug.LogError("OnSceneNeedLoad [id] have unsupported type");
			return;
		}

		LoadScene(sceneId, needUI, uiNeedDelay);
	}

	public void LoadScene(string name, bool needUI, bool uiNeedDelay) {
		LoadScene(SceneUtility.GetBuildIndexByScenePath(name), needUI, uiNeedDelay);
	}

	public void LoadScene(int id, bool needUI, bool uiNeedDelay) {
		loader = SceneManager.LoadSceneAsync(id, LoadSceneMode.Single);

		EventData eventData = new EventData("OnSceneLoadStart");
		eventData.Data.Add("id", id);
		eventData.Data.Add("loader", loader);
		eventData.Data.Add("needUI", needUI);
		if(needUI)
			eventData.Data.Add("uiNeedDelay", uiNeedDelay);
		GameManager.Instance.Events.CallOnSceneLoadStart(eventData);

		loader.completed += (a) => {
			EventData eventData_ = new EventData("OnSceneLoadStart");
			eventData_.Data.Add("id", id);
			eventData_.Data.Add("loader", loader);
			GameManager.Instance.Events.CallOnSceneLoadEnd(eventData_);
		};
	}
}
