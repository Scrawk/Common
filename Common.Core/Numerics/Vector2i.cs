using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using Common.Core.Numerics;

using REAL = System.Int32;

namespace Common.Core.Numerics
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2i : IEquatable<Vector2i>, IComparable<Vector2i>
    {
		public REAL x, y;

        public REAL a => x;
        public REAL b => y;

        public REAL u => x;
        public REAL v => y;

        /// <summary>
        /// The unit x vector.
        /// </summary>
        public readonly static Vector2i UnitX = new Vector2i(1, 0);

        /// <summary>
        /// The unit y vector.
        /// </summary>
	    public readonly static Vector2i UnitY = new Vector2i(0, 1);

        /// <summary>
        /// A vector of zeros.
        /// </summary>
	    public readonly static Vector2i Zero = new Vector2i(0);

        /// <summary>
        /// A vector of ones.
        /// </summary>
	    public readonly static Vector2i One = new Vector2i(1);

        /// <summary>
        /// A vector of min REAL.
        /// </summary>
        public readonly static Vector2i MinInt = new Vector2i(int.MinValue);

        /// <summary>
        /// A vector of max REAL.
        /// </summary>
        public readonly static Vector2i MaxInt = new Vector2i(int.MaxValue);

        /// <summary>
        /// Convert to a 3 dimension vector.
        /// </summary>
        public Vector3i x0y
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector3i(x, 0, y); }
        }

        /// <summary>
        /// Convert to a 3 dimension vector.
        /// </summary>
        public Vector3i xy0
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector3i(x, y, 0); }
        }

        /// <summary>
        /// Convert to a 4 dimension vector.
        /// </summary>
        public Vector4i xy01
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector4i(x, y, 0, 1); }
        }

        /// <summary>
        /// Convert to a 4 dimension vector.
        /// </summary>
        public Vector4i x0y1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector4i(x, 0, y, 1); }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2i(REAL v)
        {
            this.x = v;
            this.y = v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2i(REAL x, REAL y)
		{
			this.x = x;
			this.y = y;
		}

        unsafe public REAL this[int i]
        {
            get
            {
                if ((uint)i >= 2)
                    throw new IndexOutOfRangeException("Vector2i index out of range.");

                fixed (Vector2i* array = &this) { return ((REAL*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 2)
                    throw new IndexOutOfRangeException("Vector2i index out of range.");

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
        public REAL Product
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
        public double Magnitude
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return MathUtil.SafeSqrt(SqrMagnitude);
            }
        }

        /// <summary>
        /// The length of the vector squared.
        /// </summary>
        public double SqrMagnitude
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return (x * x + y * y);
            }
        }

        /// <summary>
        /// The vectors absolute values.
        /// </summary>
        public Vector2i Absolute
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new Vector2i(Math.Abs(x), Math.Abs(y));
            }
        }

        /// <summary>
        /// Add two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2i operator +(Vector2i v1, Vector2i v2)
        {
            return new Vector2i(v1.x + v2.x, v1.y + v2.y);
        }

        /// <summary>
        /// Add vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2i operator +(Vector2i v1, REAL s)
        {
            return new Vector2i(v1.x + s, v1.y + s);
        }

        /// <summary>
        /// Add vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2i operator +(REAL s, Vector2i v1)
        {
            return new Vector2i(v1.x + s, v1.y + s);
        }

        /// <summary>
        /// Negate vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2i operator -(Vector2i v)
        {
            return new Vector2i(-v.x, -v.y);
        }

        /// <summary>
        /// Subtract two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2i operator -(Vector2i v1, Vector2i v2)
        {
            return new Vector2i(v1.x - v2.x, v1.y - v2.y);
        }

        /// <summary>
        /// Subtract vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2i operator -(Vector2i v1, REAL s)
        {
            return new Vector2i(v1.x - s, v1.y - s);
        }

        /// <summary>
        /// Subtract vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2i operator -(REAL s, Vector2i v1)
        {
            return new Vector2i(s - v1.x, s - v1.y);
        }

        /// <summary>
        /// Multiply two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2i operator *(Vector2i v1, Vector2i v2)
        {
            return new Vector2i(v1.x * v2.x, v1.y * v2.y);
        }

        /// <summary>
        /// Multiply a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2i operator *(Vector2i v, REAL s)
        {
            return new Vector2i(v.x * s, v.y * s);
        }

        /// <summary>
        /// Multiply a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2i operator *(REAL s, Vector2i v)
        {
            return new Vector2i(v.x * s, v.y * s);
        }

        /// <summary>
        /// Divide two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2i operator /(Vector2i v1, Vector2i v2)
        {
            return new Vector2i(v1.x / v2.x, v1.y / v2.y);
        }

        /// <summary>
        /// Divide a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2i operator /(Vector2i v, REAL s)
        {
            return new Vector2i(v.x / s, v.y / s);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector2i(Vector2f v)
        {
            return new Vector2i((REAL)v.x, (REAL)v.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector2i(Vector2d v)
        {
            return new Vector2i((REAL)v.x, (REAL)v.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector2i(ValueTuple<REAL, REAL> v)
        {
            return new Vector2i(v.Item1, v.Item2);
        }

        /// <summary>
        /// Are these vectors equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector2i v1, Vector2i v2)
		{
			return (v1.x == v2.x && v1.y == v2.y);
		}

        /// <summary>
        /// Are these vectors not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector2i v1, Vector2i v2)
		{
			return (v1.x != v2.x || v1.y != v2.y);
		}

        /// <summary>
        /// Are these vectors equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals (object obj)
		{
			if(!(obj is Vector2i)) return false;
			Vector2i v = (Vector2i)obj;
			return this == v;
		}

        /// <summary>
        /// Are these vectors equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector2i v)
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
        public int CompareTo(Vector2i other)
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
        public static Vector2i FromString(string s)
		{
            Vector2i v = new Vector2i();

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
        public static REAL Dot(Vector2i v0, Vector2i v1)
        {
            return (v0.x * v1.x + v0.y * v1.y);
        }

        /// <summary>
        /// The abs dot product of two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static REAL AbsDot(Vector2i v0, Vector2i v1)
        {
            return Math.Abs(v0.x * v1.x + v0.y * v1.y);
        }

        /// <summary>
        /// Cross two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static REAL Cross(Vector2i v0, Vector2i v1)
        {
            return v0.x * v1.y - v0.y * v1.x;
        }

        /// <summary>
        /// Distance between two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Distance(Vector2i v0, Vector2i v1)
        {
            return MathUtil.SafeSqrt(SqrDistance(v0, v1));
        }

        /// <summary>
        /// Square distance between two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double SqrDistance(Vector2i v0, Vector2i v1)
        {
            double x = v0.x - v1.x;
            double y = v0.y - v1.y;
            return x * x + y * y;
        }

        /// <summary>
        /// Angle between two vectors in degrees from 0 to 180.
        /// A and b origin treated as 0,0 and do not need to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Angle180(Vector2i a, Vector2i b)
        {
            double dp = Dot(a, b);
            double m = a.Magnitude * b.Magnitude;
            return MathUtil.ToDegrees(MathUtil.SafeAcos(MathUtil.SafeDiv(dp, m)));
        }

        /// <summary>
        /// Angle between two vectors in degrees from 0 to 360.
        /// Angle represents moving ccw from a to b.
        /// A and b origin treated as 0,0 and do not need to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Angle360(Vector2i a, Vector2i b)
        {
            double angle = Math.Atan2(a.y, a.x) - Math.Atan2(b.y, b.x);

            if (angle <= 0.0)
                angle = MathUtil.PI * 2.0 + angle;

            angle = 360.0 - MathUtil.ToDegrees(angle);
            return angle >= 360.0 ? 0 : angle;
        }

        /// <summary>
        /// The minimum value between s and each component in vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2i Min(Vector2i v, REAL s)
        {
            v.x = Math.Min(v.x, s);
            v.y = Math.Min(v.y, s);
            return v;
        }

        /// <summary>
        /// The minimum value between each component in vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2i Min(Vector2i v0, Vector2i v1)
        {
            v0.x = Math.Min(v0.x, v1.x);
            v0.y = Math.Min(v0.y, v1.y);
            return v0;
        }

        /// <summary>
        /// The maximum value between s and each component in vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2i Max(Vector2i v, REAL s)
        {
            v.x = Math.Max(v.x, s);
            v.y = Math.Max(v.y, s);
            return v;
        }

        /// <summary>
        /// The maximum value between each component in vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2i Max(Vector2i v0, Vector2i v1)
        {
            v0.x = Math.Max(v0.x, v1.x);
            v0.y = Math.Max(v0.y, v1.y);
            return v0;
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
        public void Clamp(Vector2i min, Vector2i max)
        {
            x = Math.Max(Math.Min(x, max.x), min.x);
            y = Math.Max(Math.Min(y, max.y), min.y);
        }

    }
	
}

















