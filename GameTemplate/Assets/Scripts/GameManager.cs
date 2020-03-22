using System;
using UnityEngine;
using yaSingleton;

[CreateAssetMenu(fileName = "Game Manager", menuName = "Singletons/GameManager")]
public class GameManager : Singleton<GameManager> {
	// guarantee this will be always a singleton only - can't use the constructor!
	protected GameManager() { }

	public Camera Camera { get {
			if(mainCamera == null) 
				mainCamera = Camera.main;
			return mainCamera;
	}}
	public EventManager Events { get; private set; }
	public AudioManager audioManager;

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