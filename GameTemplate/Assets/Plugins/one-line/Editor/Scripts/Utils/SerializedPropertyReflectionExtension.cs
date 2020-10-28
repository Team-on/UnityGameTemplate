using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal static class SerializedPropertyReflectionExtension {

        private static readonly HashSet<Type> TYPES_WITH_MISSPELLING_PROPERTY_PATH = new HashSet<Type>(){
            typeof(Matrix4x4),
            typeof(Rect),
            typeof(Bounds)
        };
        private static Dictionary<string, FieldInfo> FIELDS_CACHE = new Dictionary<string, FieldInfo>();

        public static T GetCustomAttribute<T>(this SerializedProperty property, FieldInfo fieldInfo = null) where T : Attribute {
            return GetCustomAttributes<T>(property, fieldInfo).FirstOrDefault();
        }

        public static T[] GetCustomAttributes<T>(this SerializedProperty property, FieldInfo fieldInfo = null) where T : Attribute {
            if (property == null) throw new ArgumentNullException();

            if (fieldInfo == null) fieldInfo = GetFieldInfo(property);

            if (fieldInfo == null) {
                return new T[0];
            }
            else {
                return fieldInfo.GetCustomAttributes(typeof(T), false).Cast<T>().ToArray();
            }
        }

        public static Type GetRealType(this SerializedProperty property){
            var fieldInfo = GetFieldInfo(property);

            return fieldInfo == null ? null : fieldInfo.FieldType;
        }

        public static FieldInfo GetFieldInfo(this SerializedProperty property) {
            FieldInfo result = null;

            Type type = property.serializedObject.targetObject.GetType();
            string key = String.Format("{0}->{1}", type.FullName, property.propertyPath);
            if (!FIELDS_CACHE.TryGetValue(key, out result)){
                result = FindFieldInfo(property, type);
                FIELDS_CACHE[key] = result;
            }

            return result;
        }

        private static FieldInfo FindFieldInfo(SerializedProperty property, Type type){
            string[] path = property.propertyPath.Split('.');

            Type lastType = type;
            FieldInfo field = null;
            for (int i = 0; i < path.Length; i++) {
                lastType = type;
                field = null;

                do {
                    field = type.GetField(path[i], BindingFlags.Public 
                                                | BindingFlags.NonPublic
                                                | BindingFlags.Instance);
                }
                while (field == null && (type = type.BaseType) != null);

                if (field == null) {
                    NotifyMisspelledPropertyPath(lastType, property.propertyPath, path[i]);
                    return null;
                }

                type = field.FieldType;

                CrutchIfArray(path, ref i, ref type);
            }

            return field;
        }

        private static void NotifyMisspelledPropertyPath(Type parent, string propertyPath, string fieldName){
            if (!TYPES_WITH_MISSPELLING_PROPERTY_PATH.Contains(parent)){
                var message = "[OneLine] Part `{0}` of property path `{1}` doesn't match field definitions of type `{2}`";
                Debug.LogWarning(String.Format(message, fieldName, propertyPath, parent.FullName));

                TYPES_WITH_MISSPELLING_PROPERTY_PATH.Add(parent);
            }
        }

        private static void CrutchIfArray(string[] path, ref int i, ref Type type){
            int next = i + 1;
            if (next < path.Length && path[next] == "Array") {
                i += 2;
                if (type.IsArray) {
                    type = type.GetElementType();
                }
                else {
                    type = type.GetGenericArguments()[0];
                }
            }
        }

    }
}
