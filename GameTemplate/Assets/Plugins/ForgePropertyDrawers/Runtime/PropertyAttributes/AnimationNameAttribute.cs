using System;
using UnityEngine;

namespace UnityForge.PropertyDrawers
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class AnimationNameAttribute : PropertyAttribute
    {
        public string AnimationField { get; private set; }

        public AnimationNameAttribute(string animationField = null)
        {
            AnimationField = animationField;
        }
    }
}
