using System;
using UnityEngine;

namespace RectEx {
    public static class CutFromExtensions {

        private const float SPACE = 2;

        public static Rect[] CutFromRight(this Rect rect, float width, float space = SPACE){
            var second = Rect.MinMaxRect(
                xmin:rect.xMax - width,
                xmax:rect.xMax,
                ymin:rect.yMin,
                ymax:rect.yMax
            );
            float min = Math.Min(rect.xMin, second.xMin - space);
            var first = Rect.MinMaxRect(
                xmin:min,
                xmax:second.xMin - space,
                ymin:rect.yMin,
                ymax:rect.yMax
            );
            return new Rect[]{first, second};
        }

        public static Rect[] CutFromBottom(this Rect rect, float height, float space = SPACE){
            var second = Rect.MinMaxRect(
                xmin:rect.xMin,
                xmax:rect.xMax,
                ymin:rect.yMax - height,
                ymax:rect.yMax
            );
            float min = Math.Min(rect.yMin, second.yMin - space);
            var first = Rect.MinMaxRect(
                xmin:rect.xMin,
                xmax:rect.xMax,
                ymin:min,
                ymax:second.yMin - space
            );
            return new Rect[]{first, second};
        }

        public static Rect[] CutFromLeft(this Rect rect, float width, float space = SPACE){
            var first = Rect.MinMaxRect(
                xmin:rect.xMin,
                xmax:rect.xMin + width,
                ymin:rect.yMin,
                ymax:rect.yMax
            );
            float max = Math.Max(rect.xMax, first.xMax + space);
            var second = Rect.MinMaxRect(
                xmin:first.xMax + space,
                xmax:max,
                ymin:rect.yMin,
                ymax:rect.yMax
            );
            return new Rect[]{first, second};
        }

        public static Rect[] CutFromTop(this Rect rect, float height, float space = SPACE){
            var first = Rect.MinMaxRect(
                xmin:rect.xMin,
                xmax:rect.xMax,
                ymin:rect.yMin,
                ymax:rect.yMin + height
            );
            float max = Math.Max(rect.yMax, first.yMax + space);
            var second = Rect.MinMaxRect(
                xmin:rect.xMin,
                xmax:rect.xMax,
                ymin:first.yMax + space,
                ymax:max
            );
            return new Rect[]{first, second};
        }
    }
}
