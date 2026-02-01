using NUnit.Framework;
using YanickSenn.Utils.Control;

namespace YanickSenn.Utils.Editor.Tests.Control
{
    public class OptionalTests
    {
        [Test]
        public void Optional_WithReferenceType_Default_IsAbsent()
        {
            var optional = new Optional<string>();
            Assert.IsTrue(optional.IsAbsent);
            Assert.IsFalse(optional.IsPresent);
        }

        [Test]
        public void Optional_WithReferenceType_Value_IsPresent()
        {
            var optional = new Optional<string>("hello");
            Assert.IsFalse(optional.IsAbsent);
            Assert.IsTrue(optional.IsPresent);
            Assert.AreEqual("hello", optional.Value);
        }

        [Test]
        public void Optional_Filter_ReturnsMatching_IfPredicateTrue()
        {
            var optional = new Optional<string>("hello");
            var result = optional.Filter(s => s.StartsWith("h"));
            Assert.IsTrue(result.IsPresent);
            Assert.AreEqual("hello", result.Value);
        }

        [Test]
        public void Optional_Filter_ReturnsEmpty_IfPredicateFalse()
        {
            var optional = new Optional<string>("hello");
            var result = optional.Filter(s => s.StartsWith("z"));
            Assert.IsTrue(result.IsAbsent);
        }

        [Test]
        public void Optional_Map_TransformsValue()
        {
            var optional = new Optional<string>("123");
            var result = optional.Map(s => int.Parse(s));
            Assert.IsTrue(result.IsPresent);
            Assert.AreEqual(123, result.Value);
        }

        [Test]
        public void Optional_OrElse_ReturnsValue_IfPresent()
        {
            var optional = new Optional<string>("hello");
            Assert.AreEqual("hello", optional.OrElse("world"));
        }

        [Test]
        public void Optional_OrElse_ReturnsOther_IfAbsent()
        {
            var optional = new Optional<string>();
            Assert.AreEqual("world", optional.OrElse("world"));
        }

        [Test]
        public void Optional_WithValueType_Default_IsPresent()
        {
            // Current implementation note: value types are always "Present" because value != null is always true.
            var optional = new Optional<int>(); 
            Assert.IsTrue(optional.IsPresent);
        }
    }
}
