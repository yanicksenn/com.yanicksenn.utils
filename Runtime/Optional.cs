using System;
using UnityEngine;

namespace YanickSenn.Utils
{
    [Serializable]
    public struct Optional<T>
    {
        [SerializeField] private T value;
        
        public T Value => value;

        public bool IsPresent => value != null;
        public bool IsAbsent => value == null;
        
        public Optional(T value = default) {
            this.value = value;
        }

        public Optional<T> Filter(Func<T, bool> predicate) {
            if (IsAbsent) return this;
            return predicate.Invoke(value) ? this : new Optional<T>();
        }
        
        public Optional<V> Map<V>(Func<T, V> map) {
            if (IsAbsent) return new Optional<V>();
            var newValue = map.Invoke(value);
            return newValue == null
                ? new Optional<V>()
                : new Optional<V>(newValue);
        }

        public T OrElse(T other) {
            return IsAbsent ? other : Value;
        }
        
        public void DoIfPresent(Action<T> func) {
            if (IsPresent) func.Invoke(value);
        }

        public void DoIfAbsent(Action func) {
            if (IsAbsent) func.Invoke();
        }
    }
}