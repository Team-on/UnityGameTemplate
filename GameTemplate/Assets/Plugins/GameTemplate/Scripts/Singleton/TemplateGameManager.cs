using System;
using UnityEngine;
using yaSingleton;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Template Game Manager", menuName = "Singletons/TemplateGameManager")]
public class TemplateGameManager : Singleton<TemplateGameManager> {
	//Properties
	public Camera Camera { get {
			if(mainCamera == null) 
				mainCamera = Camera.main;
			return mainCamera;
	}}

	//Global data
	[ReadOnly] public string buildNameString;
	[ReadOnly] public string productName;

	//Debug UI
	[ReadOnly] public UIPopupGroup debugPopups;

	//Other singletons
	public EventManager Events { get; private set; }
	public AudioManager audioManager;
	public SceneLoader sceneLoader;

	Camera mainCamera;

	private void Awake() {
		Debug.Log("GameManager.Awake()");

		mainCamera = Camera.main;
		Input.multiTouchEnabled = false;
	}

	protected override void Initialize() {
		Debug.Log("GameManager.Initialize()");
		base.Initialize();

		Events = new EventManager();

		EventManager.OnSceneLoadEnd += OnSceneLoadEnd;
	}

	protected override void Deinitialize() {
		Debug.Log("GameManager.Deinitialize()");
		base.Deinitialize();

		EventManager.OnSceneLoadEnd -= OnSceneLoadEnd;
	}

	void OnSceneLoadEnd(EventData data) {
		mainCamera = Camera.main;
	}
}