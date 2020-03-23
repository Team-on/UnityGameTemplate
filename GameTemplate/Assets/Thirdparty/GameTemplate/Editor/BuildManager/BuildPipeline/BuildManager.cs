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
	const string butlerRelativePath = @"Thirdparty/GameTemplate/Editor/BuildManager/butler/butler.exe";

	public static void RunBuildSequnce(BuildSequence sequence) {
		Debug.Log("Start building all");
		DateTime startTime = DateTime.Now;
		BuildTarget targetBeforeStart = EditorUserBuildSettings.activeBuildTarget;
		BuildTargetGroup targetGroupBeforeStart = BuildPipeline.GetBuildTargetGroup(targetBeforeStart);

		string[] buildsPath = new string[sequence.builds.Length];
		for(byte i = 0; i < sequence.builds.Length; ++i) {
			BuildData data = sequence.builds[i];
			buildsPath[i] = BaseBuild(data.targetGroup, data.target, data.options, data.outputRoot + GetPathWithVars(data, data.middlePath));
		}

		EditorUserBuildSettings.SwitchActiveBuildTarget(targetGroupBeforeStart, targetBeforeStart);
		Debug.Log($"End building all. Elapsed time: {string.Format("{0:mm\\:ss}", DateTime.Now - startTime)}");

		startTime = DateTime.Now;
		Debug.Log($"Start compressing all");

		for (byte i = 0; i < sequence.builds.Length; ++i) {
			if (!sequence.builds[i].needZip)
				continue;

			if (!string.IsNullOrEmpty(buildsPath[i])) 
				BaseCompress(sequence.builds[i].outputRoot + GetPathWithVars(sequence.builds[i], sequence.builds[i].compressDirPath));
			else 
				Debug.LogWarning($"[Compressing] Can't find build for {GetBuildTargetExecutable(sequence.builds[i].target)}");
		}

		Debug.Log($"End compressing all. Elapsed time: {string.Format("{0:mm\\:ss}", DateTime.Now - startTime)}");


		for (byte i = 0; i < sequence.builds.Length; ++i) {
			if (!sequence.builds[i].needItchPush)
				continue;

			if (!string.IsNullOrEmpty(buildsPath[i]))
				PushItch(sequence, sequence.builds[i]);
			else
				Debug.LogWarning($"[Itch.io push] Can't find build for {GetBuildTargetExecutable(sequence.builds[i].target)}");
		}
	}

	#region Convert to strings
	public static string GetPathWithVars(BuildData data, string s) {
		s = s.Replace("$NAME", GetProductName());
		s = s.Replace("$PLATFORM", ConvertBuildTargetToString(data.target));
		s = s.Replace("$VERSION", PlayerSettings.bundleVersion);
		s = s.Replace("$DATE", $"{DateTime.Now.Date.Year}_{DateTime.Now.Date.Month}_{DateTime.Now.Date.Day}");
		s = s.Replace("$EXECUTABLE", GetBuildTargetExecutable(data.target));
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
			case BuildTarget.StandaloneWindows64:
				return ".exe";

			case BuildTarget.StandaloneLinux64:
				return ".x86_64";

			case BuildTarget.StandaloneOSX:
				return "";

			case BuildTarget.iOS:
				return ".ipa";

			case BuildTarget.Android:
				return ".apk";

			case BuildTarget.WebGL:
				return "";
		}
		return "";
	}
	#endregion

	#region Base methods
	static string BaseBuild(BuildTargetGroup buildTargetGroup, BuildTarget buildTarget, BuildOptions buildOptions, string buildPath) {
		if (buildTarget == BuildTarget.Android && PlayerSettings.Android.useCustomKeystore && string.IsNullOrEmpty(PlayerSettings.Android.keyaliasPass)) {
			PlayerSettings.Android.keyaliasPass = PlayerSettings.Android.keystorePass = "keystore";
		}

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
			Debug.Log($"{summary.platform} succeeded.  \t Time: {string.Format("{0:mm\\:ss}", summary.totalTime)}  \t Size: {summary.totalSize / 1048576} Mb");
		}
		else if (summary.result == BuildResult.Failed) {
			Debug.Log(
				$"{summary.platform} failed.   \t Time: {string.Format("{0:mm\\:ss}", summary.totalTime)}  \t Size: {summary.totalSize / 1048576} Mb" + "\n" +
				$"Warnings: {summary.totalWarnings}" + "\n" +
				$"Errors:   {summary.totalErrors}"
			);
		}

		return summary.result == BuildResult.Succeeded ? buildPath : "";
	}

	public static void BaseCompress(string dirPath) {
		using (ZipFile zip = new ZipFile()) {
			DateTime startTime = DateTime.Now;
			if (Directory.Exists(dirPath))
				zip.AddDirectory(dirPath);
			else
				zip.AddFile(dirPath);
			zip.Save(dirPath + ".zip");

			long uncompresedSize = 0;
			long compresedSize = 0;
			foreach (var e in zip.Entries) {
				uncompresedSize += e.UncompressedSize;
				compresedSize += e.CompressedSize;
			}
			Debug.Log($"Make {dirPath}.zip.  \t Time: {string.Format("{0:mm\\:ss}", DateTime.Now - startTime)}  \t Size: {uncompresedSize / 1048576} Mb - {compresedSize / 1048576} Mb");
		}
	}

	public static void PushItch(BuildSequence sequence, BuildData data) {
		StringBuilder fileName = new StringBuilder(128);
		StringBuilder args = new StringBuilder(128);
		fileName.Append(Application.dataPath);
		fileName.Append("/");
		fileName.Append(butlerRelativePath);

		args.Append(" push \"");
		args.Append(Application.dataPath);
		args.Append("/../");
		args.Append(data.outputRoot + GetPathWithVars(data, data.itchDirPath));
		args.Append("\" ");

		args.Append($"{sequence.itchGameLink}:{data.itchChannel} ");
		args.Append($"--userversion {PlayerSettings.bundleVersion} ");

		Debug.Log(fileName.ToString() + args.ToString());
		Process.Start(fileName.ToString(), args.ToString());
	}
	#endregion
}
