using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using YanickSenn.Utils.Misc;

namespace YanickSenn.Utils.Tests.Misc
{
    public class FollowerTests
    {
        [UnityTest]
        public IEnumerator Follower_ShouldFollowAnchor()
        {
            var anchorGo = new GameObject("Anchor");
            var anchor = anchorGo.AddComponent<Anchor>();
            anchorGo.transform.position = Vector3.zero;

            var followerGo = new GameObject("Follower");
            var follower = followerGo.AddComponent<Follower>();
            
            // Set private fields via reflection or specialized test helper if needed. 
            // Since fields are private [SerializeField], we can use SerializedObject in Editor tests, 
            // but this is Runtime. We might need to expose them or rely on JsonUtility to set them?
            // Or just making them public for testability / internal. 
            // For now, let's try to find the fields via reflection to set them up, 
            // as this is a common pattern for "just works" components.
            
            SetPrivateField(follower, "anchor", anchor);
            SetPrivateField(follower, "offset", new Vector3(1, 1, 1));
            
            // Wait for LateUpdate
            yield return null;
            
            Assert.AreEqual(new Vector3(1, 1, 1), followerGo.transform.position);

            anchorGo.transform.position = new Vector3(10, 0, 0);
            
            yield return null;
            
            Assert.AreEqual(new Vector3(11, 1, 1), followerGo.transform.position);

            Object.Destroy(anchorGo);
            Object.Destroy(followerGo);
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
