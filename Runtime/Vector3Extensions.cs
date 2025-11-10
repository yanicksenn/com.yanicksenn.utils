using UnityEngine;

namespace YanickSenn.Utils {
    public static class Vector3Extensions {
        
        public static Vector3 ToX(this Vector3 vector) {
            return new Vector3(vector.x, 0f, 0f);
        }
        
        public static Vector3 ToY(this Vector3 vector) {
            return new Vector3(0f, vector.y, 0f);
        }
        
        public static Vector3 ToZ(this Vector3 vector) {
            return new Vector3(0f, 0f, vector.z);
        }

        public static Vector3 ToXY(this Vector3 vector) {
            return new Vector3(vector.x, vector.y, 0f);
        }
        
        public static Vector3 ToXZ(this Vector3 vector) {
            return new Vector3(vector.x, 0f, vector.z);
        }
        
        public static Vector3 ToYZ(this Vector3 vector) {
            return new Vector3(0f, vector.y, vector.z);
        }

        public static Vector3 WithX(this Vector3 vector, float x) {
            return new Vector3(x, vector.y, vector.z);
        }

        public static Vector3 WithY(this Vector3 vector, float y) {
            return new Vector3(vector.x, y, vector.z);
        }

        public static Vector3 WithZ(this Vector3 vector, float z) {
            return new Vector3(vector.x, vector.y, z);
        }
        
        public static Vector3 WithXFrom(this Vector3 vector, Vector3 other) {
            return vector.ApplyMask(other, Vector3Mask.X);
        }
        
        public static Vector3 WithYFrom(this Vector3 vector, Vector3 other) {
            return vector.ApplyMask(other, Vector3Mask.Y);
        }
        
        public static Vector3 WithZFrom(this Vector3 vector, Vector3 other) {
            return vector.ApplyMask(other, Vector3Mask.Z);
        }

        public static Vector3 ApplyMask(this Vector3 vector, Vector3 other, Vector3Mask mask) {
            return new Vector3(
                mask.x ? other.x : vector.x,
                mask.y ? other.y : vector.y,
                mask.z ? other.z : vector.z
            );
        }

        public static Vector3 ApplyInvertedMask(this Vector3 vector, Vector3 other, Vector3Mask mask) {
            return new Vector3(
                !mask.x ? other.x : vector.x,
                !mask.y ? other.y : vector.y,
                !mask.z ? other.z : vector.z
            );
        }
    }
}