using System;
using System.Collections.Generic;

namespace YanickSenn.Utils.Extensions {
    public static class EnumerableExtensions {
        /// <summary>
        /// Flattens a sequence of sequences into a single sequence.
        /// </summary>
        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> source) {
            foreach (var inner in source) {
                if (inner == null) continue;
                foreach (var item in inner) {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Recursively flattens a sequence by applying a children selector.
        /// </summary>
        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> childrenSelector) {
            if (source == null) yield break;

            foreach (var element in source) {
                yield return element;

                var children = childrenSelector(element);
                if (children != null) {
                    foreach (var child in Flatten(children, childrenSelector)) {
                        yield return child;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the minimum value in a sequence or default if the sequence is empty.
        /// </summary>
        public static T MinOrDefault<T>(this IEnumerable<T> source) {
            if (source == null) return default;
            using var enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext()) return default;

            var min = enumerator.Current;
            var comparer = Comparer<T>.Default;
            while (enumerator.MoveNext()) {
                if (comparer.Compare(enumerator.Current, min) < 0) {
                    min = enumerator.Current;
                }
            }
            return min;
        }

        /// <summary>
        /// Returns the maximum value in a sequence or default if the sequence is empty.
        /// </summary>
        public static T MaxOrDefault<T>(this IEnumerable<T> source) {
            if (source == null) return default;
            using var enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext()) return default;

            var max = enumerator.Current;
            var comparer = Comparer<T>.Default;
            while (enumerator.MoveNext()) {
                if (comparer.Compare(enumerator.Current, max) > 0) {
                    max = enumerator.Current;
                }
            }
            return max;
        }

        /// <summary>
        /// Returns the element with the minimum key in a sequence.
        /// </summary>
        public static (TSource element, TKey key) MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            using var enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext()) throw new InvalidOperationException("Sequence contains no elements");

            var min = enumerator.Current;
            var minKey = keySelector(min);
            var comparer = Comparer<TKey>.Default;

            while (enumerator.MoveNext()) {
                var current = enumerator.Current;
                var currentKey = keySelector(current);
                if (comparer.Compare(currentKey, minKey) < 0) {
                    min = current;
                    minKey = currentKey;
                }
            }
            return (min, minKey);
        }

        /// <summary>
        /// Returns the element with the maximum key in a sequence.
        /// </summary>
        public static (TSource element, TKey key) MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            using var enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext()) throw new InvalidOperationException("Sequence contains no elements");

            var max = enumerator.Current;
            var maxKey = keySelector(max);
            var comparer = Comparer<TKey>.Default;

            while (enumerator.MoveNext()) {
                var current = enumerator.Current;
                var currentKey = keySelector(current);
                if (comparer.Compare(currentKey, maxKey) > 0) {
                    max = current;
                    maxKey = currentKey;
                }
            }
            return (max, maxKey);
        }

        /// <summary>
        /// Returns the element with the minimum key in a sequence or default if the sequence is empty.
        /// </summary>
        public static (TSource element, TKey key) MinByOrDefault<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) {
            if (source == null) return default;
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            using var enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext()) return default;

            var min = enumerator.Current;
            var minKey = keySelector(min);
            var comparer = Comparer<TKey>.Default;

            while (enumerator.MoveNext()) {
                var current = enumerator.Current;
                var currentKey = keySelector(current);
                if (comparer.Compare(currentKey, minKey) < 0) {
                    min = current;
                    minKey = currentKey;
                }
            }
            return (min, minKey);
        }

        /// <summary>
        /// Returns the element with the maximum key in a sequence or default if the sequence is empty.
        /// </summary>
        public static (TSource element, TKey key) MaxByOrDefault<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) {
            if (source == null) return default;
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            using var enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext()) return default;

            var max = enumerator.Current;
            var maxKey = keySelector(max);
            var comparer = Comparer<TKey>.Default;

            while (enumerator.MoveNext()) {
                var current = enumerator.Current;
                var currentKey = keySelector(current);
                if (comparer.Compare(currentKey, maxKey) > 0) {
                    max = current;
                    maxKey = currentKey;
                }
            }
            return (max, maxKey);
        }
    }
}
