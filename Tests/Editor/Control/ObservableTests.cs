using NUnit.Framework;
using YanickSenn.Utils.Control;

namespace YanickSenn.Utils.Editor.Tests.Control
{
    public class ObservableTests
    {
        [Test]
        public void Observable_ValueChange_TriggersEvent()
        {
            bool triggered = false;
            var observable = new Observable<int>(0);
            observable.OnValueChanged += (newVal, oldVal) => {
                triggered = true;
                Assert.AreEqual(1, newVal);
                Assert.AreEqual(0, oldVal);
            };

            observable.Value = 1;
            Assert.IsTrue(triggered);
        }

        [Test]
        public void Observable_SameValue_DoesNotTriggerEvent()
        {
            bool triggered = false;
            var observable = new Observable<int>(1);
            observable.OnValueChanged += (newVal, oldVal) => triggered = true;

            observable.Value = 1;
            Assert.IsFalse(triggered);
        }
        
        [Test]
        public void Observable_AsOptional_ReturnsOptional()
        {
             var observable = new Observable<string>("test");
             var optional = observable.AsOptional();
             Assert.IsTrue(optional.IsPresent);
             Assert.AreEqual("test", optional.Value);
        }
    }
}
