using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using TMPro;
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
	[SerializeField] VideoCaptureBase.EncodeQualityType encodeQuality = VideoCaptureBase.EncodeQualityType.Medium;
	[SerializeField] VideoCaptureBase.AntiAliasingType antiAliasing = VideoCaptureBase.AntiAliasingType._2;
	[SerializeField] VideoCaptureBase.TargetFramerateType targetFramerate = VideoCaptureBase.TargetFramerateType._30;

	[Header("Video keys")]
	[Space]
	[SerializeField] KeyCode startVideoKey = KeyCode.F1;
	[SerializeField] KeyCode pauseVideoKey = KeyCode.F2;
	[SerializeField] KeyCode stopVideoKey = KeyCode.F3;
	[SerializeField] KeyCode openVideoFolderKey = KeyCode.F4;

	[Header("UI")]
	[Space]
	[SerializeField] Image recordingImg;
	[SerializeField] TextMeshProUGUI recordingText;

	[Header("Refs")]
	[Space]
	[SerializeField] VideoCaptureCtrl videoCaptureCtrl;
	VideoCapture vc;
	AudioCapture ac;

	bool isProcessFinish;

#if UNITY_EDITOR
	private void OnValidate() {
		if (videoCaptureCtrl == null)
			videoCaptureCtrl = GetComponent<VideoCaptureCtrl>();
	}
#endif

	void Update() {
		if (Input.GetKeyDown(startVideoKey) && (videoCaptureCtrl.status == VideoCaptureCtrlBase.StatusType.NOT_START || videoCaptureCtrl.status == VideoCaptureCtrlBase.StatusType.FINISH)) {
			if(vc == null){
				vc = TemplateGameManager.Instance.Camera.gameObject.AddComponent<VideoCapture>();
				vc.customPath = useCustomSavePath;
				vc.customPathFolder = overrideSavePath;

				vc.isDedicated = false;

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

			recordingImg.gameObject.SetActive(true);
			recordingText.gameObject.SetActive(true);
			recordingText.text = "Recoring";
			isProcessFinish = false;
			videoCaptureCtrl.StartCapture();
		}
		else if (Input.GetKeyDown(pauseVideoKey) && videoCaptureCtrl != null && (videoCaptureCtrl.status == VideoCaptureCtrlBase.StatusType.STARTED || videoCaptureCtrl.status == VideoCaptureCtrlBase.StatusType.PAUSED)) {
			videoCaptureCtrl.ToggleCapture();

			if (videoCaptureCtrl.status == VideoCaptureCtrlBase.StatusType.PAUSED) {
				recordingImg.gameObject.SetActive(false);
				recordingText.gameObject.SetActive(true);
				recordingText.text = "Paused";
			}
			else {
				recordingImg.gameObject.SetActive(true);
				recordingText.gameObject.SetActive(true);
				recordingText.text = "Recoring";
			}
		}
		else if (Input.GetKeyDown(stopVideoKey) && videoCaptureCtrl != null && (videoCaptureCtrl.status == VideoCaptureCtrlBase.StatusType.STARTED || videoCaptureCtrl.status == VideoCaptureCtrlBase.StatusType.PAUSED)) {
			recordingImg.gameObject.SetActive(false);
			recordingText.gameObject.SetActive(true);
			recordingText.text = "Stopped";

			videoCaptureCtrl.StopCapture();
		}
		else if (!isProcessFinish && videoCaptureCtrl != null && videoCaptureCtrl.status == VideoCaptureCtrlBase.StatusType.FINISH) {
			isProcessFinish = true;

			recordingImg.gameObject.SetActive(false);
			recordingText.gameObject.SetActive(true);
			recordingText.text = "Completed";

			LeanTween.delayedCall(gameObject, 5.0f, () => {
				recordingImg.gameObject.SetActive(false);
				recordingText.gameObject.SetActive(false);
			});

			Debug.Log($"End saving video. {savePath}");
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
}
