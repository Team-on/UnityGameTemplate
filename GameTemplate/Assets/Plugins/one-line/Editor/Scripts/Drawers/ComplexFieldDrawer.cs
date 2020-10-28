using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {

    internal delegate Drawer DrawerProvider(SerializedProperty property);

    internal abstract class ComplexFieldDrawer : Drawer {

        protected DrawerProvider getDrawer;
        public int RootDepth { get; set; }

        public ComplexFieldDrawer(DrawerProvider getDrawer){
            this.getDrawer = getDrawer;
        }

        protected abstract IEnumerable<SerializedProperty> GetChildren(SerializedProperty property);

        #region Weights

        public override void AddSlices(SerializedProperty property, Slices slices){
            highlight.Draw(property, slices);
            DrawChildren(property, slices);
            tooltip.Draw(property, slices);
        }

        private void DrawChildren(SerializedProperty property, Slices slices){
            var childSlices = new SlicesImpl();
            GetChildren(property)
                .ForEachExceptLast((child) => {
                    DrawChildWithDecorators(property, child, childSlices, false);

                    if (childSlices.CountPayload > 0 && NeedDrawSeparator(child)){
                        separator.Draw(child, childSlices);
                    }
                }, 
                child => DrawChildWithDecorators(property, child, childSlices, true) 
            );
            if (childSlices.CountPayload > 0){
                slices.Add(childSlices);
            }
        }

        private void DrawChildWithDecorators(SerializedProperty parent, SerializedProperty child, Slices slices, bool isLast){
            space.Draw(child, slices);

            var childSlices = new SlicesImpl();
            DrawChild(parent, child, childSlices);
            if (NeedDrawHeader(parent, child)){
                header.Draw(child, childSlices);
            }
            slices.Add(childSlices);
        }

        private bool NeedDrawHeader(SerializedProperty parent, SerializedProperty child){
            bool parentIsRootArray = child.depth == RootDepth + 2 && parent.IsArrayElement();
            bool parentIsRootField = child.depth == RootDepth + 1;
            return parentIsRootArray || parentIsRootField;
        }

        private bool NeedDrawSeparator(SerializedProperty property){
            property = property.Copy();

            bool isArray = property.IsReallyArray();
            bool isComplex = property.CountChildrenAndMoveNext() > 1;

            bool nextHasAttribute = property.GetCustomAttribute<SeparatorAttribute>() != null;
            bool nextIsArray = property.IsReallyArray();
            bool nextIsComplex = property.CountChildrenAndMoveNext() > 1;
            
            return nextHasAttribute || 
                   isComplex || nextIsComplex || 
                   isArray || nextIsArray;
        }

        protected virtual void DrawChild(SerializedProperty parent, SerializedProperty child, Slices slices){
            getDrawer(child).AddSlices(child, slices);
        }
        
        #endregion

    }
}
