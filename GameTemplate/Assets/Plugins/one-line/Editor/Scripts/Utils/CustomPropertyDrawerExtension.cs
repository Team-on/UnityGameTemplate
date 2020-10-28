using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    public static class CustomPropertyDrawerExtension {
        public static Type GetTargetType(this CustomPropertyDrawer drawer){
            return typeof(CustomPropertyDrawer)
                    .GetField("m_Type", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(drawer) as Type;
        }

        public static bool IsForChildren(this CustomPropertyDrawer drawer){
            return (bool) typeof(CustomPropertyDrawer)
                    .GetField("m_UseForChildren", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(drawer);
        }

        public static void SetAttribute(this PropertyDrawer drawer, Attribute attribute){
            typeof(PropertyDrawer)
                .GetField("m_Attribute", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(drawer, attribute);
        }
    
        public static void SetFieldInfo(this PropertyDrawer drawer, FieldInfo fieldInfo){
            typeof(PropertyDrawer)
                .GetField("m_FieldInfo", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(drawer, fieldInfo);
        }
    }
}
