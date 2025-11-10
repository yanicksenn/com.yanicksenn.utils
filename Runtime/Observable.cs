using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace YanickSenn.Utils
{
    [Serializable]
    public struct Observable<T> {
        public event Action<T, T> OnValueChanged;
        
        [SerializeField] private T value;
        public T Value {
            get => value;
            set {
                var oldValue = this.value;
                var newValue = value;
                this.value = value;
                MaybeTriggerValueChanged(oldValue, newValue);
            }
        }

        public Observable(T value = default, Action<T, T> onValueChanged = null) {
            this.value = value;
            OnValueChanged = onValueChanged;
        }

        public Optional<T> AsOptional() {
            return new Optional<T>(value);
        }

        private void MaybeTriggerValueChanged(T oldValue, T newValue) {
            if (oldValue.Equals(newValue)) {
                return;
            }
            OnValueChanged?.Invoke(oldValue, newValue);
        }
        
    }
}