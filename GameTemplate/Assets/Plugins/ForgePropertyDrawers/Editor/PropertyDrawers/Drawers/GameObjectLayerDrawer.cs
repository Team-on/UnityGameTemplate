using System;
using UnityEditor;
using UnityEngine;

namespace UnityForge.PropertyDrawers.Editor
{
    [CustomPropertyDrawer(typeof(GameObjectLayerAttribute))]
    public class GameObjectLayerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                position = EditorGUI.PrefixLabel(position, label);
                EditorGUI.LabelField(position, String.Format("Error: {0} attribute can be applied only to {1} type", typeof(GameObjectLayerAttribute), SerializedPropertyType.Integer));
                return;
            }

            property.intValue = EditorGUI.LayerField(position, label, property.intValue);
        }
    }
}
