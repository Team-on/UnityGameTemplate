using System;
using UnityEngine;
using UnityEngine.InputSystem;

public static class InputEx {
	public static bool IsAnyKeyPressedThisFrame() {
		if(Mouse.current != null) {
			Vector3 mousePos = Mouse.current.position.ReadValue();
			Vector3 mouseViewPos = TemplateGameManager.Instance.Camera.ScreenToViewportPoint(mousePos);

			if (
				(	Mouse.current.leftButton.wasPressedThisFrame ||
					Mouse.current.rightButton.wasPressedThisFrame ||
					Mouse.current.middleButton.wasPressedThisFrame ||
					Mouse.current.forwardButton.wasPressedThisFrame ||
					Mouse.current.backButton.wasPressedThisFrame
				) && (0 <= mouseViewPos.x && mouseViewPos.x <= 1 && 0 <= mouseViewPos.y && mouseViewPos.y <= 1)
			)
				return true;
		}

		return (Keyboard.current?.anyKey?.wasPressedThisFrame ?? false) ||
			(Keyboard.current?.spaceKey?.wasPressedThisFrame ?? false) ||
			(Gamepad.current?.buttonEast?.wasPressedThisFrame ?? false) ||
			(Gamepad.current?.buttonNorth?.wasPressedThisFrame ?? false) ||
			(Gamepad.current?.buttonSouth?.wasPressedThisFrame ?? false) ||
			(Gamepad.current?.buttonWest?.wasPressedThisFrame ?? false) ||
			(Gamepad.current?.leftStickButton?.wasPressedThisFrame ?? false) ||
			(Gamepad.current?.rightStickButton?.wasPressedThisFrame ?? false) ||
			(Gamepad.current?.selectButton?.wasPressedThisFrame ?? false) ||
			(Gamepad.current?.startButton?.wasPressedThisFrame ?? false) ||
			(Gamepad.current?.dpad?.left?.wasPressedThisFrame ?? false) ||
			(Gamepad.current?.dpad?.up?.wasPressedThisFrame ?? false) ||
			(Gamepad.current?.dpad?.right?.wasPressedThisFrame ?? false) ||
			(Gamepad.current?.dpad?.down?.wasPressedThisFrame ?? false) ||
			(Gamepad.current?.leftShoulder?.wasPressedThisFrame ?? false) ||
			(Gamepad.current?.rightShoulder?.wasPressedThisFrame ?? false) ||
			(Gamepad.current?.leftTrigger?.wasPressedThisFrame ?? false) ||
			(Gamepad.current?.rightTrigger?.wasPressedThisFrame ?? false);
	}

	public static bool IsUseGamepad(InputDevice device) {
		return device is Gamepad;
	}
}
