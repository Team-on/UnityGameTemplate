using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
public class BuildData : ICloneable {
	public bool isEnabled = false;

	public string outputRoot;
	public string middlePath;

	public bool isPassbyBuild = false; //Use it to simulate build and give to after build hooks previously build game

	public string scriptingDefineSymbolsOverride;

	public BuildTargetGroup targetGroup;
	public BuildTarget target;
	public BuildOptions options;

	public bool isVirtualRealitySupported;

	public bool isReleaseBuild;	// Maximum compressed build with Release IL2CPP

	public bool needZip;
	public string compressDirPath;

	public bool needItchPush;
	public string itchChannel;
	public string itchDirPath;
	public bool itchAddLastChangelogUpdateNameToVerison;
	public string itchLastChangelogUpdateName;  //Fill from code

	//TODO: 
	public bool useGithubActions;
	public bool useUnityActions;
	public bool needGamejolthPush;
	public bool needSteamPush;
	public bool needGoogleDrivePush;

	public BuildData() : this(
		BuildTargetGroup.Unknown,
		BuildTarget.NoTarget
	) {

	}

	public BuildData(BuildTargetGroup targetGroup, BuildTarget target) {
		this.targetGroup = targetGroup;
		this.target = target;

		isEnabled = true;
		isPassbyBuild = false;

		scriptingDefineSymbolsOverride = "";

		options = BuildOptions.None;
		isVirtualRealitySupported = false;

		outputRoot = "Builds/";
		middlePath = "$NAME_$VERSION_$PLATFORM/$NAME_$VERSION/$NAME$EXECUTABLE";

		isReleaseBuild = false;

		needZip = false;
		compressDirPath = "$NAME_$VERSION_$PLATFORM";

		needItchPush = false;
		itchChannel = "channel";
		itchDirPath = "$NAME_$VERSION_$PLATFORM";
		itchAddLastChangelogUpdateNameToVerison = true;

		//TODO: 
		useGithubActions = false;
		useUnityActions = false;
		needGamejolthPush = false;
		needSteamPush = false;
		needGoogleDrivePush = false;
	}

	public object Clone() {
		return this.MemberwiseClone();
	}
}
