using System;
using UnityEngine;
using yaSingleton;

[CreateAssetMenu(fileName = "Game Manager", menuName = "Singletons/GameManager")]
public class GameManager : Singleton<GameManager> {
	//Properties
	public Camera Camera { get {
			if(mainCamera == null) 
				mainCamera = Camera.main;
			return mainCamera;
	}}

	//Other singletons
	public EventManager Events { get; private set; }
	public AudioManager audioManager;

	//Global data
	public string buildNameString;

	[NonSerialized] public Camera mainCamera;

	protected override void Initialize() {
		base.Initialize();

		mainCamera = Camera.main;
		Events = new EventManager();
		Input.multiTouchEnabled = false;
		LeanTween.init(800);

		EventManager.OnSceneLoadEnd += OnSceneLoadEnd;
	}

	protected override void Deinitialize() {
		base.Deinitialize();

		EventManager.OnSceneLoadEnd -= OnSceneLoadEnd;
	}

	void OnSceneLoadEnd(EventData data) {
		mainCamera = Camera.main;
	}
}