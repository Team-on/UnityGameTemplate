using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using Ionic.Zip;

using Debug = UnityEngine.Debug;

public static class BuildManager {
	const string butlerRelativePath = @"Plugins/Editor/BuildManager/BuildManager/butler/butler.exe";
	static DateTime usedDate;

	public static void RunBuildSequnce(BuildManagerSettings settings, BuildSequence sequence, ChangelogData changelog) {
		// Start init
		string buildNameString = $"{PlayerSettings.bundleVersion} - {changelog.updateName}";
#if GAME_TEMPLATE
		TemplateGameManager.InstanceEditor.buildNameString = buildNameString;
		TemplateGameManager.InstanceEditor.productName = PlayerSettings.productName;
#endif
		usedDate = DateTime.Now;
		//End init

		Debug.Log("Start building all");
		DateTime startTime = DateTime.Now;
		BuildTarget targetBeforeStart = EditorUserBuildSettings.activeBuildTarget;
		BuildTargetGroup targetGroupBeforeStart = BuildPipeline.GetBuildTargetGroup(targetBeforeStart);
		string definesBeforeStart = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroupBeforeStart);
		bool isVRSupported = PlayerSettings.virtualRealitySupported;    //TODO: PlayerSettings.virtualRealitySupported is deprecated. Replace with smth new	

		string[] buildsPath = new string[sequence.builds.Count];
		for (byte i = 0; i < sequence.builds.Count; ++i) {
			BuildData data = sequence.builds[i];

			if (!data.isEnabled)
				continue;

			if (PlayerSettings.virtualRealitySupported != data.isVirtualRealitySupported)
				PlayerSettings.virtualRealitySupported = data.isVirtualRealitySupported;

			buildsPath[i] = BaseBuild(
				data.targetGroup,
				data.target,
				data.options,
				data.outputRoot + GetPathWithVars(data, data.middlePath),
				string.Concat(settings.scriptingDefineSymbols, ";", sequence.scriptingDefineSymbolsOverride, ";", data.scriptingDefineSymbolsOverride),
				data.isPassbyBuild,
				data.isReleaseBuild
			);


		}

		EditorUserBuildSettings.SwitchActiveBuildTarget(targetGroupBeforeStart, targetBeforeStart);
		PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroupBeforeStart, definesBeforeStart);
		PlayerSettings.virtualRealitySupported = isVRSupported;
		Debug.Log($"End building all. Elapsed time: {string.Format("{0:mm\\:ss}", DateTime.Now - startTime)}");

		for (byte i = 0; i < sequence.builds.Count; ++i) {
			if (!sequence.builds[i].needZip || !sequence.builds[i].isEnabled)
				continue;

			if (sequence.builds[i].target == BuildTarget.Android) {
				Debug.Log("Skip android build to .zip, because .apk files already compressed");
				continue;
			}

			if (!string.IsNullOrEmpty(buildsPath[i])) {
				if (sequence.builds[i].isReleaseBuild) {  //Destroy IL2CPP junk after build
					string buildRootPath = Path.GetDirectoryName(buildsPath[i]);
					string[] dirs = Directory.GetDirectories(buildRootPath);
					var il2cppDirs = dirs.Where(s => s.Contains("BackUpThisFolder_ButDontShipItWithYourGame"));
					foreach (var dir in il2cppDirs)
						Directory.Delete(dir, true);
				}
			}
		}

		startTime = DateTime.Now;
		Debug.Log($"Start compressing all");

		for (byte i = 0; i < sequence.builds.Count; ++i) {
			if (!sequence.builds[i].needZip || !sequence.builds[i].isEnabled)
				continue;

			if (sequence.builds[i].target == BuildTarget.Android) {
				Debug.Log("Skip android build to .zip, because .apk files already compressed");
				continue;
			}

			if (!string.IsNullOrEmpty(buildsPath[i]))
				BaseCompress(sequence.builds[i].outputRoot + GetPathWithVars(sequence.builds[i], sequence.builds[i].compressDirPath));
			else
				Debug.LogWarning($"[Compressing] Can't find build for {GetBuildTargetExecutable(sequence.builds[i].target)}");
		}

		Debug.Log($"End compressing all. Elapsed time: {string.Format("{0:mm\\:ss}", DateTime.Now - startTime)}");


		for (byte i = 0; i < sequence.builds.Count; ++i) {
			if (!sequence.builds[i].needItchPush || !sequence.builds[i].isEnabled)
				continue;

			if (!string.IsNullOrEmpty(buildsPath[i])) {
				if (sequence.builds[i].itchAddLastChangelogUpdateNameToVerison && !string.IsNullOrEmpty(changelog?.updateName)) {
					sequence.builds[i].itchLastChangelogUpdateName = buildNameString;
				}
				PushItch(sequence, sequence.builds[i]);
			}
			else {
				Debug.LogWarning($"[Itch.io push] Can't find build for {GetBuildTargetExecutable(sequence.builds[i].target)}");
			}
		}

		ShowExplorer(sequence.builds[sequence.builds.Count - 1].outputRoot);
	}

	#region Convert to strings
	public static string GetPathWithVars(BuildData data, string s) {
		s = s.Replace("$NAME", GetProductName());
		s = s.Replace("$PLATFORM", ConvertBuildTargetToString(data.target));
		s = s.Replace("$VERSION", PlayerSettings.bundleVersion);
		s = s.Replace("$DATESHORT", $"{usedDate.Date.Year % 100}_{usedDate.Date.Month}_{usedDate.Date.Day}");
		s = s.Replace("$YEARSHORT", $"{usedDate.Date.Year % 100}");
		s = s.Replace("$DATE", $"{usedDate.Date.Year}_{usedDate.Date.Month}_{usedDate.Date.Day}");
		s = s.Replace("$YEAR", $"{usedDate.Date.Year}");
		s = s.Replace("$MONTH", $"{usedDate.Date.Month}");
		s = s.Replace("$DAY", $"{usedDate.Date.Day}");
		s = s.Replace("$TIME", $"{usedDate.Hour}_{usedDate.Minute}");
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
	static string BaseBuild(BuildTargetGroup buildTargetGroup, BuildTarget buildTarget, BuildOptions buildOptions, string buildPath, string definesSymbols, bool isPassbyBuild, bool isReleaseBuild) {
		if (isPassbyBuild) {
			return buildPath;
		}

		if (buildTarget == BuildTarget.Android && PlayerSettings.Android.useCustomKeystore && string.IsNullOrEmpty(PlayerSettings.Android.keyaliasPass)) {
			PlayerSettings.Android.keyaliasPass = PlayerSettings.Android.keystorePass = "keystore";
		}

		if (isReleaseBuild) {
			switch (buildTargetGroup) {
				case BuildTargetGroup.Standalone:
					buildOptions |= BuildOptions.CompressWithLz4;

					if (buildTarget == BuildTarget.StandaloneWindows || buildTarget == BuildTarget.StandaloneWindows64 || buildTarget == BuildTarget.StandaloneLinux64)
						PlayerSettings.SetScriptingBackend(buildTargetGroup, ScriptingImplementation.IL2CPP);
					else
						PlayerSettings.SetScriptingBackend(buildTargetGroup, ScriptingImplementation.Mono2x);
					PlayerSettings.SetIl2CppCompilerConfiguration(buildTargetGroup, Il2CppCompilerConfiguration.Master);
					break;
				case BuildTargetGroup.Android:
					buildOptions |= BuildOptions.CompressWithLz4;

					PlayerSettings.SetScriptingBackend(buildTargetGroup, ScriptingImplementation.IL2CPP);
					PlayerSettings.SetIl2CppCompilerConfiguration(buildTargetGroup, Il2CppCompilerConfiguration.Master);

					PlayerSettings.Android.targetArchitectures = AndroidArchitecture.All;
					break;
				case BuildTargetGroup.WebGL:
					PlayerSettings.SetIl2CppCompilerConfiguration(buildTargetGroup, Il2CppCompilerConfiguration.Master);
					break;
				default:
					Debug.LogWarning($"{buildTargetGroup} is unsupported for release builds. No optimizations applied");
					break;
			}
		}
		else {
			switch (buildTargetGroup) {
				case BuildTargetGroup.Standalone:
					buildOptions ^= BuildOptions.CompressWithLz4;
					buildOptions ^= BuildOptions.CompressWithLz4HC;

					PlayerSettings.SetScriptingBackend(buildTargetGroup, ScriptingImplementation.Mono2x);
					PlayerSettings.SetIl2CppCompilerConfiguration(buildTargetGroup, Il2CppCompilerConfiguration.Debug);
					break;
				case BuildTargetGroup.Android:
					buildOptions ^= BuildOptions.CompressWithLz4;
					buildOptions ^= BuildOptions.CompressWithLz4HC;

					PlayerSettings.SetScriptingBackend(buildTargetGroup, ScriptingImplementation.Mono2x);
					PlayerSettings.SetIl2CppCompilerConfiguration(buildTargetGroup, Il2CppCompilerConfiguration.Debug);

					PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
					break;
				case BuildTargetGroup.WebGL:
					PlayerSettings.SetIl2CppCompilerConfiguration(buildTargetGroup, Il2CppCompilerConfiguration.Debug);
					break;
				default:
					Debug.LogWarning($"{buildTargetGroup} is unsupported for debug builds. No optimizations applied");
					break;
			}
		}

		BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions {
			scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray(),
			locationPathName = buildPath,
			targetGroup = buildTargetGroup,
			target = buildTarget,
			options = buildOptions,
		};

		PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, definesSymbols);
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
		if (data.itchAddLastChangelogUpdateNameToVerison && !string.IsNullOrEmpty(data.itchLastChangelogUpdateName)) {
			args.Append($"--userversion \"{data.itchLastChangelogUpdateName}\" ");
		}
		else {
			args.Append($"--userversion \"{PlayerSettings.bundleVersion}\" ");
		}

		Debug.Log(fileName.ToString() + args.ToString());
		Process.Start(fileName.ToString(), args.ToString());
	}
	#endregion

	static void ShowExplorer(string itemPath) {
		itemPath = itemPath.Replace(@"/", @"\");   // explorer doesn't like front slashes
		System.Diagnostics.Process.Start("explorer.exe", "/select," + itemPath);
	}
}
