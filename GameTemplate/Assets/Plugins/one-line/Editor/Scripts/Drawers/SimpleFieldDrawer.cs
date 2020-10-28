using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal class SimpleFieldDrawer : Drawer {

        public float GetWeight(SerializedProperty property){
            var weights = property.GetCustomAttributes<WeightAttribute>();
            if (weights.Any()) { 
                return weights.Sum(w=>w.Weight);
            }
            else
            {
                switch (property.propertyType) {
                    case SerializedPropertyType.Boolean: {
                        return 0;
                    }
                    default: {
                        return 1;
                    }
                }
            }
        }

        public float GetFixedWidth(SerializedProperty property){
            var widths = property.GetCustomAttributes<WidthAttribute>();
            if (widths.Any()) {
                return widths.Sum(w=>w.Width);
            }
            else
            {
                switch (property.propertyType){
                    case SerializedPropertyType.Boolean: {
                        return EditorGUIUtility.singleLineHeight - 2;
                    }
                    default: {
                        return 0; 
                    }
                }
            }
        }

        public override void AddSlices(SerializedProperty property, Slices slices){
            highlight.Draw(property, slices);
            slices.Add(new SliceImpl(GetWeight(property), GetFixedWidth(property), rect => Draw(rect, property.Copy())));
            tooltip.Draw(property, slices);
        }

        public virtual void Draw(Rect rect, SerializedProperty property) {
            DrawProperty(rect, property);
        }

        /*
         * WORKAROUND
         * Unity3d `feature`: EditorGUI.PropertyField draws field 
         * with all decorators (like Header, Space, etc) and this behaviour 
         * can not be ommited.
         * see: http://answers.unity3d.com/questions/1394991/how-to-preserve-drawing-decoratordrawer-like-heade.html
         * Headers and Separators (provided by one-line) produces artefacts.
         * We solve this problem with reflection, but we call internal method
         * and this may be dangerous: unity3d developers may change API =(
         */
        private void DrawProperty(Rect rect, SerializedProperty property){
            //EditorGUI.PropertyField(rect, property, GUIContent.none);

            typeof(EditorGUI)
                .GetMethod("DefaultPropertyField", BindingFlags.NonPublic | BindingFlags.Static)
                .Invoke(null, new object[]{rect, property, GUIContent.none});
        }

    }
}
