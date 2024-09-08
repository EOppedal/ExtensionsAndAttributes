using UnityEngine;

namespace ExtensionMethods {
    public static class VectorExtensionMethods {
        #region ---Vector3---

        /// <summary>
        /// Returns the same vector plus the given x, y and/or z offset
        /// </summary>
        /// <param name="x">Add offset in the X-axis</param>
        /// <param name="y">Add offset in the Y-axis</param>
        /// <param name="z">Add offset in the Z-axis</param>
        public static Vector3 WithOffset(this Vector3 vector3, float x = 0, float y = 0, float z = 0) {
            return vector3 + new Vector3(x, y, z);
        }

        /// <summary>
        /// Returns the vector with values set
        /// </summary>
        /// <param name="x">Sets the value of the X-axis</param>
        /// <param name="y">Sets the value of the Y-axis</param>
        /// <param name="z">Sets the value of the Z-axis</param>
        public static Vector3 With(this ref Vector3 vector3, float? x = null, float? y = null, float? z = null) {
            if (x.HasValue) vector3.x = x.Value;
            if (y.HasValue) vector3.y = y.Value;
            if (z.HasValue) vector3.z = z.Value;
            return vector3;
        }
        
        public static void Set(this ref Vector3 vector3, float? x = null, float? y = null, float? z = null) {
            if (x.HasValue) vector3.x = x.Value;
            if (y.HasValue) vector3.y = y.Value;
            if (z.HasValue) vector3.z = z.Value;
        }

        #endregion

        #region ---Vector2---

        /// <summary>
        /// Returns the same vector plus the given x and/or y offset
        /// </summary>
        /// <param name="x">Add offset in the X-axis</param>
        /// <param name="y">Add offset in the Y-axis</param>
        public static Vector2 WithOffset(this Vector2 vector2, float x = 0, float y = 0) {
            return vector2 + new Vector2(x, y);
        }

        /// <summary>
        /// Returns the vector with values set
        /// </summary>
        /// <param name="x">Sets the value of the X-axis</param>
        /// <param name="y">Sets the value of the Y-axis</param>
        public static Vector2 With(this Vector2 vector2, float? x = null, float? y = null) {
            if (x.HasValue) vector2.x = x.Value;
            if (y.HasValue) vector2.y = y.Value;
            return vector2;
        }

        #endregion

        #region ---Quaternion---

        public static Quaternion WithOffset(this Quaternion quaternion, float x = 0, float y = 0, float z = 0,
            float w = 0) {
            quaternion.x += x;
            quaternion.y += y;
            quaternion.z += z;
            quaternion.w += w;
            return quaternion;
        }

        public static Quaternion With(this Quaternion quaternion, float? x = null, float? y = null, float? z = null,
            float? w = null) {
            if (x.HasValue) quaternion.x = x.Value;
            if (y.HasValue) quaternion.y = y.Value;
            if (z.HasValue) quaternion.z = z.Value;
            if (w.HasValue) quaternion.w = w.Value;
            return quaternion;
        }

        #endregion
    }
}