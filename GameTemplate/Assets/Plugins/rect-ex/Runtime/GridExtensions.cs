using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using RectEx.Internal;

namespace RectEx {
    public static class GridExtensions {

        private const float SPACE = 2f;

        public static Rect[,] Grid(this Rect rect, int rows, int columns, float space = SPACE){
            return Grid(rect, rows, columns, space, space);
        }

        public static Rect[,] Grid(this Rect rect, int rows, int columns, float spaceBetweenRows, float spaceBetweenColumns){
            var grid = rect.Column(rows, spaceBetweenRows)
                           .Select(x => x.Row(columns, spaceBetweenColumns))
                           .ToArray();

            var result = new Rect[rows, columns];
            for (int row = 0; row < rows; row++){
                for (int column = 0; column < columns; column++){
                    result[row,column] = grid[row][column];
                }
            }
            return result;
        }

        public static Rect[,] Grid(this Rect rect, int size, float space = SPACE){
            return Grid(rect, size, size, space, space);
        }

    }
}
