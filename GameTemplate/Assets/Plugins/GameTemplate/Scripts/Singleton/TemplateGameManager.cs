using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using yaSingleton;
using Cinemachine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Template Game Manager", menuName = "Singletons/TemplateGameManager")]
public class TemplateGameManager : Singleton<TemplateGameManager> {
	//Properties
	public Camera Camera { get {
			if(mainCamera == null) 
				mainCamera = Camera.main;
			return mainCamera;
	}}
	public CinemachineVirtualCamera VirtualCamera {
		get {
			if (virtualCamera == null)
				virtualCamera = Camera.GetComponent<CinemachineVirtualCamera>();
			return virtualCamera;
		}
	}

	//Global data
	[ReadOnly] public string buildNameString;
	[ReadOnly] public string productName;

	//UI
	[ReadOnly] public UIInput uiinput;
	[ReadOnly] public EventSystem eventSystem;
	[ReadOnly] public InputSystemUIInputModule inputSystem;

	//Debug UI
	[ReadOnly] public UIPopupGroup debugPopups;

	//Other singletons
	public EventManager Events { get; private set; }
	public AudioManager audioManager;
	public SceneLoader sceneLoader;

	Camera mainCamera;
	CinemachineVirtualCamera virtualCamera;

	protected override void Initialize() {
		Debug.Log("GameManager.Initialize()");
		base.Initialize();

		Events = new EventManager();

		EventManager.OnSceneLoadEnd += OnSceneLoadEnd;

		StartCoroutine(DelayedSetup());

		IEnumerator DelayedSetup() {
			yield return null;
			yield return null;
			Events.CallOnOnApplicationStart();
			Events.CallOnSceneLoadEnd(null);
		}
	}

	protected override void Deinitialize() {
		Debug.Log("GameManager.Deinitialize()");
		base.Deinitialize();

		EventManager.OnSceneLoadEnd -= OnSceneLoadEnd;
		Events.CallOnOnApplicationExit();
	}

	void OnSceneLoadEnd(EventData data) {
		mainCamera = Camera.main;
	}
}