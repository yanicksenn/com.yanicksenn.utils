using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using YanickSenn.Utils.Extensions;

namespace YanickSenn.Utils.Tests.Extensions
{
    public class GameObjectExtensionsTests
    {
        [UnityTest]
        public IEnumerator Destroy_ShouldDestroyGameObject()
        {
            var go = new GameObject();
            go.Destroy();
            yield return null;
            Assert.IsTrue(go == null);
        }
    }
}
