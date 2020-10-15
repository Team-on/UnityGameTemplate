using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.UIElements;

namespace Hierarchy2
{
    public class ObjectCustomizationShelf : IHierarchyShelf
    {
        HierarchyCanvas canvas;

        public void Canvas(HierarchyCanvas canvas) => this.canvas = canvas;

        public VisualElement CreateShelfElement()
        {
            VisualElement shelfButton = new VisualElement();
            shelfButton.name = nameof(OpenSettings);
            shelfButton.tooltip = "";
            shelfButton.StyleHeight(23);
            shelfButton.StyleJustifyContent(Justify.Center);
            Color c = EditorGUIUtility.isProSkin ? new Color32(32, 32, 32, 255) : new Color32(128, 128, 128, 255);
            shelfButton.StyleBorderColor(c);
            shelfButton.StyleBorderWidth(0, 0, 1, 0);

            shelfButton.Add(new Label("Custom Selection"));

            shelfButton.RegisterCallback<MouseDownEvent>((evt) =>
            {
                var isPrefabMode = PrefabStageUtility.GetCurrentPrefabStage() != null ? true : false;
                if (isPrefabMode)
                {
                    Debug.LogWarning("Cannot custom object in prefab.");
                    evt.StopPropagation();
                    return;
                }

                if (Application.isPlaying)
                {
                    Debug.LogWarning("Cannot custom object in play mode.");
                    evt.StopPropagation();
                    return;
                }
                
                if (Selection.gameObjects.Length == 1 && Selection.activeGameObject != null)
                {
                    ObjectCustomizationPopup.ShowPopup(Selection.activeGameObject);
                }
                else
                {
                    if (Selection.gameObjects.Length > 1)
                    {
                        Debug.LogWarning("Only one object is allowed.");
                    }
                    else
                        Debug.LogWarning("No object has been selected.");
                }

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
            return 98;
        }
    }

    public class ObjectCustomizationPopup : EditorWindow
    {
        static EditorWindow window;
        GameObject gameObject;
        HierarchyLocalData hierarchyLocalData;

        public static ObjectCustomizationPopup ShowPopup(GameObject gameObject)
        {
            if (Selection.gameObjects.Length > 1 || Selection.activeGameObject == null)
                return null;

            if (window == null)
                window = ObjectCustomizationPopup.GetWindow<ObjectCustomizationPopup>(gameObject.name);
            else
            {
                window.Close();
                window = ObjectCustomizationPopup.GetWindow<ObjectCustomizationPopup>(gameObject.name);
            }

            Vector2 v2 = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            Vector2 size = new Vector2(256, 138);
            window.position = new Rect(v2.x, v2.y, size.x, size.y);
            window.maxSize = window.minSize = size;
            window.ShowPopup();
            window.Focus();

            ObjectCustomizationPopup objectCustomizationPopup = window as ObjectCustomizationPopup;
            return objectCustomizationPopup;
        }

        void OnEnable()
        {
            rootVisualElement.StyleMargin(4, 4, 2, 0);

            hierarchyLocalData = HierarchyEditor.Instance.GetHierarchyLocalData(Selection.activeGameObject.scene);
            gameObject = Selection.activeGameObject;
            Selection.activeGameObject = null;

            CustomRowItem customRowItem = null;
            if (hierarchyLocalData.TryGetCustomRowData(gameObject, out customRowItem) == false)
            {
                customRowItem = hierarchyLocalData.CreateCustomRowItemFor(gameObject);
            }


            IMGUIContainer iMGUIContainer = new IMGUIContainer(() =>
            {
                customRowItem.useBackground = EditorGUILayout.Toggle("Background", customRowItem.useBackground);

                customRowItem.backgroundStyle = (CustomRowItem.BackgroundStyle)EditorGUILayout.EnumPopup("Background Style", customRowItem.backgroundStyle);
                customRowItem.backgroundMode = (CustomRowItem.BackgroundMode)EditorGUILayout.EnumPopup("Background Mode", customRowItem.backgroundMode);
                customRowItem.backgroundColor = EditorGUILayout.ColorField("Background Color", customRowItem.backgroundColor);
                
                customRowItem.overrideLabel = EditorGUILayout.Toggle("Override Label", customRowItem.overrideLabel);

                var wideMode = EditorGUIUtility.wideMode;
                EditorGUIUtility.wideMode = true;
                customRowItem.labelOffset = EditorGUILayout.Vector2Field("Label Offset", customRowItem.labelOffset);
                EditorGUIUtility.wideMode = wideMode;
                customRowItem.labelColor = EditorGUILayout.ColorField("Label Color", customRowItem.labelColor);

                if (GUI.changed)
                    EditorApplication.RepaintHierarchyWindow();
            });
            rootVisualElement.Add(iMGUIContainer);
        }
    }
}