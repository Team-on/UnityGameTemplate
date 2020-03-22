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
		BuildManagerOld.BuildAllSequence(true, true);
	}

	[MenuItem("Build/Build All \u2192 itch.io push")]
	public static void BuildAllPush() {
		BuildManagerOld.BuildAllSequence(false, true);
	}

	[MenuItem("Build/Build All \u2192 ZIP")]
	public static void BuildAllZipped() {
		BuildManagerOld.BuildAllSequence(true, false);
	}

	[MenuItem("Build/Build All")]
	public static void BuildAll() {
		BuildManagerOld.BuildAllSequence(false, false);
	}

	[MenuItem("Build/Build Windows")]
	public static void BuildWindows() {
		BuildManagerOld.BuildWindows(false);
	}

	[MenuItem("Build/Build Windows x64")]
	public static void BuildWindowsX64() {
		BuildManagerOld.BuildWindowsX64(false);
	}

	[MenuItem("Build/Build Linux")]
	public static void BuildLinux() {
		BuildManagerOld.BuildLinux(false);

	}

	[MenuItem("Build/Build OSX")]
	public static void BuildOSX() {
		BuildManagerOld.BuildOSX(false);
	}

	[MenuItem("Build/Build Web")]
	public static void BuildWeb() {
		BuildManagerOld.BuildWeb(false);
	}

	[MenuItem("Build/Build Android")]
	public static void BuildAndroid() {
		BuildManagerOld.BuildAndroid(false);
	}

	[MenuItem("Build/Build Ios")]
	public static void BuildIos() {
		BuildManagerOld.BuildIos(false);
	}
}
