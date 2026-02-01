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
