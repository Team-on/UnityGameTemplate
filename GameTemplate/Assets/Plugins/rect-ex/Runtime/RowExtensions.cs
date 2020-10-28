using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using RectEx.Internal;

namespace RectEx {
    public static class RowExtensions {

        private const float SPACE = 2f;

        public static Rect[] Row(this Rect rect, int count, float space = SPACE){
            rect = rect.Abs();
            switch (count) {
                case 1: {
                    return new Rect[] {rect};
                }
                case 2: {
                    return RowTwoSlices(rect, space);
                }
                case 3: {
                    return RowThreeSlices(rect, space);
                }
                default: {
                    var weights = Enumerable.Repeat(1f, count).ToArray();
                    var widthes = Enumerable.Repeat(0f, count).ToArray();
                    return Row(rect, weights, widthes, space);
                }
            }
        }

        public static Rect[] Row(this Rect rect, float[] weights, float space = SPACE){
            return Row(rect, weights, null, space);
        }

        public static Rect[] Row(this Rect rect, float[] weights, float[] widthes, float space = SPACE) {
            if (weights == null){
                throw new ArgumentException("Weights is null. You must specify it");
            }

            if (widthes == null){
                widthes = Enumerable.Repeat(0f, weights.Length).ToArray();
            }

            rect = rect.Abs();
            return RowSafe(rect, weights, widthes, space);
        }

        private static Rect[] RowTwoSlices(Rect rect, float space) {
            var first = new Rect(
                x: rect.x,
                y: rect.y,
                width: (rect.width - space) / 2,
                height: rect.height
            );
            var second = new Rect(
                x: first.x + space + first.width,
                y: first.y,
                width: first.width,
                height: first.height
            );
            return new Rect[] {first, second};
        }

        private static Rect[] RowThreeSlices(Rect rect, float space) {
            var first = new Rect(
                x: rect.x,
                y: rect.y,
                width: (rect.width - 2*space) / 3,
                height: rect.height
            );
            var second = new Rect(
                x: first.x + first.width + space,
                y: rect.y,
                width: first.width,
                height: first.height
            );
            var third = new Rect(
                x: second.x + second.width + space,
                y: second.y,
                width: second.width,
                height: second.height
            );
            return new Rect[] {first, second, third};
        }

        private static Rect[] RowSafe(Rect rect, float[] weights, float[] widthes, float space) {
            var cells = weights.Merge(widthes, (weight, width) => new Cell(weight, width)).Where( cell => cell.HasWidth);

            float weightUnit = GetWeightUnit(rect.width, cells, space);

            var result = new List<Rect>();
            float nextX = rect.x;
            foreach (var cell in cells) {
                result.Add(new Rect(
                               x: nextX,
                               y: rect.y,
                               width: cell.GetWidth(weightUnit),
                               height: rect.height
                           ));

                nextX += cell.HasWidth ? (cell.GetWidth(weightUnit) + space) : 0;
            }

            return result.ToArray();
        }

        private static float GetWeightUnit(float fullWidth, IEnumerable<Cell> cells, float space) {
            float result = 0;
            float weightsSum = cells.Sum(cell => cell.Weight);

            if (weightsSum > 0) {
                float fixedWidth = cells.Sum(cell => cell.FixedWidth);
                float spacesWidth = (cells.Count(cell => cell.HasWidth) - 1) * space;
                result = (fullWidth - fixedWidth - spacesWidth) / weightsSum;
            }

            return result;
        }

        private class Cell {
            public float Weight { get; private set; }
            public float FixedWidth { get; private set; }

            public Cell(float weight, float fixedWidth) {
                this.Weight = weight;
                this.FixedWidth = fixedWidth;

            }

            public bool HasWidth { get { return FixedWidth > 0 || Weight > 0; } }
            public float GetWidth(float weightUnit) {
                return FixedWidth + Weight * weightUnit;
            }
        }

    }
}
