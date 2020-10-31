using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public class BuildManagerWindow : EditorWindow {
	const string SETTINGS_DEFAULT_PATH = "Assets/Plugins/Editor/BuildManager/BuildManagerг/BuildSequences.asset"; //Need Assets in path, cuz used by AssetDatabase.CreateAsset
	const string SETTINGS_PATH_KEY = "BuildManagerWindow.SettingsPath";

	static string settingsPath;
	static BuildManagerSettings settings;

	static ChangelogData changelog;
	static bool changelogFoldout = false;
	static Vector2 scrollPosChangelog = Vector2.zero;

	static Vector2 scrollPosSequence = Vector2.zero;
	static bool zipFoldout = false;
	static bool itchFoldout = false;

	static ReorderableList sequenceList;
	static ReorderableList buildList;

	[MenuItem("Window/Custom/Builds &b")]
	public static void ShowWindow() {
		sequenceList = null;
		buildList = null;
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

		EditorGUILayout.BeginHorizontal();
		settings.scriptingDefineSymbols = EditorGUILayout.TextField("Scripting Defines", settings.scriptingDefineSymbols);
		if (GUILayout.Button($"Set defines", GUILayout.Width(100f))) {
			PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget), settings.scriptingDefineSymbols);
			//string.Concat(settings.scriptingDefineSymbols, ";", sequence.scriptingDefineSymbolsOverride, ";", data.scriptingDefineSymbolsOverride), 
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space(20);
	}

	void DrawBuildButtons() {
		if ((settings?.sequences?.Count ?? 0) != 0) {
			int enabledSequence = 0;
			foreach (var sequence in settings.sequences)
				if(sequence.isEnabled)
					++enabledSequence;

			if (enabledSequence == 0)
				return;
			
			EditorGUILayout.Space(20);
			Color prevColor = GUI.backgroundColor;
			GUI.backgroundColor = new Color(0.773f, 0.345098f, 0.345098f);

			EditorGUILayout.LabelField("Start build sequence(they red not becouse error, but becouse build stuck your pc if you accidentaly press it)");
			EditorGUILayout.LabelField("Don't forget to manually download new version of polyglot localization if you want to update it");

			EditorGUILayout.BeginHorizontal();
			for(int i = 0; i < settings.sequences.Count; ++i) {
				BuildSequence sequence = settings.sequences[i];

				if(i != 0 && i % 3 == 0) {
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
				}

				if (sequence.isEnabled && GUILayout.Button($"Build {sequence.editorName}")) {
					BuildManager.RunBuildSequnce(settings, sequence, changelog);
				}
			}
			EditorGUILayout.EndHorizontal();

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
			ChangelogData.SaveChangelog(changelog);

		string DrawStringField(string label, string text) {
			tmpString = EditorGUILayout.TextField(label, text);
			if (text != tmpString)
				isChanged = true;
			return tmpString;
		}
	}

	void DrawSequenceList() {
		if (sequenceList == null) {
			PredefinedBuildConfigs.Init();
			sequenceList = BuildSequenceReordableList.Create(settings.sequences, OnSequenceAdd, "Builds sequences");
			sequenceList.onSelectCallback += OnSequenceSelectionChanged;
			sequenceList.index = 0;
		}

		sequenceList.DoLayoutList();

		if (0 <= sequenceList.index && sequenceList.index < sequenceList.count) {
			BuildSequence selected = settings.sequences[sequenceList.index];

			EditorGUILayout.BeginHorizontal();
			selected.scriptingDefineSymbolsOverride = EditorGUILayout.TextField("Defines sequence override", selected.scriptingDefineSymbolsOverride);
			if (GUILayout.Button($"Set defines", GUILayout.Width(100f))) {
				PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget), string.Concat(settings.scriptingDefineSymbols, ";", selected.scriptingDefineSymbolsOverride));
			}
			EditorGUILayout.EndHorizontal();

			//selected.editorName = EditorGUILayout.TextField("Sequence name", selected.editorName);
			//selected.itchGameLink = EditorGUILayout.TextField("Itch.io link", selected.itchGameLink);
		}
	}

	void DrawSelectedSequenceData() {
		EditorGUILayout.Space(20);

		if (sequenceList.index < 0 || sequenceList.index >= settings.sequences.Count) {
			buildList = null;
			return;
		}

		if (buildList == null) {
			buildList = BuildDataReordableList.Create(settings.sequences[sequenceList.index].builds, OnBuildAdd, "Builds");
			buildList.onSelectCallback += OnBuildSelectionChanged; ;
			buildList.index = 0;
		}

		buildList.DoLayoutList();

		if (buildList.index < 0 || buildList.index >= settings.sequences[sequenceList.index].builds.Count)
			return;

		BuildData selected = settings.sequences[sequenceList.index].builds[buildList.index];

		SerializedObject obj = new SerializedObject(settings);

		selected.isPassbyBuild = EditorGUILayout.Toggle("Is Passby build", selected.isPassbyBuild);
		selected.isReleaseBuild = EditorGUILayout.Toggle("Is Release build", selected.isReleaseBuild);
		selected.isVirtualRealitySupported = EditorGUILayout.Toggle("VR Supported", selected.isVirtualRealitySupported);

		EditorGUILayout.Space(20);
		selected.outputRoot = EditorGUILayout.TextField("Output root", selected.outputRoot);
		selected.middlePath = EditorGUILayout.TextField("Middle path", selected.middlePath);

		EditorGUILayout.BeginHorizontal();
		selected.scriptingDefineSymbolsOverride = EditorGUILayout.TextField("Defines build override", selected.scriptingDefineSymbolsOverride);
		if (GUILayout.Button($"Set defines", GUILayout.Width(100f))) {
			PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget), string.Concat(settings.scriptingDefineSymbols, ";", settings.sequences[sequenceList.index].scriptingDefineSymbolsOverride, ";", selected.scriptingDefineSymbolsOverride));
		}
		EditorGUILayout.EndHorizontal();

		//EditorGUILayout.BeginHorizontal();
		//EditorGUILayout.LabelField("Build Target Group", GUILayout.MinWidth(0));
		//selected.targetGroup = (BuildTargetGroup)EditorGUILayout.EnumPopup(selected.targetGroup);
		//EditorGUILayout.EndHorizontal();

		//EditorGUILayout.BeginHorizontal();
		//EditorGUILayout.LabelField("Build Target", GUILayout.MinWidth(0));
		//selected.target = (BuildTarget)EditorGUILayout.EnumPopup(selected.target);
		//EditorGUILayout.EndHorizontal();

		//EditorGUILayout.BeginHorizontal();
		//EditorGUILayout.LabelField("Build Options", GUILayout.MinWidth(0));
		//selected.options = (BuildOptions)EditorGUILayout.EnumFlagsField(selected.options);
		//EditorGUILayout.EndHorizontal();
		//EditorGUILayout.Space(20);


		EditorGUILayout.Space(20);
		zipFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(zipFoldout, "7zip");
		if (zipFoldout) {
			++EditorGUI.indentLevel;
			selected.needZip = EditorGUILayout.Toggle("Compress", selected.needZip);
			selected.compressDirPath = EditorGUILayout.TextField("Dir path", selected.compressDirPath);
			--EditorGUI.indentLevel;
		}
		EditorGUILayout.EndFoldoutHeaderGroup();

		itchFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(itchFoldout, "itch.io");
		if (itchFoldout) {
			++EditorGUI.indentLevel;
			selected.needItchPush = EditorGUILayout.Toggle("Push to itch.io", selected.needItchPush);
			selected.itchDirPath = EditorGUILayout.TextField("Dir path", selected.itchDirPath);
			selected.itchChannel = EditorGUILayout.TextField("Channel", selected.itchChannel);
			selected.itchAddLastChangelogUpdateNameToVerison = EditorGUILayout.Toggle("Add Changelog Update Name To Verison", selected.itchAddLastChangelogUpdateNameToVerison);
			--EditorGUI.indentLevel;
		}
		EditorGUILayout.EndFoldoutHeaderGroup();

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
	}

	static void LoadChangelog() {
		changelog = ChangelogData.LoadChangelog();
	}

	static void OnSequenceSelectionChanged(ReorderableList list) {
		buildList = null;
	}

	static void OnBuildSelectionChanged(ReorderableList list) {

	}

	static void OnSequenceAdd(object target) {
		settings.sequences.Add((target as BuildSequence).Clone() as BuildSequence);
	}

	static void OnBuildAdd(object target) {
		settings.sequences[sequenceList.index].builds.Add((target as BuildData).Clone() as BuildData);
	}

	#region Helpers
	void GuiLine(int i_height = 1) {
		Rect rect = EditorGUILayout.GetControlRect(false, i_height);
		rect.height = i_height;
		EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
	}
	#endregion
}
