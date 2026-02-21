using NUnit.Framework;
using UnityEngine;
using YanickSenn.Utils.Variables;

namespace YanickSenn.Utils.Tests.Variables
{
    public class ReferenceTests
    {
        [Test]
        public void Value_UseConstant_ShouldReturnConstant()
        {
            var reference = new FloatReference(10f);
            // Default useVariable is false
            
            Assert.AreEqual(10f, reference.Value);
        }

        [Test]
        public void Value_UseVariable_ShouldReturnVariableValue()
        {
            var variable = ScriptableObject.CreateInstance<FloatVariable>();
            variable.Value = 20f;

            var reference = new FloatReference();
            
            SetPrivateField(reference, "useVariable", true);
            SetPrivateField(reference, "_variable", variable);

            Assert.AreEqual(20f, reference.Value);
        }

        [Test]
        public void Value_Set_ShouldTriggerOnValueChanged()
        {
            var reference = new FloatReference(10f);
            float newValue = 0;
            float oldValue = 0;
            reference.OnValueChanged += (n, o) => {
                newValue = n;
                oldValue = o;
            };

            reference.Value = 15f;

            Assert.AreEqual(15f, reference.Value);
            Assert.AreEqual(15f, newValue);
            Assert.AreEqual(10f, oldValue);
        }

        [Test]
        public void Value_SetWithVariable_ShouldTriggerOnValueChanged()
        {
            var variable = ScriptableObject.CreateInstance<FloatVariable>();
            variable.Value = 20f;
            var reference = new FloatReference();
            SetPrivateField(reference, "useVariable", true);
            SetPrivateField(reference, "_variable", variable);

            float newValue = 0;
            float oldValue = 0;
            reference.OnValueChanged += (n, o) => {
                newValue = n;
                oldValue = o;
            };

            reference.Value = 25f;

            Assert.AreEqual(25f, reference.Value);
            Assert.AreEqual(25f, variable.Value);
            Assert.AreEqual(25f, newValue);
            Assert.AreEqual(20f, oldValue);
        }

        private void SetPrivateField(object target, string fieldName, object value)
        {
            // Base type might have the field
            var type = target.GetType();
            System.Reflection.FieldInfo field = null;
            while (type != null && field == null) {
                field = type.GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                type = type.BaseType;
            }

            if (field != null)
            {
                field.SetValue(target, value);
            }
            else
            {
                Assert.Fail($"Field {fieldName} not found on {target.GetType()}");
            }
        }
    }
}
