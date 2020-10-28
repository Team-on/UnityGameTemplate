using System;

namespace OneLine {

    ///<summary>
    ///Hides buttons "+" and "-" of array.
    ///Available only on NESTED FIELDS. Has no effect on ROOT FIELD.
    ///You can change length of this array by context-menu commands.
    ///</summary>
    [AttributeUsage(validOn: AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class HideButtonsAttribute : Attribute {

    }
}
