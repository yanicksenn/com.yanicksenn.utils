using NUnit.Framework;
using UnityEngine;
using YanickSenn.Utils.Extensions;
using YanickSenn.Utils.Misc;

namespace YanickSenn.Utils.Tests.Extensions
{
    public class Vector3ExtensionsTests
    {
        [Test]
        public void ToX_ShouldKeepOnlyX()
        {
            var v = new Vector3(1, 2, 3);
            Assert.AreEqual(new Vector3(1, 0, 0), v.ToX());
        }

        [Test]
        public void WithX_ShouldReplaceX()
        {
            var v = new Vector3(1, 2, 3);
            Assert.AreEqual(new Vector3(5, 2, 3), v.WithX(5));
        }

        [Test]
        public void ApplyMask_ShouldMixVectors()
        {
            var v1 = new Vector3(1, 1, 1);
            var v2 = new Vector3(2, 2, 2);
            var mask = new Vector3Mask(true, false, true); // X and Z from v2, Y from v1

            var result = v1.ApplyMask(v2, mask);
            Assert.AreEqual(new Vector3(2, 1, 2), result);
        }

        [Test]
        public void ApplyInvertedMask_ShouldMixVectorsOpposite()
        {
            var v1 = new Vector3(1, 1, 1);
            var v2 = new Vector3(2, 2, 2);
            var mask = new Vector3Mask(true, false, true); // X and Z masked -> Inverted means X and Z from v1, Y from v2

            var result = v1.ApplyInvertedMask(v2, mask);
            Assert.AreEqual(new Vector3(1, 2, 1), result);
        }

        [Test]
        public void GetCenter_ShouldReturnAverage()
        {
            var points = new[]
            {
                new Vector3(0, 0, 0),
                new Vector3(2, 0, 0),
                new Vector3(0, 2, 0)
            };
            
            // Sum = (2, 2, 0), Count = 3 -> (0.66, 0.66, 0)
            var center = points.GetCenter();
            Assert.IsTrue(Vector3.Distance(new Vector3(2f/3f, 2f/3f, 0), center) < 0.0001f);
        }

        [Test]
        public void GetNormal_ShouldReturnPlaneNormal()
        {
            var points = new[]
            {
                new Vector3(0, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(0, 0, 1)
            };

            // Plane XZ, Normal should be Up (0, 1, 0)
            var normal = points.GetNormal();
            
            // The algorithm might return up or down depending on winding order
            Assert.AreEqual(Vector3.up, normal);
        }
    }
}
