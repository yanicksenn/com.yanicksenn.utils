using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using YanickSenn.Utils.Misc;

namespace YanickSenn.Utils.Tests.Misc
{
    public class SelfDestructTests
    {
        [UnityTest]
        public IEnumerator SelfDestruct_ShouldDestroyObject_AfterTime()
        {
            var go = new GameObject("Destructible");
            go.SetActive(false);
            var selfDestruct = go.AddComponent<SelfDestruct>();
            
            // Speed up test by setting low time via reflection
            SetPrivateField(selfDestruct, "autoDestroyAfter", 0.1f);
            
            go.SetActive(true);
            
            yield return new WaitForSeconds(0.5f);
            
            Assert.IsTrue(go == null);
        }

        private void SetPrivateField(object target, string fieldName, object value)
        {
            var field = target.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(target, value);
            }
        }
    }
}
