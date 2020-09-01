using System.IO;
using System.Linq;
using UnityEngine;
using NaughtyAttributes;

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
			string path = ScreenshotTaker.TakeScreenshot(savePath);
			//ScreenshotTaker.TakeScreenshotTexture2D();
			TemplateGameManager.Instance.debugPopups.ShowPopup($"Capture screenshot to {path}\nPress {openScreenshotFolderKey} to open folder with it");
		}
		else if (Input.GetKeyDown(openScreenshotFolderKey)) {
			var file = Directory.EnumerateFiles(savePath).FirstOrDefault();
			if (!string.IsNullOrEmpty(file))
				ShowExplorer(Path.Combine(savePath, file));
			else
				ShowExplorer(savePath);
		}
	}

	//TODO: move to sole utils class
	// https://stackoverflow.com/questions/2315561/correct-way-in-net-to-switch-the-focus-to-another-application
	void ShowExplorer(string itemPath) {
		itemPath = itemPath.Replace(@"/", @"\");   // explorer doesn't like front slashes
		System.Diagnostics.Process.Start("explorer.exe", "/select," + itemPath);
	}
}
