using System;
using UnityEngine;

namespace UnityForge.PropertyDrawers
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class AnimatorParameterNameAttribute : AnimatorPropertyAttribute
    {
        public AnimatorControllerParameterType ParameterType { get; private set; }

        public AnimatorParameterNameAttribute(AnimatorControllerParameterType parameterType, string animatorField = null) : base(animatorField)
        {
            ParameterType = parameterType;
        }
    }
}
