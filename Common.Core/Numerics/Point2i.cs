using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using REAL = System.Int32;

namespace Common.Core.Numerics
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Point2i : IEquatable<Point2i>
    {
        public REAL x, y;

        /// <summary>
        /// The dimension is the number components in the point.
        /// </summary>
        public const int Dimension = 2;

        /// <summary>
        /// The unit x point.
        /// </summary>
	    public readonly static Point2i UnitX = new Point2i(1, 0);

        /// <summary>
        /// The unit y point.
        /// </summary>
	    public readonly static Point2i UnitY = new Point2i(0, 1);

        /// <summary>
        /// A point of zeros.
        /// </summary>
	    public readonly static Point2i Zero = new Point2i(0);

        /// <summary>
        /// A point of ones.
        /// </summary>
	    public readonly static Point2i One = new Point2i(1);

        /// <summary>
        /// A point of positive infinity.
        /// </summary>
        public readonly static Point2i MaxValue= new Point2i(REAL.MaxValue);

        /// <summary>
        /// A point of negative infinity.
        /// </summary>
        public readonly static Point2i MinValue= new Point2i(REAL.MinValue);

        /// <summary>
        /// 2D point to 3D point with z as 0.
        /// </summary>
        public Point3i xy0 => new Point3i(x, y, 0);

        /// <summary>
        /// 2D point to 3D point with y as z.
        /// </summary>
        public Point3i x0y => new Point3i(x,0, y);

        /// <summary>
        /// 2D point to 3D point with z as 1.
        /// </summary>
        public Point3i xy1 => new Point3i(x, y, 1);

        /// <summary>
        /// 2D point to 4D point with z as 0 and w as 0.
        /// </summary>
        public Point4i xy00 => new Point4i(x, y, 0, 0);

        /// <summary>
        /// 2D point to 4D point with z as 0 and w as 1.
        /// </summary>
        public Point4i xy01 => new Point4i(x, y, 0, 1);

        /// <summary>
        /// A point all with the value v.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2i(REAL v)
        {
            this.x = v;
            this.y = v;
        }

        /// <summary>
        /// A point from the varibles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2i(REAL x, REAL y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// A point from the varibles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2i(float x, float y)
        {
            this.x = (REAL)x;
            this.y = (REAL)y;
        }

        /// <summary>
        /// A point from the varibles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2i(double x, double y)
        {
            this.x = (REAL)x;
            this.y = (REAL)y;
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
                if ((uint)i >= Dimension)
                    throw new IndexOutOfRangeException("Point2i index out of range.");

                fixed (Point2i* array = &this) { return ((REAL*)array)[i]; }
            }
            set
            {
                if ((uint)i >= Dimension)
                    throw new IndexOutOfRangeException("Point2i index out of range.");

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
                return x + y;
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
                return x * y;
            }
        }

        /// <summary>
        /// The points absolute values.
        /// </summary>
        public Point2i Absolute
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new Point2i(Math.Abs(x), Math.Abs(y));
            }
        }

        /// <summary>
        /// The length of the point squared.
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
        /// Add two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2i operator +(Point2i v1, Point2i v2)
        {
            return new Point2i(v1.x + v2.x, v1.y + v2.y);
        }

        /// <summary>
        /// Add point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2i operator +(Point2i v1, REAL s)
        {
            return new Point2i(v1.x + s, v1.y + s);
        }

        /// <summary>
        /// Add point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2i operator +(REAL s, Point2i v1)
        {
            return new Point2i(s + v1.x, s + v1.y);
        }

        /// <summary>
        /// Negate point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2i operator -(Point2i v)
        {
            return new Point2i(-v.x, -v.y);
        }

        /// <summary>
        /// Subtract two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2i operator -(Point2i v1, Point2i v2)
        {
            return new Point2i(v1.x - v2.x, v1.y - v2.y);
        }

        /// <summary>
        /// Subtract point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2i operator -(Point2i v1, REAL s)
        {
            return new Point2i(v1.x - s, v1.y - s);
        }

        /// <summary>
        /// Subtract point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2i operator -(REAL s, Point2i v1)
        {
            return new Point2i(s - v1.x, s - v1.y);
        }

        /// <summary>
        /// Multiply two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2i operator *(Point2i v1, Point2i v2)
        {
            return new Point2i(v1.x * v2.x, v1.y * v2.y);
        }

        /// <summary>
        /// Multiply a point and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2i operator *(Point2i v, REAL s)
        {
            return new Point2i(v.x * s, v.y * s);
        }

        /// <summary>
        /// Multiply a point and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2i operator *(REAL s, Point2i v)
        {
            return new Point2i(v.x * s, v.y * s);
        }

        /// <summary>
        /// Divide two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2i operator /(Point2i v1, Point2i v2)
        {
            return new Point2i(v1.x / v2.x, v1.y / v2.y);
        }

        /// <summary>
        /// Divide a point and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2i operator /(Point2i v, REAL s)
        {
            return new Point2i(v.x / s, v.y / s);
        }

        /// <summary>
        /// Divide a scalar and a point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2i operator /(REAL s, Point2i v)
        {
            return new Point2i(s / v.x, s / v.y);
        }

        /// <summary>
        /// Cast from Point2f to Point2i.
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Point2i(Point2f v)
        {
            return new Point2i(v.x, v.y);
        }

        /// <summary>
        /// Cast from Point2d to Point2i.
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Point2i(Point2d v)
        {
            return new Point2i(v.x, v.y);
        }

        /// <summary>
        /// Cast from Vector2f to Point2i.
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Point2i(Vector2f v)
        {
            return new Point2i(v.x, v.y);
        }

        /// <summary>
        /// Cast from Vector2d to Point2i.
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Point2i(Vector2d v)
        {
            return new Point2i(v.x, v.y);
        }

        /// <summary>
        /// Are these points equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Point2i v1, Point2i v2)
        {
            return (v1.x == v2.x && v1.y == v2.y);
        }

        /// <summary>
        /// Are these points not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Point2i v1, Point2i v2)
        {
            return (v1.x != v2.x || v1.y != v2.y);
        }

        /// <summary>
        /// Are these points equal.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is Point2i)) return false;
            Point2i v = (Point2i)obj;
            return this == v;
        }

        /// <summary>
        /// Are these points equal.
        /// </summary>
        public bool Equals(Point2i v)
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
                return hash;
            }
        }

        /// <summary>
        /// Vector as a string.
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0},{1}", x, y);
        }

        /// <summary>
        /// Vector as a string.
        /// </summary>
        public string ToString(string f)
        {
            return string.Format("{0},{1}", x.ToString(f), y.ToString(f));
        }

        /// <summary>
        /// Distance between two points.
        /// </summary>
        public static double Distance(Point2i v0, Point2i v1)
        {
            return MathUtil.Sqrt(SqrDistance(v0, v1));
        }

        /// <summary>
        /// Square distance between two points.
        /// </summary>
        public static REAL SqrDistance(Point2i v0, Point2i v1)
        {
            REAL x = v0.x - v1.x;
            REAL y = v0.y - v1.y;
            return x * x + y * y;
        }

        /// <summary>
        /// The minimum value between s and each component in point.
        /// </summary>
        public static Point2i Min(Point2i v, REAL s)
        {
            v.x = MathUtil.Min(v.x, s);
            v.y = MathUtil.Min(v.y, s);
            return v;
        }

        /// <summary>
        /// The minimum value between each component in points.
        /// </summary>
        public static Point2i Min(Point2i v0, Point2i v1)
        {
            v0.x = MathUtil.Min(v0.x, v1.x);
            v0.y = MathUtil.Min(v0.y, v1.y);
            return v0;
        }

        /// <summary>
        /// The maximum value between s and each component in point.
        /// </summary>
        public static Point2i Max(Point2i v, REAL s)
        {
            v.x = MathUtil.Max(v.x, s);
            v.y = MathUtil.Max(v.y, s);
            return v;
        }

        /// <summary>
        /// The maximum value between each component in points.
        /// </summary>
        public static Point2i Max(Point2i v0, Point2i v1)
        {
            v0.x = MathUtil.Max(v0.x, v1.x);
            v0.y = MathUtil.Max(v0.y, v1.y);
            return v0;
        }

        /// <summary>
        /// Clamp each component to specified min and max.
        /// </summary>
        public static Point2i Clamp(Point2i v, REAL min, REAL max)
        {
            v.x = MathUtil.Max(MathUtil.Min(v.x, max), min);
            v.y = MathUtil.Max(MathUtil.Min(v.y, max), min);
            return v;
        }

        /// <summary>
        /// Clamp each component to specified min and max.
        /// </summary>
        public static Point2i Clamp(Point2i v, Point2i min, Point2i max)
        {
            v.x = MathUtil.Max(MathUtil.Min(v.x, max.x), min.x);
            v.y = MathUtil.Max(MathUtil.Min(v.y, max.y), min.y);
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
        /// <returns>The new vector</returns>
        public Point2i Permutate(int i, int j)
        {
            return new Point2i(this[i], this[j]);
        }

    }
}
