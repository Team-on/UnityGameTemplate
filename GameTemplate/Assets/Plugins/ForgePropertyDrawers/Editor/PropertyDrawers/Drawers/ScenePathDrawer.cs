using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityForge.PropertyDrawers.Editor
{
    [CustomPropertyDrawer(typeof(ScenePathAttribute))]
    public class ScenePathDrawer : PropertyDrawer
    {
        private const string SlashUnicode = "\u200A\u2215\u200A";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                position = EditorGUI.PrefixLabel(position, label);
                EditorGUI.LabelField(position, String.Format("Error: {0} attribute can be applied only to {1} type", typeof(ScenePathAttribute), SerializedPropertyType.String));
                return;
            }

            var scenePaths = GetScenePaths();

            // Unity menus treat slash as nested menu content. To display scene paths
            // as single items replace slash with unicode symbols. See more info on the issue
            // here: https://answers.unity.com/questions/398495/can-genericmenu-item-content-display-.html
            var popupScenePaths = Array.ConvertAll(scenePaths, p => p.Replace("/", SlashUnicode));

            EditorGUI.BeginChangeCheck();
            var index = EditorGUI.Popup(position, property.displayName,
                Array.IndexOf(scenePaths, property.stringValue),
                popupScenePaths);

            if (EditorGUI.EndChangeCheck() && index >= 0)
            {
                property.stringValue = scenePaths[index];
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        private string[] GetScenePaths()
        {
            string[] scenePaths;
            var scenePathAttribute = (ScenePathAttribute)attribute;
            if (scenePathAttribute.FromBuildSettings)
            {
                scenePaths = scenePathAttribute.OnlyEnabled
                    ? EditorBuildSettings.scenes.Where(s => s.enabled).Select(x => x.path).ToArray()
                    : Array.ConvertAll(EditorBuildSettings.scenes, x => x.path);
            }
            else
            {
                var assetGuids = AssetDatabase.FindAssets("t:scene");
                scenePaths = Array.ConvertAll(assetGuids, x => AssetDatabase.GUIDToAssetPath(x));
            }

            if (!scenePathAttribute.FullProjectPath)
            {
                scenePaths = Array.ConvertAll(scenePaths, x =>
                {
                    if (x.StartsWith("Assets/") && x.EndsWith(".unity"))
                    {
                        x = x.Substring("Assets/".Length, x.Length - "Assets/".Length - ".unity".Length);
                    }
                    return x;
                });
            }

            return scenePaths;
        }
    }
}
