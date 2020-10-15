using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Hierarchy2
{
    public class GroupSelection : IHierarchyShelf
    {
        HierarchyCanvas canvas;

        Color backgroundColor =
            EditorGUIUtility.isProSkin ? new Color32(60, 60, 60, 255) : new Color32(203, 203, 203, 255);

        Color borderColor = new Color32(40, 40, 40, 255);

        bool IsSelectionsSameScene()
        {
            var scene = Selection.activeGameObject.scene;
            foreach (var gameObject in Selection.gameObjects)
            {
                if (scene != gameObject.scene)
                    return false;
            }

            return true;
        }

        bool IsSelectionsSameParent()
        {
            var parent = Selection.activeGameObject.transform.parent;
            foreach (var gameObject in Selection.gameObjects)
            {
                if (parent != gameObject.transform.parent)
                    return false;
            }

            return true;
        }

        public int ShelfPriority()
        {
            return 97;
        }

        public VisualElement CreateShelfElement()
        {
            VisualElement shelfButton = new VisualElement();
            shelfButton.name = nameof(GroupSelection);
            shelfButton.StyleHeight(23);
            shelfButton.StyleJustifyContent(Justify.Center);
            Color c = EditorGUIUtility.isProSkin ? new Color32(32, 32, 32, 255) : new Color32(128, 128, 128, 255);
            shelfButton.StyleBorderColor(c);
            shelfButton.StyleBorderWidth(0, 0, 1, 0);

            // Image image = new Image();
            // image.StyleSize(HierarchyEditor.GLOBAL_SPACE_OFFSET_LEFT, 16);
            // image.scaleMode = ScaleMode.ScaleToFit;
            // string path = string.Format("Assets/Duy Assets/Hierarchy 2/Editor/Icons/{0}", EditorGUIUtility.isProSkin ? "d_GroupSelectionIcon.png" : "GroupSelectionIcon.png");
            // image.image = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;
            // shelfButton.Add(image);

            shelfButton.Add(new Label("Group Selections"));

            VerticalLayout menuOptions = new VerticalLayout();
            menuOptions.StylePosition(Position.Absolute);
            menuOptions.StyleTop(-1);
            menuOptions.StyleWidth(128);
            menuOptions.StyleBackgroundColor(backgroundColor);
            menuOptions.StyleMarginLeft(-1);
            menuOptions.StyleBorderWidth(1, 1, 1, 1);
            menuOptions.StyleBorderRadius(0, 0, 0, 0);
            menuOptions.StyleBorderColor(borderColor);
            menuOptions.StyleDisplay(false);
            shelfButton.Add(menuOptions);

            Label option1 = new Label("Same parent (Offset)");
            option1.StyleHeight(21);
            option1.StylePadding(4, 4, 0, 0);
            option1.StyleTextAlign(TextAnchor.MiddleLeft);
            option1.RegisterCallback<MouseEnterEvent>((evt) =>
            {
                option1.StyleBackgroundColor(new Color(.5f, .5f, .5f, .5f));
            });
            option1.RegisterCallback<MouseLeaveEvent>((evt) => { option1.StyleBackgroundColor(Color.clear); });
            option1.RegisterCallback<MouseDownEvent>((evt) =>
            {
                if (Selection.gameObjects.Length > 0)
                {
                    if (IsSelectionsSameScene())
                    {
                        if (IsSelectionsSameParent())
                        {
                            var scene = Selection.activeGameObject.scene;
                            GameObject group = new GameObject();
                            group.name = "New Group";
                            Undo.RegisterCreatedObjectUndo(group, group.name);
                            EditorSceneManager.MoveGameObjectToScene(group, scene);

                            Vector3 offset = Vector3.zero;
                            foreach (var go in Selection.gameObjects)
                                offset += go.transform.position;

                            group.transform.position = offset;
                            group.transform.rotation = Quaternion.identity;
                            group.transform.localScale = Vector3.one;

                            group.transform.SetParent(Selection.activeTransform.parent);

                            foreach (var go in Selection.gameObjects)
                                Undo.SetTransformParent(go.transform, group.transform, option1.text);

                            Selection.activeGameObject = group;
                            EditorSceneManager.MarkSceneDirty(scene);
                        }
                        else
                        {
                            Debug.LogWarning("Can't group selections different parent, use global instead.");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Can't group selections from multiple scene.");
                    }
                }

                shelfButton.parent.StyleDisplay(false);
                evt.StopPropagation();
            });
            menuOptions.Add(option1);

            Label option2 = new Label("Global (Offset)");
            option2.StyleHeight(21);
            option2.StylePadding(4, 4, 0, 0);
            option2.StyleTextAlign(TextAnchor.MiddleLeft);
            option2.RegisterCallback<MouseEnterEvent>((evt) =>
            {
                option2.StyleBackgroundColor(new Color(.5f, .5f, .5f, .5f));
            });
            option2.RegisterCallback<MouseLeaveEvent>((evt) => { option2.StyleBackgroundColor(Color.clear); });
            option2.RegisterCallback<MouseDownEvent>((evt) =>
            {
                if (Selection.gameObjects.Length > 0)
                {
                    if (IsSelectionsSameScene())
                    {
                        var scene = Selection.activeGameObject.scene;
                        GameObject group = new GameObject();
                        group.name = "New Group";
                        Undo.RegisterCreatedObjectUndo(group, group.name);
                        EditorSceneManager.MoveGameObjectToScene(group, scene);

                        Vector3 offset = Vector3.zero;
                        foreach (var go in Selection.gameObjects)
                            offset += go.transform.position;

                        group.transform.position = offset;
                        group.transform.rotation = Quaternion.identity;
                        group.transform.localScale = Vector3.one;

                        foreach (var go in Selection.gameObjects)
                            Undo.SetTransformParent(go.transform, group.transform, option2.text);

                        Selection.activeGameObject = group;
                        EditorSceneManager.MarkSceneDirty(scene);
                    }
                    else
                    {
                        Debug.LogWarning("Can't group selections from multiple scene.");
                    }
                }

                shelfButton.parent.StyleDisplay(false);
                evt.StopPropagation();
            });
            menuOptions.Add(option2);


            Label option3 = new Label("Global (Zero)");
            option3.StyleHeight(21);
            option3.StylePadding(4, 4, 0, 0);
            option3.StyleTextAlign(TextAnchor.MiddleLeft);
            option3.RegisterCallback<MouseEnterEvent>((evt) =>
            {
                option3.StyleBackgroundColor(new Color(.5f, .5f, .5f, .5f));
            });
            option3.RegisterCallback<MouseLeaveEvent>((evt) => { option3.StyleBackgroundColor(Color.clear); });
            option3.RegisterCallback<MouseDownEvent>((evt) =>
            {
                if (Selection.gameObjects.Length > 0)
                {
                    if (IsSelectionsSameScene())
                    {
                        var scene = Selection.activeGameObject.scene;
                        GameObject group = new GameObject();
                        group.name = "New Group";
                        Undo.RegisterCreatedObjectUndo(group, group.name);
                        EditorSceneManager.MoveGameObjectToScene(group, scene);

                        group.transform.position = Vector3.zero;
                        group.transform.rotation = Quaternion.identity;
                        group.transform.localScale = Vector3.one;

                        foreach (var go in Selection.gameObjects)
                            Undo.SetTransformParent(go.transform, group.transform, option3.text);

                        Selection.activeGameObject = group;
                        EditorSceneManager.MarkSceneDirty(scene);
                    }
                    else
                    {
                        Debug.LogWarning("Can't group selections from multiple scene.");
                    }
                }

                shelfButton.parent.StyleDisplay(false);
                evt.StopPropagation();
            });
            menuOptions.Add(option3);

            shelfButton.RegisterCallback<MouseDownEvent>((evt) => { evt.StopPropagation(); });

            shelfButton.RegisterCallback<MouseEnterEvent>((evt) =>
            {
                shelfButton.StyleBackgroundColor(new Color(.5f, .5f, .5f, .5f));
                menuOptions.StyleLeft(shelfButton.layout.width + 1);
                menuOptions.StyleDisplay(true);
            });

            shelfButton.RegisterCallback<MouseLeaveEvent>((evt) =>
            {
                shelfButton.StyleBackgroundColor(Color.clear);
                menuOptions.StyleDisplay(false);
            });

            return shelfButton;
        }

        public void Canvas(HierarchyCanvas canvas) => this.canvas = canvas;
    }
}