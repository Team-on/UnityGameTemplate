using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BuildManagerWindow : EditorWindow {
	const string SETTINGS_DEFAULT_PATH = "Assets/Thirdparty/GameTemplate/Editor/BuildManager/BuildSequences.asset";
	const string SETTINGS_PATH_KEY = "BuildManagerWindow.SettingsPath";
	static string settingsPath;

	static BuildManagerSettings settings;

	static Vector2 scrollPosAll;
	static InspectorList<BuildSequence> sequencesList;
	static InspectorList<BuildData> buidsList;

	[MenuItem("Window/Builds &b")]
	public static void ShowWindow() {
		EditorWindow.GetWindow(typeof(BuildManagerWindow), false, "Builds", true);

		LoadSettings();
	}

	void OnGUI() {
		if (settings == null)
			LoadSettings();

		scrollPosAll = EditorGUILayout.BeginScrollView(scrollPosAll);

		if ((settings?.sequences?.Length ?? 0) != 0) {
			EditorGUILayout.LabelField("Start build sequence");
			foreach (var sequence in settings.sequences) {
				if (GUILayout.Button($"Build {sequence.editorName}")) {
					BuildManager.RunBuildSequnce(sequence);
				}
			}
		}

		EditorGUILayout.Space(20);
		settings.sequences = sequencesList.Show();
		if (sequencesList?.Selected != null) {
			sequencesList.Selected.editorName = EditorGUILayout.TextField("Sequence name", sequencesList.Selected.editorName);
		}

		EditorGUILayout.Space(20);
		sequencesList.Selected.builds = buidsList.Show();
		if (buidsList?.Selected != null) {
			SerializedObject obj = new SerializedObject(settings);

			buidsList.Selected.outputRoot = EditorGUILayout.TextField("Output root", buidsList.Selected.outputRoot);
			buidsList.Selected.middlePath = EditorGUILayout.TextField("Middle path", buidsList.Selected.middlePath);

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Build Target Group", GUILayout.MinWidth(0));
			buidsList.Selected.targetGroup = (BuildTargetGroup)EditorGUILayout.EnumPopup(buidsList.Selected.targetGroup);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Build Target", GUILayout.MinWidth(0));
			buidsList.Selected.target = (BuildTarget)EditorGUILayout.EnumPopup(buidsList.Selected.target);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Build Options", GUILayout.MinWidth(0));
			buidsList.Selected.options = (BuildOptions)EditorGUILayout.EnumFlagsField(buidsList.Selected.options);
			EditorGUILayout.EndHorizontal();
		}

		EditorGUILayout.EndScrollView();
	}

	static void LoadSettings() {
		settingsPath = PlayerPrefs.GetString(SETTINGS_PATH_KEY, "");
		settings = null;

		//Find path. Try to load settings
		if (!string.IsNullOrEmpty(settingsPath)) {
			settings = AssetDatabase.LoadAssetAtPath<BuildManagerSettings>(settingsPath);
			if (settings == null) {
				settingsPath = null;
			}
		}

		//No path, or cant locate asset at path. Try to find settings in assets.
		if (string.IsNullOrEmpty(settingsPath)) {
			string[] guids = AssetDatabase.FindAssets("t:BuildManagerSettings");
			if (guids.Length >= 2) {
				Debug.LogError("[BuildManagerWindow]. 2+ BuildManagerSettings exist. Consider on using only 1 setting. The first on will be used.");
			}

			if (guids.Length != 0) {
				settingsPath = AssetDatabase.GUIDToAssetPath(guids[0]);
				PlayerPrefs.SetString(SETTINGS_PATH_KEY, settingsPath);
				settings = AssetDatabase.LoadAssetAtPath<BuildManagerSettings>(settingsPath);
			}
		}

		//Cant find settings. Create new
		if (settings == null) {
			settings = (BuildManagerSettings)ScriptableObject.CreateInstance(typeof(BuildManagerSettings));
			AssetDatabase.CreateAsset(settings, SETTINGS_DEFAULT_PATH);
			settingsPath = SETTINGS_DEFAULT_PATH;
			PlayerPrefs.SetString(SETTINGS_PATH_KEY, SETTINGS_DEFAULT_PATH);
		}

		sequencesList = new InspectorList<BuildSequence>();
		sequencesList.Init(settings.sequences, "Builds sequences", (BuildSequence seq, int i) => seq.editorName);
		buidsList = new InspectorList<BuildData>();
		buidsList.Init(sequencesList.Selected.builds, "Builds", FormBuildNameInList);

		sequencesList.OnChangeSelectionAction += OnSequenceSelectionChanged;
		buidsList.OnChangeSelectionAction += OnBuildSelectionChanged;
	}

	static void OnSequenceSelectionChanged(BuildSequence sequence) {
		buidsList.Init(sequence.builds, "Builds", FormBuildNameInList);
	}

	static void OnBuildSelectionChanged(BuildData data) {

	}

	static string FormBuildNameInList(BuildData build, int i){
		return BuildManager.ConvertBuildTargetToString(build.target);
	}
}
