using UnityEngine.InputSystem;

public static class InputEx {
	public static bool IsAnyKeyPressedThisFrame() {
		return (Mouse.current?.leftButton?.wasPressedThisFrame ?? false) ||
			(Mouse.current?.rightButton?.wasPressedThisFrame ?? false) ||
			(Mouse.current?.middleButton?.wasPressedThisFrame ?? false) ||
			(Mouse.current?.forwardButton?.wasPressedThisFrame ?? false) ||
			(Mouse.current?.backButton?.wasPressedThisFrame ?? false) ||
			(Keyboard.current?.anyKey?.wasPressedThisFrame ?? false) ||
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
