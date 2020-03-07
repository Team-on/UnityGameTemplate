using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayWindow : BaseWindow {
	[SerializeField] float AnimTime;
	[SerializeField] Button ArrowLeft;
	[SerializeField] Button ArrowRight;
	[SerializeField] CanvasGroup[] Tabs;

	byte currTab = 0;

	new void Awake() {
		base.Awake();

		foreach (var tab in Tabs)
			tab.alpha = 0;

		Tabs[0].alpha = 1;
		ArrowLeft.gameObject.SetActive(false);
	}

	public override void Show(bool isForce) {
		Tabs[currTab].alpha = 0;
		Tabs[currTab = 0].alpha = 1;
		ArrowLeft.gameObject.SetActive(false);
		ArrowRight.gameObject.SetActive(true);

		base.Show(isForce);
	}

	public void TabLeft() {
		if(currTab != 0) {
			if (currTab == Tabs.Length - 1)
				ArrowRight.gameObject.SetActive(true);

			LeanTween.alphaCanvas(Tabs[currTab], 0, AnimTime);
			LeanTween.alphaCanvas(Tabs[--currTab], 1, AnimTime);

			if (currTab == 0)
				ArrowLeft.gameObject.SetActive(false);
		}
	}

	public void TabRight() {
		if (currTab != Tabs.Length - 1) {
			if (currTab == 0)
				ArrowLeft.gameObject.SetActive(true);

			LeanTween.alphaCanvas(Tabs[currTab], 0, AnimTime);
			LeanTween.alphaCanvas(Tabs[++currTab], 1, AnimTime);

			if (currTab == Tabs.Length - 1)
				ArrowRight.gameObject.SetActive(false);
		}
	}
}
