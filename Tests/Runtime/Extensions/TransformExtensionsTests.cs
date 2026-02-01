using NUnit.Framework;
using UnityEngine;
using YanickSenn.Utils.Extensions;
using YanickSenn.Utils.Misc;

namespace YanickSenn.Utils.Tests.Extensions
{
    public class TransformExtensionsTests
    {
        [Test]
        public void LookAt_WithMask_XZ_ShouldIgnoreYDifference()
        {
            var source = new GameObject("Source").transform;
            source.position = Vector3.zero;

            var target = new Vector3(10f, 10f, 10f);
            var mask = Vector3Mask.XZ;

            source.LookAt(target, mask);

            var expectedForward = new Vector3(10f, 0f, 10f).normalized;

            // Allow for some floating point error
            Assert.That(source.forward.x, Is.EqualTo(expectedForward.x).Within(0.001f));
            Assert.That(source.forward.y, Is.EqualTo(expectedForward.y).Within(0.001f));
            Assert.That(source.forward.z, Is.EqualTo(expectedForward.z).Within(0.001f));

            Object.DestroyImmediate(source.gameObject);
        }

        [Test]
        public void LookAt_WithMask_Y_ShouldLookUp()
        {
            var source = new GameObject("Source").transform;
            source.position = Vector3.zero;

            var target = new Vector3(10f, 10f, 10f);
            var mask = Vector3Mask.Y;

            source.LookAt(target, mask);

            // Mask Y: x=source.x, y=target.y, z=source.z -> (0, 10, 0)
            // Looking at (0, 10, 0) from (0, 0, 0) -> Up
            var expectedForward = Vector3.up;

            Assert.That(source.forward.x, Is.EqualTo(expectedForward.x).Within(0.001f));
            Assert.That(source.forward.y, Is.EqualTo(expectedForward.y).Within(0.001f));
            Assert.That(source.forward.z, Is.EqualTo(expectedForward.z).Within(0.001f));

            Object.DestroyImmediate(source.gameObject);
        }

        [Test]
        public void LookAt_WithMask_None_ShouldNotRotate()
        {
            var source = new GameObject("Source").transform;
            source.position = Vector3.zero;
            source.rotation = Quaternion.identity;

            var target = new Vector3(10f, 10f, 10f);
            var mask = Vector3Mask.None;

            source.LookAt(target, mask);

            var expectedForward = Vector3.forward;

            Assert.That(source.forward.x, Is.EqualTo(expectedForward.x).Within(0.001f));
            Assert.That(source.forward.y, Is.EqualTo(expectedForward.y).Within(0.001f));
            Assert.That(source.forward.z, Is.EqualTo(expectedForward.z).Within(0.001f));

            Object.DestroyImmediate(source.gameObject);
        }
    }
}
