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
    }
}