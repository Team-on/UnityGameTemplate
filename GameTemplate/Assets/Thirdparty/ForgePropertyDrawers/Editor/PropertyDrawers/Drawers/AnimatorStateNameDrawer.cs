using System;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace UnityForge.PropertyDrawers.Editor
{
    [CustomPropertyDrawer(typeof(AnimatorStateNameAttribute))]
    public class AnimatorStateNameDrawer : RuntimeAnimatorControllerPropertyDrawer<AnimatorStateNameAttribute>
    {
        public AnimatorStateNameDrawer() : base(SerializedPropertyType.String)
        {
        }

        protected override string GetPropertyPath(AnimatorStateNameAttribute attribute)
        {
            return attribute.AnimatorField;
        }

        protected override void DrawAnimatorControllerProperty(Rect position, SerializedProperty property, AnimatorController animatorController)
        {
            var propertyStringValue = property.hasMultipleDifferentValues ? "-" : property.stringValue;
            var content = String.IsNullOrEmpty(propertyStringValue) ? new GUIContent("<None>") : new GUIContent(propertyStringValue);
            if (GUI.Button(position, content, EditorStyles.popup))
            {
                StateSelector(property, animatorController);
            }
        }

        private static void StateSelector(SerializedProperty property, AnimatorController animatorController)
        {
            var menu = new GenericMenu();
            foreach (var layer in animatorController.layers)
            {
                var stateNamePrefix = layer.name + "/";
                foreach (var childState in layer.stateMachine.states)
                {
                    var stateName = childState.state.name;
                    menu.AddItem(new GUIContent(stateNamePrefix + stateName),
                        stateName == property.stringValue,
                        StringPropertyPair.HandlePairObjectSelect,
                        new StringPropertyPair(stateName, property));
                }
            }
            menu.ShowAsContext();
        }
    }
}
