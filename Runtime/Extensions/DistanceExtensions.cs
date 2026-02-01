using UnityEngine;
using YanickSenn.Utils.Misc;

namespace YanickSenn.Utils.Extensions {
    public static class DistanceExtensions {

        public static float DistanceTo(this GameObject origin, GameObject target, Vector3Mask? mask = null) {
            return DistanceTo(origin.transform.position, target.transform.position, mask);
        }

        public static float DistanceTo(this GameObject origin, Component target, Vector3Mask? mask = null) {
            return DistanceTo(origin.transform.position, target.transform.position, mask);
        }

        public static float DistanceTo(this GameObject origin, Vector3 target, Vector3Mask? mask = null) {
            return DistanceTo(origin.transform.position, target, mask);
        }

        public static float DistanceTo(this Component origin, GameObject target, Vector3Mask? mask = null) {
            return DistanceTo(origin.transform.position, target.transform.position, mask);
        }

        public static float DistanceTo(this Component origin, Component target, Vector3Mask? mask = null) {
            return DistanceTo(origin.transform.position, target.transform.position, mask);
        }

        public static float DistanceTo(this Component origin, Vector3 target, Vector3Mask? mask = null) {
            return DistanceTo(origin.transform.position, target, mask);
        }

        public static float DistanceTo(this Vector3 origin, GameObject target, Vector3Mask? mask = null) {
            return DistanceTo(origin, target.transform.position, mask);
        }

        public static float DistanceTo(this Vector3 origin, Component target, Vector3Mask? mask = null) {
            return DistanceTo(origin, target.transform.position, mask);
        }

        public static float DistanceTo(this Vector3 origin, Vector3 target, Vector3Mask? mask = null) {
            if (mask == null) {
                return Vector3.Distance(origin, target);
            }

            var m = mask.Value;
            var dx = m.x ? origin.x - target.x : 0f;
            var dy = m.y ? origin.y - target.y : 0f;
            var dz = m.z ? origin.z - target.z : 0f;
            return Mathf.Sqrt(dx * dx + dy * dy + dz * dz);
        }
    }
}