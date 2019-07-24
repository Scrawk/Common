using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using Common.Core.Numerics;

using REAL = System.Double;

namespace Common.Core.Numerics
{

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3d : IEquatable<Vector3d>, IComparable<Vector3d>
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
        public readonly static Vector3d UnitX = new Vector3d(1, 0, 0);

        /// <summary>
        /// The unit y vector.
        /// </summary>
	    public readonly static Vector3d UnitY = new Vector3d(0, 1, 0);

        /// <summary>
        /// The unit z vector.
        /// </summary>
	    public readonly static Vector3d UnitZ = new Vector3d(0, 0, 1);

        /// <summary>
        /// A vector of zeros.
        /// </summary>
	    public readonly static Vector3d Zero = new Vector3d(0);

        /// <summary>
        /// A vector of ones.
        /// </summary>
        public readonly static Vector3d One = new Vector3d(1);

        /// <summary>
        /// A vector of 0.5.
        /// </summary>
        public readonly static Vector3d Half = new Vector3d(0.5);

        /// <summary>
        /// A vector of positive infinity.
        /// </summary>
        public readonly static Vector3d PositiveInfinity = new Vector3d(REAL.PositiveInfinity);

        /// <summary>
        /// A vector of negative infinity.
        /// </summary>
        public readonly static Vector3d NegativeInfinity = new Vector3d(REAL.NegativeInfinity);

        /// <summary>
        /// Convert to a 2 dimension vector.
        /// </summary>
	    public Vector2d xy
	    {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector2d(x, y); }
	    }

        /// <summary>
        /// Convert to a 2 dimension vector.
        /// </summary>
        public Vector2d xz
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector2d(x, z); }
        }

        /// <summary>
        /// Convert to a 2 dimension vector.
        /// </summary>
        public Vector2d zy
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector2d(z, y); }
        }

        /// <summary>
        /// Convert to a 2 dimension vector.
        /// </summary>
        public Vector2d ab
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector2d(a, b); }
        }

        /// <summary>
        /// Convert to a 2 dimension vector.
        /// </summary>
        public Vector2d bc
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector2d(b, c); }
        }

        /// <summary>
        /// Convert to a 2 dimension vector.
        /// </summary>
        public Vector2d ca
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector2d(c, a); }
        }

        /// <summary>
        /// Convert to a 4 dimension vector.
        /// </summary>
        public Vector4d xyz0
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector4d(x, y, z, 0); }
        }

        /// <summary>
        /// Convert to a 4 dimension vector.
        /// </summary>
        public Vector4d xyz1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector4d(x, y, z, 1); }
        }

        /// <summary>
        /// A vector all with the value v.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3d(REAL v) 
		{
			this.x = v; 
			this.y = v; 
			this.z = v;
		}

        /// <summary>
        /// A vector from the varibles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3d(REAL x, REAL y, REAL z) 
		{
			this.x = x; 
			this.y = y;
			this.z = z;
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3d(REAL x, REAL y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;
        }

        /// <summary>
        /// A vector from a 2d vector and the z varible.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3d(Vector2d v, REAL z) 
		{ 
			x = v.x; 
			y = v.y; 
			this.z = z;
		}

        unsafe public REAL this[int i]
        {
            get
            {
                if ((uint)i >= 3)
                    throw new IndexOutOfRangeException("Vector3d index out of range.");

                fixed (Vector3d* array = &this) { return ((REAL*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 3)
                    throw new IndexOutOfRangeException("Vector3d index out of range.");

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
        public REAL Magnitude
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
        /// The vector normalized.
        /// </summary>
        public Vector3d Normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                REAL invLength = DMath.SafeInvSqrt(1.0, x * x + y * y + z * z);
                return new Vector3d(x * invLength, y * invLength, z * invLength);
            }
        }

        /// <summary>
        /// The vectors absolute values.
        /// </summary>
        public Vector3d Absolute
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new Vector3d(Math.Abs(x), Math.Abs(y), Math.Abs(z));
            }
        }

        /// <summary>
        /// Convert a normalized vector to tangent space.
        /// </summary>
        public Vector3d TangentSpaceNormal
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                REAL x = this.x * 0.5 + 0.5;
                REAL y = this.z * 0.5 + 0.5;
                REAL z = this.y;

                return new Vector3d(x, y, z);
            }
        }

        /// <summary>
        /// Add two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator +(Vector3d v1, Vector3d v2)
        {
            return new Vector3d(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        /// <summary>
        /// Add vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator +(Vector3d v1, REAL s)
        {
            return new Vector3d(v1.x + s, v1.y + s, v1.z + s);
        }

        /// <summary>
        /// Add vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator +(REAL s, Vector3d v1)
        {
            return new Vector3d(v1.x + s, v1.y + s, v1.z + s);
        }

        /// <summary>
        /// Negate vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator -(Vector3d v)
        {
            return new Vector3d(-v.x, -v.y, -v.z);
        }

        /// <summary>
        /// Subtract two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator -(Vector3d v1, Vector3d v2)
        {
            return new Vector3d(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        /// <summary>
        /// Subtract vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator -(Vector3d v1, REAL s)
        {
            return new Vector3d(v1.x - s, v1.y - s, v1.z - s);
        }

        /// <summary>
        /// Subtract vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator -(REAL s, Vector3d v1)
        {
            return new Vector3d(s - v1.x, s - v1.y, s - v1.z);
        }

        /// <summary>
        /// Multiply two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator *(Vector3d v1, Vector3d v2)
        {
            return new Vector3d(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        /// <summary>
        /// Multiply a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator *(Vector3d v, REAL s)
        {
            return new Vector3d(v.x * s, v.y * s, v.z * s);
        }

        /// <summary>
        /// Multiply a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator *(REAL s, Vector3d v)
        {
            return new Vector3d(v.x * s, v.y * s, v.z * s);
        }

        /// <summary>
        /// Divide two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator /(Vector3d v1, Vector3d v2)
        {
            return new Vector3d(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
        }

        /// <summary>
        /// Divide a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator /(Vector3d v, REAL s)
        {
            return new Vector3d(v.x / s, v.y / s, v.z / s);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector3d(Vector3f v)
        {
            return new Vector3d(v.x, v.y, v.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector3d(Vector3i v)
        {
            return new Vector3d(v.x, v.y, v.z);
        }

        /// <summary>
        /// Are these vectors equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector3d v1, Vector3d v2)
		{
			return (v1.x == v2.x && v1.y == v2.y && v1.z == v2.z);
		}

        /// <summary>
        /// Are these vectors not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector3d v1, Vector3d v2)
		{
			return (v1.x != v2.x || v1.y != v2.y || v1.z != v2.z);
		}

        /// <summary>
        /// Are these vectors equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals (object obj)
		{
			if(!(obj is Vector3d)) return false;
			Vector3d v = (Vector3d)obj;
			return this == v;
		}

        /// <summary>
        /// Are these vectors equal given the error.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool EqualsWithError(Vector3d v, REAL eps)
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
        public bool Equals(Vector3d v)
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
        public int CompareTo(Vector3d other)
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
        static public Vector3d FromString(string s)
		{
            Vector3d v = new Vector3d();

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
        public static REAL Dot(Vector3d v0, Vector3d v1)
		{
			return (v0.x*v1.x + v0.y*v1.y + v0.z*v1.z);
		}

        /// <summary>
        /// Normalize the vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
		{
            REAL invLength = DMath.SafeInvSqrt(1.0, x * x + y * y + z * z);
	    	x *= invLength;
			y *= invLength;
			z *= invLength;
		}

        /// <summary>
        /// Angle between two vectors in degrees from 0 to 180.
        /// A and b origin treated as 0,0 and do not need to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static REAL Angle180(Vector3d a, Vector3d b)
        {
            REAL dp = Vector3d.Dot(a, b);
            REAL m = a.Magnitude * b.Magnitude;
            return DMath.SafeAcos(DMath.SafeDiv(dp, m)) * DMath.Rad2Deg;
        }

        /// <summary>
        /// Cross two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3d Cross(Vector3d v)
		{
			return new Vector3d(y*v.z - z*v.y, z*v.x - x*v.z, x*v.y - y*v.x);
		}

        /// <summary>
        /// Cross two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d Cross(Vector3d v0, Vector3d v1)
		{
			return new Vector3d(v0.y*v1.z - v0.z*v1.y, v0.z*v1.x - v0.x*v1.z, v0.x*v1.y - v0.y*v1.x);
		}

        /// <summary>
        /// Distance between two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static REAL Distance(Vector3d v0, Vector3d v1)
        {
            return DMath.SafeSqrt(SqrDistance(v0, v1));
        }

        /// <summary>
        /// Square distance between two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static REAL SqrDistance(Vector3d v0, Vector3d v1)
        {
            REAL x = v0.x - v1.x;
            REAL y = v0.y - v1.y;
            REAL z = v0.z - v1.z;
            return x * x + y * y + z * z;
        }

        /// <summary>
        /// Project v on to normal n.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d Project(Vector3d v, Vector3d n)
        {
            return Dot(v, n) * n;
        }

        /// <summary>
        /// Given an incident vector i and a normal vector n.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d Reflect(Vector3d i, Vector3d n)
        {
            return i - 2 * n * Dot(i, n);
        }

        /// <summary>
        /// Returns the refraction vector given the incident vector i, 
        /// the normal vector n and the refraction index eta.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d Refract(Vector3d i, Vector3d n, float eta)
        {
            REAL ni = Dot(n, i);
            REAL k = 1.0f - eta * eta * (1.0f - ni * ni);

            return (k >= 0) ? eta * i - (eta * ni + DMath.SafeSqrt(k)) * n : Zero;
        }

        /// <summary>
        /// The minimum value between s and each component in vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d Min(Vector3d v, REAL s)
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
        public static Vector3d Min(Vector3d v0, Vector3d v1)
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
        public static Vector3d Max(Vector3d v, REAL s)
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
        public static Vector3d Max(Vector3d v0, Vector3d v1)
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
        public void Clamp(Vector3d min, Vector3d max)
        {
            x = Math.Max(Math.Min(x, max.x), min.x);
            y = Math.Max(Math.Min(y, max.y), min.y);
            z = Math.Max(Math.Min(z, max.z), min.z);
        }

        /// <summary>
        /// Lerp between two vectors.
        /// </summary>
        public static Vector3d Lerp(Vector3d from, Vector3d to, REAL t)
        {
            if (t < 0.0) t = 0.0;
            if (t > 1.0) t = 1.0;

            if (t == 0.0) return from;
            if (t == 1.0) return to;

            REAL t1 = 1.0 - t;
            Vector3d v = new Vector3d();
            v.x = from.x * t1 + to.x * t;
            v.y = from.y * t1 + to.y * t;
            v.z = from.z * t1 + to.z * t;
            return v;
        }

        /// <summary>
        /// Slerp between two vectors arc.
        /// </summary>
        public static Vector3d Slerp(Vector3d from, Vector3d to, REAL t)
        {
            if (t < 0.0f) t = 0.0f;
            if (t > 1.0f) t = 1.0f;

            if (t == 0.0f) return from;
            if (t == 1.0f) return to;
            if (to.x == from.x && to.y == from.y && to.z == from.z) return to;

            REAL m = from.Magnitude * to.Magnitude;
            if (DMath.IsZero(m)) return Vector3d.Zero;

            REAL theta = Math.Acos(Dot(from, to) / m);

            if (theta == 0.0) return to;

            REAL sinTheta = Math.Sin(theta);
            REAL st1 = Math.Sin((1.0 - t) * theta) / sinTheta;
            REAL st = Math.Sin(t * theta) / sinTheta;

            Vector3d v = new Vector3d();
            v.x = from.x * st1 + to.x * st;
            v.y = from.y * st1 + to.y * st;
            v.z = from.z * st1 + to.z * st;

            return v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Round(int digits = 0)
        {
            x = Math.Round(x, digits);
            y = Math.Round(y, digits);
            z = Math.Round(z, digits);
        }

    }

}


































