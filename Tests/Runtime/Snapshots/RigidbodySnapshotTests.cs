using NUnit.Framework;
using UnityEngine;
using YanickSenn.Utils.Snapshots;

namespace YanickSenn.Utils.Tests.Snapshots
{
    public class RigidbodySnapshotTests
    {
        [Test]
        public void Snapshot_ShouldCaptureAndRestoreState()
        {
            var go = new GameObject();
            var rb = go.AddComponent<Rigidbody>();
            
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearDamping = 0.5f;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

            var snapshot = RigidbodySnapshot.From(rb);

            // Change state
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearDamping = 0f;
            rb.constraints = RigidbodyConstraints.None;
            rb.interpolation = RigidbodyInterpolation.None;
            // Note: changing collision detection mode might fail at runtime if not compatible with kinematic change, but here we just check values.
            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;

            snapshot.ApplyTo(rb);

            Assert.IsTrue(rb.isKinematic);
            Assert.IsFalse(rb.useGravity);
            Assert.AreEqual(0.5f, rb.linearDamping);
            Assert.AreEqual(RigidbodyConstraints.FreezeAll, rb.constraints);
            Assert.AreEqual(RigidbodyInterpolation.Interpolate, rb.interpolation);
            Assert.AreEqual(CollisionDetectionMode.Continuous, rb.collisionDetectionMode);
            
            Object.DestroyImmediate(go);
        }
    }
}
