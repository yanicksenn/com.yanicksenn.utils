using NUnit.Framework;
using YanickSenn.Utils.Misc;

namespace YanickSenn.Utils.Tests.Misc
{
    public class Vector3MaskTests
    {
        [Test]
        public void Constructor_ShouldSetValues()
        {
            var mask = new Vector3Mask(true, false, true);
            Assert.IsTrue(mask.x);
            Assert.IsFalse(mask.y);
            Assert.IsTrue(mask.z);
        }

        [Test]
        public void Invert_ShouldInvertValues()
        {
            var mask = new Vector3Mask(true, false, true);
            var inverted = mask.Invert();
            Assert.IsFalse(inverted.x);
            Assert.IsTrue(inverted.y);
            Assert.IsFalse(inverted.z);
        }

        [Test]
        public void Constants_ShouldBeCorrect()
        {
            Assert.IsTrue(Vector3Mask.X.x && !Vector3Mask.X.y && !Vector3Mask.X.z);
            Assert.IsTrue(Vector3Mask.All.x && Vector3Mask.All.y && Vector3Mask.All.z);
            Assert.IsTrue(!Vector3Mask.None.x && !Vector3Mask.None.y && !Vector3Mask.None.z);
        }
    }
}
