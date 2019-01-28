using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using Common.Core.Mathematics;

namespace Common.Core.LinearAlgebra
{

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3f : IEquatable<Vector3f>, IComparable<Vector3f>
    {
        public float x, y, z;

        /// <summary>
        /// The unit x vector.
        /// </summary>
        public readonly static Vector3f UnitX = new Vector3f(1, 0, 0);

        /// <summary>
        /// The unit y vector.
        /// </summary>
	    public readonly static Vector3f UnitY = new Vector3f(0, 1, 0);

        /// <summary>
        /// The unit z vector.
        /// </summary>
	    public readonly static Vector3f UnitZ = new Vector3f(0, 0, 1);

        /// <summary>
        /// A vector of zeros.
        /// </summary>
	    public readonly static Vector3f Zero = new Vector3f(0);

        /// <summary>
        /// A vector of ones.
        /// </summary>
        public readonly static Vector3f One = new Vector3f(1);

        /// <summary>
        /// A vector of 0.5.
        /// </summary>
        public readonly static Vector3f Half = new Vector3f(0.5f);

        /// <summary>
        /// A vector of positive infinity.
        /// </summary>
        public readonly static Vector3f PositiveInfinity = new Vector3f(float.PositiveInfinity);

        /// <summary>
        /// A vector of negative infinity.
        /// </summary>
        public readonly static Vector3f NegativeInfinity = new Vector3f(float.NegativeInfinity);

        /// <summary>
        /// Convert to a 2 dimension vector.
        /// </summary>
        public Vector2f xy
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector2f(x, y); }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { x = value.x; y = value.y; }
        }

        /// <summary>
        /// Convert to a 2 dimension vector.
        /// </summary>
        public Vector2f xz
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector2f(x, z); }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { x = value.x; z = value.y; }
        }

        /// <summary>
        /// Convert to a 4 dimension vector.
        /// </summary>
        public Vector4f xyz0
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector4f(x, y, z, 0); }
        }

        /// <summary>
        /// Convert to a 4 dimension vector.
        /// </summary>
        public Vector4f xyz1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector4f(x, y, z, 1); }
        }

        /// <summary>
        /// A vector all with the value v.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3f(float v)
        {
            this.x = v;
            this.y = v;
            this.z = v;
        }

        /// <summary>
        /// A vector from the varibles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3f(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3f(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;
        }

        /// <summary>
        /// A vector from a 2d vector and the z varible.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3f(Vector2f v, float z)
        {
            x = v.x;
            y = v.y;
            this.z = z;
        }

        public float this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    default: throw new IndexOutOfRangeException("Vector3f index out of range: " + i);
                }
            }
            set
            {
                switch (i)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                    default: throw new IndexOutOfRangeException("Vector3f index out of range: " + i);
                }
            }
        }

        /// <summary>
        /// The length of the vector.
        /// </summary>
        public float Magnitude
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return FMath.SafeSqrt(SqrMagnitude);
            }
        }

        /// <summary>
        /// The length of the vector squared.
        /// </summary>
		public float SqrMagnitude
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return (x * x + y * y + z * z);
            }
        }

        /// <summary>
        /// The vector normalized.
        /// </summary>
        public Vector3f Normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                float invLength = FMath.SafeInvSqrt(1.0f, x * x + y * y + z * z);
                return new Vector3f(x * invLength, y * invLength, z * invLength);
            }
        }

        /// <summary>
        /// The vectors absolute values.
        /// </summary>
        public Vector3f Absolute
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new Vector3f(Math.Abs(x), Math.Abs(y), Math.Abs(z));
            }
        }

        /// <summary>
        /// Add two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3f operator +(Vector3f v1, Vector3f v2)
        {
            return new Vector3f(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        /// <summary>
        /// Add vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3f operator +(Vector3f v1, float s)
        {
            return new Vector3f(v1.x + s, v1.y + s, v1.z + s);
        }

        /// <summary>
        /// Add vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3f operator +(float s, Vector3f v1)
        {
            return new Vector3f(v1.x + s, v1.y + s, v1.z + s);
        }

        /// <summary>
        /// Negate vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3f operator -(Vector3f v)
        {
            return new Vector3f(-v.x, -v.y, -v.z);
        }

        /// <summary>
        /// Subtract two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3f operator -(Vector3f v1, Vector3f v2)
        {
            return new Vector3f(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        /// <summary>
        /// Subtract vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3f operator -(Vector3f v1, float s)
        {
            return new Vector3f(v1.x - s, v1.y - s, v1.z - s);
        }

        /// <summary>
        /// Subtract vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3f operator -(float s, Vector3f v1)
        {
            return new Vector3f(v1.x - s, v1.y - s, v1.z - s);
        }

        /// <summary>
        /// Multiply two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3f operator *(Vector3f v1, Vector3f v2)
        {
            return new Vector3f(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        /// <summary>
        /// Multiply a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3f operator *(Vector3f v, float s)
        {
            return new Vector3f(v.x * s, v.y * s, v.z * s);
        }

        /// <summary>
        /// Multiply a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3f operator *(float s, Vector3f v)
        {
            return new Vector3f(v.x * s, v.y * s, v.z * s);
        }

        /// <summary>
        /// Divide two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3f operator /(Vector3f v1, Vector3f v2)
        {
            return new Vector3f(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
        }

        /// <summary>
        /// Divide a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3f operator /(Vector3f v, float s)
        {
            return new Vector3f(v.x / s, v.y / s, v.z / s);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector3f(Vector3d v)
        {
            return new Vector3f((float)v.x, (float)v.y, (float)v.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector3f(Vector3i v)
        {
            return new Vector3f(v.x, v.y, v.z);
        }

        /// <summary>
        /// Are these vectors equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector3f v1, Vector3f v2)
		{
			return (v1.x == v2.x && v1.y == v2.y && v1.z == v2.z);
		}

        /// <summary>
        /// Are these vectors not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector3f v1, Vector3f v2)
		{
			return (v1.x != v2.x || v1.y != v2.y || v1.z != v2.z);
		}

        /// <summary>
        /// Are these vectors equal.
        /// </summary>
        public override bool Equals (object obj)
		{
			if(!(obj is Vector3f)) return false;
			Vector3f v = (Vector3f)obj;
			return this == v;
		}

        /// <summary>
        /// Are these vectors equal given the error.
        /// </summary>
        public bool EqualsWithError(Vector3f v, float eps)
		{
			if(Math.Abs(x-v.x)> eps) return false;
			if(Math.Abs(y-v.y)> eps) return false;
			if(Math.Abs(z-v.z)> eps) return false;
			return true;
		}

        /// <summary>
        /// Are these vectors equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector3f v)
        {
            return this == v;
        }

        /// <summary>
        /// Vectors hash code. 
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
        /// Compare two vectors by axis.
        /// </summary>
        public int CompareTo(Vector3f other)
        {
            if (x != other.x)
                return x < other.x ? -1 : 1;
            else if (y != other.y)
                return y < other.y ? -1 : 1;
            else if (z != other.z)
                return z < other.z ? -1 : 1;
            return 0;
        }

        /// <summary>
        /// Vector as a string.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return string.Format("{0},{1},{2}", x, y, z);
        }

        /// <summary>
        /// Vector as a string.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(string f)
        {
            return string.Format("{0},{1},{2}", x.ToString(f), y.ToString(f), z.ToString(f));
        }

        /// <summary>
        /// Vector from a string.
        /// </summary>
        static public Vector3f FromString(string s)
		{
            Vector3f v = new Vector3f();

            try
            {
                string[] separators = new string[] { "," };
                string[] result = s.Split(separators, StringSplitOptions.None);

                v.x = float.Parse(result[0]);
                v.y = float.Parse(result[1]);
                v.z = float.Parse(result[2]);
            }
            catch { }
			
			return v;
		}

        /// <summary>
        /// The dot product of two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Dot(Vector3f v0, Vector3f v1)
		{
			return (v0.x * v1.x + v0.y * v1.y + v0.z * v1.z);
		}

        /// <summary>
        /// Normalize the vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            float invLength = FMath.SafeInvSqrt(1.0f, x * x + y * y + z * z);
            x *= invLength;
            y *= invLength;
            z *= invLength;
        }

        /// <summary>
        /// Angle between two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Angle180(Vector3f a, Vector3f b)
        {
            float dp = Vector3f.Dot(a, b);
            float m = a.Magnitude * b.Magnitude;

            return (float)Math.Acos(FMath.SafeDiv(dp, m)) * FMath.Rad2Deg;
        }

        /// <summary>
        /// Cross two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3f Cross(Vector3f v)
        {
            return new Vector3f(y * v.z - z * v.y, z * v.x - x * v.z, x * v.y - y * v.x);
        }

        /// <summary>
        /// Cross two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3f Cross(Vector3f v0, Vector3f v1)
		{
			return new Vector3f(v0.y * v1.z - v0.z * v1.y, v0.z * v1.x - v0.x * v1.z, v0.x * v1.y - v0.y * v1.x);
		}

        /// <summary>
        /// Distance between two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(Vector3f v0, Vector3f v1)
        {
            return FMath.SafeSqrt(SqrDistance(v0, v1));
        }

        /// <summary>
        /// Square distance between two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SqrDistance(Vector3f v0, Vector3f v1)
        {
            float x = v0.x - v1.x;
            float y = v0.y - v1.y;
            float z = v0.z - v1.z;
            return x * x + y * y + z * z;
        }

        /// <summary>
        /// Project v on to normal n.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3f Project(Vector3f v, Vector3f n)
        {
            return Dot(v, n) * n;
        }

        /// <summary>
        /// Given an incident vector i and a normal vector n.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3f Reflect(Vector3f i, Vector3f n)
        {
            return i - 2 * n * Dot(i, n);
        }

        /// <summary>
        /// Returns the refraction vector given the incident vector i, 
        /// the normal vector n and the refraction index eta.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3f Refract(Vector3f i, Vector3f n, float eta)
        {
            float ni = Dot(n, i);
            float k = 1.0f - eta * eta * (1.0f - ni * ni);

            return (k >= 0) ? eta * i - (eta * ni + FMath.SafeSqrt(k)) * n : Zero;
        }

        /// <summary>
        /// The minimum value between s and each component in vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Min(float s)
        {
            x = Math.Min(x, s);
            y = Math.Min(y, s);
            z = Math.Min(z, s);
        }

        /// <summary>
        /// The minimum value between each component in vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Min(Vector3f v)
        {
            x = Math.Min(x, v.x);
            y = Math.Min(y, v.y);
            z = Math.Min(z, v.z);
        }

        /// <summary>
        /// The maximum value between s and each component in vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Max(float s)
        {
            x = Math.Max(x, s);
            y = Math.Max(y, s);
            z = Math.Max(z, s);
        }

        /// <summary>
        /// The maximum value between each component in vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Max(Vector3f v)
        {
            x = Math.Max(x, v.x);
            y = Math.Max(y, v.y);
            z = Math.Max(z, v.z);
        }

        /// <summary>
        /// The absolute vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Abs()
        {
            x = Math.Abs(x);
            y = Math.Abs(y);
            z = Math.Abs(z);
        }

        /// <summary>
        /// Clamp the each component to specified min and max.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clamp(float min, float max)
		{
			x = Math.Max(Math.Min(x, max), min);
			y = Math.Max(Math.Min(y, max), min);
			z = Math.Max(Math.Min(z, max), min);
		}

        /// <summary>
        /// Clamp the each component to specified min and max.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clamp(Vector3f min, Vector3f max)
        {
            x = Math.Max(Math.Min(x, max.x), min.x);
            y = Math.Max(Math.Min(y, max.y), min.y);
            z = Math.Max(Math.Min(z, max.z), min.z);
        }

        /// <summary>
        /// Lerp between two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3f Lerp(Vector3f from, Vector3f to, float t)
        {
            if (t < 0.0f) t = 0.0f;
            if (t > 1.0f) t = 1.0f;

            if (t == 0.0f) return from;
            if (t == 1.0f) return to;

            float t1 = 1.0f - t;
            Vector3f v = new Vector3f();
            v.x = from.x * t1 + to.x * t;
            v.y = from.y * t1 + to.y * t;
            v.z = from.z * t1 + to.z * t;
            return v;
        }

        /// <summary>
        /// Slerp between two vectors arc.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3f Slerp(Vector3f from, Vector3f to, float t)
        {
            if (t < 0.0f) t = 0.0f;
            if (t > 1.0f) t = 1.0f;

            if (t == 0.0f) return from;
            if (t == 1.0f) return to;
            if (to.x == from.x && to.y == from.y && to.z == from.z) return to;

            float m = from.Magnitude * to.Magnitude;
            if (FMath.IsZero(m)) return Vector3f.Zero;

            double theta = Math.Acos(Dot(from, to) / m);

            if (theta == 0.0) return to;

            double sinTheta = Math.Sin(theta);
            float st1 = (float)(Math.Sin((1.0 - t) * theta) / sinTheta);
            float st = (float)(Math.Sin(t * theta) / sinTheta);

            Vector3f v = new Vector3f();
            v.x = from.x * st1 + to.x * st;
            v.y = from.y * st1 + to.y * st;
            v.z = from.z * st1 + to.z * st;

            return v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Round(int digits = 0)
        {
            x = (float)Math.Round(x, digits);
            y = (float)Math.Round(y, digits);
            z = (float)Math.Round(z, digits);
        }

    }


}


































