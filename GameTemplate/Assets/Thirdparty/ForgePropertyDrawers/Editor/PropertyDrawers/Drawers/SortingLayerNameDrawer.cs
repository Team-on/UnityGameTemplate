using System;
using UnityEditor;
using UnityEngine;

namespace UnityForge.PropertyDrawers.Editor
{
    [CustomPropertyDrawer(typeof(SortingLayerNameAttribute))]
    public class SortingLayerNameDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position = EditorGUI.PrefixLabel(position, label);

            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.LabelField(position, String.Format("Error: {0} attribute can be applied only to {1} type", typeof(SortingLayerNameAttribute), SerializedPropertyType.String));
                return;
            }

            DrawPropertyButton(position, property);
        }

        private static void DrawPropertyButton(Rect position, SerializedProperty property)
        {
            var propertyStringValue = property.hasMultipleDifferentValues ? "-" : property.stringValue;
            var content = String.IsNullOrEmpty(propertyStringValue) ? new GUIContent("<None>") : new GUIContent(propertyStringValue);
            if (GUI.Button(position, content, EditorStyles.popup))
            {
                SortingLayerSelector(property);
            }
        }

        private static void SortingLayerSelector(SerializedProperty property)
        {
            var menu = new GenericMenu();

            var layers = SortingLayer.layers;
            foreach (var layer in layers)
            {
                var name = layer.name;
                menu.AddItem(new GUIContent(name),
                    name == property.stringValue,
                    StringPropertyPair.HandlePairObjectSelect,
                    new StringPropertyPair(name, property));
            }

            menu.ShowAsContext();
        }
    }
}
