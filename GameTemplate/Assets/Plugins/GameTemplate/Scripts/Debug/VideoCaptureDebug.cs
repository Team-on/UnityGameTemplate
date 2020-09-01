using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using TMPro;
using RockVR.Video;

public class VideoCaptureDebug : MonoBehaviour {
	string savePath => vc.filePath;

	[Header("Data")]
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
			if (vc == null){
				vc = TemplateGameManager.Instance.Camera.gameObject.AddComponent<VideoCapture>();
				vc.customPath = false;
				vc.customPathFolder = "";

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

			LeanTween.cancel(gameObject, false);
			recordingText.alpha = 1;
			recordingImg.color = recordingImg.color.SetA(1);
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
			recordingText.text = "Stopped. (Save in progress)";

			videoCaptureCtrl.StopCapture();
		}
		else if (!isProcessFinish && videoCaptureCtrl != null && videoCaptureCtrl.status == VideoCaptureCtrlBase.StatusType.FINISH) {
			isProcessFinish = true;

			recordingImg.gameObject.SetActive(false);
			recordingText.gameObject.SetActive(true);
			recordingText.text = $"Completed. (Press {openVideoFolderKey} to open)";

			LeanTween.delayedCall(gameObject, 2.0f, () => {
				LeanTween.value(gameObject, recordingText.alpha, 0.0f, 3.0f)
				.setOnUpdate((float a) => {
					recordingText.alpha = a;
					recordingImg.color = recordingImg.color.SetA(a);
				})
				.setOnComplete(()=> {
					recordingImg.gameObject.SetActive(false);
					recordingText.gameObject.SetActive(false);
				});
			});

			Debug.Log($"End saving video. {savePath}");
		}
		else if (Input.GetKeyDown(openVideoFolderKey)) {
			string dir = PathConfig.SaveFolder;

			var file = Directory.EnumerateFiles(dir).FirstOrDefault();
			if (!string.IsNullOrEmpty(file))
				ShowExplorer(Path.Combine(dir, file));
			else
				ShowExplorer(dir);
		}
	}

	void ShowExplorer(string itemPath) {
		itemPath = itemPath.Replace(@"/", @"\");   // explorer doesn't like front slashes
		System.Diagnostics.Process.Start("explorer.exe", "/select," + itemPath);
	}
}
