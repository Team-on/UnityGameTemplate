using System;

namespace UnityForge.PropertyDrawers
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class AnimatorLayerNameAttribute : AnimatorPropertyAttribute
    {
        public AnimatorLayerNameAttribute(string animatorField = null) : base(animatorField)
        {
        }
    }
}
