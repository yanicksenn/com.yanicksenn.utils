using NUnit.Framework;
using UnityEngine;
using YanickSenn.Utils.Misc;

namespace YanickSenn.Utils.Tests.Misc
{
    public class Vector3PidTests
    {
        [Test]
        public void Calculate_ShouldProduceOutput()
        {
            var pid = new Vector3Pid(1f, 0f, 0f, new Vector3Pid.Unclamped());
            var current = Vector3.zero;
            var target = new Vector3(10, 0, 0);
            
            pid.Calculate(current, target, 1f);
            
            // P = 1 * 10 = 10. I=0, D=0 (first frame previous error is same)
            Assert.AreEqual(new Vector3(10, 0, 0), pid.Value);
        }

        [Test]
        public void Calculate_Clamped_ShouldLimitMagnitude()
        {
            var pid = new Vector3Pid(10f, 0f, 0f, new Vector3Pid.Clamped(5f));
            var current = Vector3.zero;
            var target = new Vector3(10, 0, 0);

            pid.Calculate(current, target, 1f);
            
            // P = 10 * 10 = 100. Clamped to 5.
            Assert.AreEqual(5f, pid.Value.magnitude, 0.001f);
        }

        [Test]
        public void Calculate_WithDerivative_ShouldDamp()
        {
            // High P, High D
            var pid = new Vector3Pid(1f, 0f, 1f, new Vector3Pid.Unclamped());
            
            // Frame 1
            pid.Calculate(Vector3.zero, new Vector3(10, 0, 0), 1f);
            // Error = 10. Prev = 10. Deriv = 0.
            // Out = 10.

            // Frame 2: Moved closer
            pid.Calculate(new Vector3(5, 0, 0), new Vector3(10, 0, 0), 1f);
            // Error = 5. Prev = 10. Deriv = (5 - 10)/1 = -5.
            // P = 5. D = 1 * -5 = -5.
            // Out = 5 - 5 = 0. (Damping stopped it exactly here)
            
            Assert.AreEqual(Vector3.zero, pid.Value);
        }
    }
}
