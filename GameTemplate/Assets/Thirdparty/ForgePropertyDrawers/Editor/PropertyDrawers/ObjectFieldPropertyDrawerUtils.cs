using System;
using UnityEditor;
using UnityEngine;

namespace UnityForge.PropertyDrawers.Editor
{
    public static class ObjectFieldPropertyDrawerUtils
    {
        public static void DrawObjectFieldPoperty<TObject>(
            Rect position, SerializedProperty property, string field,
            Action<Rect, SerializedProperty, TObject> drawer)
            where TObject : UnityEngine.Object
        {
            var fieldProperty = property.serializedObject.FindProperty(field);
            if (fieldProperty != null)
            {
                var fieldObjectReferenceValue = fieldProperty.objectReferenceValue;
                if (fieldObjectReferenceValue != null)
                {
                    var objectReference = fieldObjectReferenceValue as TObject;
                    if (objectReference != null)
                    {
                        drawer(position, property, objectReference);
                    }
                    else
                    {
                        EditorGUI.LabelField(position, String.Format("Error: field type is not {0}", typeof(TObject)));
                    }
                }
                else
                {
                    EditorGUI.LabelField(position, String.Format("Error: {0} field is not set", typeof(TObject)));
                }
            }
            else
            {
                EditorGUI.LabelField(position, String.Format("Error: {0} field {1} not found in inspected object", typeof(TObject), field));
            }
        }
    }
}
