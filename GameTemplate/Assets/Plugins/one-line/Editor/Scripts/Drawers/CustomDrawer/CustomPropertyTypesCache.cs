using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;

namespace OneLine {
    internal class CustomDrawerTypesCache : IEnumerable<TypeForDrawing> {
        private static IEnumerable<TypeForDrawing> customDrawers;

        private void Init(){
            if (customDrawers == null) {
                customDrawers = findAllCustomDrawers();
            }
        }

        private static IEnumerable<TypeForDrawing> findAllCustomDrawers(){
            return (
                from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from type in assembly.GetTypes()
                where type.IsSubclassOf(typeof(PropertyDrawer))
                where type != typeof(OneLinePropertyDrawer) && ! type.IsSubclassOf(typeof(OneLinePropertyDrawer))
                from attribute in type.GetCustomAttributes(typeof(CustomPropertyDrawer), true)
                select new TypeForDrawing(attribute as CustomPropertyDrawer, type)
            ).ToArray();
        }

        public IEnumerator<TypeForDrawing> GetEnumerator() {
            Init();
            return customDrawers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
