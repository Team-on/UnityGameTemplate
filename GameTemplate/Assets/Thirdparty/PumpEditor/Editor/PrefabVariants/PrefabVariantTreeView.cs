#if UNITY_2018_3_OR_NEWER

using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace PumpEditor
{
    public class PrefabVariantTreeView : TreeView
    {
        private const float SpaceBeforeIconAndLabel = 18.0f;
        private const float AssetIconWidth = 16.0f;

        private static Texture2D PrefabIcon = EditorGUIUtility.FindTexture("prefab icon");
        private static Texture2D PrefabVariantIcon = EditorGUIUtility.FindTexture("prefabvariant icon");

        public PrefabVariantTreeView(TreeViewState treeViewState)
        : base(treeViewState)
        {
            extraSpaceBeforeIconAndLabel = SpaceBeforeIconAndLabel;
            Reload();
        }

        public PrefabTreeViewItem FindItem(int id)
        {
            return (PrefabTreeViewItem)FindItem(id, rootItem);
        }

        protected override TreeViewItem BuildRoot()
        {
            // Do not Object.GetInstanceID for item id since
            // tree view proceeds item selection based on item id.
            // Since prefab can be used more than once as base for
            // prefab variant, such items would all be selected if
            // instance id is used. So generate item id manually
            // though this is dangerous in terms of tree state
            // serialization.
            var itemId = 0;

            var root = new PrefabTreeViewItem
            {
                id = itemId,
                depth = -1
            };

            var inheritanceChains = PrefabInheritanceHelper.GetInheritanceChains();
            foreach (var inheritanceChain in inheritanceChains)
            {
                if (inheritanceChain.Count == 1)
                {
                    continue;
                }

                var prefabAsset = inheritanceChain[0];
                var prefabItem = new PrefabTreeViewItem
                {
                    id = ++itemId,
                    displayName = prefabAsset.name,
                    PrefabAsset = prefabAsset,
                };
                root.AddChild(prefabItem);

                var i = 1;
                while (i < inheritanceChain.Count)
                {
                    var nestedPrefabAsset = inheritanceChain[i];
                    var nestedPrefabItem = new PrefabTreeViewItem
                    {
                        id = ++itemId,
                        displayName = nestedPrefabAsset.name,
                        PrefabAsset = nestedPrefabAsset,
                    };

                    prefabItem.AddChild(nestedPrefabItem);
                    prefabItem = nestedPrefabItem;

                    ++i;
                }
            }

            SetupDepthsFromParentsAndChildren(root);

            return root;
        }

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return false;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var assetIconRect = args.rowRect;
            assetIconRect.x += GetContentIndent(args.item);
            assetIconRect.width = AssetIconWidth;

            var prefabItem = (PrefabTreeViewItem)args.item;
            var asset = prefabItem.PrefabAsset;
            var isVariant = PrefabUtility.GetPrefabAssetType(asset) == PrefabAssetType.Variant;

            var assetIconTexture = isVariant ? PrefabVariantIcon : PrefabIcon;
            GUI.DrawTexture(assetIconRect, assetIconTexture);

            base.RowGUI(args);
        }
    }
}

#endif
