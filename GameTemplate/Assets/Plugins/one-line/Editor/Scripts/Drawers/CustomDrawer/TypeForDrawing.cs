using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;

namespace OneLine {
    internal class TypeForDrawing {
        private Type Type {get; set;}
        private bool UseForChildren {get; set;}
        public Type DrawerType {get; private set;}

        public TypeForDrawing(CustomPropertyDrawer attribute, Type drawerType) {
            Type = attribute.GetTargetType();
            UseForChildren = attribute.IsForChildren();

            DrawerType = drawerType;
        }

        public bool IsMatch(Type target){
            if (target == null) return false;

            if (UseForChildren){
                return Type == target || target.IsSubclassOf(Type);
            }
            else {
                return Type == target;
            }
        }
    }

}
