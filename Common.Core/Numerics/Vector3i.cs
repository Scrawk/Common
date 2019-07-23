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
    public struct Vector3i : IEquatable<Vector3i>, IComparable<Vector3i>
    {
		public REAL x, y, z;

        public REAL a => x;
        public REAL b => y;
        public REAL c => z;

        public REAL u => x;
        public REAL v => y;
        public REAL w => z;

        /// <summary>
        /// The unit x vector.
        /// </summary>
	    public readonly static Vector3i UnitX = new Vector3i(1, 0, 0);

        /// <summary>
        /// The unit y vector.
        /// </summary>
	    public readonly static Vector3i UnitY = new Vector3i(0, 1, 0);

        /// <summary>
        /// The unit z vector.
        /// </summary>
	    public readonly static Vector3i UnitZ = new Vector3i(0, 0, 1);

        /// <summary>
        /// A vector of zeros.
        /// </summary>
	    public readonly static Vector3i Zero = new Vector3i(0);

        /// <summary>
        /// A vector of ones.
        /// </summary>
        public readonly static Vector3i One = new Vector3i(1);

        /// <summary>
        /// A vector of min REAL.
        /// </summary>
        public readonly static Vector3i MinInt = new Vector3i(REAL.MinValue);

        /// <summary>
        /// A vector of max REAL.
        /// </summary>
        public readonly static Vector3i MaxInt = new Vector3i(REAL.MaxValue);

        /// <summary>
        /// Convert to a 2 dimension vector.
        /// </summary>
        public Vector2i xy
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector2i(x, y); }
        }

        /// <summary>
        /// Convert to a 2 dimension vector.
        /// </summary>
        public Vector2i xz
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector2i(x, z); }
        }

        /// <summary>
        /// Convert to a 2 dimension vector.
        /// </summary>
        public Vector2i zy
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector2i(z, y); }
        }

        /// <summary>
        /// Convert to a 2 dimension vector.
        /// </summary>
        public Vector2i ab
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector2i(a, b); }
        }

        /// <summary>
        /// Convert to a 2 dimension vector.
        /// </summary>
        public Vector2i bc
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector2i(b, c); }
        }

        /// <summary>
        /// Convert to a 2 dimension vector.
        /// </summary>
        public Vector2i ca
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector2i(c, a); }
        }

        /// <summary>
        /// Convert to a 4 dimension vector.
        /// </summary>
        public Vector4i xyz0
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector4i(x, y, z, 0); }
        }

        /// <summary>
        /// Convert to a 4 dimension vector.
        /// </summary>
        public Vector4i xyz1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector4i(x, y, z, 1); }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3i(REAL v)
        {
            this.x = v;
            this.y = v;
            this.z = v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3i(REAL x, REAL y, REAL z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3i(REAL x, REAL y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;
        }

        unsafe public REAL this[int i]
        {
            get
            {
                if ((uint)i >= 3)
                    throw new IndexOutOfRangeException("Vector3i index out of range.");

                fixed (Vector3i* array = &this) { return ((REAL*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 3)
                    throw new IndexOutOfRangeException("Vector3i index out of range.");

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
                return x + y + z;
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
                return x * y * z;
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
                return DMath.SafeSqrt(SqrMagnitude);
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
                return (x * x + y * y + z * z);
            }
        }

        /// <summary>
        /// The vectors absolute values.
        /// </summary>
        public Vector3i Absolute
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new Vector3i(Math.Abs(x), Math.Abs(y), Math.Abs(z));
            }
        }

        /// <summary>
        /// Add two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3i operator +(Vector3i v1, Vector3i v2)
        {
            return new Vector3i(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        /// <summary>
        /// Add vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3i operator +(Vector3i v1, REAL s)
        {
            return new Vector3i(v1.x + s, v1.y + s, v1.z + s);
        }

        /// <summary>
        /// Add vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3i operator +(REAL s, Vector3i v1)
        {
            return new Vector3i(v1.x + s, v1.y + s, v1.z + s);
        }

        /// <summary>
        /// Negate vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3i operator -(Vector3i v)
        {
            return new Vector3i(-v.x, -v.y, -v.z);
        }

        /// <summary>
        /// Subtract two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3i operator -(Vector3i v1, Vector3i v2)
        {
            return new Vector3i(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        /// <summary>
        /// Subtract vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3i operator -(Vector3i v1, REAL s)
        {
            return new Vector3i(v1.x - s, v1.y - s, v1.z - s);
        }

        /// <summary>
        /// Subtract vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3i operator -(REAL s, Vector3i v1)
        {
            return new Vector3i(s - v1.x, s - v1.y, s - v1.z);
        }

        /// <summary>
        /// Multiply two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3i operator *(Vector3i v1, Vector3i v2)
        {
            return new Vector3i(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        /// <summary>
        /// Multiply a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3i operator *(Vector3i v, REAL s)
        {
            return new Vector3i(v.x * s, v.y * s, v.z * s);
        }

        /// <summary>
        /// Multiply a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3i operator *(REAL s, Vector3i v)
        {
            return new Vector3i(v.x * s, v.y * s, v.z * s);
        }

        /// <summary>
        /// Divide two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3i operator /(Vector3i v1, Vector3i v2)
        {
            return new Vector3i(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
        }

        /// <summary>
        /// Divide a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3i operator /(Vector3i v, REAL s)
        {
            return new Vector3i(v.x / s, v.y / s, v.z / s);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector3i(Vector3f v)
        {
            return new Vector3i((REAL)v.x, (REAL)v.y, (REAL)v.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector3i(Vector3d v)
        {
            return new Vector3i((REAL)v.x, (REAL)v.y, (REAL)v.z);
        }

        /// <summary>
        /// Are these vectors equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector3i v1, Vector3i v2)
		{
			return (v1.x == v2.x && v1.y == v2.y && v1.z == v2.z);
		}

        /// <summary>
        /// Are these vectors not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector3i v1, Vector3i v2)
		{
			return (v1.x != v2.x || v1.y != v2.y || v1.z != v2.z);
		}

        /// <summary>
        /// Are these vectors equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals (object obj)
		{
			if(!(obj is Vector3i)) return false;
			Vector3i v = (Vector3i)obj;
			return this == v;
		}

        /// <summary>
        /// Are these vectors equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector3i v)
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
                hash = (hash * 16777619) ^ z.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Compare two vectors by axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(Vector3i other)
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
        static public Vector3i FromString(string s)
		{
            Vector3i v = new Vector3i();
            try
            {
                string[] separators = new string[] { "," };
                string[] result = s.Split(separators, StringSplitOptions.None);

                v.x = REAL.Parse(result[0]);
                v.y = REAL.Parse(result[1]);
                v.z = REAL.Parse(result[2]);
            }
            catch { }
			
			return v;
		}

        /// <summary>
        /// The dot product of two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static REAL Dot(Vector3i v0, Vector3i v1)
        {
            return (v0.x * v1.x + v0.y * v1.y + v0.z * v1.z);
        }

        /// <summary>
        /// Cross two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3i Cross(Vector3i v0, Vector3i v1)
        {
            return new Vector3i(v0.y * v1.z - v0.z * v1.y, v0.z * v1.x - v0.x * v1.z, v0.x * v1.y - v0.y * v1.x);
        }

        /// <summary>
        /// Distance between two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Distance(Vector3i v0, Vector3i v1)
        {
            return DMath.SafeSqrt(SqrDistance(v0, v1));
        }

        /// <summary>
        /// Square distance between two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double SqrDistance(Vector3i v0, Vector3i v1)
        {
            double x = v0.x - v1.x;
            double y = v0.y - v1.y;
            double z = v0.z - v1.z;
            return x * x + y * y + z * z;
        }

        /// <summary>
        /// The minimum value between s and each component in vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3i Min(Vector3i v, REAL s)
        {
            v.x = Math.Min(v.x, s);
            v.y = Math.Min(v.y, s);
            v.z = Math.Min(v.z, s);
            return v;
        }

        /// <summary>
        /// The minimum value between each component in vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3i Min(Vector3i v0, Vector3i v1)
        {
            v0.x = Math.Min(v0.x, v1.x);
            v0.y = Math.Min(v0.y, v1.y);
            v0.z = Math.Min(v0.z, v1.z);
            return v0;
        }

        /// <summary>
        /// The maximum value between s and each component in vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3i Max(Vector3i v, REAL s)
        {
            v.x = Math.Max(v.x, s);
            v.y = Math.Max(v.y, s);
            v.z = Math.Max(v.z, s);
            return v;
        }

        /// <summary>
        /// The maximum value between each component in vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3i Max(Vector3i v0, Vector3i v1)
        {
            v0.x = Math.Max(v0.x, v1.x);
            v0.y = Math.Max(v0.y, v1.y);
            v0.z = Math.Max(v0.z, v1.z);
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
            z = Math.Abs(z);
        }

        /// <summary>
        /// Clamp the each component to specified min and max.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clamp(REAL min, REAL max)
        {
            x = Math.Max(Math.Min(x, max), min);
            y = Math.Max(Math.Min(y, max), min);
            z = Math.Max(Math.Min(z, max), min);
        }

        /// <summary>
        /// Clamp the each component to specified min and max.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clamp(Vector3i min, Vector3i max)
        {
            x = Math.Max(Math.Min(x, max.x), min.x);
            y = Math.Max(Math.Min(y, max.y), min.y);
            z = Math.Max(Math.Min(z, max.z), min.z);
        }

    }
	
}









