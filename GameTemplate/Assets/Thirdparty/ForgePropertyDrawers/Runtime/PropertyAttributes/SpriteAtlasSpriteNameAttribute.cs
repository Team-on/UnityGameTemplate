#if UNITY_2017_1_OR_NEWER

using System;
using UnityEngine;

namespace UnityForge.PropertyDrawers
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class SpriteAtlasSpriteNameAttribute : PropertyAttribute
    {
        public string SpriteAtlasField { get; private set; }

        public SpriteAtlasSpriteNameAttribute(string spriteAtlasField)
        {
            SpriteAtlasField = spriteAtlasField;
        }
    }
}

#endif
