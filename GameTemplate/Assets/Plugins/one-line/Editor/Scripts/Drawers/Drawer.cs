using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using RectEx;

namespace OneLine {
    internal abstract class Drawer {

        protected SpaceDrawer space = new SpaceDrawer();
        protected SeparatorDrawer separator = new SeparatorDrawer();
        protected HeaderDrawer header = new HeaderDrawer();
        protected HighlightDrawer highlight = new HighlightDrawer();
        protected TooltipDrawer tooltip = new TooltipDrawer();

        public abstract void AddSlices(SerializedProperty property, Slices slices);

    }
}
