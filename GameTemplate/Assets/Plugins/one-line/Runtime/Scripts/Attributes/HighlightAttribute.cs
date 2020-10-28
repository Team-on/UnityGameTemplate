using System;
using UnityEngine;

namespace OneLine {
    ///<summary>
    ///Highlights field by rgb color with values in range[0..1] (red by default)
    ///Available anywhere (ROOT or NESTED FIELDS).
    ///If ROOT FIELD is highlighted its label prefix is highlighted too.
    ///</summary>
    [AttributeUsageAttribute(validOn: AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class HighlightAttribute : Attribute {
        public Color color;

        public HighlightAttribute() {
            color = new Color(1, 0, 0, 0.6f);
        }

        public HighlightAttribute(float red, float green, float blue, float alpha = 0.6f) {
            color = new Color(red, green, blue, alpha);
        }

        public Color Color { get { return color; } }

    }
}
