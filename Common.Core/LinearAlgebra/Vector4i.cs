using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using Common.Core.Mathematics;

namespace Common.Core.LinearAlgebra
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector4i : IEquatable<Vector4i>, IComparable<Vector4i>
	{
		public int x, y, z, w;

        /// <summary>
        /// The unit x vector.
        /// </summary>
	    public readonly static Vector4i UnitX = new Vector4i(1, 0, 0, 0);

        /// <summary>
        /// The unit y vector.
        /// </summary>
	    public readonly static Vector4i UnitY = new Vector4i(0, 1, 0, 0);

        /// <summary>
        /// The unit z vector.
        /// </summary>
	    public readonly static Vector4i UnitZ = new Vector4i(0, 0, 1, 0);

        /// <summary>
        /// The unit w vector.
        /// </summary>
	    public readonly static Vector4i UnitW = new Vector4i(0, 0, 0, 1);

        /// <summary>
        /// A vector of zeros.
        /// </summary>
	    public readonly static Vector4i Zero = new Vector4i(0);

        /// <summary>
        /// A vector of ones.
        /// </summary>
	    public readonly static Vector4i One = new Vector4i(1);

        /// <summary>
        /// A vector of min int.
        /// </summary>
        public readonly static Vector4i MinInt = new Vector4i(int.MinValue);

        /// <summary>
        /// A vector of max int.
        /// </summary>
        public readonly static Vector4i MaxInt = new Vector4i(int.MaxValue);

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
        /// Convert to a 3 dimension vector.
        /// </summary>
        public Vector3i xyz
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector3i(x, y, z); }
        }

        /// <summary>
        /// A copy of the vector with w as 0.
        /// </summary>
        public Vector4i xyz0
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector4i(x, y, z, 0); }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4i(int v)
        {
            this.x = v;
            this.y = v;
            this.z = v;
            this.w = v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4i(int x, int y, int z, int w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

        public int this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    case 3: return w;
                    default: throw new IndexOutOfRangeException("Vector4i index out of range: " + i);
                }
            }
            set
            {
                switch (i)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                    case 3: w = value; break;
                    default: throw new IndexOutOfRangeException("Vector4i index out of range: " + i);
                }
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
        public int SqrMagnitude
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return (x * x + y * y + z * z + w * w);
            }
        }

        /// <summary>
        /// The vectors absolute values.
        /// </summary>
        public Vector4i Absolute
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new Vector4i(Math.Abs(x), Math.Abs(y), Math.Abs(z), Math.Abs(w));
            }
        }

        /// <summary>
        /// Add two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4i operator +(Vector4i v1, Vector4i v2)
        {
            return new Vector4i(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z, v1.w + v2.w);
        }

        /// <summary>
        /// Add vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4i operator +(Vector4i v1, int s)
        {
            return new Vector4i(v1.x + s, v1.y + s, v1.z + s, v1.w + s);
        }

        /// <summary>
        /// Add vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4i operator +(int s, Vector4i v1)
        {
            return new Vector4i(v1.x + s, v1.y + s, v1.z + s, v1.w + s);
        }

        /// <summary>
        /// Negate vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4i operator -(Vector4i v)
        {
            return new Vector4i(-v.x, -v.y, -v.z, -v.w);
        }

        /// <summary>
        /// Subtract two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4i operator -(Vector4i v1, Vector4i v2)
        {
            return new Vector4i(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z, v1.w - v2.w);
        }

        /// <summary>
        /// Subtract vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4i operator -(Vector4i v1, int s)
        {
            return new Vector4i(v1.x - s, v1.y - s, v1.z - s, v1.w - s);
        }

        /// <summary>
        /// Subtract vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4i operator -(int s, Vector4i v1)
        {
            return new Vector4i(v1.x - s, v1.y - s, v1.z - s, v1.w - s);
        }

        /// <summary>
        /// Multiply two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4i operator *(Vector4i v1, Vector4i v2)
        {
            return new Vector4i(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z, v1.w * v2.w);
        }

        /// <summary>
        /// Multiply a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4i operator *(Vector4i v, int s)
        {
            return new Vector4i(v.x * s, v.y * s, v.z * s, v.w * s);
        }

        /// <summary>
        /// Multiply a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4i operator *(int s, Vector4i v)
        {
            return new Vector4i(v.x * s, v.y * s, v.z * s, v.w * s);
        }

        /// <summary>
        /// Divide two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4i operator /(Vector4i v1, Vector4i v2)
        {
            return new Vector4i(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z, v1.w / v2.w);
        }

        /// <summary>
        /// Divide a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4i operator /(Vector4i v, int s)
        {
            return new Vector4i(v.x / s, v.y / s, v.z / s, v.w / s);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector4i(Vector4f v)
        {
            return new Vector4i((int)v.x, (int)v.y, (int)v.z, (int)v.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector4i(Vector4d v)
        {
            return new Vector4i((int)v.x, (int)v.y, (int)v.z, (int)v.w);
        }

        /// <summary>
        /// Are these vectors equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector4i v1, Vector4i v2)
		{
			return (v1.x == v2.x && v1.y == v2.y && v1.z == v2.z && v1.w == v2.w);
		}

        /// <summary>
        /// Are these vectors not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector4i v1, Vector4i v2)
		{
			return (v1.x != v2.x || v1.y != v2.y || v1.z != v2.z || v1.w != v2.w);
		}

        /// <summary>
        /// Are these vectors equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals (object obj)
		{
			if(!(obj is Vector4i)) return false;
			Vector4i v = (Vector4i)obj;
			return this == v;
		}

        /// <summary>
        /// Are these vectors equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector4i v)
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
                hash = (hash * 16777619) ^ w.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Compare two vectors by axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(Vector4i other)
        {
            if (x != other.x)
                return x < other.x ? -1 : 1;
            else if (y != other.y)
                return y < other.y ? -1 : 1;
            else if (z != other.z)
                return z < other.z ? -1 : 1;
            else if (w != other.w)
                return w < other.w ? -1 : 1;
            return 0;
        }

        /// <summary>
        /// Vector as a string.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
		{
			return string.Format("{0},{1},{2},{3}", x, y, z, w);
		}

        /// <summary>
        /// Vector as a string.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(string f)
        {
            return string.Format("{0},{1},{2},{3}", x.ToString(f), y.ToString(f), z.ToString(f), w.ToString(f));
        }

        /// <summary>
        /// Vector from a string.
        /// </summary>
        public static Vector4i FromString(string s)
		{
            Vector4i v = new Vector4i();

            try
            {
                string[] separators = new string[] { "," };
                string[] result = s.Split(separators, StringSplitOptions.None);

                v.x = int.Parse(result[0]);
                v.y = int.Parse(result[1]);
                v.z = int.Parse(result[2]);
                v.w = int.Parse(result[3]);
            }
            catch { }
			
			return v;
		}

        /// <summary>
        /// The dot product of two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Dot(Vector4i v0, Vector4i v1)
        {
            return (v0.x * v1.x + v0.y * v1.y + v0.z * v1.z + v0.w * v1.w);
        }

        /// <summary>
        /// Distance between two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Distance(Vector4i v0, Vector4i v1)
        {
            return DMath.SafeSqrt(SqrDistance(v0, v1));
        }

        /// <summary>
        /// Square distance between two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double SqrDistance(Vector4i v0, Vector4i v1)
        {
            double x = v0.x - v1.x;
            double y = v0.y - v1.y;
            double z = v0.z - v1.z;
            double w = v0.w - v1.w;
            return x * x + y * y + z * z + w * w;
        }

        /// <summary>
        /// The minimum value between s and each component in vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Min(int s)
        {
            x = Math.Min(x, s);
            y = Math.Min(y, s);
            z = Math.Min(z, s);
            w = Math.Min(w, s);
        }

        /// <summary>
        /// The minimum value between each component in vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Min(Vector4i v)
        {
            x = Math.Min(x, v.x);
            y = Math.Min(y, v.y);
            z = Math.Min(z, v.z);
            w = Math.Min(w, v.w);
        }

        /// <summary>
        /// The maximum value between s and each component in vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Max(int s)
        {
            x = Math.Max(x, s);
            y = Math.Max(y, s);
            z = Math.Max(z, s);
            w = Math.Max(w, s);
        }

        /// <summary>
        /// The maximum value between each component in vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Max(Vector4i v)
        {
            x = Math.Max(x, v.x);
            y = Math.Max(y, v.y);
            z = Math.Max(z, v.z);
            w = Math.Max(w, v.w);
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
            w = Math.Abs(w);
        }

        /// <summary>
        /// Clamp the each component to specified min and max.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clamp(int min, int max)
        {
            x = Math.Max(Math.Min(x, max), min);
            y = Math.Max(Math.Min(y, max), min);
            z = Math.Max(Math.Min(z, max), min);
            w = Math.Max(Math.Min(w, max), min);
        }

        /// <summary>
        /// Clamp the each component to specified min and max.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clamp(Vector4i min, Vector4i max)
        {
            x = Math.Max(Math.Min(x, max.x), min.x);
            y = Math.Max(Math.Min(y, max.y), min.y);
            z = Math.Max(Math.Min(z, max.z), min.z);
            w = Math.Max(Math.Min(w, max.w), min.w);
        }

    }
	
}
















