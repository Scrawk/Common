using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using REAL = System.Int32;

namespace Common.Core.Numerics
{

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Point3i : IEquatable<Point3i>
    {
        public REAL x, y, z;

        /// <summary>
        /// The dimension is the number components in the point.
        /// </summary>
        public const int Dimension = 3;

        /// <summary>
        /// The unit x point.
        /// </summary>
	    public readonly static Point3i UnitX = new Point3i(1, 0, 0);

        /// <summary>
        /// The unit y point.
        /// </summary>
	    public readonly static Point3i UnitY = new Point3i(0, 1, 0);

        /// <summary>
        /// The unit z point.
        /// </summary>
        public readonly static Point3i UnitZ = new Point3i(0, 0, 1);

        /// <summary>
        /// A point of zeros.
        /// </summary>
	    public readonly static Point3i Zero = new Point3i(0);

        /// <summary>
        /// A point of ones.
        /// </summary>
	    public readonly static Point3i One = new Point3i(1);

        /// <summary>
        /// A point of positive infinity.
        /// </summary>
        public readonly static Point3i MaxValue = new Point3i(REAL.MaxValue);

        /// <summary>
        /// A point of negative infinity.
        /// </summary>
        public readonly static Point3i MinValue = new Point3i(REAL.MinValue);

        /// <summary>
        /// 3D point to 3D swizzle point.
        /// </summary>
        public Point3i xzy => new Point3i(x, z, y);

        /// <summary>
        /// 3D point to 2D point.
        /// </summary>
        public Point2i xy => new Point2i(x, y);

        /// <summary>
        /// 3D point to 2D point.
        /// </summary>
        public Point2i xz => new Point2i(x, z);

        /// <summary>
        /// 3D point to 4D point with w as 0.
        /// </summary>
        public Point4i xyz0 => new Point4i(x, y, z, 0);

        /// <summary>
        /// 3D point to 4D point with w as 1.
        /// </summary>
        public Point4i xyz1 => new Point4i(x, y, z, 1);

        /// <summary>
        /// A point all with the value v.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3i(REAL v)
        {
            this.x = v;
            this.y = v;
            this.z = v;
        }

        /// <summary>
        /// A point from the varibles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3i(REAL x, REAL y, REAL z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// A point from the varibles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3i(float x, float y, float z)
        {
            this.x = (REAL)x;
            this.y = (REAL)y;
            this.z = (REAL)z;
        }

        /// <summary>
        /// A point from the varibles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3i(double x, double y, double z)
        {
            this.x = (REAL)x;
            this.y = (REAL)y;
            this.z = (REAL)z;
        }

        /// <summary>
        /// Array accessor for variables. 
        /// </summary>
        /// <param name="i">The variables index.</param>
        /// <returns>The variable value</returns>
        unsafe public REAL this[int i]
        {
            get
            {
                if ((uint)i >= 3)
                    throw new IndexOutOfRangeException("Point3i index out of range.");

                fixed (Point3i* array = &this) { return ((REAL*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 3)
                    throw new IndexOutOfRangeException("Point3i index out of range.");

                fixed (REAL* array = &x) { array[i] = value; }
            }
        }

        /// <summary>
        /// The sum of the points components.
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
        /// The product of the points components.
        /// </summary>
        public REAL Product
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return x * y * z;
            }
        }

        /// <summary>
        /// The points absolute values.
        /// </summary>
        public Point3i Absolute
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new Point3i(Math.Abs(x), Math.Abs(y), Math.Abs(z));
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
        /// Add two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3i operator +(Point3i v1, Point3i v2)
        {
            return new Point3i(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        /// <summary>
        /// Add point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3i operator +(Point3i v1, REAL s)
        {
            return new Point3i(v1.x + s, v1.y + s, v1.z + s);
        }

        /// <summary>
        /// Add point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3i operator +(REAL s, Point3i v1)
        {
            return new Point3i(s + v1.x, s + v1.y, s + v1.z);
        }

        /// <summary>
        /// Negate point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3i operator -(Point3i v)
        {
            return new Point3i(-v.x, -v.y, -v.z);
        }

        /// <summary>
        /// Subtract two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3i operator -(Point3i v1, Point3i v2)
        {
            return new Point3i(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        /// <summary>
        /// Subtract point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3i operator -(Point3i v1, REAL s)
        {
            return new Point3i(v1.x - s, v1.y - s, v1.z - s);
        }

        /// <summary>
        /// Subtract point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3i operator -(REAL s, Point3i v1)
        {
            return new Point3i(s - v1.x, s - v1.y, s - v1.z);
        }

        /// <summary>
        /// Multiply two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3i operator *(Point3i v1, Point3i v2)
        {
            return new Point3i(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        /// <summary>
        /// Multiply a point and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3i operator *(Point3i v, REAL s)
        {
            return new Point3i(v.x * s, v.y * s, v.z * s);
        }

        /// <summary>
        /// Multiply a point and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3i operator *(REAL s, Point3i v)
        {
            return new Point3i(v.x * s, v.y * s, v.z * s);
        }

        /// <summary>
        /// Divide two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3i operator /(Point3i v1, Point3i v2)
        {
            return new Point3i(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
        }

        /// <summary>
        /// Divide a point and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3i operator /(Point3i v, REAL s)
        {
            return new Point3i(v.x / s, v.y / s, v.z / s);
        }

        /// <summary>
        /// Divide a scalar and a point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3i operator /(REAL s, Point3i v)
        {
            return new Point3i(s / v.x, s / v.y, s / v.z);
        }

        /// <summary>
        /// Cast from Point3f to Point3i.
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Point3i(Point3f v)
        {
            return new Point3i(v.x, v.y, v.z);
        }

        /// <summary>
        /// Cast from Point3d to Point3i.
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Point3i(Point3d v)
        {
            return new Point3i(v.x, v.y, v.z);
        }
        /// <summary>
        /// Cast from Vector3f to Point3i.
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Point3i(Vector3f v)
        {
            return new Point3i(v.x, v.y, v.z);
        }

        /// <summary>
        /// Cast from Vector3d to Point3i.
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Point3i(Vector3d v)
        {
            return new Point3i(v.x, v.y, v.z);
        }

        /// <summary>
        /// Are these points equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Point3i v1, Point3i v2)
        {
            return (v1.x == v2.x && v1.y == v2.y && v1.z == v2.z);
        }

        /// <summary>
        /// Are these points not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Point3i v1, Point3i v2)
        {
            return (v1.x != v2.x || v1.y != v2.y || v1.z != v2.z);
        }

        /// <summary>
        /// Are these points equal.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is Point3i)) return false;
            Point3i v = (Point3i)obj;
            return this == v;
        }

        /// <summary>
        /// Are these points equal.
        /// </summary>
        public bool Equals(Point3i v)
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
                int hash = (int)MathUtil.HASH_PRIME_1;
                hash = (hash * MathUtil.HASH_PRIME_2) ^ x.GetHashCode();
                hash = (hash * MathUtil.HASH_PRIME_2) ^ y.GetHashCode();
                hash = (hash * MathUtil.HASH_PRIME_2) ^ z.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Vector as a string.
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0},{1},{2}", x, y, z);
        }

        /// <summary>
        /// Vector as a string.
        /// </summary>
        public string ToString(string f)
        {
            return string.Format("{0},{1},{2}", x.ToString(f), y.ToString(f), z.ToString(f));
        }

        /// <summary>
        /// Distance between two points.
        /// </summary>
        public static double Distance(Point3i v0, Point3i v1)
        {
            return MathUtil.Sqrt(SqrDistance(v0, v1));
        }

        /// <summary>
        /// Square distance between two points.
        /// </summary>
        public static REAL SqrDistance(Point3i v0, Point3i v1)
        {
            REAL x = v0.x - v1.x;
            REAL y = v0.y - v1.y;
            REAL z = v0.z - v1.z;
            return x * x + y * y + z * z;
        }

        /// <summary>
        /// The minimum value between s and each component in point.
        /// </summary>
        public static Point3i Min(Point3i v, REAL s)
        {
            v.x = MathUtil.Min(v.x, s);
            v.y = MathUtil.Min(v.y, s);
            v.z = MathUtil.Min(v.z, s);
            return v;
        }

        /// <summary>
        /// The minimum value between each component in points.
        /// </summary>
        public static Point3i Min(Point3i v0, Point3i v1)
        {
            v0.x = MathUtil.Min(v0.x, v1.x);
            v0.y = MathUtil.Min(v0.y, v1.y);
            v0.z = MathUtil.Min(v0.z, v1.z);
            return v0;
        }

        /// <summary>
        /// The maximum value between s and each component in point.
        /// </summary>
        public static Point3i Max(Point3i v, REAL s)
        {
            v.x = MathUtil.Max(v.x, s);
            v.y = MathUtil.Max(v.y, s);
            v.z = MathUtil.Max(v.z, s);
            return v;
        }

        /// <summary>
        /// The maximum value between each component in points.
        /// </summary>
        public static Point3i Max(Point3i v0, Point3i v1)
        {
            v0.x = MathUtil.Max(v0.x, v1.x);
            v0.y = MathUtil.Max(v0.y, v1.y);
            v0.z = MathUtil.Max(v0.z, v1.z);
            return v0;
        }

        /// <summary>
        /// Clamp each component to specified min and max.
        /// </summary>
        public static Point3i Clamp(Point3i v, REAL min, REAL max)
        {
            v.x = MathUtil.Max(MathUtil.Min(v.x, max), min);
            v.y = MathUtil.Max(MathUtil.Min(v.y, max), min);
            v.z = MathUtil.Max(MathUtil.Min(v.z, max), min);
            return v;
        }

        /// <summary>
        /// Clamp each component to specified min and max.
        /// </summary>
        public static Point3i Clamp(Point3i v, Point3i min, Point3i max)
        {
            v.x = MathUtil.Max(MathUtil.Min(v.x, max.x), min.x);
            v.y = MathUtil.Max(MathUtil.Min(v.y, max.y), min.y);
            v.z = MathUtil.Max(MathUtil.Min(v.z, max.z), min.z);
            return v;
        }

        /// <summary>
        /// The index of the min component. 
        /// </summary>
        /// <param name="abs">Should the components abs value be used.</param>
        /// <returns>The index of the min component.</returns>
        public int MinDimension(bool abs = false)
        {
            int index = 0;
            REAL min = REAL.MaxValue;

            for (int i = 0; i < Dimension; i++)
            {
                REAL v = this[i];
                if (abs) v = Math.Abs(v);

                if (v < min)
                {
                    min = v;
                    index = i;
                }
            }

            return index;
        }

        /// <summary>
        /// The index of the max component. 
        /// </summary>
        /// <param name="abs">Should the components abs value be used.</param>
        /// <returns>The index of the max component.</returns>
        public int MaxDimension(bool abs = false)
        {
            int index = 0;
            REAL max = REAL.MinValue;

            for (int i = 0; i < Dimension; i++)
            {
                REAL v = this[i];
                if (abs) v = Math.Abs(v);

                if (v > max)
                {
                    max = v;
                    index = i;
                }
            }

            return index;
        }

        /// <summary>
        /// The min component in the point. 
        /// </summary>
        /// <param name="abs">Should the components abs value be used.</param>
        /// <returns>The min components value.</returns>
        public REAL MinComponent(bool abs = false)
        {
            return this[MinDimension(abs)];
        }

        /// <summary>
        /// The max component in the point. 
        /// </summary>
        /// <param name="abs">Should the components abs value be used.</param>
        /// <returns>The max components value.</returns>
        public REAL MaxComponent(bool abs = false)
        {
            return this[MaxDimension(abs)];
        }

        /// <summary>
        /// Create a new point by reordering the componets.
        /// </summary>
        /// <param name="i">The index to take x value from.></param>
        /// <param name="j">The index to take y value from.</param>
        /// <param name="k">The index to take z value from.</param>
        /// <returns>The new vector</returns>
        public Point3i Permutate(int i, int j, int k)
        {
            return new Point3i(this[i], this[j], this[k]);
        }

    }
}
