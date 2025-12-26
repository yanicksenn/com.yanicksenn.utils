using System;
using UnityEngine;
using UnityEngine.Events;

namespace YanickSenn.Utils {
    public class GlobalEventListener : MonoBehaviour {
        [SerializeField] private GlobalEvent globalEvent;
        [SerializeField] private UnityEvent onTrigger;

        private void OnEnable() {
            globalEvent.OnTrigger += OnTrigger;
        }

        private void OnDisable() {
            globalEvent.OnTrigger -= OnTrigger;
        }

        private void OnTrigger(GlobalEvent.Metadata metadata) {
            onTrigger?.Invoke();
        }
    }
}