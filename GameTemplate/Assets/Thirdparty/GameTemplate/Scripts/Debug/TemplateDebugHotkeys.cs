using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class TemplateDebugHotkeys : MonoBehaviour {
	CinemachineVirtualCamera cam;

	void Update() {
		if (Input.GetKeyDown(KeyCode.R)) {
			SceneManager.LoadScene(0);
		}
		else if (Input.GetKeyDown(KeyCode.Escape)) {
			QuitGame.QuitApp();
		}
		else if (Input.mouseScrollDelta.y != 0) {
			if (cam == null) {
				cam = TemplateGameManager.Instance.Camera.GetComponent<CinemachineVirtualCamera>();
				if(cam == null) {
					cam = TemplateGameManager.Instance.Camera.GetComponent<CinemachineBrain>()?.ActiveVirtualCamera as CinemachineVirtualCamera;
					if (cam == null)
						cam = FindObjectOfType<CinemachineVirtualCamera>();
				}
			}

			if (cam != null)
				cam.m_Lens.OrthographicSize -= 0.3f * Input.mouseScrollDelta.y;
		}
	}
}
