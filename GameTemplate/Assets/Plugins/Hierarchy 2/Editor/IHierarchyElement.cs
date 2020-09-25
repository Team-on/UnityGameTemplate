using UnityEngine.UIElements;

namespace Hierarchy2
{
    internal interface IHierarchyElement
    {
        void Canvas(HierarchyCanvas canvas);
        VisualElement CreateCanvasElement();
    }
}


