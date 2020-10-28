using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using RectEx;

namespace OneLine {
    internal class HighlightDrawer {

        public void Draw(SerializedProperty property, Slices slices){
            var attribute = property.GetCustomAttribute<HighlightAttribute>();
            if (attribute == null) return;

            var slice = new DrawableImpl(rect => GuiUtil.DrawRect(rect.Extend(1), attribute.Color));
            slices.AddBefore(slice);
        }

    }
}
