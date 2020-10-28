using System;
using UnityEngine;

namespace OneLine {
    ///<summary>
    ///Draws horizontal or vertical separator
    ///</summary>
    [AttributeUsage(validOn: AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class SeparatorAttribute : PropertyAttribute {

        public SeparatorAttribute() {
            Text = "";
            Thickness = 2;
        }

        public SeparatorAttribute(string text) : this() {
            Text = text;
        }

        public SeparatorAttribute(string text, int thickness) : this(text){
            Thickness = thickness;
        }

        public string Text { get; set; }
        public int Thickness { get; set; }

    }
}
