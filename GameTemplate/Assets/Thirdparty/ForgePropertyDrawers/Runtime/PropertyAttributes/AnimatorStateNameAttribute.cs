using System;

namespace UnityForge.PropertyDrawers
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class AnimatorStateNameAttribute : AnimatorPropertyAttribute
    {
        public AnimatorStateNameAttribute(string animatorField = null) : base(animatorField)
        {
        }
    }
}
