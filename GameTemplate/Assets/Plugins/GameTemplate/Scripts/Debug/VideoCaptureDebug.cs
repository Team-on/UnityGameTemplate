using System;
using System.IO;
using System.Linq;
using UnityEngine;
using NaughtyAttributes;
using RockVR.Video;

public class VideoCaptureDebug : MonoBehaviour {
	string savePath => useCustomSavePath ? overrideSavePath : vc.filePath;


	[Header("Data")]
	[Space]
	[SerializeField] bool useCustomSavePath;
	[EnableIf("useCustomSavePath")]
	[SerializeField] string overrideSavePath = "C:\\Users\\LenovoLegionAdmin\\Documents\\ScreenshotsUnity\\";
	[Space]
	[SerializeField] VideoCaptureBase.FrameSizeType frameSize = VideoCaptureBase.FrameSizeType._1920x1080;
	[SerializeField] bool isOfflineRenderer = true;
	[SerializeField] VideoCaptureBase.EncodeQualityType encodeQuality = VideoCaptureBase.EncodeQualityType.High;
	[SerializeField] VideoCaptureBase.AntiAliasingType antiAliasing = VideoCaptureBase.AntiAliasingType._8;
	[SerializeField] VideoCaptureBase.TargetFramerateType targetFramerate = VideoCaptureBase.TargetFramerateType._60;

	[Header("Viedo keys")]
	[Space]
	[SerializeField] KeyCode startVideoKey = KeyCode.F1;
	[SerializeField] KeyCode pauseVideoKey = KeyCode.F2;
	[SerializeField] KeyCode stopVideoKey = KeyCode.F3;
	[SerializeField] KeyCode openVideoFolderKey = KeyCode.F4;

	[Header("Refs")]
	[Space]
	[SerializeField] VideoCaptureCtrl videoCaptureCtrl;
	VideoCapture vc;
	AudioCapture ac;

#if UNITY_EDITOR
	private void OnValidate() {
		if (videoCaptureCtrl == null)
			videoCaptureCtrl = GetComponent<VideoCaptureCtrl>();
	}
#endif

	void Update() {
		if (Input.GetKeyDown(startVideoKey)) {
			if(vc == null){
				vc = TemplateGameManager.Instance.Camera.gameObject.AddComponent<VideoCapture>();
				vc.customPath = useCustomSavePath;
				vc.customPathFolder = overrideSavePath;


				vc.frameSize = frameSize;
				vc.offlineRender = isOfflineRenderer;
				vc.encodeQuality = encodeQuality;
				vc._antiAliasing = antiAliasing;
				vc._targetFramerate = targetFramerate;

				videoCaptureCtrl.videoCaptures[0] = vc;
			}
			if (ac == null) {
				ac = TemplateGameManager.Instance.Camera.gameObject.AddComponent<AudioCapture>();

				videoCaptureCtrl.audioCapture = ac;
			}


			videoCaptureCtrl.StartCapture();
		}
		else if (Input.GetKeyDown(pauseVideoKey) && videoCaptureCtrl != null) {
			videoCaptureCtrl.ToggleCapture();
		}
		else if (Input.GetKeyDown(stopVideoKey) && videoCaptureCtrl != null) {
			videoCaptureCtrl.StopCapture();
			videoCaptureCtrl.eventDelegate.OnComplete += OnCompleteSaving;
		}
		else if (Input.GetKeyDown(openVideoFolderKey) && (useCustomSavePath || videoCaptureCtrl != null)) {
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

	void OnCompleteSaving() {
		Debug.Log($"End saving video. {savePath}");
	}
}
