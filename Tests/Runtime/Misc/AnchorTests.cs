using NUnit.Framework;
using UnityEngine;
using YanickSenn.Utils.Misc;

namespace YanickSenn.Utils.Tests.Misc
{
    public class AnchorTests
    {
        [Test]
        public void Anchor_CanBeAdded()
        {
            var go = new GameObject();
            var anchor = go.AddComponent<Anchor>();
            Assert.IsNotNull(anchor);
            Object.DestroyImmediate(go);
        }
    }
}
