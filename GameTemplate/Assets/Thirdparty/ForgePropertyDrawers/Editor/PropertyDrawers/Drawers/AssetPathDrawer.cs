using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace UnityForge.PropertyDrawers.Editor
{
    [CustomPropertyDrawer(typeof(AssetPathAttribute))]
    public class AssetPathDrawer : PropertyDrawer
    {
        private const string ResourcesFolderPath = "/Resources/";
        private const int PathPreviewHeight = 16;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var assetPathAttribute = (AssetPathAttribute)attribute;
            if (assetPathAttribute.ShowPathPreview)
            {
                position.height -= PathPreviewHeight;
            }

            if (!IsPropertyTypeValid(property))
            {
                position = EditorGUI.PrefixLabel(position, label);
                EditorGUI.LabelField(position, String.Format("Error: {0} attribute can be applied only to {1} type", typeof(AssetPathAttribute), SerializedPropertyType.String));
                return;
            }

            var assetPath = property.stringValue;
            UnityEngine.Object asset = null;
            if (!String.IsNullOrEmpty(assetPath))
            {
                if (assetPathAttribute.ResourcesRelative)
                {
                    asset = Resources.Load(assetPath);
                }
                else
                {
                    asset = AssetDatabase.LoadAssetAtPath(assetPath, assetPathAttribute.AssetType);
                }
            }

            EditorGUI.BeginChangeCheck();
            asset = EditorGUI.ObjectField(position, label, asset, assetPathAttribute.AssetType, false);
            if (EditorGUI.EndChangeCheck())
            {
                if (asset == null)
                {
                    property.stringValue = null;
                }
                else
                {
                    assetPath = AssetDatabase.GetAssetPath(asset);
                    if (assetPathAttribute.ResourcesRelative)
                    {
                        if (assetPath.Contains(ResourcesFolderPath))
                        {
                            assetPath = assetPath
                                .Substring(assetPath.IndexOf(ResourcesFolderPath) + ResourcesFolderPath.Length)
                                .Replace(Path.GetExtension(assetPath), String.Empty);
                            property.stringValue = assetPath;
                        }
                    }
                    else
                    {
                        property.stringValue = assetPath;
                    }
                }
            }

            if (assetPathAttribute.ShowPathPreview)
            {
                position.y += PathPreviewHeight;
                position = EditorGUI.PrefixLabel(position, new GUIContent("  Asset Path Preview"));
                EditorGUI.LabelField(position, String.Format("\"{0}\"", assetPath));
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (IsPropertyTypeValid(property) && ((AssetPathAttribute)attribute).ShowPathPreview)
            {
                return base.GetPropertyHeight(property, label) + PathPreviewHeight;
            }
            else
            {
                return base.GetPropertyHeight(property, label);
            }
        }

        private bool IsPropertyTypeValid(SerializedProperty property)
        {
            return property.propertyType == SerializedPropertyType.String;
        }
    }
}
