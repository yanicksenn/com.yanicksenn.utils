using System;
using UnityEngine;

namespace YanickSenn.Utils.Control
{
    [Serializable]
    public struct Observable<T> : IChangeEventEmitter<T> {
        public event Action<T, T> OnValueChanged;
        
        [SerializeField] private T value;
        public T Value {
            get => value;
            set {
                var oldValue = this.value;
                var newValue = value;
                this.value = value;
                MaybeTriggerValueChanged(newValue, oldValue);
            }
        }

        public Observable(T value = default, Action<T, T> onValueChanged = null) {
            this.value = value;
            OnValueChanged = onValueChanged;
        }

        public Optional<T> AsOptional() {
            return new Optional<T>(value);
        }

        private void MaybeTriggerValueChanged(T newValue, T oldValue) {
            if (newValue.Equals(oldValue)) {
                return;
            }
            OnValueChanged?.Invoke(newValue, oldValue);
        }
        
    }
}