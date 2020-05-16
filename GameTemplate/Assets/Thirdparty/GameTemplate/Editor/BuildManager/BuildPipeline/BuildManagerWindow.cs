using UnityEngine;
using UnityEditor;

public class BuildManagerWindow : EditorWindow {
	const string SETTINGS_DEFAULT_PATH = "Assets/Thirdparty/GameTemplate/Editor/BuildManager/BuildSequences.asset"; //Need Assets in path, cuz used by AssetDatabase.CreateAsset
	const string SETTINGS_PATH_KEY = "BuildManagerWindow.SettingsPath";
	const string CHANGELOG_DEFAULT_PATH = "Thirdparty/GameTemplate/Editor/BuildManager/Changelog.json";//Dont need Assets in path, cuz used with Application.dataPath
	const string CHANGELOG_JSON_PATH_KEY = "BuildManagerWindow.ChangelogJsonPath";

	static string settingsPath;
	static BuildManagerSettings settings;

	static string changelogPath;
	static ChangelogData changelog;
	static bool changelogFoldout = false;
	static Vector2 scrollPosChangelog = Vector2.zero;

	static Vector2 scrollPosSequence = Vector2.zero;
	static bool zipFoldout = false;
	static bool itchFoldout = false;
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
		if (changelog == null)
			LoadChangelog();

		DrawGlobalBuildData();
		DrawChangelogInfo();

		DrawBuildButtons();

		EditorGUILayout.Space(20);
		scrollPosSequence = EditorGUILayout.BeginScrollView(scrollPosSequence);

		DrawSequenceList();
		DrawSelectedSequenceData();

		EditorGUILayout.EndScrollView();
	}

	#region Main Drawers
	void DrawGlobalBuildData() {
		PlayerSettings.bundleVersion = EditorGUILayout.TextField("Version", PlayerSettings.bundleVersion);
		PlayerSettings.Android.bundleVersionCode = EditorGUILayout.IntField("Android bundle version", PlayerSettings.Android.bundleVersionCode);
		EditorGUILayout.Space(20);
	}

	void DrawBuildButtons() {
		if ((settings?.sequences?.Length ?? 0) != 0) {
			EditorGUILayout.Space(20);
			Color prevColor = GUI.backgroundColor;
			GUI.backgroundColor = new Color(0.773f, 0.345098f, 0.345098f);

			EditorGUILayout.LabelField("Start build sequence(they red not becouse error, but becouse build stuck your pc if you accidentaly press it)");
			EditorGUILayout.LabelField("Don't forget to manually download new version of polyglot localization if you want to update it");
			foreach (var sequence in settings.sequences) {
				if (GUILayout.Button($"Build {sequence.editorName}")) {
					BuildManager.RunBuildSequnce(sequence, changelog);
				}
			}

			GUI.backgroundColor = prevColor;
		}
	}

	void DrawChangelogInfo() {
		bool isChanged = false;
		string tmpString = "";

		changelogFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(changelogFoldout, "Changelog");
		if (changelogFoldout) {
			scrollPosChangelog = EditorGUILayout.BeginScrollView(scrollPosChangelog);
			++EditorGUI.indentLevel;

			changelog.updateName = DrawStringField("Update name", changelog.updateName);

			--EditorGUI.indentLevel;
			EditorGUILayout.EndScrollView();
		}
		EditorGUILayout.EndFoldoutHeaderGroup();

		if (isChanged)
			ChangelogData.SaveChangelogToFile(changelogPath, changelog);

		string DrawStringField(string label, string text) {
			tmpString = EditorGUILayout.TextField(label, text);
			if (text != tmpString)
				isChanged = true;
			return tmpString;
		}
	}

	void DrawSequenceList() {
		settings.sequences = sequencesList.Show();
		if (sequencesList?.Selected != null) {
			sequencesList.Selected.editorName = EditorGUILayout.TextField("Sequence name", sequencesList.Selected.editorName);
			sequencesList.Selected.itchGameLink = EditorGUILayout.TextField("Itch.io link", sequencesList.Selected.itchGameLink);
		}
	}

	void DrawSelectedSequenceData() {
		EditorGUILayout.Space(20);
		sequencesList.Selected.builds = buidsList.Show();
		if (buidsList?.Selected != null) {
			SerializedObject obj = new SerializedObject(settings);

			buidsList.Selected.outputRoot = EditorGUILayout.TextField("Output root", buidsList.Selected.outputRoot);
			buidsList.Selected.middlePath = EditorGUILayout.TextField("Middle path", buidsList.Selected.middlePath);
			buidsList.Selected.scriptingDefinySymbols = EditorGUILayout.TextField("Scripting Defines", buidsList.Selected.scriptingDefinySymbols);

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
			EditorGUILayout.Space(20);

			buidsList.Selected.isVirtualRealitySupported = EditorGUILayout.Toggle("VR Supported", buidsList.Selected.isVirtualRealitySupported);

			zipFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(zipFoldout, "7zip");
			if (zipFoldout) {
				++EditorGUI.indentLevel;
				buidsList.Selected.needZip = EditorGUILayout.Toggle("Compress", buidsList.Selected.needZip);
				buidsList.Selected.compressDirPath = EditorGUILayout.TextField("Dir path", buidsList.Selected.compressDirPath);
				--EditorGUI.indentLevel;
			}
			EditorGUILayout.EndFoldoutHeaderGroup();

			itchFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(itchFoldout, "itch.io");
			if (itchFoldout) {
				++EditorGUI.indentLevel;
				buidsList.Selected.needItchPush = EditorGUILayout.Toggle("Push to itch.io", buidsList.Selected.needItchPush);
				buidsList.Selected.itchDirPath = EditorGUILayout.TextField("Dir path", buidsList.Selected.itchDirPath);
				buidsList.Selected.itchChannel = EditorGUILayout.TextField("Channel", buidsList.Selected.itchChannel);
				buidsList.Selected.itchAddLastChangelogUpdateNameToVerison = EditorGUILayout.Toggle("Add Changelog Update Name To Verison", buidsList.Selected.itchAddLastChangelogUpdateNameToVerison);
				--EditorGUI.indentLevel;
			}
			EditorGUILayout.EndFoldoutHeaderGroup();
		}

		EditorUtility.SetDirty(settings);
	}
	#endregion

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

	static void LoadChangelog() {
		changelogPath = PlayerPrefs.GetString(CHANGELOG_JSON_PATH_KEY, "");
		changelog = null;

		//Find path. Try to load settings
		if (!string.IsNullOrEmpty(changelogPath)) {
			changelog = ChangelogData.LoadChangelogFromFile(changelogPath);
			if (changelog == null) {
				changelogPath = null;
			}
		}

		//No path, or cant locate asset at path. Try to find settings in assets.
		if (string.IsNullOrEmpty(changelogPath)) {
			string[] guids = AssetDatabase.FindAssets("Changelog.json");
			if (guids.Length >= 2) {
				Debug.LogError("[BuildManagerWindow]. 2+ Changelog.json exist. Consider on using only 1 changelog. The first on will be used.");
			}

			if (guids.Length != 0) {
				changelogPath = AssetDatabase.GUIDToAssetPath(guids[0]);
				PlayerPrefs.SetString(CHANGELOG_JSON_PATH_KEY, changelogPath);
				changelog = ChangelogData.LoadChangelogFromFile(changelogPath);
			}
		}

		//Cant find settings. Create new
		if (changelog == null) {
			changelog = new ChangelogData();
			changelogPath = CHANGELOG_DEFAULT_PATH;
			PlayerPrefs.SetString(CHANGELOG_JSON_PATH_KEY, CHANGELOG_DEFAULT_PATH);

			ChangelogData.SaveChangelogToFile(changelogPath, changelog);
		}
	}

	static void OnSequenceSelectionChanged(BuildSequence sequence) {
		buidsList.Init(sequence.builds, "Builds", FormBuildNameInList);
	}

	static void OnBuildSelectionChanged(BuildData data) {

	}

	static string FormBuildNameInList(BuildData build, int i){
		return BuildManager.ConvertBuildTargetToString(build.target);
	}

	#region Helpers
	void GuiLine(int i_height = 1) {
		Rect rect = EditorGUILayout.GetControlRect(false, i_height);
		rect.height = i_height;
		EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
	}
	#endregion
}
