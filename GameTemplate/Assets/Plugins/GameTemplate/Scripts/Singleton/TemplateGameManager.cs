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
	//Cameras
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
	Camera mainCamera;
	CinemachineVirtualCamera virtualCamera;

	//Resources
	public int this[ResourceType type] {
		get {
			return resources[(int)type];
		}
		set {
			resources[(int)type] = value;
			onResourceChange[(int)type]?.Invoke(value);
		}
	}
	public Action<int>[] onResourceChange;
	[SerializeField] int[] startResources;
	[ReadOnly] int[] resources;

	[Header("Global data"), Space]
	[ReadOnly] public string buildNameString;
	[ReadOnly] public string productName;

	[Header("UI navigation"), Space]
	[ReadOnly] public UIInput uiinput;
	[ReadOnly] public EventSystem eventSystem;
	[ReadOnly] public InputSystemUIInputModule inputSystem;

	[Header("UI"), Space]
	[ReadOnly] public ArrowsController arrows;

	[Header("Debug UI"), Space]
	[ReadOnly] public UIPopupGroup debugPopups;

	//Other singletons
	public PlayerInputActions actions;
	public EventManager events { get; private set; }
	public AudioManager audioManager;
	public SceneLoader sceneLoader;

#if UNITY_EDITOR
	private void OnValidate() {
		int newLen = Enum.GetNames(typeof(ResourceType)).Length;
		if (startResources.Length != newLen) {
			int[] newArr = new int[newLen];

			for (int i = 0; i < startResources.Length && i < newArr.Length; ++i)
				newArr[i] = startResources[i];

			startResources = newArr;
		}
	}
#endif

	protected override void Initialize() {
		Debug.Log("GameManager.Initialize()");
		base.Initialize();

		events = new EventManager();
		EventManager.OnSceneLoadEnd += OnSceneLoadEnd;

		resources = new int[startResources.Length];
		for(int i = 0; i < startResources.Length; ++i) 
			resources[i] = startResources[i];

		actions = new PlayerInputActions();
		actions.Enable();

		StartCoroutine(DelayedSetup());

		IEnumerator DelayedSetup() {
			yield return null;
			yield return null;
			events.CallOnOnApplicationStart();
			events.CallOnSceneLoadEnd(null);
		}
	}

	protected override void Deinitialize() {
		Debug.Log("GameManager.Deinitialize()");
		base.Deinitialize();

		EventManager.OnSceneLoadEnd -= OnSceneLoadEnd;
		events.CallOnOnApplicationExit();
	}

	void OnSceneLoadEnd(EventData data) {
		mainCamera = Camera.main;
	}
}

#if UNITY_EDITOR

[UnityEditor.InitializeOnLoad]
class SingletoneImporter {
	static UnityEngine.Object[] assets = null;
	static UnityEngine.Object selectedBefore = null;
	static int i = 0;
	static bool isSubscribeBefore = false;

	static SingletoneImporter() {
		if (!isSubscribeBefore && UnityEditor.EditorApplication.timeSinceStartup < 30f) {
			isSubscribeBefore = true;
			UnityEditor.EditorApplication.update += Update;
		}
	}

	static void Update() {
		ImportSingletons();
	}

	static void ImportSingletons() {
		if(assets == null) {
			Debug.Log("Init singletons");
			
			string[] guids = UnityEditor.AssetDatabase.FindAssets("t:scriptableobject", new[] { "Assets/ScriptableObjects/Singletons" });
			assets = new UnityEngine.Object[guids.Length];

			for(int i = 0; i < assets.Length; ++i) {
				assets[i] = UnityEditor.AssetDatabase.LoadMainAssetAtPath(UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]));
			}

			selectedBefore = UnityEditor.Selection.activeObject;
		}

		if(i == assets.Length) {
			UnityEditor.EditorApplication.update -= Update;
			UnityEditor.Selection.activeObject = selectedBefore;
		}
		else {
			UnityEditor.Selection.activeObject = assets[i++];
		}
	}
}

#endif
