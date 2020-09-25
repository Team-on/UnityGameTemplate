using UnityEngine.UIElements;

namespace Hierarchy2
{
    internal interface IHierarchyShelf
    {
        void Canvas(HierarchyCanvas canvas);
        int ShelfPriority();
        VisualElement CreateShelfElement();
    }
}