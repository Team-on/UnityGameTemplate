using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class CheatcodesHandler : MonoBehaviour {
	public void Start() {
		TemplateGameManager.Instance.actions.Cheats.ToggleDebugMode.performed += OnDebugModeToggle;
	}

	private void OnDestroy() {
		TemplateGameManager.Instance.actions.Cheats.ToggleDebugMode.performed -= OnDebugModeToggle;
	}

	void OnDebugModeToggle(InputAction.CallbackContext context) {
		if (context.performed)
			TemplateGameManager.Instance.IsDebugMode = !TemplateGameManager.Instance.IsDebugMode;
	}
}
