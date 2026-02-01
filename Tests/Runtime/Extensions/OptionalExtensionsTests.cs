using NUnit.Framework;
using UnityEngine;
using YanickSenn.Utils.Control;
using YanickSenn.Utils.Events;
using YanickSenn.Utils.Extensions;

namespace YanickSenn.Utils.Tests.Extensions
{
    public class OptionalExtensionsTests
    {
        [Test]
        public void InvokeIfPresent_ShouldInvokeEvent_WhenPresent()
        {
            var globalEvent = ScriptableObject.CreateInstance<GlobalEvent>();
            var optional = new Optional<GlobalEvent>(globalEvent);
            bool invoked = false;

            globalEvent.OnTrigger += (m) => invoked = true;

            optional.InvokeIfPresent();

            Assert.IsTrue(invoked);
        }

        [Test]
        public void InvokeIfPresent_ShouldNotCrash_WhenEmpty() {
            var optional = new Optional<GlobalEvent>();
            Assert.DoesNotThrow(() => optional.InvokeIfPresent());
        }
    }
}
