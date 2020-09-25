using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace Hierarchy2
{
    public class OpenSettings : IHierarchyShelf
    {
        HierarchyCanvas canvas;

        public void Canvas(HierarchyCanvas canvas) => this.canvas = canvas;

        public VisualElement CreateShelfElement()
        {
            VisualElement shelfButton = new VisualElement();
            shelfButton.name = nameof(OpenSettings);
            shelfButton.tooltip = "Open hierarchy settings window";
            shelfButton.StyleHeight(23);
            shelfButton.StyleJustifyContent(Justify.Center);
            Color c = EditorGUIUtility.isProSkin ? new Color32(32, 32, 32, 255) : new Color32(128, 128, 128, 255);
            shelfButton.StyleBorderColor(c);
            shelfButton.StyleBorderWidth(0, 0, 1, 0);

            // Image image = new Image();
            // image.StyleSize(HierarchyEditor.GLOBAL_SPACE_OFFSET_LEFT, 16);
            // image.scaleMode = ScaleMode.ScaleToFit;
            // string path = string.Format("Assets/Duy Assets/Hierarchy 2/Editor/Icons/{0}", EditorGUIUtility.isProSkin ? "d_SettingsIcon.png" : "SettingsIcon.png");
            // image.image = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;
            // shelfButton.Add(image);

            shelfButton.Add(new Label("Settings"));

            shelfButton.RegisterCallback<MouseDownEvent>((evt) =>
            {
                SettingsService.OpenProjectSettings("Project/Hierarchy");
                evt.StopPropagation();
            });

            shelfButton.RegisterCallback<MouseEnterEvent>((evt) =>
            {
                shelfButton.StyleBackgroundColor(new Color(.5f, .5f, .5f, .5f));
            });

            shelfButton.RegisterCallback<MouseLeaveEvent>((evt) => { shelfButton.StyleBackgroundColor(Color.clear); });

            return shelfButton;
        }

        public int ShelfPriority()
        {
            return 99;
        }
    }
}