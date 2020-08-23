using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabControl : MonoBehaviour {
	
	[SerializeField] TabControlButton[] buttons;
	[SerializeField] GameObject[] content;

	byte currTab = 0;

	private void Awake() {
		for(byte i = 0; i < content.Length; ++i) {
			buttons[i].Init(OnTabClick, i);
			buttons[i].SetSelection(i == currTab);

			content[i].SetActive(i == currTab);
		}
	}

	public void OnTabClick(byte tabId) {
		content[currTab].SetActive(false);
		buttons[currTab].SetSelection(false);

		content[(currTab = tabId)].SetActive(true);
		buttons[currTab].SetSelection(true);
	}
}
