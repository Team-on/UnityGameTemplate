using System;
using UnityEngine;

namespace OneLine {
    ///<summary>
    /// Foolows object reference and draw it in the popup
    ///</summary>
    [AttributeUsage(validOn: AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class ExpandableAttribute : PropertyAttribute {

        public bool ReadOnly { get; private set; }

        public ExpandableAttribute(bool readOnly = false) {
            ReadOnly = readOnly;
        }

    }

    [AttributeUsage(validOn: AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class ReadOnlyExpandableAttribute : ExpandableAttribute {

        public ReadOnlyExpandableAttribute() :base(true) {
        }

    }

    
}
