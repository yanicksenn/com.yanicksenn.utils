using UnityEngine;
using Util;

namespace YanickSenn.Utils
{
    [DisallowMultipleComponent]
    public class Follower : MonoBehaviour
    {
        [SerializeField] private Anchor anchor;
        [SerializeField] private Vector3 offset;
        [SerializeField] private Vector3Mask mask = Vector3Mask.None;

        private void LateUpdate() {
            if (anchor == null) {
                return;
            }

            var newPos = anchor.transform.position + offset;
            transform.position = newPos.ApplyInvertedMask(transform.position, mask);
        }
    }
}