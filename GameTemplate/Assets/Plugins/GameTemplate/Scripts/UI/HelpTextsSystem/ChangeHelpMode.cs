using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeHelpMode : MonoBehaviour {
	public void CycleHelpLevel() {
		if(TemplateGameManager.Instance.HelpLevelMode == TemplateGameManager.Instance.minHelpLevel) {
			TemplateGameManager.Instance.HelpLevelMode = TemplateGameManager.Instance.maxHelpLevel;
		}
		else {
			--TemplateGameManager.Instance.HelpLevelMode;
		}
	}
}
