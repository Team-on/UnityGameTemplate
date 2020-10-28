using System;
using UnityEngine;

namespace RectEx {
    public static class MoveToExtensions {

        private const float SPACE = 2f;

        public static Rect MoveRight(this Rect rect, float space = SPACE){
            rect = rect.Abs();
            rect.x += rect.width + space;
            return rect;
        }

        public static Rect MoveRightFor(this Rect rect, float newWidth, float space = SPACE){
            rect = rect.Abs();
            rect.x += rect.width + space;
            rect.width = newWidth;
            return rect.Abs();
        }

        public static Rect MoveLeft(this Rect rect, float space = SPACE){
            rect = rect.Abs();
            rect.x -= rect.width + space;
            return rect;
        }

        public static Rect MoveLeftFor(this Rect rect, float newWidth, float space = SPACE){
            rect = rect.Abs();
            rect.x -= newWidth + space;
            rect.width = newWidth;
            return rect.Abs();
        }

        public static Rect MoveUp(this Rect rect, float space = SPACE){
            rect = rect.Abs();
            rect.y -= rect.height + space;
            return rect;
        }

        public static Rect MoveUpFor(this Rect rect, float newHeight, float space = SPACE){
            rect = rect.Abs();
            rect.y -= newHeight + space;
            rect.height = newHeight;
            return rect.Abs();
        }

        public static Rect MoveDown(this Rect rect, float space = SPACE){
            rect = rect.Abs();
            rect.y += rect.height + space;
            return rect;
        }

        public static Rect MoveDownFor(this Rect rect, float newHeight, float space = SPACE){
            rect = rect.Abs();
            rect.y += rect.height + space;
            rect.height = newHeight;
            return rect.Abs();
        }
    }
}