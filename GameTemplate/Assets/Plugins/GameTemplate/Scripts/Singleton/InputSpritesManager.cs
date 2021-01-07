using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.LowLevel;
using yaSingleton;
using Cinemachine;
using NaughtyAttributes;
using TMPro;

[CreateAssetMenu(fileName = "InputSpritesManager", menuName = "Singletons/InputSpritesManager")]
public class InputSpritesManager : Singleton<InputSpritesManager> {
	public KeyboardIcons keyboard;
	public MouseIcons mouse;

	public GamepadIcons xbox360;
	public GamepadIcons xboxOne;
	public GamepadIcons xboxSeriesX;

	public GamepadIcons ps3;
	public GamepadIcons ps4;
	public GamepadIcons ps5;

	public GamepadIcons amazonLuna;
	public GamepadIcons stadia;
	public GamepadIcons steam;
	public GamepadIcons quya;
	public GamepadIcons switchGamepad;

	public Sprite controllerDisconnected;

	public Sprite GetSprite(GamepadButton gamepadButtom) {
		switch (gamepadButtom) {
			case GamepadButton.DpadUp:
				break;
			case GamepadButton.DpadDown:
				break;
			case GamepadButton.DpadLeft:
				break;
			case GamepadButton.DpadRight:
				break;

			//case GamepadButton.Y:
			//case GamepadButton.Triangle:
			case GamepadButton.North:

				break;

			//case GamepadButton.B:
			//case GamepadButton.Circle:
			case GamepadButton.East:

				break;

			//case GamepadButton.A:
			//case GamepadButton.Cross:
			case GamepadButton.South:

				break;

			//case GamepadButton.X:
			//case GamepadButton.Square:
			case GamepadButton.West:

				break;

			case GamepadButton.LeftStick:
				break;
			case GamepadButton.RightStick:
				break;

			case GamepadButton.LeftShoulder:
				break;
			case GamepadButton.RightShoulder:
				break;

			case GamepadButton.Start:
				break;
			case GamepadButton.Select:
				break;

			case GamepadButton.LeftTrigger:
				break;
			case GamepadButton.RightTrigger:
				break;
		}

		return null;
	}

	public Sprite GetSprite(MouseButton mouseButton) {
		switch (mouseButton) {
			case MouseButton.Left:
				break;
			case MouseButton.Right:
				break;
			case MouseButton.Middle:
				break;
			case MouseButton.Forward:
				break;
			case MouseButton.Back:
				break;
		}

		return null;
	}

	public Sprite GetSprite(Key key) {

		return null;
	}

	public void OnUpdateBindingDisplay(RebindActionUI component, string bindingDisplayString, string deviceLayoutName, string controlPath) {
		if (string.IsNullOrEmpty(deviceLayoutName) || string.IsNullOrEmpty(controlPath))
			return;

		var icon = default(Sprite);
		if (InputSystem.IsFirstLayoutBasedOnSecond(deviceLayoutName, "DualShockGamepad"))
			icon = ps4.GetSprite(controlPath);
		else if (InputSystem.IsFirstLayoutBasedOnSecond(deviceLayoutName, "Gamepad"))
			icon = xboxOne.GetSprite(controlPath);

		var textComponent = component.bindingText;

		// Grab Image component.
		var imageGO = textComponent.transform.parent.Find("ActionBindingIcon");
		var imageComponent = imageGO.GetComponent<Image>();

		if (icon != null) {
			textComponent.gameObject.SetActive(false);
			imageComponent.sprite = icon;
			imageComponent.gameObject.SetActive(true);
		}
		else {
			textComponent.gameObject.SetActive(true);
			imageComponent.gameObject.SetActive(false);
		}
	}

	[Serializable]
	public struct GamepadIcons {
		[InspectorName("Bottom A")] public Sprite buttonSouth;
		[InspectorName("Top Y")] public Sprite buttonNorth;
		[InspectorName("Left B")] public Sprite buttonEast;
		[InspectorName("Right X")] public Sprite buttonWest;
		[Space]
		[Space]
		public Sprite selectButton;
		public Sprite startButton;
		[Space]
		[Space]
		public Sprite leftShoulder;
		public Sprite leftTrigger;
		[Space]
		public Sprite rightShoulder;
		public Sprite rightTrigger;
		[Space]
		[Space]
		public Sprite dpad;
		[Space]
		public Sprite dpadUp;
		public Sprite dpadDown;
		public Sprite dpadLeft;
		public Sprite dpadRight;
		[Space]
		[Space]
		public Sprite leftStick;
		public Sprite leftStickPress;
		[Space]
		public Sprite rightStick;
		public Sprite rightStickPress;

		public Sprite GetSprite(string controlPath) {
			switch (controlPath) {
				case "buttonSouth": return buttonSouth;
				case "buttonNorth": return buttonNorth;
				case "buttonEast": return buttonEast;
				case "buttonWest": return buttonWest;
				case "start": return startButton;
				case "select": return selectButton;
				case "leftTrigger": return leftTrigger;
				case "rightTrigger": return rightTrigger;
				case "leftShoulder": return leftShoulder;
				case "rightShoulder": return rightShoulder;
				case "dpad": return dpad;
				case "dpad/up": return dpadUp;
				case "dpad/down": return dpadDown;
				case "dpad/left": return dpadLeft;
				case "dpad/right": return dpadRight;
				case "leftStick": return leftStick;
				case "rightStick": return rightStick;
				case "leftStickPress": return leftStickPress;
				case "rightStickPress": return rightStickPress;
			}
			return null;
		}
	}

	[Serializable]
	public struct KeyboardIcons {
		public Sprite[] keys;
		[Space]
		public Sprite windowsKey;
		public Sprite macKey;
		[Space]
		public Sprite blank;
	}

	[Serializable]
	public struct MouseIcons {
		public Sprite leftButton;
		public Sprite rightButton;

		public Sprite middleButton;

		public Sprite forwardButton;
		public Sprite backButton;

	}
}
