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

	//Help level
	public int HelpLevelMode {
		get => helpLevelMode;
		set {
			if (helpLevelMode != value) {
				helpLevelMode = value;
				OnHelpModeChange?.Invoke(helpLevelMode);
			}
		}
	}
	public Action<int> OnHelpModeChange;
	int helpLevelMode = 0;

	//Debug mode
	public bool IsDebugMode {
		get => isDebugMode;
		set {
			if (isDebugMode != value) {
				isDebugMode = value;
				OnDebugModeChange?.Invoke(isDebugMode);
			}
		}
	}
	public Action<bool> OnDebugModeChange;
#if UNITY_EDITOR
	bool isDebugMode = true;
#else
	bool isDebugMode = false;
#endif

	[Header("Help"), Space]
	[Tooltip("Inclusive minimum value")] public int minHelpLevel = 0;
	[Tooltip("Inclusive maximum value")] public int maxHelpLevel = 2;
	[Tooltip("")] public int startHelpLevel = 2;

	[Header("Resources"), Space]
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
	[ReadOnly] public MouseTooltip tooltip;

	[Header("Debug UI"), Space]
	[ReadOnly] public UIPopupGroup debugPopups;

	[Header("Floating text"), Space]
	public GameObject floatingTextDefaultPrefab;
	public GameObject floatingTextCapsPrefab;

	[Header("Other singlethones"), Space]
	public AudioManager audioManager;
	public SceneLoader sceneLoader;
	public PlayerInputActions actions;
	public EventManager events { get; private set; }

	[Header("Saving data"), Space]
	[ReadOnly] public GameSettingsData settingsData;
	

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

		actions = new PlayerInputActions();

		settingsData = GameSettingsData.Load();

		StartCoroutine(DelayedSetup());

		IEnumerator DelayedSetup() {
			yield return null;
			yield return null;

			//TODO: save this on exit and load on enter
			//TODO: use this for each save
			helpLevelMode = startHelpLevel;

			//TODO: save this on exit and load on enter
			//TODO: use this for each save
			resources = new int[startResources.Length];
			for (int i = 0; i < startResources.Length; ++i)
				resources[i] = startResources[i];

			actions.Enable();

			settingsData.ApplyAllSettings();

			events.CallOnOnApplicationStart();
			events.CallOnSceneLoadEnd(null);
		}
	}

	protected override void Deinitialize() {
		Debug.Log("GameManager.Deinitialize()");
		base.Deinitialize();

		GameSettingsData.Save(settingsData);

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
