using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace Hierarchy2
{
    public class HierarchyCanvas : VisualElement
    {
        Dictionary<string, object> customHierarchys = new Dictionary<string, object>();
        List<VisualElement> elementCanvas = new List<VisualElement>();

        VisualElement shelfHoverElement;
        VerticalLayout shelfContentElement;

        static List<Type> customHierarchyTypes = new List<Type>();

        static HierarchyCanvas()
        {
            var itypes = typeof(HierarchyEditor).Assembly.GetTypes().ToList();
            itypes.RemoveAll(type =>
                (!typeof(IHierarchyShelf).IsAssignableFrom(type) &&
                 !typeof(IHierarchyElement).IsAssignableFrom(type)) || type.IsInterface || type.IsAbstract);
            foreach (var type in itypes)
                customHierarchyTypes.Add(type);
        }

        public HierarchyCanvas()
        {
            pickingMode = PickingMode.Ignore;
            this.StretchToParentSize();

            shelfHoverElement = new VisualElement();
            shelfHoverElement.name = nameof(shelfHoverElement).ToUpper();
            shelfHoverElement.StyleBottom(1);
            shelfHoverElement.StyleSize(36, 20);

            Add(shelfHoverElement);

            shelfContentElement = new VerticalLayout();
            shelfContentElement.name = nameof(shelfContentElement).ToUpper();
            Color backgroundColor = EditorGUIUtility.isProSkin
                ? new Color32(60, 60, 60, 255)
                : new Color32(203, 203, 203, 255);
            shelfContentElement.StyleBackgroundColor(backgroundColor);
            shelfContentElement.StylePosition(Position.Absolute);
            shelfContentElement.StyleMargin(0, 0, 19, 0);
            shelfContentElement.StylePadding(0, 0, 0, 0);
            shelfContentElement.StyleMinWidth(HierarchyEditor.GLOBAL_SPACE_OFFSET_LEFT);
            shelfContentElement.StyleBorderWidth(0, 1, 0, 1);
            shelfContentElement.StyleBorderRadius(0, 0, 0, 0);
            Color borderColor = new Color32(40, 40, 40, 255);
            shelfContentElement.StyleBorderColor(borderColor);
            shelfContentElement.StyleDisplay(false);
            shelfHoverElement.RegisterCallback<MouseEnterEvent>((evt) =>
            {
                shelfContentElement.StyleDisplay(true);
                evt.StopPropagation();
            });
            shelfContentElement.RegisterCallback<MouseLeaveEvent>((evt) =>
            {
                shelfContentElement.StyleDisplay(false);
                evt.StopPropagation();
            });
            RegisterCallback<MouseEnterEvent>((evt) =>
            {
                shelfContentElement.StyleDisplay(false);
                evt.StopPropagation();
            });
            RegisterCallback<MouseLeaveEvent>((evt) =>
            {
                shelfContentElement.StyleDisplay(false);
                evt.StopPropagation();
            });
            Add(shelfContentElement);

            VisualElement mask = new VisualElement();
            mask.name = nameof(mask);
            mask.StretchToParentSize();
            mask.StyleBackgroundColor(Color.black);
            mask.StylePosition(Position.Absolute);
            mask.style.opacity = .4f;
            mask.focusable = false;
            mask.pickingMode = PickingMode.Ignore;
            mask.StyleDisplay(false);
            shelfContentElement.RegisterCallback<GeometryChangedEvent>((evt) =>
            {
                mask.StyleDisplay(shelfContentElement.style.display.value);
            });
            Add(mask);

            List<IHierarchyShelf> listShelf = new List<IHierarchyShelf>();
            List<IHierarchyElement> listCanvasElement = new List<IHierarchyElement>();

            foreach (var customHierarchyType in customHierarchyTypes)
            {
                if (typeof(IHierarchyShelf).IsAssignableFrom(customHierarchyType))
                {
                    var instance = Activator.CreateInstance(customHierarchyType) as IHierarchyShelf;
                    instance.Canvas(this);
                    listShelf.Add(instance);
                }

                if (typeof(IHierarchyElement).IsAssignableFrom(customHierarchyType))
                {
                    var instance = Activator.CreateInstance(customHierarchyType) as IHierarchyElement;
                    instance.Canvas(this);
                    listCanvasElement.Add(instance);
                }
            }

            listShelf = listShelf.OrderBy(item => item.ShelfPriority()).ToList();

            foreach (var shelf in listShelf)
            {
                var visualElement = shelf.CreateShelfElement();
                if (visualElement != null)
                {
                    shelfContentElement.Add(visualElement);
                }

                customHierarchys.Add(shelf.GetType().Name, shelf);
            }

            // if (listShelf.Count > 0)
            // {
            //     VisualElement separator = new VisualElement();
            //     separator.StyleHeight(1);
            //     Color c = EditorGUIUtility.isProSkin ? new Color32(32, 32, 32, 255) : new Color32(128, 128, 128, 255);
            //     separator.StyleBackgroundColor(c);
            //     shelfContentElement.Add(separator);
            // }

            foreach (var canvasElement in listCanvasElement)
            {
                var visualElement = canvasElement.CreateCanvasElement();
                if (visualElement != null)
                {
                    this.Add(visualElement);
                    elementCanvas.Add(visualElement);
                }

                customHierarchys.Add(canvasElement.GetType().Name, canvasElement);
            }

            mask.SendToBack();
        }

        public T GetElement<T>(string name)
        {
            object result = null;
            customHierarchys.TryGetValue(name, out result);
            return (T) result;
        }

        public void CloseAllElementCanvas()
        {
            foreach (var element in elementCanvas)
            {
                element.StyleDisplay(false);
            }
        }
    }
}