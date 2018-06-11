using System;
using System.Runtime.InteropServices;

using Common.Core.Mathematics;

namespace Common.Core.LinearAlgebra
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Quaternion3f
    {

        public float x, y, z, w;

        public readonly static Quaternion3f Identity = new Quaternion3f(0, 0, 0, 1);

        public readonly static Quaternion3f Zero = new Quaternion3f(0, 0, 0, 0);

        /// <summary>
        /// A Quaternion from varibles.
        /// </summary>
        public Quaternion3f(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        /// <summary>
        /// A Quaternion copied from a array.
        /// </summary>
        public Quaternion3f(float[] v)
        {
            this.x = v[0];
            this.y = v[1];
            this.z = v[2];
            this.w = v[3];
        }

        /// <summary>
        /// The inverse of the quaternion.
        /// </summary>
        public Quaternion3f Inverse
        {
            get
            {
                return new Quaternion3f(-x, -y, -z, w);
            }
        }

        /// <summary>
        /// The length of the quaternion.
        /// </summary>
        float Length
        {
            get
            {
                float len = x * x + y * y + z * z + w * w;
                return FMath.SafeSqrt(len);
            }
        }

        /// <summary>
        /// The a normalized quaternion.
        /// </summary>
        public Quaternion3f Normalized
        {
            get
            {
                float inv = FMath.SafeInv(Length);
                return new Quaternion3f(x * inv, y * inv, z * inv, w * inv);
            }
        }

        /// <summary>
        /// Are these Quaternions equal.
        /// </summary>
        public static bool operator ==(Quaternion3f v1, Quaternion3f v2)
		{
			return (v1.x == v2.x && v1.y == v2.y && v1.z == v2.z && v1.w == v2.w);
		}
		
		/// <summary>
		/// Are these Quaternions not equal.
		/// </summary>
		public static bool operator !=(Quaternion3f v1, Quaternion3f v2)
		{
			return (v1.x != v2.x || v1.y != v2.y || v1.z != v2.z || v1.w != v2.w);
		}
		
		/// <summary>
		/// Are these Quaternions equal.
		/// </summary>
		public override bool Equals (object obj)
		{
			if(!(obj is Quaternion3f)) return false;
			
			Quaternion3f v = (Quaternion3f)obj;
			
			return this == v;
		}
		
		/// <summary>
		/// Quaternions hash code. 
		/// </summary>
		public override int GetHashCode()
		{
            float hashcode = 23;
            hashcode = (hashcode * 37) + x;
            hashcode = (hashcode * 37) + y;
            hashcode = (hashcode * 37) + z;
            hashcode = (hashcode * 37) + w;

            return unchecked((int)hashcode);
        }
		
		/// <summary>
		/// Quaternion as a string.
		/// </summary>
		public override string ToString()
		{
			return "(" + x + "," + y + "," + z + "," + w + ")";
		}

        /// <summary>
        /// A Quaternion from a vector axis and angle.
        /// The axis is the up direction and the angle is the rotation.
        /// </summary>
        public Quaternion3f(Vector3f axis, float angle)
        {
            Vector3f axisN = axis.Normalized;
            float a = angle * 0.5f;
			float sina = (float)Math.Sin(a);
			float cosa = (float)Math.Cos(a);
            x = axisN.x * sina;
            y = axisN.y * sina;
            z = axisN.z * sina;
            w = cosa;
        }

        /// <summary>
        /// A quaternion with the rotation required to
        /// rotation from the from direction to the to direction.
        /// </summary>
        public Quaternion3f(Vector3f to, Vector3f from)
        {
            Vector3f f = from.Normalized;
            Vector3f t = to.Normalized;

            float dotProdPlus1 = 1.0f + Vector3f.Dot(f, t);

            if (dotProdPlus1 < FMath.EPS)
            {
                w = 0;
                if (Math.Abs(f.x) < 0.6f)
                {
					float norm = (float)Math.Sqrt(1 - f.x * f.x);
                    x = 0;
                    y = f.z / norm;
                    z = -f.y / norm;
                }
                else if (Math.Abs(f.y) < 0.6f)
                {
					float norm = (float)Math.Sqrt(1 - f.y * f.y);
                    x = -f.z / norm;
                    y = 0;
                    z = f.x / norm;
                }
                else
                {
					float norm = (float)Math.Sqrt(1 - f.z * f.z);
                    x = f.y / norm;
                    y = -f.x / norm;
                    z = 0;
                }
            }
            else
            {
				float s = (float)Math.Sqrt(0.5f * dotProdPlus1);
                Vector3f tmp = (f.Cross(t)) / (2.0f * s);
                x = tmp.x;
                y = tmp.y;
                z = tmp.z;
                w = s;
            }
        }

        /// <summary>
        /// Multiply two quternions together.
        /// </summary>
		public static Quaternion3f operator *(Quaternion3f q1, Quaternion3f q2)
        {
			return new Quaternion3f(q2.w * q1.x + q2.x * q1.w + q2.y * q1.z - q2.z * q1.y,
                            		q2.w * q1.y - q2.x * q1.z + q2.y * q1.w + q2.z * q1.x,
                            		q2.w * q1.z + q2.x * q1.y - q2.y * q1.x + q2.z * q1.w,
                            		q2.w * q1.w - q2.x * q1.x - q2.y * q1.y - q2.z * q1.z);
        }

        /// <summary>
        /// Multiply a quaternion and a vector together.
        /// </summary>
		public static Vector3f operator *(Quaternion3f q, Vector3f v)
        {
            return q.ToMatrix3x3f() * v;
        }

        /// <summary>
        /// Convert to a single precision 3 dimension matrix.
        /// </summary>
        public Matrix3x3f ToMatrix3x3f()
        {
            float   xx = x * x,
                    xy = x * y,
                    xz = x * z,
                    xw = x * w,
                    yy = y * y,
                    yz = y * z,
                    yw = y * w,
                    zz = z * z,
                    zw = z * w;

            return new Matrix3x3f
            (
                1.0f - 2.0f * (yy + zz), 2.0f * (xy - zw), 2.0f * (xz + yw),
                2.0f * (xy + zw), 1.0f - 2.0f * (xx + zz), 2.0f * (yz - xw),
                2.0f * (xz - yw), 2.0f * (yz + xw), 1.0f - 2.0f * (xx + yy)
            );
        }

        /// <summary>
        /// Convert to a single precision 4 dimension matrix.
        /// </summary>
        public Matrix4x4f ToMatrix4x4f()
        {
            float   xx = x * x,
                    xy = x * y,
                    xz = x * z,
                    xw = x * w,
                    yy = y * y,
                    yz = y * z,
                    yw = y * w,
                    zz = z * z,
                    zw = z * w;

            return new Matrix4x4f
            (
                1.0f - 2.0f * (yy + zz), 2.0f * (xy - zw), 2.0f * (xz + yw), 0.0f,
                2.0f * (xy + zw), 1.0f - 2.0f * (xx + zz), 2.0f * (yz - xw), 0.0f,
                2.0f * (xz - yw), 2.0f * (yz + xw), 1.0f - 2.0f * (xx + yy), 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f
            );
        }

        /// <summary>
        /// The normalize the quaternion.
        /// </summary>
        public void Normalize()
        {
            float invLength = FMath.SafeInv(Length);
            x *= invLength;
            y *= invLength;
            z *= invLength;
            w *= invLength;
        }

        /// <summary>
        /// Slerp the quaternion from the from rotation to the to rotation by t.
        /// </summary>
		public static Quaternion3f Slerp(Quaternion3f from, Quaternion3f to, float t)
        {
            if (t <= 0.0f)
            {
                return from;
            }
            else if (t >= 1.0f)
            {
                return to;
            }
            else
            {
                float cosom = from.x * to.x + from.y * to.y + from.z * to.z + from.w * to.w;
                float absCosom = Math.Abs(cosom);

                float scale0;
                float scale1;

                if ((1 - absCosom) > FMath.EPS)
                {
                    float omega = FMath.SafeAcos(absCosom);
					float sinom = 1.0f / (float)Math.Sin(omega);
					scale0 = (float)Math.Sin((1.0f - t) * omega) * sinom;
					scale1 = (float)Math.Sin(t * omega) * sinom;
                }
                else
                {
                    scale0 = 1 - t;
                    scale1 = t;
                }
                Quaternion3f res = new Quaternion3f(scale0 * from.x + scale1 * to.x,
                                                    scale0 * from.y + scale1 * to.y,
                                                    scale0 * from.z + scale1 * to.z,
                                                    scale0 * from.w + scale1 * to.w);

                return res.Normalized;
            }
        }

        /// <summary>
        /// Create a rotation out of a vector.
        /// </summary>
        public static Quaternion3f FromEuler(Vector3f euler)
        {
            float heading = euler.y * FMath.Deg2Rad;
            float attitude = euler.z * FMath.Deg2Rad;
            float bank = euler.x * FMath.Deg2Rad;

            double c1 = Math.Cos(heading / 2);
            double s1 = Math.Sin(heading / 2);
            double c2 = Math.Cos(attitude / 2);
            double s2 = Math.Sin(attitude / 2);
            double c3 = Math.Cos(bank / 2);
            double s3 = Math.Sin(bank / 2);
            double c1c2 = c1 * c2;
            double s1s2 = s1 * s2;

            Quaternion3f q;
            q.w = (float)(c1c2 * c3 - s1s2 * s3);
            q.x = (float)(c1c2 * s3 + s1s2 * c3);
            q.y = (float)(s1 * c2 * c3 + c1 * s2 * s3);
            q.z = (float)(c1 * s2 * c3 - s1 * c2 * s3);

            return q;
        }


    }

}
























