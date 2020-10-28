using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal class TooltipDrawer {

        public void Draw(SerializedProperty property, Slices slices){
            var attribute = property.GetCustomAttribute<TooltipAttribute>();
            if (attribute == null) return;

            var slice = new DrawableImpl(rect => EditorGUI.LabelField(rect, new GUIContent("", attribute.tooltip)));
            slices.AddAfter(slice);
        }

    }
}
