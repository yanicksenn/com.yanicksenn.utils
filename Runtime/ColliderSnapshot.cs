using UnityEngine;

namespace YanickSenn.Utils
{
    public struct ColliderSnapshot {
        private bool _enabled;
        private LayerMask _includeLayers;
        private LayerMask _excludeLayers;

        public static ColliderSnapshot From(Collider collider) {
            return new ColliderSnapshot() {
                _enabled = collider.enabled,
                _includeLayers = collider.includeLayers,
                _excludeLayers = collider.excludeLayers,
            };
        }

        public void ApplyTo(Collider collider) {
            collider.enabled = _enabled;
            collider.includeLayers = _includeLayers;
            collider.excludeLayers = _excludeLayers;
        }
    }
}