using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class ScreenShooterConfigList {
	public static UnityEditorInternal.ReorderableList Create(List<ScreenshotData> configsList, GenericMenu.MenuFunction2 menuItemHandler) {
		var reorderableList = new UnityEditorInternal.ReorderableList(configsList, typeof(ScreenshotData), true, false, true, true);

		reorderableList.elementHeight = EditorGUIUtility.singleLineHeight + 4;
		reorderableList.drawElementCallback = (position, index, isActive, isFocused) => {
			const float enabledWidth = 15f;
			const float cameraWidth = 100f;
			const float textWidth = 10f;
			const float sizeWidth = 50f;
			const float multWidth = 30f;
			const float uiWidth = 15f;
			const float space = 10f;
			const float minNameWidth = 100f;

			const float singleWidth = 10;


			var config = configsList[index];
			var nameWidth = position.width - space * 6 - enabledWidth - cameraWidth - textWidth - sizeWidth * 2 - multWidth - uiWidth - singleWidth * 21;
			if (nameWidth < minNameWidth)
				nameWidth = minNameWidth;

			position.y += 2;
			position.height -= 4;

			position.x += space;
			position.width = enabledWidth;
			config.isEnabled = EditorGUI.Toggle(position, config.isEnabled);

			EditorGUI.BeginDisabledGroup(!config.isEnabled);
			position.x += position.width + space;
			position.width = cameraWidth;
			config.targetCamera = (ScreenshooterTargetCamera)EditorGUI.EnumPopup(position, config.targetCamera);

			position.x += position.width + space;
			position.width = nameWidth;
			config.name = EditorGUI.TextField(position, config.name);

			position.x += position.width + space;
			position.width = singleWidth * 4;
			EditorGUI.LabelField(position, "Size");

			position.x += position.width;
			position.width = sizeWidth;
			config.resolution.x = EditorGUI.IntField(position, (int)config.resolution.x);

			position.x += position.width;
			position.width = textWidth;
			EditorGUI.LabelField(position, "x");

			position.x += position.width;
			position.width = sizeWidth;
			config.resolution.y = EditorGUI.IntField(position, (int)config.resolution.y);

			position.x += position.width + space;
			position.width = singleWidth * 9;
			EditorGUI.LabelField(position, "Size Multiplier");

			position.x += position.width;
			position.width = multWidth;
			config.resolutionMultiplier = EditorGUI.FloatField(position, config.resolutionMultiplier);

			position.x += position.width + space;
			position.width = singleWidth * 2;
			EditorGUI.LabelField(position, "UI");

			position.x += position.width;
			position.width = uiWidth;
			if (config.targetCamera == ScreenshooterTargetCamera.GameView) {
				config.captureOverlayUI = EditorGUI.Toggle(position, config.captureOverlayUI);
			}
			else {
				EditorGUI.BeginDisabledGroup(true);
				config.captureOverlayUI = EditorGUI.Toggle(position, false);
				EditorGUI.EndDisabledGroup();
			}
			EditorGUI.EndDisabledGroup();
		};

		reorderableList.onAddDropdownCallback = (buttonRect, list) => {
			var menu = new GenericMenu();

			menu.AddItem(new GUIContent("Custom"), false, menuItemHandler, new ScreenshotData("Custom", 1920, 1080));
			menu.AddSeparator("");

			foreach (var config in PredefinedConfigs.Android) {
				if (config is SeparatorScreenshotData) {
					menu.AddSeparator("Android/");
				}
				else {
					var label = "Android/" + config.name + " (" + config.resolution.x + "x" + config.resolution.y + ") (Portrait)";
					menu.AddItem(new GUIContent(label), false, menuItemHandler, config);
				}
			}
			menu.AddSeparator("Android/");
			menu.AddSeparator("Android/");
			foreach (var config in PredefinedConfigs.Android) {
				if (config is SeparatorScreenshotData) {
					menu.AddSeparator("Android/");
				}
				else {
					ScreenshotData altData = config;
					altData.resolution = new Vector2(config.resolution.y, config.resolution.x);
					var label = "Android/" + altData.name + " (" + altData.resolution.x + "x" + altData.resolution.y + ") (Landscape)";
					menu.AddItem(new GUIContent(label), false, menuItemHandler, altData);
				}
			}

			foreach (var config in PredefinedConfigs.iOS) {
				if (config is SeparatorScreenshotData) {
					menu.AddSeparator("iOS/");
				}
				else {
					var label = "iOS/" + config.name + " (" + config.resolution.x + "x" + config.resolution.y + ") (Portrait)";
					menu.AddItem(new GUIContent(label), false, menuItemHandler, config);
				}
			}
			menu.AddSeparator("iOS/");
			menu.AddSeparator("iOS/");
			foreach (var config in PredefinedConfigs.iOS) {
				if (config is SeparatorScreenshotData) {
					menu.AddSeparator("iOS/");
				}
				else {
					ScreenshotData altData = config;
					altData.resolution = new Vector2(config.resolution.y, config.resolution.x);
					var label = "iOS/" + altData.name + " (" + altData.resolution.x + "x" + altData.resolution.y + ") (Landscape)";
					menu.AddItem(new GUIContent(label), false, menuItemHandler, altData);
				}
			}

			foreach (var config in PredefinedConfigs.Standalone) {
				if (config is SeparatorScreenshotData) {
					menu.AddSeparator("Standalone/");
				}
				else {
					var label = "Standalone/" + config.name + " (" + config.resolution.x + "x" + config.resolution.y + ")";
					menu.AddItem(new GUIContent(label), false, menuItemHandler, config);
				}
			}

			menu.ShowAsContext();
		};

		return reorderableList;
	}
}

