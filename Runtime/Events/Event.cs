using System;
using UnityEngine;
using YanickSenn.Utils.Variables;

namespace YanickSenn.Utils.Events
{
    public abstract class Event<T> : ScriptableObject {
        [SerializeField] private BoolReference disabled = new();
        [SerializeField] private BoolReference debug = new();
        [SerializeField, TextArea] private string description;

        public event Action<T> OnTrigger;

        public void Invoke(T payload) {
            if (debug.Value) {
                var stateString = disabled.Value ? "DISABLED" : "ENABLED";
                Debug.Log($"[{stateString}] {name} invoked - {payload}", this);
            }

            if (disabled.Value) {
                return;
            }

            OnTrigger?.Invoke(payload);
        }
    }
}
