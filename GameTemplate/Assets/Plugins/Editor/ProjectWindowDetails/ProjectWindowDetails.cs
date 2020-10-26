using System;
using System.Collections.Generic;
using System.Reflection;
using ProjectWindowDetail.Details;
using UnityEditor;
using UnityEngine;

namespace ProjectWindowDetail
{

	// Do not run this on the server because it could slow down tests and builds:
	#if !CONTINUOUS_INTEGRATION


	/// <summary>
	/// This class draws additional columns into the project window.
	/// </summary>
	[InitializeOnLoad]
	public static class ProjectWindowDetails
	{
		private static readonly List<ProjectWindowDetailBase> _details = new List<ProjectWindowDetailBase>();
		private static readonly ProjectWindowDetailsData detailsData;
		private static GUIStyle _rightAlignedStyle;

		private const int SpaceBetweenColumns = 10;
		private const int MenuIconWidth = 20;

		static ProjectWindowDetails()
		{
			EditorApplication.projectWindowItemOnGUI += DrawAssetDetails;

			detailsData = ProjectWindowDetailsData.LoadSettings();

			foreach (var type in GetAllDetailTypes()) {
				ProjectWindowDetailBase lastValue = (ProjectWindowDetailBase)Activator.CreateInstance(type);
				_details.Add(lastValue);
				lastValue.Visible = detailsData.GetVisible(lastValue.Name);
			}
		}

		public static IEnumerable<Type> GetAllDetailTypes()
		{
			// Get all classes that inherit from ProjectViewDetailBase:
			var types = Assembly.GetExecutingAssembly().GetTypes();
			foreach (var type in types)
			{
				if (type.BaseType == typeof(ProjectWindowDetailBase))
				{
					yield return type;
				}

			}
		}

		[MenuItem("Assets/Details...")]
		public static void Menu()
		{
			//Event.current.Use();
			ShowContextMenu();
		}

		private static void DrawAssetDetails(string guid, Rect rect)
		{
			if (Application.isPlaying)
			{
				return;
			}

			if (!IsMainListAsset(rect))
			{
				return;
			}

			if (Event.current.type == EventType.MouseDown &&
				Event.current.button == 0 &&
				Event.current.mousePosition.x > rect.xMax - MenuIconWidth)
			{
				Event.current.Use();
				ShowContextMenu();
			}

			if (Event.current.type != EventType.Repaint)
			{
				return;
			}

			var isSelected = Array.IndexOf(Selection.assetGUIDs, guid) >= 0;

			// Right align label and leave some space for the menu icon:
			rect.x += rect.width;
			rect.x -= MenuIconWidth * 0.9f;
			rect.width = MenuIconWidth;

			if (isSelected)
			{
				DrawMenuIcon(rect);
			}

			var assetPath = AssetDatabase.GUIDToAssetPath(guid);
			if (AssetDatabase.IsValidFolder(assetPath))
			{
				return;
			}

			var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
			if (asset == null)
			{
				// this entry could be Favourites or Packages. Ignore it.
				return;
			}

			for (var i = _details.Count - 1; i >= 0; i--)
			{
				var detail = _details[i];
				if (!detail.Visible)
				{
					continue;
				}

				string label = detail.GetLabel(guid, assetPath, asset);

				if(!string.IsNullOrEmpty(detail.GetLabel(guid, assetPath, asset)))
				{
					rect.width = detail.ColumnWidth;
					rect.x -= detail.ColumnWidth + SpaceBetweenColumns;
					GUI.Label(rect, new GUIContent(label, detail.Name),
						GetStyle(detail.Alignment));
				}
			}
		}

		private static void DrawMenuIcon(Rect rect)
		{
			var icon = EditorGUIUtility.IconContent("_Menu");
			EditorGUI.LabelField(rect, icon);
		}

		private static GUIStyle GetStyle(TextAlignment alignment)
		{
			return alignment == TextAlignment.Left ? EditorStyles.label : RightAlignedStyle;
		}

		private static GUIStyle RightAlignedStyle
		{
			get
			{
				if (_rightAlignedStyle == null)
				{
					_rightAlignedStyle = new GUIStyle(EditorStyles.label);
					_rightAlignedStyle.alignment = TextAnchor.MiddleRight;
				}

				return _rightAlignedStyle;
			}
		}

		private static void ShowContextMenu()
		{
			var menu = new GenericMenu();
			foreach (var detail in _details)
			{
				menu.AddItem(new GUIContent(detail.Name), detail.Visible, ToggleMenu, detail);
			}
			menu.AddSeparator("");
			menu.AddItem(new GUIContent("All"), false, ShowAllDetails);
			menu.AddItem(new GUIContent("None"), false, HideAllDetails);
			menu.DropDown(new Rect(Vector2.zero, Vector2.zero));
		}

		private static void HideAllDetails()
		{
			foreach (var detail in _details)
			{
				detail.Visible = false;
				detailsData.SetValueOrCreateNew(detail.Name, detail.Visible);
			}
			ProjectWindowDetailsData.SaveSettings(detailsData);
		}

		private static void ShowAllDetails()
		{
			foreach (var detail in _details)
			{
				detail.Visible = true;
				detailsData.SetValueOrCreateNew(detail.Name, detail.Visible);
			}
			ProjectWindowDetailsData.SaveSettings(detailsData);
		}

		public static void ToggleMenu(object data)
		{
			var detail = (ProjectWindowDetailBase) data;
			detail.Visible = !detail.Visible;
			detailsData.SetValueOrCreateNew(detail.Name, detail.Visible);
			ProjectWindowDetailsData.SaveSettings(detailsData);
		}

		private static bool IsMainListAsset(Rect rect)
		{
			// Don't draw details if project view shows large preview icons:
			if (rect.height > 20)
			{
				return false;
			}

			// Don't draw details if this asset is a sub asset:
			if (rect.x > 16)
			{
				return false;
			}

			return true;
		}
	}

	#endif
}
