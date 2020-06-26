#if UNITY_2017_1_OR_NEWER

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

namespace UnityForge.PropertyDrawers.Editor
{
    [CustomPropertyDrawer(typeof(SpriteAtlasSpriteNameAttribute))]
    public class SpriteAtlasSpriteNameDrawer : PropertyDrawer
    {
        private const string SpriteNameSuffix = "(Clone)";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position = EditorGUI.PrefixLabel(position, label);

            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.LabelField(position, String.Format("Error: {0} attribute can be applied only to {1} type", typeof(SpriteAtlasSpriteNameAttribute), SerializedPropertyType.String));
                return;
            }

            var spriteAtlasField = ((SpriteAtlasSpriteNameAttribute)attribute).SpriteAtlasField;
            if (!String.IsNullOrEmpty(spriteAtlasField))
            {
                ObjectFieldPropertyDrawerUtils.DrawObjectFieldPoperty<SpriteAtlas>(
                    position, property, spriteAtlasField,
                    DrawSpriteNameSelect);
            }
            else
            {
                EditorGUI.LabelField(position, String.Format("Error: sprite atlas field is null or empty for {0} attribute", typeof(SpriteAtlasSpriteNameAttribute)));
            }
        }

        private static void DrawSpriteNameSelect(Rect position, SerializedProperty property, SpriteAtlas atlas)
        {
            var propertyStringValue = property.hasMultipleDifferentValues ? "-" : property.stringValue;
            var content = String.IsNullOrEmpty(propertyStringValue) ? new GUIContent("<None>") : new GUIContent(propertyStringValue);
            if (GUI.Button(position, content, EditorStyles.popup))
            {
                SpriteNameSelector(property, atlas);
            }
        }

        private static void SpriteNameSelector(SerializedProperty property, SpriteAtlas atlas)
        {
            var menu = new GenericMenu();

            var spritesCount = atlas.spriteCount;
            var sprites = new Sprite[spritesCount];
            atlas.GetSprites(sprites);

            foreach (var sprite in sprites)
            {
                var name = sprite.name;
                if (name.EndsWith(SpriteNameSuffix))
                {
                    name = name.Remove(name.Length - SpriteNameSuffix.Length);
                }

                // While sprite atlas supports sprites with duplicate names, GenericMenu does not,
                // so it will display only one items for all sprites with duplicate name.
                menu.AddItem(new GUIContent(name),
                    name == property.stringValue,
                    StringPropertyPair.HandlePairObjectSelect,
                    new StringPropertyPair(name, property));
            }

            menu.ShowAsContext();
        }
    }
}

#endif
