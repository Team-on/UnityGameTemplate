﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NaughtyAttributes;

public class BuildManagerSettings : ScriptableObject{
	public BuildSequence[] sequences = new BuildSequence[1] { new BuildSequence() };
}

[Serializable]
public class BuildSequence {
	public string editorName = "New build sequence";
	public BuildData[] builds = new BuildData[1] { new BuildData() };
}

[Serializable]
public class BuildData {
	public string outputRoot = "Builds/";
	public string middlePath = "$NAME_$VERSION_$PLATFORM/$NAME_$VERSION/";

	public BuildTargetGroup targetGroup = BuildTargetGroup.Unknown;
	public BuildTarget target = BuildTarget.NoTarget;
	public BuildOptions options = BuildOptions.None;

	public bool needZip;

	public bool needItchPush;

	//TODO: 
	public bool useGithubActions;
	public bool useUnityActions;
	public bool needGamejolthPush;
	public bool needSteamPush;
	public bool needGoogleDricePush;
}