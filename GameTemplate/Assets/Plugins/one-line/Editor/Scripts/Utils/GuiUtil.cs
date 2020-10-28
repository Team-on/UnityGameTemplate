using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    // Workaround to draw rects, see http://answers.unity3d.com/questions/377207/drawing-a-texture-in-a-custom-propertydrawer.html
    internal static class GuiUtil {

        public static Color GrayColor { get { return new Color (0.3f, 0.3f, 0.3f); } }

        private static Dictionary<Color, GUIStyle> styles;


        static GuiUtil(){
            styles = new Dictionary<Color, GUIStyle>();
        }

        public static void DrawRect(Rect rect, Color color) {
            PrepareStyle(color);
            EditorGUI.LabelField(rect, GUIContent.none, styles[color]);
        }

        private static void PrepareStyle(Color color){
            if (styles.ContainsKey(color)) { return; }

            var texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            
            var style = new GUIStyle();
            style.normal.background = texture;
            styles[color] = style;
        }
    }
}
