using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;

public static class BuildManagerEditor {
	[MenuItem("Build/Build All \u2192 ZIP \u2192 itch.io push")]
	public static void BuildAllZippedPush() {
		BuildManager.BuildAllSequence(true, true);
	}

	[MenuItem("Build/Build All \u2192 itch.io push")]
	public static void BuildAllPush() {
		BuildManager.BuildAllSequence(false, true);
	}

	[MenuItem("Build/Build All \u2192 ZIP")]
	public static void BuildAllZipped() {
		BuildManager.BuildAllSequence(true, false);
	}

	[MenuItem("Build/Build All")]
	public static void BuildAll() {
		BuildManager.BuildAllSequence(false, false);
	}

	[MenuItem("Build/Build Windows")]
	public static void BuildWindows() {
		BuildManager.BuildWindows(false);
	}

	[MenuItem("Build/Build Windows x64")]
	public static void BuildWindowsX64() {
		BuildManager.BuildWindowsX64(false);

	}

	[MenuItem("Build/Build Linux")]
	public static void BuildLinux() {
		BuildManager.BuildLinux(false);

	}

	[MenuItem("Build/Build OSX")]
	public static void BuildOSX() {
		BuildManager.BuildOSX(false);

	}

	[MenuItem("Build/Build Web")]
	public static void BuildWeb() {
		BuildManager.BuildWeb(false);
	}

	[MenuItem("Build/Build Android")]
	public static void BuildAndroid() {
		BuildManager.BuildAndroid(false);
	}

	[MenuItem("Build/Build Ios")]
	public static void BuildIos() {
		BuildManager.BuildIos(false);
	}
}
