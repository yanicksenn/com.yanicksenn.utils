using UnityEngine;
using YanickSenn.Utils.Misc;

namespace YanickSenn.Utils.Extensions {
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
        
        public static Vector3 GetNormal(this Vector3[] points) {
            //https://www.ilikebigbits.com/2015_03_04_plane_from_points.html
            if (points.Length < 3) {
                return Vector3.up;
            }

            var center = GetCenter(points);
            float xx = 0f, xy = 0f, xz = 0f, yy = 0f, yz = 0f, zz = 0f;

            foreach (var t in points) {
                var r = t - center;
                xx += r.x * r.x;
                xy += r.x * r.y;
                xz += r.x * r.z;
                yy += r.y * r.y;
                yz += r.y * r.z;
                zz += r.z * r.z;
            }

            var detX = yy * zz - yz * yz;
            var detY = xx * zz - xz * xz;
            var detZ = xx * yy - xy * xy;

            if (detX > detY && detX > detZ) {
                return new Vector3(detX, xz * yz - xy * zz, xy * yz - xz * yy).normalized;
            }

            return detY > detZ 
                ? new Vector3(xz * yz - xy * zz, detY, xy * xz - yz * xx).normalized 
                : new Vector3(xy * yz - xz * yy, xy * xz - yz * xx, detZ).normalized;
        }

        public static Vector3 GetCenter(this Vector3[] points) {
            var center = Vector3.zero;
            foreach (var t in points) {
                center += t / points.Length;
            }
            return center;
        }
    }
}