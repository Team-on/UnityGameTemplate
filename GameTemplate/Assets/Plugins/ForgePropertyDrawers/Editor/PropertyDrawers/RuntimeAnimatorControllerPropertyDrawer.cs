using System;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace UnityForge.PropertyDrawers.Editor
{
    // Base class for custom property drawers of animator controller linked in Animator component. This property drawer is not
    // designed to be used on naked animator controller which is editor entity, if such property drawer is needed,
    // it should derive from ComponentFieldPropertyDrawer<TAttribute, AnimatorController>.
    public abstract class RuntimeAnimatorControllerPropertyDrawer<TAttribute> : ComponentFieldPropertyDrawer<TAttribute, Animator>
        where TAttribute : PropertyAttribute
    {
        public RuntimeAnimatorControllerPropertyDrawer(SerializedPropertyType propertyType) : base(propertyType)
        {
        }

        protected abstract void DrawAnimatorControllerProperty(Rect position, SerializedProperty property, AnimatorController animatorController);

        protected override void DrawComponentProperty(Rect position, SerializedProperty property, Animator animator)
        {
            var runtimeAnimatorController = animator.runtimeAnimatorController;
            if (runtimeAnimatorController != null)
            {
                var animatorController = runtimeAnimatorController as AnimatorController;
                if (animatorController != null)
                {
                    DrawAnimatorControllerProperty(position, property, animatorController);
                }
                else
                {
                    var animatorOverrideController = runtimeAnimatorController as AnimatorOverrideController;
                    if (animatorOverrideController != null)
                    {
                        animatorController = animatorOverrideController.runtimeAnimatorController as AnimatorController;
                        if (animatorController != null)
                        {
                            DrawAnimatorControllerProperty(position, property, animatorController);
                        }
                        else
                        {
                            EditorGUI.LabelField(position, String.Format("Error: not supported type of overridden controller {0} for {1} attribute", animatorController.GetType(), typeof(TAttribute)));
                        }
                    }
                    else
                    {
                        EditorGUI.LabelField(position, String.Format("Error: not supported type of controller {0} for {1} attribute", runtimeAnimatorController.GetType(), typeof(TAttribute)));
                    }
                }
            }
            else
            {
                EditorGUI.LabelField(position, String.Format("Error: animator controller not found for {0} attribute", typeof(TAttribute)));
            }
        }
    }
}
