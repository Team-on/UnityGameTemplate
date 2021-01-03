using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabMenuBase : PopupMenuBase {
	[Header("Tab menu settings")]
	[SerializeField] float alphaChangeSpeed = 0.2f;
	[SerializeField] Button ArrowLeft;
	[SerializeField] Button ArrowRight;
	[SerializeField] CanvasGroup[] Tabs;

	byte currTab = 0;

	new void Awake() {
		base.Awake();

		foreach (var tab in Tabs) {
			tab.alpha = 0;
			tab.interactable = tab.blocksRaycasts = false;
		}

		if (Tabs.Length != 0) {
			Tabs[currTab].alpha = 1;
			Tabs[currTab].interactable = Tabs[currTab].blocksRaycasts = true;
		}

		ArrowLeft.gameObject.SetActive(false);
	}

	internal override void Show(bool isForce) {
		if(Tabs.Length != 0) {
			Tabs[currTab].alpha = 0;
			Tabs[currTab].interactable = Tabs[currTab].blocksRaycasts = false;

			currTab = 0;
			Tabs[currTab].alpha = 1;
			Tabs[currTab].interactable = Tabs[currTab].blocksRaycasts = true;
		}

		if (Tabs.Length > 1) {
			ArrowLeft.gameObject.SetActive(false);
			ArrowRight.gameObject.SetActive(true);
		}
		else {
			ArrowLeft.gameObject.SetActive(false);
			ArrowRight.gameObject.SetActive(false);
		}

		base.Show(isForce);
	}

	public void TabLeft() {
		if (currTab != 0) {
			if (currTab == Tabs.Length - 1)
				ArrowRight.gameObject.SetActive(true);

			LeanTween.alphaCanvas(Tabs[currTab], 0, alphaChangeSpeed);
			Tabs[currTab].interactable = Tabs[currTab].blocksRaycasts = false;

			--currTab;
			LeanTween.alphaCanvas(Tabs[currTab], 1, alphaChangeSpeed);
			Tabs[currTab].interactable = Tabs[currTab].blocksRaycasts = true;

			if (currTab == 0)
				ArrowLeft.gameObject.SetActive(false);
		}
	}

	public void TabRight() {
		if (currTab != Tabs.Length - 1) {
			if (currTab == 0)
				ArrowLeft.gameObject.SetActive(true);

			LeanTween.alphaCanvas(Tabs[currTab], 0, alphaChangeSpeed);
			Tabs[currTab].interactable = Tabs[currTab].blocksRaycasts = false;

			++currTab;
			LeanTween.alphaCanvas(Tabs[currTab], 1, alphaChangeSpeed);
			Tabs[currTab].interactable = Tabs[currTab].blocksRaycasts = true;

			if (currTab == Tabs.Length - 1)
				ArrowRight.gameObject.SetActive(false);
		}
	}
}
