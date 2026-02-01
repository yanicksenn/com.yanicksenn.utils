using UnityEngine;
using YanickSenn.Utils.Misc;

namespace YanickSenn.Utils.Extensions {
    public static class DirectionExtensions {

        public static Vector3 DirectionTo(this GameObject origin, GameObject target, Vector3Mask? mask = null) {
            return DirectionTo(origin.transform.position, target.transform.position, mask);
        }

        public static Vector3 DirectionTo(this GameObject origin, Component target, Vector3Mask? mask = null) {
            return DirectionTo(origin.transform.position, target.transform.position, mask);
        }

        public static Vector3 DirectionTo(this GameObject origin, Vector3 target, Vector3Mask? mask = null) {
            return DirectionTo(origin.transform.position, target, mask);
        }

        public static Vector3 DirectionTo(this Component origin, GameObject target, Vector3Mask? mask = null) {
            return DirectionTo(origin.transform.position, target.transform.position, mask);
        }

        public static Vector3 DirectionTo(this Component origin, Component target, Vector3Mask? mask = null) {
            return DirectionTo(origin.transform.position, target.transform.position, mask);
        }

        public static Vector3 DirectionTo(this Component origin, Vector3 target, Vector3Mask? mask = null) {
            return DirectionTo(origin.transform.position, target, mask);
        }

        public static Vector3 DirectionTo(this Vector3 origin, GameObject target, Vector3Mask? mask = null) {
            return DirectionTo(origin, target.transform.position, mask);
        }

        public static Vector3 DirectionTo(this Vector3 origin, Component target, Vector3Mask? mask = null) {
            return DirectionTo(origin, target.transform.position, mask);
        }

        public static Vector3 DirectionTo(this Vector3 origin, Vector3 target, Vector3Mask? mask = null) {
            var diff = target - origin;
            
            if (mask != null) {
                var m = mask.Value;
                diff = new Vector3(
                    m.x ? diff.x : 0f,
                    m.y ? diff.y : 0f,
                    m.z ? diff.z : 0f
                );
            }

            return diff.normalized;
        }
    }
}
