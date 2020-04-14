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

	Camera mainCamera;

	private void Awake() {
		mainCamera = Camera.main;
		Input.multiTouchEnabled = false;
		LeanTween.init(800);
	}

	protected override void Initialize() {
		base.Initialize();

		Events = new EventManager();

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