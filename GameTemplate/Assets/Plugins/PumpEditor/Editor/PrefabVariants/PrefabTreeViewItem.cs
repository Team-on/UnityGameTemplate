#if UNITY_2018_3_OR_NEWER

using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace PumpEditor
{
    public class PrefabTreeViewItem : TreeViewItem
    {
        public Object PrefabAsset { get; set; }
    }
}

#endif
