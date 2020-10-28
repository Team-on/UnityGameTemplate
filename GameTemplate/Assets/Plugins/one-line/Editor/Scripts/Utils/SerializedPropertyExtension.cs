using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal static class SerializedPropertyExtension {

        public static IEnumerable<SerializedProperty> GetChildren(this SerializedProperty property) {
            if (!property.hasVisibleChildren) {
                yield break;
            }

            var copy = property.Copy();
            int depth = copy.depth;

            copy.Next(true);
            do {
                string lastPath = copy.propertyPath;
                yield return copy.Copy();

                if (copy.propertyPath != lastPath) {
                    var message =
                        string.Format("Property path'd been changed while iteration. Last iteration path: {0}, current path: {1}", lastPath, copy.propertyPath);
                    throw new InvalidOperationException(message);
                }
            }
            while (copy.Next(false) && copy.depth > depth);
        }

        public static int CountChildrenAndMoveNext(this SerializedProperty property){
            var depth = property.depth;
            int result = 0;
            while (property.NextVisible(true) && property.depth > depth){
                result++;
            }
            return result;
        }

        public static bool IsReallyArray(this SerializedProperty property){
            return property.isArray && property.propertyType != SerializedPropertyType.String;
        }

        public static IEnumerable<SerializedProperty> GetArrayElements(this SerializedProperty property) {
            if (!property.IsReallyArray()) {
                string message = string.Format("Property {0} is not array or list", property.displayName);
                throw new InvalidOperationException(message);
            }

            property = property.Copy();

            string path = property.propertyPath;
            int size = property.arraySize;
            for (int i = 0; i < size; i++) {
                if (property.propertyPath != path) {
                    string message = string.Format("Property path {0} is changed during iteration", property.displayName);
                    throw new InvalidOperationException(message);
                }
                yield return property.GetArrayElementAtIndex(i).Copy();
            }
        }

        public static bool IsArrayElement(this SerializedProperty property){
            var path = property.propertyPath;
            return path.Substring(path.Length - 1, 1) == "]" ;
        }

        public static bool IsArrayFirstElement(this SerializedProperty property){
            var path = property.propertyPath;
            return path.Substring(path.Length - 3, 3) == "[0]" ;
        }

    }
}
