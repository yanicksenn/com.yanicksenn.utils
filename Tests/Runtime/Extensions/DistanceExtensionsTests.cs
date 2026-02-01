using NUnit.Framework;
using UnityEngine;
using YanickSenn.Utils.Extensions;
using YanickSenn.Utils.Misc;

namespace YanickSenn.Utils.Tests.Extensions
{
    public class DistanceExtensionsTests
    {
        [Test]
        public void DistanceTo_Vector3_ShouldReturnDistance()
        {
            var origin = Vector3.zero;
            var target = new Vector3(3, 4, 0); // 3-4-5 triangle

            var distance = origin.DistanceTo(target);

            Assert.AreEqual(5f, distance);
        }

        [Test]
        public void DistanceTo_Vector3_WithMask_ShouldCalculateProjectedDistance()
        {
            var origin = Vector3.zero;
            var target = new Vector3(3, 4, 10);
            var mask = Vector3Mask.XY; // Ignore Z difference

            var distance = origin.DistanceTo(target, mask);

            Assert.AreEqual(5f, distance); // Sqrt(3^2 + 4^2) = 5
        }
        
        [Test]
        public void DistanceTo_GameObject_ShouldReturnDistance()
        {
            var origin = new GameObject();
            origin.transform.position = Vector3.zero;
            var target = new GameObject();
            target.transform.position = new Vector3(10, 0, 0);
            
            Assert.AreEqual(10f, origin.DistanceTo(target));
            
            Object.DestroyImmediate(origin);
            Object.DestroyImmediate(target);
        }
    }
}
