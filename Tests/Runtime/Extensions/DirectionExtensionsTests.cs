using NUnit.Framework;
using UnityEngine;
using YanickSenn.Utils.Extensions;
using YanickSenn.Utils.Misc;

namespace YanickSenn.Utils.Tests.Extensions
{
    public class DirectionExtensionsTests
    {
        [Test]
        public void DirectionTo_Vector3_ShouldReturnNormalizedDirection()
        {
            var origin = Vector3.zero;
            var target = new Vector3(10, 0, 0);
            
            var direction = origin.DirectionTo(target);
            
            Assert.AreEqual(new Vector3(1, 0, 0), direction);
        }

        [Test]
        public void DirectionTo_Vector3_WithMask_ShouldIgnoreMaskedAxes()
        {
            var origin = Vector3.zero;
            var target = new Vector3(10, 10, 10);
            var mask = Vector3Mask.X; // Only X

            var direction = origin.DirectionTo(target, mask);
            
            Assert.AreEqual(new Vector3(1, 0, 0), direction);
        }

        [Test]
        public void DirectionTo_GameObject_ShouldReturnDirection()
        {
            var originGo = new GameObject("Origin");
            originGo.transform.position = Vector3.zero;
            var targetGo = new GameObject("Target");
            targetGo.transform.position = new Vector3(0, 5, 0);

            var direction = originGo.DirectionTo(targetGo);

            Assert.AreEqual(new Vector3(0, 1, 0), direction);

            Object.DestroyImmediate(originGo);
            Object.DestroyImmediate(targetGo);
        }
    }
}
