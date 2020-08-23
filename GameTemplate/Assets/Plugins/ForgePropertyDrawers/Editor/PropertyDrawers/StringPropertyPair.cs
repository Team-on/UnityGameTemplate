using UnityEditor;

namespace UnityForge.PropertyDrawers.Editor
{
    public struct StringPropertyPair
    {
        public string str;
        public SerializedProperty property;

        public StringPropertyPair(string str, SerializedProperty property)
        {
            this.str = str;
            this.property = property;
        }

        public static void HandlePairObjectSelect(object selectedObject)
        {
            var clickedItem = (StringPropertyPair)selectedObject;
            clickedItem.property.stringValue = clickedItem.str;
            clickedItem.property.serializedObject.ApplyModifiedProperties();
        }
    }
}
