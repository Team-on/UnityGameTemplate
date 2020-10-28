using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal class FixedArrayDrawer : ComplexFieldDrawer {

        public FixedArrayDrawer(DrawerProvider getDrawer) : base(getDrawer) {
        }

        protected override IEnumerable<SerializedProperty> GetChildren(SerializedProperty property){
            return property.GetArrayElements();
        }

        public override void AddSlices(SerializedProperty property, Slices slices){
            ModifyLength(property);
            base.AddSlices(property, slices);
        }

        protected virtual int ModifyLength(SerializedProperty property) {
            var attribute = property.GetCustomAttribute<ArrayLengthAttribute>();
            if (attribute == null) {
                var message = string.Format("Can not find ArrayLengthAttribute at property {0)", property.propertyPath);
                throw new InvalidOperationException(message);
            }
            property.arraySize = attribute.Length;
            return property.arraySize;
        }

    }
}
