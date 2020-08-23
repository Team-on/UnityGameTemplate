using System;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace UnityForge.PropertyDrawers.Editor
{
    [CustomPropertyDrawer(typeof(AnimatorLayerNameAttribute))]
    public class AnimatorLayerNameDrawer : RuntimeAnimatorControllerPropertyDrawer<AnimatorLayerNameAttribute>
    {
        public AnimatorLayerNameDrawer() : base(SerializedPropertyType.String)
        {
        }

        protected override string GetPropertyPath(AnimatorLayerNameAttribute attribute)
        {
            return attribute.AnimatorField;
        }

        protected override void DrawAnimatorControllerProperty(Rect position, SerializedProperty property, AnimatorController animatorController)
        {
            var propertyStringValue = property.hasMultipleDifferentValues ? "-" : property.stringValue;
            var content = String.IsNullOrEmpty(propertyStringValue) ? new GUIContent("<None>") : new GUIContent(propertyStringValue);
            if (GUI.Button(position, content, EditorStyles.popup))
            {
                LayerSelector(property, animatorController);
            }
        }

        private static void LayerSelector(SerializedProperty property, AnimatorController animatorController)
        {
            var menu = new GenericMenu();
            foreach (var layer in animatorController.layers)
            {
                var layerName = layer.name;
                menu.AddItem(new GUIContent(layerName),
                        layerName == property.stringValue,
                        StringPropertyPair.HandlePairObjectSelect,
                        new StringPropertyPair(layerName, property));
            }
            menu.ShowAsContext();
        }
    }
}
