#if UNITY_2017_3_OR_NEWER

using UnityEditor;

namespace PumpEditor
{
    public static class ForceReserializeAssetsMenuItems
    {
        private const string SelectedItemName = "Assets/Pump Editor/Force Reserialize";
        private const string AllItemName = "Assets/Pump Editor/Force Reserialize All";

        [MenuItem(SelectedItemName)]
        private static void ForceReserializeSelected()
        {
            ForceReserializeAssetsUtils.ForceReserializeSelectedAssets();
        }

        [MenuItem(SelectedItemName, true)]
        private static bool ForceReserializeSelectedValidateFunction()
        {
            var assetGUIDs = Selection.assetGUIDs;
            return assetGUIDs.Length != 0;
        }

        [MenuItem(AllItemName)]
        private static void ForceReserializeAll()
        {
            ForceReserializeAssetsUtils.ForceReserializeAllAssets();
        }
    }
}

#endif
