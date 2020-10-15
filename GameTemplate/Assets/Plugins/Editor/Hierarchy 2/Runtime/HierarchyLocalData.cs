using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hierarchy2
{
    [ExecuteInEditMode, AddComponentMenu("Hierarchy 2/Hierarchy Local Data", 100)]
    public class HierarchyLocalData : MonoBehaviour
    {
        public static Dictionary<Scene, HierarchyLocalData> instances = new Dictionary<Scene, HierarchyLocalData>();
        public Dictionary<GameObject, CustomRowItem> dCustomRowItems = new Dictionary<GameObject, CustomRowItem>();
        public List<CustomRowItem> lCustomRowItems = new List<CustomRowItem>();


        void OnEnable()
        {
            if (!instances.ContainsKey(gameObject.scene))
                instances.Add(gameObject.scene, this);

            if (!gameObject.CompareTag("EditorOnly"))
                gameObject.tag = "EditorOnly";
            
            gameObject.hideFlags = HideFlags.DontSaveInBuild;

            ClearNullRef();
            ConvertToDic();
        }

        void OnDestroy()
        {
            if (instances.ContainsKey(gameObject.scene))
                instances.Remove(gameObject.scene);
        }

        public static bool GetInstance(Scene scene, out HierarchyLocalData hierarchyLocalData)
        {
            return instances.TryGetValue(scene, out hierarchyLocalData);
        }

        public CustomRowItem CreateCustomRowItemFor(GameObject go)
        {
            CustomRowItem customRowItem = new CustomRowItem(go);
            lCustomRowItems.Add(customRowItem);

            ClearNullRef();
            ConvertToDic();

            return customRowItem;
        }

        public void RemoveCustomRowItemOf(GameObject go)
        {
            lCustomRowItems.RemoveAll(item => item.gameObject == go);
            dCustomRowItems.Remove(go);

            ClearNullRef();
            ConvertToDic();
        }

        public bool TryGetCustomRowData(GameObject go, out CustomRowItem customRowItem)
        {
            return dCustomRowItems.TryGetValue(go, out customRowItem);
        }

        void ConvertToDic() => dCustomRowItems = lCustomRowItems.ToDictionary(item => item.gameObject);

        public void ClearNullRef() => lCustomRowItems.RemoveAll(item => item.gameObject == null);
    }
}
