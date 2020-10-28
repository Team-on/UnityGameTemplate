using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;

namespace OneLine {
    internal class CustomPropertyDrawers {

        /*
         * Optimization
         * To avoid creating separate drawers for each array element
         */
        private static readonly Regex REGEXP_ARRAY_INDEX = new Regex(".data\\[\\d+\\]");

        private static CustomDrawerTypesCache types = new CustomDrawerTypesCache();
        private readonly Dictionary<string, PropertyDrawer> drawers = new Dictionary<string, PropertyDrawer>();

        public PropertyDrawer GetCustomPropertyDrawerFor(SerializedProperty property){
            var key = REGEXP_ARRAY_INDEX.Replace(property.propertyPath, "");
            PropertyDrawer result = null;

            if (! drawers.TryGetValue(key, out result)){
                result = CreatePropertyDrawerFor(property);
                drawers[key] = result;
            }

            return result;
        }

        private PropertyDrawer CreatePropertyDrawerFor(SerializedProperty property){
            var field = property.GetFieldInfo();
            if (field == null) return null;
            
            var result = FindAttributeDrawer(property, field);

            if (result == null) {
                result = FindDirectDrawer(property, field);
            }

            return result;
        }

        private PropertyDrawer FindAttributeDrawer(SerializedProperty property, FieldInfo field){
            TypeForDrawing typeDrawer = null;
            Attribute drawerAttribute = null;

            var attributes = property.GetCustomAttributes<PropertyAttribute>(field);

            foreach (var type in types){
                foreach (var attribute in attributes){
                    if (type.IsMatch(attribute.GetType())) {
                        typeDrawer = type;
                        drawerAttribute = attribute;
                        break;
                    }
                }
            }

            if (typeDrawer == null) return null;

            var drawer = CreateDrawer(field, typeDrawer.DrawerType);
            drawer.SetAttribute(drawerAttribute);
            return drawer;
        }

        private PropertyDrawer FindDirectDrawer(SerializedProperty property, FieldInfo field){
            var typeDrawer = types.FirstOrDefault(x => x.IsMatch(field.FieldType));

            if (typeDrawer == null) return null;

            return CreateDrawer(field, typeDrawer.DrawerType);
        }

        private PropertyDrawer CreateDrawer(FieldInfo field, Type type){
            var result = Activator.CreateInstance(type) as PropertyDrawer;
            result.SetFieldInfo(field);
            return result;
        }
    }
}
