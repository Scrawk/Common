using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using Common.Core.Numerics;

using REAL = System.Single;

namespace Common.Core.Numerics
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2f : IEquatable<Vector2f>, IComparable<Vector2f>
    {
        public REAL x, y;

        public REAL a => x;
        public REAL b => y;

        public REAL u => x;
        public REAL v => y;

        /// <summary>
        /// The unit x vector.
        /// </summary>
        public readonly static Vector2f UnitX = new Vector2f(1, 0);

        /// <summary>
        /// The unit y vector.
        /// </summary>
	    public readonly static Vector2f UnitY = new Vector2f(0, 1);

        /// <summary>
        /// A vector of zeros.
        /// </summary>
	    public readonly static Vector2f Zero = new Vector2f(0);

        /// <summary>
        /// A vector of ones.
        /// </summary>
	    public readonly static Vector2f One = new Vector2f(1);

        /// <summary>
        /// A vector of 0.5.
        /// </summary>
        public readonly static Vector2f Half = new Vector2f(0.5f);

        /// <summary>
        /// A vector of positive infinity.
        /// </summary>
        public readonly static Vector2f PositiveInfinity = new Vector2f(REAL.PositiveInfinity);

        /// <summary>
        /// A vector of negative infinity.
        /// </summary>
        public readonly static Vector2f NegativeInfinity = new Vector2f(REAL.NegativeInfinity);

        /// <summary>
        /// Convert to a 3 dimension vector.
        /// </summary>
        public Vector3f x0y
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector3f(x, 0, y); }
        }

        /// <summary>
        /// Convert to a 3 dimension vector.
        /// </summary>
        public Vector3f xy0
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector3f(x, y, 0); }
        }

        /// <summary>
        /// Convert to a 4 dimension vector.
        /// </summary>
        public Vector4f xy01
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector4f(x, y, 0, 1); }
        }

        /// <summary>
        /// Convert to a 4 dimension vector.
        /// </summary>
        public Vector4f x0y1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector4f(x, 0, y, 1); }
        }

        /// <summary>
        /// A vector all with the value v.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2f(REAL v)
        {
            this.x = v;
            this.y = v;
        }

        /// <summary>
        /// A vector from the variables.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2f(REAL x, REAL y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// A vector from the varibles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2f(double x, double y)
        {
            this.x = (float)x;
            this.y = (float)y;
        }

        unsafe public REAL this[int i]
        {
            get
            {
                if ((uint)i >= 2)
                    throw new IndexOutOfRangeException("Vector2f index out of range.");

                fixed (Vector2f* array = &this) { return ((REAL*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 2)
                    throw new IndexOutOfRangeException("Vector2f index out of range.");

                fixed (REAL* array = &x) { array[i] = value; }
            }
        }

        /// <summary>
        /// The sum of the vector.
        /// </summary>
        public REAL Sum
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return x + y;
            }
        }

        /// <summary>
        /// The multiple of the vector.
        /// </summary>
        public REAL Mul
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return x * y;
            }
        }

        /// <summary>
        /// The length of the vector.
        /// </summary>
        public REAL Magnitude
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
		public REAL SqrMagnitude
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return (x * x + y * y);
            }
        }

        /// <summary>
        /// The vector normalized.
        /// </summary>
        public Vector2f Normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                REAL invLength = FMath.SafeInvSqrt(1.0f, x * x + y * y);
                return new Vector2f(x * invLength, y * invLength);
            }
        }

        /// <summary>
        /// Counter clock-wise perpendicular.
        /// </summary>
        public Vector2f PerpendicularCCW
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new Vector2f(-y, x);
            }
        }

        /// <summary>
        /// Clock-wise perpendicular.
        /// </summary>
        public Vector2f PerpendicularCW
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new Vector2f(y, -x);
            }
        }

        /// <summary>
        /// The vectors absolute values.
        /// </summary>
        public Vector2f Absolute
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new Vector2f(Math.Abs(x), Math.Abs(y));
            }
        }

        /// <summary>
        /// Add two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator +(Vector2f v1, Vector2f v2)
        {
            return new Vector2f(v1.x + v2.x, v1.y + v2.y);
        }

        /// <summary>
        /// Add vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator +(Vector2f v1, REAL s)
        {
            return new Vector2f(v1.x + s, v1.y + s);
        }

        /// <summary>
        /// Add vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator +(REAL s, Vector2f v1)
        {
            return new Vector2f(v1.x + s, v1.y + s);
        }

        /// <summary>
        /// Negate vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator -(Vector2f v)
        {
            return new Vector2f(-v.x, -v.y);
        }

        /// <summary>
        /// Subtract two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator -(Vector2f v1, Vector2f v2)
        {
            return new Vector2f(v1.x - v2.x, v1.y - v2.y);
        }

        /// <summary>
        /// Subtract vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator -(Vector2f v1, REAL s)
        {
            return new Vector2f(v1.x - s, v1.y - s);
        }

        /// <summary>
        /// Subtract vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator -(REAL s, Vector2f v1)
        {
            return new Vector2f(s - v1.x, s - v1.y);
        }

        /// <summary>
        /// Multiply two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator *(Vector2f v1, Vector2f v2)
        {
            return new Vector2f(v1.x * v2.x, v1.y * v2.y);
        }

        /// <summary>
        /// Multiply a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator *(Vector2f v, REAL s)
        {
            return new Vector2f(v.x * s, v.y * s);
        }

        /// <summary>
        /// Multiply a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator *(REAL s, Vector2f v)
        {
            return new Vector2f(v.x * s, v.y * s);
        }

        /// <summary>
        /// Divide two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator /(Vector2f v1, Vector2f v2)
        {
            return new Vector2f(v1.x / v2.x, v1.y / v2.y);
        }

        /// <summary>
        /// Divide a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator /(Vector2f v, REAL s)
        {
            return new Vector2f(v.x / s, v.y / s);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector2f(Vector2d v)
        {
            return new Vector2f((REAL)v.x, (REAL)v.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector2f(Vector2i v)
        {
            return new Vector2f(v.x, v.y);
        }

        /// <summary>
        /// Are these vectors equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector2f v1, Vector2f v2)
		{
			return (v1.x == v2.x && v1.y == v2.y);
		}

        /// <summary>
        /// Are these vectors not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector2f v1, Vector2f v2)
		{
			return (v1.x != v2.x || v1.y != v2.y);
		}

        /// <summary>
        /// Are these vectors equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals (object obj)
		{
			if(!(obj is Vector2f)) return false;
			Vector2f v = (Vector2f)obj;
			return this == v;
		}

        /// <summary>
        /// Are these vectors equal given the error.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AlmostEqual(Vector2f v, REAL eps)
		{
			if(Math.Abs(x-v.x)> eps) return false;
			if(Math.Abs(y-v.y)> eps) return false;
			return true;
		}

        /// <summary>
        /// Are these vectors equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector2f v)
        {
            return this == v;
        }

        /// <summary>
        /// Vectors hash code. 
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ x.GetHashCode();
                hash = (hash * 16777619) ^ y.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Compare two vectors by axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(Vector2f other)
        {
            if (x != other.x)
                return x < other.x ? -1 : 1;
            else if (y != other.y)
                return y < other.y ? -1 : 1;
            return 0;
        }

        /// <summary>
        /// Vector as a string.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return string.Format("{0},{1}", x, y);
        }

        /// <summary>
        /// Vector as a string.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(string f)
        {
            return string.Format("{0},{1}", x.ToString(f), y.ToString(f));
        }

        /// <summary>
        /// Vector from a string.
        /// </summary>
        public static Vector2f FromString(string s)
		{
            Vector2f v = new Vector2f();

            try
            {
                string[] separators = new string[] { "," };
                string[] result = s.Split(separators, StringSplitOptions.None);

                v.x = REAL.Parse(result[0]);
                v.y = REAL.Parse(result[1]);
            }
            catch { }
			
			return v;
		}

        /// <summary>
        /// The dot product of two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static REAL Dot(Vector2f v0, Vector2f v1)
		{
			return v0.x * v1.x + v0.y * v1.y;
		}

        /// <summary>
        /// The abs dot product of two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static REAL AbsDot(Vector2f v0, Vector2f v1)
        {
            return Math.Abs(v0.x * v1.x + v0.y * v1.y);
        }

        /// <summary>
        /// Normalize the vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            REAL invLength = FMath.SafeInvSqrt(1.0f, x * x + y * y);
            x *= invLength;
            y *= invLength;
        }

        /// <summary>
        /// Cross two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static REAL Cross(Vector2f v0, Vector2f v1)
        {
            return v0.x * v1.y - v0.y * v1.x;
        }

        /// <summary>
        /// Distance between two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static REAL Distance(Vector2f v0, Vector2f v1)
        {
            return FMath.SafeSqrt(SqrDistance(v0, v1));
        }

        /// <summary>
        /// Square distance between two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static REAL SqrDistance(Vector2f v0, Vector2f v1)
        {
            REAL x = v0.x - v1.x;
            REAL y = v0.y - v1.y;
            return x * x + y * y;
        }

        /// <summary>
        /// Given an incident vector i and a normal vector n.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f Reflect(Vector2f i, Vector2f n)
        {
            return i - 2 * n * Dot(i, n);
        }

        /// <summary>
        /// Returns the refraction vector given the incident vector i, 
        /// the normal vector n and the refraction index eta.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f Refract(Vector2f i, Vector2f n, REAL eta)
        {
            REAL ni = Dot(n, i);
            REAL k = 1.0f - eta * eta * (1.0f - ni * ni);

            return (k >= 0) ? eta * i - (eta * ni + FMath.SafeSqrt(k)) * n : Zero;
        }

        /// <summary>
        /// Angle between two vectors in degrees from 0 to 180.
        /// A and b origin treated as 0,0 and do not need to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static REAL Angle180(Vector2f a, Vector2f b)
        {
            REAL dp = Vector2f.Dot(a, b);
            REAL m = a.Magnitude * b.Magnitude;
            return FMath.SafeAcos(FMath.SafeDiv(dp, m)) * FMath.Rad2Deg;
        }

        /// <summary>
        /// Angle between two vectors in degrees from 0 to 360.
        /// Angle represents moving ccw from a to b.
        /// A and b origin treated as 0,0 and do not need to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static REAL Angle360(Vector2f a, Vector2f b)
        {
            REAL angle = FMath.Atan2(a.y, a.x) - FMath.Atan2(b.y, b.x);

            if (angle <= 0.0f)
                angle = FMath.PI * 2.0f + angle;

            angle = 360.0f - angle * FMath.Rad2Deg;
            return angle >= 360.0f ? 0 : angle;
        }

        /// <summary>
        /// The minimum value between s and each component in vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f Min(Vector2f v, REAL s)
        {
            v.x = Math.Min(v.x, s);
            v.y = Math.Min(v.y, s);
            return v;
        }

        /// <summary>
        /// The minimum value between each component in vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f Min(Vector2f v0, Vector2f v1)
        {
            v0.x = Math.Min(v0.x, v1.x);
            v0.y = Math.Min(v0.y, v1.y);
            return v0;
        }

        /// <summary>
        /// The maximum value between s and each component in vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f Max(Vector2f v, REAL s)
        {
            v.x = Math.Max(v.x, s);
            v.y = Math.Max(v.y, s);
            return v;
        }

        /// <summary>
        /// The maximum value between each component in vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f Max(Vector2f v0, Vector2f v1)
        {
            v0.x = Math.Max(v0.x, v1.x);
            v0.y = Math.Max(v0.y, v1.y);
            return v0;
        }

        /// <summary>
        /// The absolute vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Abs()
        {
            x = Math.Abs(x);
            y = Math.Abs(y);
        }

        /// <summary>
        /// Clamp the each component to specified min and max.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clamp(REAL min, REAL max)
		{
			x = Math.Max(Math.Min(x, max), min);
			y = Math.Max(Math.Min(y, max), min);
		}

        /// <summary>
        /// Clamp the each component to specified min and max.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clamp(Vector2f min, Vector2f max)
        {
            x = Math.Max(Math.Min(x, max.x), min.x);
            y = Math.Max(Math.Min(y, max.y), min.y);
        }

        /// <summary>
        /// Lerp between two vectors.
        /// </summary>
        public static Vector2f Lerp(Vector2f from, Vector2f to, REAL t)
        {
            if (t < 0.0f) t = 0.0f;
            if (t > 1.0f) t = 1.0f;

            if (t == 0.0f) return from;
            if (t == 1.0f) return to;

            REAL t1 = 1.0f - t;
			Vector2f v = new Vector2f();
            v.x = from.x * t1 + to.x * t;
            v.y = from.y * t1 + to.y * t;
			return v;
        }

        /// <summary>
        /// Slerp between two vectors arc.
        /// </summary>
        public static Vector2f Slerp(Vector2f from, Vector2f to, REAL t)
        {
            if (t < 0.0f) t = 0.0f;
            if (t > 1.0f) t = 1.0f;

            if (t == 0.0f) return from;
            if (t == 1.0f) return to;
            if (to.x == from.x && to.y == from.y) return to;

            REAL m = from.Magnitude * to.Magnitude;
            if (FMath.IsZero(m)) return Vector2f.Zero;

            double theta = Math.Acos(Dot(from, to) / m);

            if (theta == 0.0) return to;

            double sinTheta = Math.Sin(theta);
            REAL st1 = (REAL)(Math.Sin((1.0 - t) * theta) / sinTheta);
            REAL st = (REAL)(Math.Sin(t * theta) / sinTheta);

            Vector2f v = new Vector2f();
            v.x = from.x * st1 + to.x * st;
            v.y = from.y * st1 + to.y * st;

            return v;
        }

        /// <summary>
        /// Round vector.
        /// </summary>
        /// <param name="digits">number of digits to round to.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Round(int digits = 0)
        {
            x = (REAL)Math.Round(x, digits);
            y = (REAL)Math.Round(y, digits);
        }

        /// <summary>
        /// Returns if list of verts make a CCW polygon.
        /// Presumes polygon is simple.
        /// </summary>
        public static bool IsCCW(IList<Vector2f> vertices)
        {
            REAL sum = 0.0f;
            for (int i = 0; i < vertices.Count; i++)
            {
                Vector2f v1 = vertices[i];
                Vector2f v2 = vertices[(i + 1) % vertices.Count];
                sum += (v2.x - v1.x) * (v2.y + v1.y);
            }
            return sum < 0.0f;
        }

    }

}


































