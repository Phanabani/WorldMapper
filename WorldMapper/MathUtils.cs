using System;
using System.Numerics;

namespace WorldMapper
{
    public class MathUtils
    {
        public static float Hypot(float a, float b)
        {
            return (float) Math.Sqrt((float) Math.Pow(a, 2) + (float) Math.Pow(b, 2));
        }

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
            // Source: https://github.com/dfelinto/blender/blob/c4ef90f5a0b1d05b16187eb6e32323defe6461c0/source/blender/blenlib/intern/math_rotation.c#L1714
            Vector3 eul1, eul2;
            var cy = Hypot(m.M11, m.M12);

            if (cy > 16.0f * float.Epsilon)
            {
                eul1.X = (float) Math.Atan2(m.M23, m.M33);
                eul1.Y = (float) Math.Atan2(-m.M13, cy);
                eul1.Z = (float) Math.Atan2(m.M12, m.M11);

                eul2.X = (float) Math.Atan2(-m.M23, -m.M33);
                eul2.Y = (float) Math.Atan2(-m.M13, -cy);
                eul2.Z = (float) Math.Atan2(-m.M12, -m.M11);
            }
            else
            {
                eul1.X = (float) Math.Atan2(-m.M32, m.M22);
                eul1.Y = (float) Math.Atan2(-m.M13, cy);
                eul1.Z = 0;

                // This is the only possible result, so return now
                return eul1;
            }

            // https://github.com/dfelinto/blender/blob/c4ef90f5a0b1d05b16187eb6e32323defe6461c0/source/blender/blenlib/intern/math_rotation.c#L1598
            // There is a parity issue we skipped since XYZ axis order (assumed)
            // is of even parity, otherwise we'd negate the angles

            // Source: https://github.com/dfelinto/blender/blob/c4ef90f5a0b1d05b16187eb6e32323defe6461c0/source/blender/blenlib/intern/math_rotation.c#L1761
            // Now pick the best angles set, which is just that with the lowest
            // absolute sum
            var d1 = Math.Abs(eul1.X) + Math.Abs(eul1.Y) + Math.Abs(eul1.Z);
            var d2 = Math.Abs(eul2.X) + Math.Abs(eul2.Y) + Math.Abs(eul2.Z);
            return (d1 < d2) ? eul1 : eul2;
        }

        public static Matrix4x4 EulerToMatrix(Vector3 euler)
        {
            double ci, cj, ch, si, sj, sh, cc, cs, sc, ss;

            ci = Math.Cos(euler.X);
            cj = Math.Cos(euler.Y);
            ch = Math.Cos(euler.Z);
            si = Math.Sin(euler.X);
            sj = Math.Sin(euler.Y);
            sh = Math.Sin(euler.Z);
            cc = ci * ch;
            cs = ci * sh;
            sc = si * ch;
            ss = si * sh;

            var m = new Matrix4x4
            {
                M11 = (float) (cj * ch),
                M12 = (float) (cj * sh),
                M13 = (float) -sj,
                M21 = (float) (sj * sc - cs),
                M22 = (float) (sj * ss + cc),
                M23 = (float) (cj * si),
                M31 = (float) (sj * cc + ss),
                M32 = (float) (sj * cs - sc),
                M33 = (float) (cj * ci),
                M44 = 1
            };

            return m;
        }
    }
}
