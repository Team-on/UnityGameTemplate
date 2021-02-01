using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanelGroup : MonoBehaviour {
	[SerializeField] KeyCode showKey = KeyCode.BackQuote;

	[Header("Refs"), Space]
	[SerializeField] CanvasGroup cg;
	[SerializeField] CanvasGroup disablecg;

	bool isShowed;

	private void Awake() {
		isShowed = false;
		cg.interactable = cg.blocksRaycasts = false;
		cg.alpha = 0.0f;

		disablecg.interactable = disablecg.blocksRaycasts = false;
		disablecg.alpha = 0.0f;
	}

	private void Update() {
		if (Input.GetKeyDown(showKey)) {
			if (isShowed)
				Hide();
			else
				Show();
		}
	}

	void Show() {
		if (isShowed)
			return;
		isShowed = true;

		LeanTween.cancel(gameObject);

		disablecg.interactable = disablecg.blocksRaycasts = true;
		disablecg.alpha = 1.0f;
		cg.interactable = cg.blocksRaycasts = true;

		LeanTween.alphaCanvas(cg, 1.0f, 0.05f);
	}

	void Hide() {
		if (!isShowed)
			return;
		isShowed = false;

		LeanTween.cancel(gameObject);

		cg.interactable = cg.blocksRaycasts = false;
		LeanTween.alphaCanvas(cg, 0.0f, 0.05f)
		.setOnComplete(()=> {
			disablecg.interactable = disablecg.blocksRaycasts = false;
			disablecg.alpha = 0.0f;
		});
	}
}
