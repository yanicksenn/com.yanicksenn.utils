using UnityEngine;

namespace YanickSenn.Utils {
    public static class VectorUtils {
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