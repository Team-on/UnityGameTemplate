using System;
using UnityEngine;

namespace OneLine.Settings {
    [Serializable]
    public class TernaryBoolean : object{
        public static TernaryBoolean NULL { get { return new TernaryBoolean(NULL_VALUE); } }
        public static TernaryBoolean TRUE { get { return new TernaryBoolean(TRUE_VALUE); } }
        public static TernaryBoolean FALSE { get { return new TernaryBoolean(FALSE_VALUE); } }

        private const byte NULL_VALUE = 0;
        private const byte TRUE_VALUE = 1;
        private const byte FALSE_VALUE = 2;

        [SerializeField, Range(0, 2)]
        private byte value;

        public TernaryBoolean() {
            value = NULL_VALUE;
        }

        public TernaryBoolean(byte value) {
            this.value = value;
            assertValue();
        }

        public TernaryBoolean(Nullable<bool> value) {
            switch (value) {
                case null:
                    this.value = NULL_VALUE;
                    break;
                case true:
                    this.value = TRUE_VALUE;
                    break;
                case false:
                    this.value = FALSE_VALUE;
                    break;
            }
            assertValue();
        }

        private void assertValue() {
            if (value != NULL_VALUE && value != TRUE_VALUE && value != FALSE_VALUE) {
                throw new System.InvalidOperationException("Triple Boolean value contain wrong value");
            }
        }

        public byte RawValue {
            get {
                return value;
            }
        }

        public bool HasValue { 
            get {
                assertValue();
                return value != NULL_VALUE;
            }
        }

        public Nullable<bool> Value {
            get {
                assertValue();
                if (value == NULL_VALUE) {
                    return null;
                }
                else {
                    return value == TRUE_VALUE;
                }
            }
        }

        public bool BoolValue {
            get {
                assertValue();
                if (value == NULL_VALUE) {
                    throw new NullReferenceException();
                }
                else {
                    return value == TRUE_VALUE;
                }
            }
        }

        public void SwitchToNext() {
            value = (byte) ((value + 1) % 3);
        }

        public override string ToString() {
            assertValue();
            switch (value) {
                case NULL_VALUE:
                    return "NULL";
                case TRUE_VALUE:
                    return "TRUE";
                case FALSE_VALUE:
                    return "FALSE";
                default:
                    throw new InvalidOperationException();
            }
        }

        public static bool operator ==(TernaryBoolean left, TernaryBoolean right) {
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null)) {
                return ReferenceEquals(left, right);
            }
            return left.RawValue == right.RawValue;
        }

        public static bool operator !=(TernaryBoolean left, TernaryBoolean right) {
            return !(left == right);
        }

        public override bool Equals(object obj) {
            if (!(obj is TernaryBoolean)){
                return false;
            }
            return this == (obj as TernaryBoolean);
        }

        public override int GetHashCode(){
            return value;
        }

        public static TernaryBoolean operator !(TernaryBoolean value) {
            return new TernaryBoolean(!value.Value);
        }

        public static implicit operator bool(TernaryBoolean value) {
            return value == TRUE;
        }
    }
}