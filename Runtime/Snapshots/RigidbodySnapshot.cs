using UnityEngine;

namespace YanickSenn.Utils.Snapshots
{
    public struct RigidbodySnapshot {
        private bool _isKinematic;
        private bool _useGravity;
        private float _linearDamping;
        private RigidbodyConstraints _constraints;
        private RigidbodyInterpolation _interpolation;
        private CollisionDetectionMode _collisionDetectionMode;

        public static RigidbodySnapshot From(Rigidbody rigidbody) {
            return new RigidbodySnapshot() {
                _isKinematic = rigidbody.isKinematic,
                _useGravity = rigidbody.useGravity,
                _linearDamping = rigidbody.linearDamping,
                _constraints = rigidbody.constraints,
                _interpolation = rigidbody.interpolation,
                _collisionDetectionMode = rigidbody.collisionDetectionMode,
            };
        }

        public void ApplyTo(Rigidbody rigidbody) {
            rigidbody.isKinematic = _isKinematic;
            rigidbody.useGravity = _useGravity;
            rigidbody.linearDamping = _linearDamping;
            rigidbody.constraints = _constraints;
            rigidbody.interpolation = _interpolation;
            rigidbody.collisionDetectionMode = _collisionDetectionMode;
        }
    }
}