using System;
using UnityEngine;

namespace YanickSenn.Utils.Control {
    [Serializable]
    public class Cooldown {
        [SerializeField] private float duration;
        private float _nextReadyTime;

        public float Duration => duration;
        public float RemainingTime => Mathf.Max(0, _nextReadyTime - Time.time);
        public bool IsReady => Time.time >= _nextReadyTime;
        public float Progress => duration > 0 ? Mathf.Clamp01(1 - (RemainingTime / duration)) : 1;

        public Cooldown(float duration = 0) {
            this.duration = duration;
            _nextReadyTime = 0;
        }

        public bool Use() {
            if (!IsReady) {
                return false;
            }

            Restart();
            return true;
        }

        public void Restart() {
            _nextReadyTime = Time.time + duration;
        }

        public void Reset() {
            _nextReadyTime = 0;
        }
    }
}