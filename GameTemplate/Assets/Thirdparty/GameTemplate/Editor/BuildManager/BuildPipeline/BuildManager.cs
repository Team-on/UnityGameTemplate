using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using Ionic.Zip;

using Debug = UnityEngine.Debug;

public static class BuildManager {
	public static void RunBuildSequnce(BuildSequence sequence) {
		Debug.Log("Start building all");
		DateTime startTime = DateTime.Now;
		BuildTarget targetBeforeStart = EditorUserBuildSettings.activeBuildTarget;
		BuildTargetGroup targetGroupBeforeStart = BuildPipeline.GetBuildTargetGroup(targetBeforeStart);

		string[] buildsPath = new string[sequence.builds.Length];
		for(byte i = 0; i < sequence.builds.Length; ++i) {
			BuildData data = sequence.builds[i];
			buildsPath[i] = BaseBuild(data.targetGroup, data.target, data.options, data.outputRoot + GetMiddlePath(data) + GetBuildTargetExecutable(data.target));
		}

		EditorUserBuildSettings.SwitchActiveBuildTarget(targetGroupBeforeStart, targetBeforeStart);
		Debug.Log($"End building all. Elapsed time: {string.Format("{0:mm\\:ss}", DateTime.Now - startTime)}");
	}

	public static string GetMiddlePath(BuildData data) {
		string s = data.middlePath;
		s = s.Replace("$NAME", GetProductName());
		s = s.Replace("$PLATFORM", ConvertBuildTargetToString(data.target));
		s = s.Replace("$VERSION", PlayerSettings.bundleVersion);
		s = s.Replace("DATE", $"{DateTime.Now.Date.Year}_{DateTime.Now.Date.Month}_{DateTime.Now.Date.Day}");
		return s;
	}

	public static string ConvertBuildTargetToString(BuildTarget target) {
		switch (target) {
			case BuildTarget.StandaloneOSX:
				return "OSX";
			case BuildTarget.StandaloneWindows:
				return "Windows32";
			case BuildTarget.StandaloneWindows64:
				return "Windows64";
			case BuildTarget.StandaloneLinux64:
				return "Linux";
		}
		return target.ToString();
	}

	public static string GetProductName() {
		return PlayerSettings.productName.Replace(' ', '_');
	}

	public static string GetBuildTargetExecutable(BuildTarget target) {
		switch (target) {
			case BuildTarget.StandaloneWindows:
				return GetProductName() + ".exe";

			case BuildTarget.StandaloneWindows64:
				return GetProductName() + ".exe";

			case BuildTarget.StandaloneLinux64:
				return GetProductName() + ".x86_64";

			case BuildTarget.StandaloneOSX:
				return GetProductName();

			case BuildTarget.iOS:
				return GetProductName() + ".ipa";

			case BuildTarget.Android:
				return GetProductName() + ".apk";

			case BuildTarget.WebGL:
				return "";
		}
		return "";
	}

	static string BaseBuild(BuildTargetGroup buildTargetGroup, BuildTarget buildTarget, BuildOptions buildOptions, string buildPath) {
		BuildTarget targetBeforeStart = EditorUserBuildSettings.activeBuildTarget;
		BuildTargetGroup targetGroupBeforeStart = BuildPipeline.GetBuildTargetGroup(targetBeforeStart);

		BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions {
			scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray(),
			locationPathName = buildPath,
			targetGroup = buildTargetGroup,
			target = buildTarget,
			options = buildOptions,
		};

		BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
		BuildSummary summary = report.summary;

		if (summary.result == BuildResult.Succeeded) {
			Debug.Log($"{summary.platform} succeeded.  \t Time: {string.Format("{0:mm\\:ss}", summary.totalTime)}  \t Size: {summary.totalSize / 1048576}");
		}
		else if (summary.result == BuildResult.Failed) {
			Debug.Log(
				$"{summary.platform} failed.   \t Time: {string.Format("{0:mm\\:ss}", summary.totalTime)}  \t Size: {summary.totalSize / 1048576}" + "\n" +
				$"Warnings: {summary.totalWarnings}" + "\n" +
				$"Errors:   {summary.totalErrors}"
			);
		}

		return summary.result == BuildResult.Succeeded ? buildPath : "";
	}
}
