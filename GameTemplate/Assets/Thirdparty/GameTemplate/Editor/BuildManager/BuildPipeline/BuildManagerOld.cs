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

//TODO: publish to itch.io via butler
public static class BuildManagerOld {
	//TODO: move to settings that can be changed in editor;
	const string butlerRelativePath = @"Thirdparty/GameTemplate/Editor/BuildManager/butler/butler.exe";
	static string[] channelNames = new string[] {
		"windows-32",
		"windows-64",
		"linux-universal",
		"osx-universal",
		"webgl",
		"android",
		"ios",
		"uwp",
	};


	public static string LastBundleVersion {
		get {
			if (_LastBundleVersion == null)
				_LastBundleVersion = PlayerPrefs.GetString("BuildManager.LastBundleVersion", "0.0.0.0");
			return _LastBundleVersion;
		}
		set {
			_LastBundleVersion = value;
			PlayerPrefs.SetString("BuildManager.LastBundleVersion", _LastBundleVersion);
			PlayerPrefs.Save();
		}
	}
	public static int LastBuildPatch {
		get {
			if (_LastBuildPatch == -1)
				_LastBuildPatch = PlayerPrefs.GetInt("BuildManager.LastBuildPatch", 0);
			return _LastBuildPatch;
		}
		set {
			_LastBuildPatch = value;
			PlayerPrefs.SetInt("BuildManager.LastBuildPatch", _LastBuildPatch);
			PlayerPrefs.Save();
		}
	}
	static string _LastBundleVersion = null;
	static int _LastBuildPatch = -1;

	static List<string> buildsPath;

	// Для пабліша на itch.io не треба zip. Він все одно розархівується, а потім заархівується по новому, так як треба itch
	// Але я це оставив, щоб не робити архіви ручками, якщо щось треба отправить
	public static void BuildAllSequence(bool needZip, bool needPush) {
		BuildAll();

		if (needZip)
			CompressAll(buildsPath);

		if (needPush)
			PushAll(buildsPath);

		++LastBuildPatch;
	}


	static void BuildAll() {
		Debug.Log("Start building all");
		DateTime startTime = DateTime.Now;
		BuildTarget targetBeforeStart = EditorUserBuildSettings.activeBuildTarget;
		BuildTargetGroup targetGroupBeforeStart = BuildPipeline.GetBuildTargetGroup(targetBeforeStart);

		buildsPath = new List<string>(6);
		buildsPath.Add(BuildWindows(true));
		buildsPath.Add(BuildWindowsX64(true));
		buildsPath.Add(BuildLinux(true));
		buildsPath.Add(BuildOSX(true));
		buildsPath.Add(BuildWeb(true));
		buildsPath.Add(BuildAndroid(true));
		EditorUserBuildSettings.SwitchActiveBuildTarget(targetGroupBeforeStart, targetBeforeStart);
		Debug.Log($"End building all. Elapsed time: {string.Format("{0:mm\\:ss}", DateTime.Now - startTime)}");
	}

	static void CompressAll(List<string> buildsPath) {
		DateTime startTime = DateTime.Now;
		Debug.Log($"Start compressing all");

		for (byte i = 0; i < buildsPath.Count; ++i)
			if(buildsPath[i] != "")
				Compress(buildsPath[i]);

		Debug.Log($"End compressing all. Elapsed time: {string.Format("{0:mm\\:ss}", DateTime.Now - startTime)}");
	}

	static void PushAll(List<string> buildsPath) {
		DateTime startTime = DateTime.Now;
		Debug.Log($"Start pushing all");

		for (byte i = 0; i < buildsPath.Count; ++i) 
			if(buildsPath[i] != "")
				Push(buildsPath[i], channelNames[i]);

		Debug.Log($"End pushing all. Elapsed time: {string.Format("{0:mm\\:ss}", DateTime.Now - startTime)}");
	}

	public static string BuildWindows(bool isInBuildSequence) {
		return BaseBuild(
			BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows,
			isInBuildSequence ? BuildOptions.None : BuildOptions.ShowBuiltPlayer,
			!isInBuildSequence,
			!isInBuildSequence,
			$"_Windows",
			$"_Windows/{PlayerSettings.productName}_{PlayerSettings.bundleVersion}.{LastBuildPatch}/{PlayerSettings.productName}.exe"
		);
	}

	public static string BuildWindowsX64(bool isInBuildSequence) {
		return BaseBuild(
			BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64,
			isInBuildSequence ? BuildOptions.None : BuildOptions.ShowBuiltPlayer,
			!isInBuildSequence,
			!isInBuildSequence,
			"_Windows64",
			$"_Windows64/{PlayerSettings.productName}_{PlayerSettings.bundleVersion}.{LastBuildPatch}/{PlayerSettings.productName}.exe"
		);
	}

	public static string BuildLinux(bool isInBuildSequence) {
		return BaseBuild(
			BuildTargetGroup.Standalone, BuildTarget.StandaloneLinux64,
			isInBuildSequence ? BuildOptions.None : BuildOptions.ShowBuiltPlayer,
			!isInBuildSequence,
			!isInBuildSequence,
			"_Linux",
			$"_Linux/{PlayerSettings.productName}_{PlayerSettings.bundleVersion}.{LastBuildPatch}/{PlayerSettings.productName}.x86_64"
		);
	}

	public static string BuildOSX(bool isInBuildSequence) {
		return BaseBuild(
			BuildTargetGroup.Standalone, BuildTarget.StandaloneOSX,
			isInBuildSequence ? BuildOptions.None : BuildOptions.ShowBuiltPlayer,
			!isInBuildSequence,
			!isInBuildSequence,
			"_OSX",
			$"_OSX/{PlayerSettings.productName}_{PlayerSettings.bundleVersion}.{LastBuildPatch}/{PlayerSettings.productName}"
		);
	}

	public static string BuildWeb(bool isInBuildSequence) {
		return BaseBuild(
			BuildTargetGroup.WebGL, BuildTarget.WebGL,
			isInBuildSequence ? BuildOptions.None : BuildOptions.ShowBuiltPlayer,
			!isInBuildSequence,
			!isInBuildSequence,
			"_Web",
			$"_Web"
		);
	}

	public static string BuildAndroid(bool isInBuildSequence) {
		return BaseBuild(
			BuildTargetGroup.Android, BuildTarget.Android,
			isInBuildSequence ? BuildOptions.None : BuildOptions.ShowBuiltPlayer,
			!isInBuildSequence,
			!isInBuildSequence,
			".apk",
			$".apk"
		);
	}

	public static string BuildIos(bool isInBuildSequence) {
		return BaseBuild(
			BuildTargetGroup.iOS, BuildTarget.iOS,
			isInBuildSequence ? BuildOptions.None : BuildOptions.ShowBuiltPlayer,
			!isInBuildSequence,
			!isInBuildSequence,
			".ipa",
			$".ipa"
		);
	}

	static string BaseBuild(BuildTargetGroup buildTargetGroup, BuildTarget buildTarget, BuildOptions buildOptions, bool needReturnBuildTarget, bool incrementPatch, string folderPath, string buildPath) {
		folderPath = folderPath.Replace(' ', '-');
		buildPath = buildPath.Replace(' ', '-');
		string basePath = $"Builds/{PlayerSettings.productName}_{PlayerSettings.bundleVersion}.{LastBuildPatch}".Replace(' ', '-');
		BuildTarget targetBeforeStart = EditorUserBuildSettings.activeBuildTarget;
		BuildTargetGroup targetGroupBeforeStart = BuildPipeline.GetBuildTargetGroup(targetBeforeStart);

		if (LastBundleVersion != PlayerSettings.bundleVersion) {
			LastBundleVersion = PlayerSettings.bundleVersion;
			LastBuildPatch = 0;
		}

		BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions {
			scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray(),
			locationPathName = basePath + buildPath,
			targetGroup = buildTargetGroup,
			target = buildTarget,
			options = buildOptions,
		};


		BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
		BuildSummary summary = report.summary;

		//TODO: Зробити вивід гарнішим. Щоб виглядало по типу таблиці.
		//Зараз \t не вирівнює його, коли summary.platform дуже різних довжин, наприклад StandaloneWindows та StandaloneOSX
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

		if (incrementPatch)
			++LastBuildPatch;

		if (needReturnBuildTarget)
			EditorUserBuildSettings.SwitchActiveBuildTarget(targetGroupBeforeStart, targetBeforeStart);

		return summary.result == BuildResult.Succeeded ? basePath + folderPath : "";
	}

	public static void Compress(string dirPath) {
		using (ZipFile zip = new ZipFile()) {
			DateTime startTime = DateTime.Now;
			if(Directory.Exists(dirPath))
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
			Debug.Log($"Make .ZIP.  \t\t\t Time: {string.Format("{0:mm\\:ss}", DateTime.Now - startTime)}  \t Size: {uncompresedSize / 1048576} - {compresedSize / 1048576}");
		}
	}

	public static void Push(string dirPath, string channelName) {
		StringBuilder fileName = new StringBuilder(128);
		StringBuilder args = new StringBuilder(128);
		fileName.Append(Application.dataPath);
		fileName.Append("/");
		fileName.Append(butlerRelativePath);

		args.Append(" push \"");
		args.Append(Application.dataPath);
		args.Append("/../");
		args.Append(dirPath);
		args.Append("\" ");

		args.Append($"teamon/{PlayerSettings.productName.Replace(' ', '-')}:{channelName} ");
		args.Append($"--userversion {PlayerSettings.bundleVersion}.{LastBuildPatch} ");

		Debug.Log(fileName.ToString() + args.ToString());
		Process.Start(fileName.ToString(), args.ToString());
	}
}
