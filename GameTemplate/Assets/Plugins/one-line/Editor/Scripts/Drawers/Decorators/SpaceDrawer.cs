using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal class SpaceDrawer {

        public void Draw(SerializedProperty property, Slices slices){
            property.GetCustomAttribute<SpaceAttribute>()
                    .IfPresent(x => slices.Add(new SliceImpl(0, x.height, rect => {})));
        }

    }
}
