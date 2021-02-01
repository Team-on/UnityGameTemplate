using System;
using UnityEngine;
using UnityEngine.InputSystem;

public static class InputEx {
	public static bool IsAnyKeyPressedThisFrame() {
		if(Mouse.current != null) {
			Vector3 mousePos = Mouse.current.position.ReadValue();
			Vector3 mouseViewPos = TemplateGameManager.Instance.Camera.ScreenToViewportPoint(mousePos);

			if (
				(	Mouse.current.leftButton.wasReleasedThisFrame ||
					Mouse.current.rightButton.wasReleasedThisFrame ||
					Mouse.current.middleButton.wasReleasedThisFrame ||
					Mouse.current.forwardButton.wasReleasedThisFrame ||
					Mouse.current.backButton.wasReleasedThisFrame
				) && (0 <= mouseViewPos.x && mouseViewPos.x <= 1 && 0 <= mouseViewPos.y && mouseViewPos.y <= 1)
			)
				return true;
		}

		return (Keyboard.current?.anyKey?.wasReleasedThisFrame ?? false) ||
			(Keyboard.current?.spaceKey?.wasReleasedThisFrame ?? false) ||

			(Gamepad.current?.buttonEast?.wasReleasedThisFrame ?? false) ||
			(Gamepad.current?.buttonNorth?.wasReleasedThisFrame ?? false) ||
			(Gamepad.current?.buttonSouth?.wasReleasedThisFrame ?? false) ||
			(Gamepad.current?.buttonWest?.wasReleasedThisFrame ?? false) ||
			(Gamepad.current?.leftStickButton?.wasReleasedThisFrame ?? false) ||
			(Gamepad.current?.rightStickButton?.wasReleasedThisFrame ?? false) ||
			(Gamepad.current?.selectButton?.wasReleasedThisFrame ?? false) ||
			(Gamepad.current?.startButton?.wasReleasedThisFrame ?? false) ||
			(Gamepad.current?.dpad?.left?.wasReleasedThisFrame ?? false) ||
			(Gamepad.current?.dpad?.up?.wasReleasedThisFrame ?? false) ||
			(Gamepad.current?.dpad?.right?.wasReleasedThisFrame ?? false) ||
			(Gamepad.current?.dpad?.down?.wasReleasedThisFrame ?? false) ||
			(Gamepad.current?.leftShoulder?.wasReleasedThisFrame ?? false) ||
			(Gamepad.current?.rightShoulder?.wasReleasedThisFrame ?? false) ||
			(Gamepad.current?.leftTrigger?.wasReleasedThisFrame ?? false) ||
			(Gamepad.current?.rightTrigger?.wasReleasedThisFrame ?? false) ||

			(Touchscreen.current?.press?.wasReleasedThisFrame ?? false) 
			;
	}

	public static bool IsUseGamepad(InputDevice device) {
		return device is Gamepad;
	}
}
