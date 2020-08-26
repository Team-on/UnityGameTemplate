using System;
using System.IO;
using System.Linq;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;

public class ScreenshotCaptureDebug : MonoBehaviour {
	string savePath => useCustomSavePath ? overrideSavePath : ScreenshotTaker.GetDefaultScreenshotPath();

	[Header("Data")]
	[Space]
	[SerializeField] bool useCustomSavePath;
	[EnableIf("useCustomSavePath")]
	[SerializeField] string overrideSavePath = "C:\\Users\\LenovoLegionAdmin\\Documents\\ScreenshotsUnity\\";

	[Header("Screenshot keys")]
	[Space]
	[SerializeField] KeyCode screenshotKey = KeyCode.F7;
	[SerializeField] KeyCode openScreenshotFolderKey = KeyCode.F8;

	void Update() {
		if (Input.GetKeyDown(screenshotKey)) {
			ScreenshotTaker.TakeScreenshot(savePath);
		}
		else if (Input.GetKeyDown(openScreenshotFolderKey)) {
			var file = Directory.EnumerateFiles(savePath).FirstOrDefault();
			if (!string.IsNullOrEmpty(file))
				ShowExplorer(Path.Combine(savePath, file));
			else
				ShowExplorer(savePath);
		}
	}

	void ShowExplorer(string itemPath) {
		itemPath = itemPath.Replace(@"/", @"\");   // explorer doesn't like front slashes
		System.Diagnostics.Process.Start("explorer.exe", "/select," + itemPath);
	}
}
