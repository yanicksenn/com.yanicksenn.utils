using System;
using UnityEngine;
using YanickSenn.Utils.Variables;

namespace YanickSenn.Utils.Control {
    [Serializable]
    public class Cooldown {
        [SerializeField] private float duration;
        private float _remainingTime;

        public float Duration => duration;
        public float RemainingTime => _remainingTime;
        public bool IsReady => _remainingTime <= 0;
        public float Progress => Duration > 0 ? Mathf.Clamp01(1 - (_remainingTime / Duration)) : 1;

        public Cooldown(float duration = 0) {
            this.duration = duration;
            _remainingTime = 0;
        }

        public void Tick(float deltaTime) {
            if (_remainingTime > 0) {
                _remainingTime -= deltaTime;
            }
        }

        public bool Use() {
            if (!IsReady) {
                return false;
            }

            Restart();
            return true;
        }

        public void Restart() {
            _remainingTime = Duration;
        }

        public void Reset() {
            _remainingTime = 0;
        }
    }
}
