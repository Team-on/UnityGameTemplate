using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal class DynamicArrayDrawer : FixedArrayDrawer {
        private Drawer buttons;
        private Action<SerializedProperty> notifyChange;

        public DynamicArrayDrawer(DrawerProvider getDrawer, Action<SerializedProperty> notifyChange) : base(getDrawer) {
            buttons = new ArrayButtonsDrawer(notifyChange);
            this.notifyChange = notifyChange;
        }

        public override void AddSlices(SerializedProperty property, Slices slices){
            base.AddSlices(property, slices);
            buttons.AddSlices(property, slices);
        }

        protected override int ModifyLength(SerializedProperty property) {
            return property.arraySize;
        }

        protected override void DrawChild(SerializedProperty parent, SerializedProperty child, Slices slices){
            var childSlices = new SlicesImpl();

            childSlices.AddBefore(new DrawableImpl(rect => DrawElementContextMenu(rect, parent, child)));
            base.DrawChild(parent, child, childSlices);

            slices.Add(childSlices);
        }

        private void DrawElementContextMenu(Rect rect, SerializedProperty parent, SerializedProperty element) {
            Event current = Event.current;
            if (current.type == EventType.ContextClick && rect.Contains(current.mousePosition)) {
                current.Use();

                element = element.Copy();

                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("Dublicate"), false, () => {
                    element.DuplicateCommand();
                    element.serializedObject.ApplyModifiedProperties();
                    notifyChange(parent);
                });
                menu.AddItem(new GUIContent("Delete"), false, () => {
                    element.DeleteCommand();
                    element.serializedObject.ApplyModifiedProperties();
                    notifyChange(parent);
                });
                menu.DropDown(rect);
            }
        }

    }
}
