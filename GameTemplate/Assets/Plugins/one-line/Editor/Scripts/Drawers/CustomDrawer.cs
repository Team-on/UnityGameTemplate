using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;

namespace OneLine {
    internal class CustomDrawer : SimpleFieldDrawer {

        private CustomPropertyDrawers drawers = new CustomPropertyDrawers();

        public bool HasCustomDrawer(SerializedProperty property){
#if ! ONE_LINE_CUSTOM_DRAWER_DISABLE
            var drawer = drawers.GetCustomPropertyDrawerFor(property);

            return drawer != null && drawer.GetPropertyHeight(property, GUIContent.none) < 20;
#else
            return false;
#endif
        }

        public override void Draw(Rect rect, SerializedProperty property) {
            DrawProperty(rect, property);
        }

        private void DrawProperty(Rect rect, SerializedProperty property){
            var drawer = drawers.GetCustomPropertyDrawerFor(property);
            if (drawer != null) {
                drawer.OnGUI(rect, property, GUIContent.none);
            }
            else {
                var message = "[OneLine] Can not draw CustomPropertyDrawer for `{0}` at property path `{1}";
                throw new Exception(string.Format(message, property.type, property.propertyPath));
            }
        }

    }
}
