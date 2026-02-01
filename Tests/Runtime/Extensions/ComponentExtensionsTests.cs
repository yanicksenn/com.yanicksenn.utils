using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using YanickSenn.Utils.Extensions;

namespace YanickSenn.Utils.Tests.Extensions
{
    public class ComponentExtensionsTests
    {
        [Test]
        public void ToOptional_ShouldReturnPresentOptional_WhenComponentExists()
        {
            var go = new GameObject();
            var component = go.AddComponent<BoxCollider>();
            var optional = component.ToOptional();

            Assert.IsTrue(optional.IsPresent);
            Assert.AreEqual(component, optional.Value);

            Object.DestroyImmediate(go);
        }

        [UnityTest]
        public IEnumerator Destroy_ShouldDestroyComponent()
        {
            var go = new GameObject();
            var component = go.AddComponent<BoxCollider>();
            
            component.Destroy();

            yield return null;
            
            Assert.IsTrue(component == null); // Unity overrides == null
            
            Object.DestroyImmediate(go);
        }

        [UnityTest]
        public IEnumerator DestroyGameObject_ShouldDestroyGameObject()
        {
            var go = new GameObject();
            var component = go.AddComponent<BoxCollider>();
            
            component.DestroyGameObject();

            yield return null;
            
            Assert.IsTrue(go == null);
        }
    }
}
