using System;
using UnityEngine;

namespace YanickSenn.Utils
{
    [CreateAssetMenu(fileName = "GlobalEvent", menuName = "Global Event")]
    public class GlobalEvent : ScriptableObject
    {
        [SerializeField] private bool enabled;
        [SerializeField, TextArea] private string description;
        
        public event Action<Metadata> OnTrigger;
        
        public void Invoke() => Invoke(payload: null, sender: null);
        public void Invoke(Sender sender) => Invoke(payload: null, sender: sender);
        public void Invoke(object payload) => Invoke(payload: payload, sender: null);
        
        public void Invoke(object payload, Sender sender) {
            if (!enabled) return;
            OnTrigger?.Invoke(new Metadata(
                new Optional<object>(payload),
                new Optional<Sender>(sender),
                Time.realtimeSinceStartup));
        }

        [Serializable]
        public class Sender {
            public GameObject GameObject { get; }
            
            public Sender(GameObject gameObject) {
                GameObject = gameObject;
            }
        }

        [Serializable]
        public struct Metadata {
            public Optional<object> Payload { get; }
            public Optional<Sender> Sender { get; }
            public float Timestamp { get; }

            public Metadata(Optional<object> payload, Optional<Sender> sender, float timestamp) {
                Payload = payload;
                Sender = sender;
                Timestamp = timestamp;
            }
        }
    }
}