using System;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace UnityForge.PropertyDrawers.Editor
{
    [CustomPropertyDrawer(typeof(AnimatorParameterNameAttribute))]
    public class AnimatorParameterNameDrawer : RuntimeAnimatorControllerPropertyDrawer<AnimatorParameterNameAttribute>
    {
        public AnimatorParameterNameDrawer() : base(SerializedPropertyType.String)
        {
        }

        protected override string GetPropertyPath(AnimatorParameterNameAttribute attribute)
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

        private void LayerSelector(SerializedProperty property, AnimatorController animatorController)
        {
            var menu = new GenericMenu();

            var parameterAttribute = (AnimatorParameterNameAttribute)attribute;
            var parameterType = parameterAttribute.ParameterType;
            foreach (var parameter in animatorController.parameters)
            {
                if (parameter.type == parameterType)
                {
                    var parameterName = parameter.name;
                    menu.AddItem(new GUIContent(parameterName),
                            parameterName == property.stringValue,
                            StringPropertyPair.HandlePairObjectSelect,
                            new StringPropertyPair(parameterName, property));
                }
            }

            menu.ShowAsContext();
        }
    }
}
