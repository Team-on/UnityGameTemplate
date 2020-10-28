using UnityEditor;
using UnityEngine;
using OneLine;
using RectEx;

namespace OneLine {
#if ! ONE_LINE_HORIZONTAL_SEPARATOR_DISABLE
    [CustomPropertyDrawer(typeof(SeparatorAttribute))]
#endif
    public class SeparatorAttributeDrawer : UnityEditor.DecoratorDrawer {

        private new SeparatorAttribute attribute { get { return base.attribute as SeparatorAttribute; }}

        private Color Color { get { return GuiUtil.GrayColor; } }
        private GUIStyle textStyle;
        private const float spaceBefore = 0;

        public override float GetHeight(){
            return spaceBefore + Mathf.Max(attribute.Thickness, EditorGUIUtility.singleLineHeight + 5);
        }

        public override void OnGUI(Rect rect) {
            PrepareStyles();
            rect.y += spaceBefore;
            rect.height -= spaceBefore;

            string text = attribute.Text;
            int thickness = attribute.Thickness;

            if (string.IsNullOrEmpty(text)){
                DrawLine(rect, thickness);
            }
            else {
                var textSize = textStyle.CalcSize(new GUIContent(text));
                var rects = rect.Row(new float[]{1,0,1}, new float[]{0, textSize.x, 0});

                DrawLine(rects[0], thickness);
                DrawText(rects[1], text);
                DrawLine(rects[2], thickness);
            }
        }

        private void PrepareStyles(){
            if (textStyle != null){ return; }

            textStyle = new GUIStyle();
            textStyle.fontStyle = FontStyle.Bold;
            textStyle.normal.textColor = Color;
            textStyle.alignment = TextAnchor.MiddleCenter;
        }

        private void DrawText(Rect rect, string text){
            EditorGUI.LabelField(rect, text, textStyle); 
        }
        private void DrawLine(Rect rect, int thickness){
            rect.y += (rect.height - thickness) / 2;
            rect.height = thickness;
            GuiUtil.DrawRect(rect, Color);
        }
    }
}
