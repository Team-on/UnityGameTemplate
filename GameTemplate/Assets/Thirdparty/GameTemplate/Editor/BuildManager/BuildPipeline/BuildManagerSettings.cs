using System;
using UnityEngine;
using UnityEditor;

public class BuildManagerSettings : ScriptableObject{
	public BuildSequence[] sequences = new BuildSequence[1] { new BuildSequence() };
}

[Serializable]
public class BuildSequence {
	public string editorName = "New build sequence";
	public string itchGameLink = "teamon/game-link";
	public BuildData[] builds = new BuildData[1] { new BuildData() };
}

[Serializable]
public class BuildData {
	public string outputRoot = "Builds/";
	public string middlePath = "$NAME_$VERSION_$PLATFORM/$NAME_$VERSION/$NAME$EXECUTABLE";

	public string scriptingDefinySymbols = "UNITY_POST_PROCESSING_STACK_V2;TMP_PRESENT;ARABSUPPORT_ENABLED";


	public BuildTargetGroup targetGroup = BuildTargetGroup.Unknown;
	public BuildTarget target = BuildTarget.NoTarget;
	public BuildOptions options = BuildOptions.None;

	public bool isVirtualRealitySupported = false;

	public bool needZip = false;
	public string compressDirPath;

	public bool needItchPush = false;
	public string itchChannel;
	public string itchDirPath;
	public bool itchAddLastChangelogUpdateNameToVerison;
	public string itchLastChangelogUpdateName;	//Fill from code

	//TODO: 
	public bool useGithubActions;
	public bool useUnityActions;
	public bool needGamejolthPush;
	public bool needSteamPush;
	public bool needGoogleDricePush;
}