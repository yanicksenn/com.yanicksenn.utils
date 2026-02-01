using UnityEngine;
using YanickSenn.Utils.Misc;

namespace YanickSenn.Utils.Extensions
{
    public static class TransformExtensions
    {
        public static void LookAt(this Transform transform, Vector3 target, Vector3Mask mask)
        {
            transform.LookAt(target, mask, Vector3.up);
        }

        public static void LookAt(this Transform transform, Vector3 target, Vector3Mask mask, Vector3 worldUp)
        {
            var position = transform.position;
            var lookAtPosition = position.ApplyMask(target, mask);
            if (lookAtPosition == position)
            {
                return;
            }

            transform.LookAt(lookAtPosition, worldUp);
        }

        public static void LookAt(this Transform transform, Transform target, Vector3Mask mask)
        {
            transform.LookAt(target.position, mask, Vector3.up);
        }

        public static void LookAt(this Transform transform, Transform target, Vector3Mask mask, Vector3 worldUp)
        {
            transform.LookAt(target.position, mask, worldUp);
        }
    }
}
