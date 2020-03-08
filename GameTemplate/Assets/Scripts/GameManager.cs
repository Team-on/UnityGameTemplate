using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager> {
	// guarantee this will be always a singleton only - can't use the constructor!
	protected GameManager() { }

	public Camera Camera { get {
			if(mainCamera == null) 
				mainCamera = Camera.main;
			return mainCamera;
	}}
	public EventManager Events { get; private set; }

	public Camera mainCamera;

	new void Awake() {
		base.Awake();

		mainCamera = Camera.main;
		Events = new EventManager();
		Input.multiTouchEnabled = false;
		LeanTween.init(800);

		EventManager.OnSceneLoadEnd += OnSceneLoadEnd;
	}

	new void OnDestroy() {
		base.OnDestroy();

		EventManager.OnSceneLoadEnd -= OnSceneLoadEnd;
	}

	void OnSceneLoadEnd(EventData data) {
		mainCamera = Camera.main;
	}
}