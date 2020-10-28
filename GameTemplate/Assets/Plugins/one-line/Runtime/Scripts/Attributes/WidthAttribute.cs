using System;

namespace OneLine {
    ///<summary>
    ///Defines additive fixed width (in GUI units) on marked field in the line.
    ///By default sets weight of field to 0.
    ///Available only on SIMPLE NESTED FIELDS. Has no effect on ROOT FIELD.
    ///If field is marked by [WidthAttribute] and [WeightAttribute], it will get both effects additively.
    ///Applied to nested arrays defines width of each element.
    ///</summary>
    [AttributeUsageAttribute(validOn: AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class WidthAttribute : WeightAttribute {
        private float width;

        public WidthAttribute(float width) : base(0) {
            this.width = width;
        }

        public float Width { get { return width; } }

    }
}
