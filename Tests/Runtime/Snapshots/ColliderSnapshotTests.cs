using NUnit.Framework;
using UnityEngine;
using YanickSenn.Utils.Snapshots;

namespace YanickSenn.Utils.Tests.Snapshots
{
    public class ColliderSnapshotTests
    {
        [Test]
        public void Snapshot_ShouldCaptureAndRestoreState()
        {
            var go = new GameObject();
            var collider = go.AddComponent<BoxCollider>();
            
            collider.enabled = true;
            collider.includeLayers = LayerMask.GetMask("Default");
            collider.excludeLayers = LayerMask.GetMask("TransparentFX");

            var snapshot = ColliderSnapshot.From(collider);

            // Change state
            collider.enabled = false;
            collider.includeLayers = 0;
            collider.excludeLayers = 0;

            snapshot.ApplyTo(collider);

            Assert.IsTrue(collider.enabled);
            Assert.AreEqual(LayerMask.GetMask("Default"), collider.includeLayers.value);
            Assert.AreEqual(LayerMask.GetMask("TransparentFX"), collider.excludeLayers.value);
            
            Object.DestroyImmediate(go);
        }
    }
}
