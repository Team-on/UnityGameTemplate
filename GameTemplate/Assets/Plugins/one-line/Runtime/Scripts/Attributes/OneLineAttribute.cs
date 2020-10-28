using System;
using UnityEngine;

namespace OneLine {
    ///<summary>
    ///Draws field into one line in InspectorWindow with  all nested fields (even arrays)
    ///which usual is presented in InspectorWindow in bulky weird view.
    ///
    ///Marked field is called ROOT FIELD.
    ///All internal fields in one scope are called NESTED FIELDS.
    ///
    ///Fields of classes, which have native representation in Inspector are called SIMPLE FIELDS.
    ///Fields, contains other fields are called COMPLEX FIELDS.
    ///</summary>
    [AttributeUsage(validOn: AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class OneLineAttribute : PropertyAttribute {
        public LineHeader Header { get; set; }
    }

    [AttributeUsage(validOn: AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class OneLineWithHeaderAttribute : OneLineAttribute {
        public OneLineWithHeaderAttribute() : base(){
            Header = LineHeader.Short;
        }
    }

    public enum LineHeader {
        None = 0,
        Short = 1
    }
}
