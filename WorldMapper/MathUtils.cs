using System;
using System.Numerics;

namespace WorldMapper
{
    public class MathUtils
    {
        public static float[] MatrixToArray(Matrix4x4 m)
        {
            return new[]
            {
                m.M11, m.M12, m.M13, m.M14,
                m.M21, m.M22, m.M23, m.M24,
                m.M31, m.M32, m.M33, m.M34,
                m.M41, m.M42, m.M43, m.M44,
            };
        }

        public static Vector3 MatrixToEuler(Matrix4x4 m)
        {
            var euler = new Vector3();
            euler.X = (float) Math.Atan2(m.M32, m.M33);
            var sqrt_32_33 = (float) Math.Sqrt(
                Math.Pow(m.M32, 2) + Math.Pow(m.M33, 2)
            );
            euler.Y = (float) Math.Atan2(-m.M31, sqrt_32_33);
            euler.Z = (float) Math.Atan2(m.M21, m.M11);
            return euler;
        }

        public static Matrix4x4 MatrixZUpToYUp(Matrix4x4 m)
        {
            // return new Matrix4x4(
            //      m.M11,   m.M13, -m.M12,  m.M14,
            //      m.M31,   m.M33, -m.M32,  m.M34,
            //     -m.M21,  -m.M23,  m.M22, -m.M24,
            //      m.M41,   m.M43, -m.M42,  m.M44
            // );

            // first switch the second and third row
            // then switch the second and third column
            // multiply the values in the third row by -1
            // multiply the values in the third column by -1
            // finally, we matrix multiply by a 90deg rotation about the Y axis
            // 0  0 -1  0
            // 0  1  0  0
            // 1  0  0  0
            // 0  0  0  1
            return
                Matrix4x4.CreateRotationY((float)-Math.PI/2)
                * new Matrix4x4(
                    m.M11,   m.M13, -m.M12,  m.M14,
                    m.M31,   m.M33, -m.M32,  m.M34,
                   -m.M21,  -m.M23,  m.M22, -m.M24,
                    m.M41,   m.M43, -m.M42,  m.M44
                );
        }
    }
}
