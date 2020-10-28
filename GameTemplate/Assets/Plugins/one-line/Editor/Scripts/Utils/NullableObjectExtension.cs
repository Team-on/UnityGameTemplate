using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    public static class NullableObjectExtension {
        public static R IfPresent<T, R> (this T obj, Func<T, R> a) where T:class {
            if (obj != null){
                return a(obj);
            }
            else {
                return default(R);
            }
        }

        public static void IfPresent<T> (this T obj, Action<T> a) where T:class {
            if (obj != null){
                a(obj);
            }
        }

        public static T OrElse<T> (this T obj, T defaultValue) where T:class {
            return obj != null ? obj : defaultValue;
        }
    }
}
