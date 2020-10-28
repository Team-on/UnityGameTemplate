using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;

namespace OneLine {
    internal static class IEnumerableExtension {

        public static IEnumerable<TResult> Merge<TFirst, TSecond, TResult> (this IEnumerable<TFirst> first,
                                                                            IEnumerable<TSecond> second,
                                                                            Func<TFirst, TSecond, TResult> selector){
            var firstEnumerator = first.GetEnumerator();
            var secondEnumerator = second.GetEnumerator();
            while (firstEnumerator.MoveNext() && secondEnumerator.MoveNext()){
                yield return selector(firstEnumerator.Current, secondEnumerator.Current);
            }
        }

        public static void ForEachMerged<TFirst, TSecond> (this IEnumerable<TFirst> first,
                                                   IEnumerable<TSecond> second,
                                                   Action<TFirst, TSecond> action){
            var firstEnumerator = first.GetEnumerator();
            var secondEnumerator = second.GetEnumerator();
            while (firstEnumerator.MoveNext() && secondEnumerator.MoveNext()){
                action(firstEnumerator.Current, secondEnumerator.Current);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action){
            foreach (var value in enumerable){
                action(value);
            }
        }

        public static void ForEachExceptLast<T> (this IEnumerable<T> enumerable, Action<T> action, Action<T> lastAction = null){
            var enumerator = enumerable.GetEnumerator();
            var has = enumerator.MoveNext();
            while (has){
                var current = enumerator.Current;
                has = enumerator.MoveNext();
                if (has) {
                    action(current);
                }
                else if (lastAction != null) {
                    lastAction(current);
                }
            }
        }

    }
}
