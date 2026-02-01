using NUnit.Framework;
using UnityEngine;
using YanickSenn.Utils.Variables;

namespace YanickSenn.Utils.Tests.Variables
{
    public class VariableTests
    {
        [Test]
        public void Value_Set_ShouldInvokeOnValueChanged()
        {
            var variable = ScriptableObject.CreateInstance<FloatVariable>();
            variable.Value = 0f;

            bool invoked = false;
            float capturedOld = -1f;
            float capturedNew = -1f;

            variable.OnValueChanged += (n, o) => {
                invoked = true;
                capturedNew = n;
                capturedOld = o;
            };

            variable.Value = 10f;

            Assert.IsTrue(invoked);
            Assert.AreEqual(10f, variable.Value);
            Assert.AreEqual(10f, capturedNew);
            Assert.AreEqual(0f, capturedOld);
        }

        [Test]
        public void Value_SetSame_ShouldNotInvokeOnValueChanged()
        {
            var variable = ScriptableObject.CreateInstance<FloatVariable>();
            variable.Value = 5f;

            bool invoked = false;
            variable.OnValueChanged += (n, o) => invoked = true;

            variable.Value = 5f;

            Assert.IsFalse(invoked);
        }
    }
}
