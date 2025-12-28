using System;
using UnityEngine;
using YanickSenn.Utils.Control;
using YanickSenn.Utils.RegistryGeneration;
using YanickSenn.Utils.Variables;

namespace YanickSenn.Utils.Events
{
    [CreateAssetMenu(fileName = "GlobalEvent", menuName = "Global Event")]
    [GenerateInjectionRegistry]
    public class GlobalEvent : ScriptableObject {
        [SerializeField] private BoolReference disabled = new();
        [SerializeField] private BoolReference debug = new();
        [SerializeField, TextArea] private string description;
        
        public event Action<Metadata> OnTrigger;
        
        public void Invoke() => Invoke(payload: null, sender: null);
        public void Invoke(Sender sender) => Invoke(payload: null, sender: sender);
        public void Invoke(object payload) => Invoke(payload: payload, sender: null);
        
        public void Invoke(object payload, Sender sender) {
            var metadata = new Metadata(
                new Optional<object>(payload),
                new Optional<Sender>(sender),
                Time.realtimeSinceStartup);

            if (debug.Value) {
                var stateString = disabled.Value ? "DISABLED" : "ENABLED";
                var senderString = metadata.Sender.IsPresent ? $"Sender: {metadata.Sender.Value.GameObject.name}" : "No Sender";
                var payloadString = metadata.Payload.IsPresent ? $"Payload: {metadata.Payload.Value}" : "No Payload";
                Debug.Log($"[{stateString}] {name} invoked - {senderString}, {payloadString}", this);
            }
            
            if (disabled.Value) {
                return;
            }
            
            OnTrigger?.Invoke(metadata);
        }

        [Serializable]
        public class Sender {
            public GameObject GameObject { get; }

            public static Sender Of(GameObject gameObject) {
                return new Sender(gameObject);
            }
            
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