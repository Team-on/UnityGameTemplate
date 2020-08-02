using UnityEngine;
using yaSingleton;

[CreateAssetMenu(fileName = "Game Manager", menuName = "Singletons/GameManager")]
public class TemplateGameManager : Singleton<TemplateGameManager> {
	//Properties
	public Camera Camera { get {
			if(mainCamera == null) 
				mainCamera = Camera.main;
			return mainCamera;
	}}

	//Global data
	public string buildNameString;

	//Other singletons
	public EventManager Events { get; private set; }
	public AudioManager audioManager;

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