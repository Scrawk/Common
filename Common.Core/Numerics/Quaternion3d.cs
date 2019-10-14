using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using Common.Core.Numerics;

namespace Common.Core.Numerics
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Quaternion3d : IEquatable<Quaternion3d>
    {

        public double x, y, z, w;

        public readonly static Quaternion3d Identity = new Quaternion3d(0, 0, 0, 1);

        public readonly static Quaternion3d Zero = new Quaternion3d(0, 0, 0, 0);

        /// <summary>
        /// A Quaternion from varibles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Quaternion3d(double x, double y, double z, double w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        /// <summary>
        /// A Quaternion from a vector axis and angle.
        /// The axis is the up direction and the angle is the rotation.
        /// </summary>
        public Quaternion3d(Vector3d axis, double angle)
        {
            Vector3d axisN = axis.Normalized;
            double a = angle * 0.5f;
            double sina = Math.Sin(a);
            double cosa = Math.Cos(a);
            x = axisN.x * sina;
            y = axisN.y * sina;
            z = axisN.z * sina;
            w = cosa;
        }

        /// <summary>
        /// A quaternion with the rotation required to
        /// rotation from the from direction to the to direction.
        /// </summary>
        public Quaternion3d(Vector3d to, Vector3d from)
        {
            Vector3d f = from.Normalized;
            Vector3d t = to.Normalized;

            double dotProdPlus1 = 1.0 + Vector3d.Dot(f, t);

            if (dotProdPlus1 < DMath.EPS)
            {
                w = 0;
                if (Math.Abs(f.x) < 0.6f)
                {
                    double norm = Math.Sqrt(1 - f.x * f.x);
                    x = 0;
                    y = f.z / norm;
                    z = -f.y / norm;
                }
                else if (Math.Abs(f.y) < 0.6f)
                {
                    double norm = Math.Sqrt(1 - f.y * f.y);
                    x = -f.z / norm;
                    y = 0;
                    z = f.x / norm;
                }
                else
                {
                    double norm = Math.Sqrt(1 - f.z * f.z);
                    x = f.y / norm;
                    y = -f.x / norm;
                    z = 0;
                }
            }
            else
            {
                double s = Math.Sqrt(0.5f * dotProdPlus1);
                Vector3d tmp = (Vector3d.Cross(f, t)) / (2.0 * s);
                x = tmp.x;
                y = tmp.y;
                z = tmp.z;
                w = s;
            }
        }

        /// <summary>
        /// Returns the conjugate of a quaternion value.
        /// </summary>
        public Quaternion3d Conjugate
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new Quaternion3d(-x, -y, -z, w);
            }
        }

        /// <summary>
        /// The inverse of the quaternion.
        /// </summary>
        public Quaternion3d Inverse
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                double im = DMath.SafeInv(SqrMagnitude);
                return new Quaternion3d(im * -x, im * -y, im * -z, im * w);
            }
        }

        /// <summary>
        /// The length of the quaternion.
        /// </summary>
        double Magnitude
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return DMath.SafeSqrt(SqrMagnitude);
            }
        }

        /// <summary>
        /// The sqr length of the quaternion.
        /// </summary>
        double SqrMagnitude
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return x * x + y * y + z * z + w * w;
            }
        }

        /// <summary>
        /// The a normalized quaternion.
        /// </summary>
        public Quaternion3d Normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                double inv = DMath.SafeInv(Magnitude);
                return new Quaternion3d(x * inv, y * inv, z * inv, w * inv);
            }
        }

        /// <summary>
        /// Multiply two quternions together.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion3d operator *(Quaternion3d q1, Quaternion3d q2)
        {
            return new Quaternion3d(q2.w * q1.x + q2.x * q1.w + q2.y * q1.z - q2.z * q1.y,
                                    q2.w * q1.y - q2.x * q1.z + q2.y * q1.w + q2.z * q1.x,
                                    q2.w * q1.z + q2.x * q1.y - q2.y * q1.x + q2.z * q1.w,
                                    q2.w * q1.w - q2.x * q1.x - q2.y * q1.y - q2.z * q1.z);
        }

        /// <summary>
        /// Multiply a quaternion and a vector together.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator *(Quaternion3d q, Vector3d v)
        {
            Vector3d xyz = new Vector3d(q.x, q.y, q.z);
            Vector3d t = 2 * Vector3d.Cross(xyz, v);
            return v + q.w * t + Vector3d.Cross(xyz, t);
        }

        /// <summary>
        /// Multiply a quaternion and a vector together.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator *(Vector3d v, Quaternion3d q)
        {
            Vector3d xyz = new Vector3d(q.x, q.y, q.z);
            Vector3d t = 2 * Vector3d.Cross(xyz, v);
            return v + q.w * t + Vector3d.Cross(xyz, t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Quaternion3d(Quaternion3f q)
        {
            return new Quaternion3d(q.x, q.y, q.z, q.w);
        }

        /// <summary>
        /// Are these Quaternions equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Quaternion3d v1, Quaternion3d v2)
        {
            return (v1.x == v2.x && v1.y == v2.y && v1.z == v2.z && v1.w == v2.w);
        }

        /// <summary>
        /// Are these Quaternions not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Quaternion3d v1, Quaternion3d v2)
        {
            return (v1.x != v2.x || v1.y != v2.y || v1.z != v2.z || v1.w != v2.w);
        }

        /// <summary>
        /// Are these Quaternions equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Quaternion3d q)
        {
            return this == q;
        }

        /// <summary>
        /// Are these Quaternions equal.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is Quaternion3d)) return false;
            Quaternion3d v = (Quaternion3d)obj;
            return this == v;
        }

        /// <summary>
        /// Quaternions hash code. 
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ x.GetHashCode();
                hash = (hash * 16777619) ^ y.GetHashCode();
                hash = (hash * 16777619) ^ z.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Quaternion as a string.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3}", x, y, z, w);
        }

        /// <summary>
        /// Quaternion as a string.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(string f)
        {
            return string.Format("{0},{1},{2},{3}", x.ToString(f), y.ToString(f), z.ToString(f), w.ToString(f));
        }

        /// <summary>
        /// Quaternion from a string.
        /// </summary>
        public static Quaternion3d FromString(string s)
        {
            Quaternion3d q = new Quaternion3d();
            try
            {
                string[] separators = new string[] { "," };
                string[] result = s.Split(separators, StringSplitOptions.None);

                q.x = double.Parse(result[0]);
                q.y = double.Parse(result[1]);
                q.z = double.Parse(result[2]);
                q.w = double.Parse(result[3]);
            }
            catch { }

            return q;
        }

        /// <summary>
        /// Convert to a single precision 3 dimension matrix.
        /// </summary>
        public Matrix3x3d ToMatrix3x3d()
        {
            double xx = x * x,
                    xy = x * y,
                    xz = x * z,
                    xw = x * w,
                    yy = y * y,
                    yz = y * z,
                    yw = y * w,
                    zz = z * z,
                    zw = z * w;

            return new Matrix3x3d
            (
                1.0 - 2.0 * (yy + zz), 2.0 * (xy - zw), 2.0 * (xz + yw),
                2.0 * (xy + zw), 1.0 - 2.0 * (xx + zz), 2.0 * (yz - xw),
                2.0 * (xz - yw), 2.0 * (yz + xw), 1.0 - 2.0 * (xx + yy)
            );
        }

        /// <summary>
        /// Convert to a single precision 4 dimension matrix.
        /// </summary>
        public Matrix4x4d ToMatrix4x4d()
        {
            double xx = x * x,
                    xy = x * y,
                    xz = x * z,
                    xw = x * w,
                    yy = y * y,
                    yz = y * z,
                    yw = y * w,
                    zz = z * z,
                    zw = z * w;

            return new Matrix4x4d
            (
                1.0 - 2.0 * (yy + zz), 2.0 * (xy - zw), 2.0 * (xz + yw), 0.0,
                2.0 * (xy + zw), 1.0 - 2.0 * (xx + zz), 2.0 * (yz - xw), 0.0,
                2.0 * (xz - yw), 2.0 * (yz + xw), 1.0 - 2.0 * (xx + yy), 0.0,
                0.0, 0.0, 0.0, 1.0
            );
        }

        /// <summary>
        /// The normalize the quaternion.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            double invLength = DMath.SafeInv(Magnitude);
            x *= invLength;
            y *= invLength;
            z *= invLength;
            w *= invLength;
        }

        /// <summary>
        /// The dot product of two quaternion..
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Dot(Quaternion3d q0, Quaternion3d q1)
        {
            return q0.x * q1.x + q0.y * q1.y + q0.z * q1.z + q0.w * q1.w;
        }

        /// <summary>
        /// Slerp the quaternion from the from rotation to the to rotation by t.
        /// </summary>
		public static Quaternion3d Slerp(Quaternion3d from, Quaternion3d to, double t)
        {
            if (t <= 0.0)
            {
                return from;
            }
            else if (t >= 1.0)
            {
                return to;
            }
            else
            {
                double cosom = from.x * to.x + from.y * to.y + from.z * to.z + from.w * to.w;
                double absCosom = Math.Abs(cosom);

                double scale0;
                double scale1;

                if ((1 - absCosom) > DMath.EPS)
                {
                    double omega = DMath.SafeAcos(absCosom);
                    double sinom = 1.0 / Math.Sin(omega);
                    scale0 = Math.Sin((1.0 - t) * omega) * sinom;
                    scale1 = Math.Sin(t * omega) * sinom;
                }
                else
                {
                    scale0 = 1 - t;
                    scale1 = t;
                }
                Quaternion3d res = new Quaternion3d(scale0 * from.x + scale1 * to.x,
                                                    scale0 * from.y + scale1 * to.y,
                                                    scale0 * from.z + scale1 * to.z,
                                                    scale0 * from.w + scale1 * to.w);

                return res.Normalized;
            }
        }

        /// <summary>
        /// Create a rotation out of a vector.
        /// Uses Unity euler axis (+x right, +y up, +z forward)
        /// </summary>
        public static Quaternion3d FromEuler(Vector3d euler)
        {
            double heading = euler.y * DMath.Deg2Rad;
            double attitude = euler.z * DMath.Deg2Rad;
            double bank = euler.x * DMath.Deg2Rad;

            double c1 = Math.Cos(heading / 2);
            double s1 = Math.Sin(heading / 2);
            double c2 = Math.Cos(attitude / 2);
            double s2 = Math.Sin(attitude / 2);
            double c3 = Math.Cos(bank / 2);
            double s3 = Math.Sin(bank / 2);
            double c1c2 = c1 * c2;
            double s1s2 = s1 * s2;

            Quaternion3d q;
            q.w = (c1c2 * c3 - s1s2 * s3);
            q.x = (c1c2 * s3 + s1s2 * c3);
            q.y = (s1 * c2 * c3 + c1 * s2 * s3);
            q.z = (c1 * s2 * c3 - s1 * c2 * s3);

            return q;
        }

        /// <summary>
        /// Returns a double4x4 matrix that rotates around the x-axis by a given number of degrees.
        /// </summary>
        /// <param name="angle">
        /// The clockwise rotation angle when looking along the x-axis towards the origin in degrees.
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion3d RotateX(double angle)
        {
            double a = angle * DMath.Deg2Rad * 0.5f;
            double sina = Math.Sin(a);
            double cosa = Math.Cos(a);
            return new Quaternion3d(sina, 0.0, 0.0, cosa);
        }

        /// <summary>
        /// Returns a double4x4 matrix that rotates around the y-axis by a given number of degrees.
        /// </summary>
        /// <param name="angle">
        /// The clockwise rotation angle when looking along the y-axis towards the origin in degrees.
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion3d RotateY(double angle)
        {
            double a = angle * DMath.Deg2Rad * 0.5f;
            double sina = Math.Sin(a);
            double cosa = Math.Cos(a);
            return new Quaternion3d(0.0, sina, 0.0, cosa);
        }

        /// <summary>
        /// Returns a double4x4 matrix that rotates around the z-axis by a given number of degrees.
        /// </summary>
        /// <param name="angle">
        /// The clockwise rotation angle when looking along the z-axis towards the origin in degrees.
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion3d RotateZ(double angle)
        {
            double a = angle * DMath.Deg2Rad * 0.5f;
            double sina = Math.Sin(a);
            double cosa = Math.Cos(a);
            return new Quaternion3d(0.0, 0.0, sina, cosa);
        }

    }

}
























