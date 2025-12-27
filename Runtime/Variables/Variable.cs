using System;
using UnityEngine;

namespace YanickSenn.Utils.Variables
{
    public abstract class Variable<T> : ScriptableObject where T : IComparable<T> {
        public event Action<T, T> OnValueChanged;
        
        [SerializeField, TextArea] private string description;
        [SerializeField] private T value;

        public T Value {
            get => value;
            set {
                var oldValue = this.value;
                if (oldValue.CompareTo(value) == 0) {
                    return;
                }
                this.value = value;
                OnValueChanged?.Invoke(this.value, oldValue);
            }
        }
    }
}