﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using REAL = System.Single;

namespace Common.Core.Numerics
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Point2f : IEquatable<Point2f>
    {
        public REAL x, y;

        /// <summary>
        /// The unit x point.
        /// </summary>
	    public readonly static Point2f UnitX = new Point2f(1, 0);

        /// <summary>
        /// The unit y point.
        /// </summary>
	    public readonly static Point2f UnitY = new Point2f(0, 1);

        /// <summary>
        /// A point of zeros.
        /// </summary>
	    public readonly static Point2f Zero = new Point2f(0);

        /// <summary>
        /// A point of ones.
        /// </summary>
	    public readonly static Point2f One = new Point2f(1);

        /// <summary>
        /// A point of 0.5.
        /// </summary>
        public readonly static Point2f Half = new Point2f(0.5f);

        /// <summary>
        /// A point of positive infinity.
        /// </summary>
        public readonly static Point2f PositiveInfinity = new Point2f(REAL.PositiveInfinity);

        /// <summary>
        /// A point of negative infinity.
        /// </summary>
        public readonly static Point2f NegativeInfinity = new Point2f(REAL.NegativeInfinity);

        /// <summary>
        /// 2D point to 3D point with z as 0.
        /// </summary>
        public Point3f xy0 => new Point3f(x, y, 0);

        /// <summary>
        /// 2D point to 3D point with y as z.
        /// </summary>
        public Point3f x0y => new Point3f(x, 0, y);

        /// <summary>
        /// 2D point to 3D point with z as 1.
        /// </summary>
        public Point3f xy1 => new Point3f(x, y, 1);

        /// <summary>
        /// 2D point to 4D point with z as 0 and w as 0.
        /// </summary>
        public Point4f xy00 => new Point4f(x, y, 0, 0);

        /// <summary>
        /// 2D point to 4D point with z as 0 and w as 1.
        /// </summary>
        public Point4f xy01 => new Point4f(x, y, 0, 1);

        /// <summary>
        /// A point all with the value v.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2f(REAL v)
        {
            this.x = v;
            this.y = v;
        }

        /// <summary>
        /// A point from the varibles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2f(REAL x, REAL y)
        {
            this.x = x;
            this.y = y;
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
                if ((uint)i >= 2)
                    throw new IndexOutOfRangeException("Point2f index out of range.");

                fixed (Point2f* array = &this) { return ((REAL*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 2)
                    throw new IndexOutOfRangeException("Point2f index out of range.");

                fixed (REAL* array = &x) { array[i] = value; }
            }
        }

        /// <summary>
        /// Point as a vector.
        /// </summary>
        public Vector2f Vector2f
        {
            get
            {
                return new Vector2f(x, y);
            }
        }

        /// <summary>
        /// The length of the point from the origin.
        /// </summary>
        public REAL Magnitude
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                REAL sqm = SqrMagnitude;
                if (sqm != 0)
                    return MathUtil.Sqrt(sqm);
                else
                    return 0;
            }
        }

        /// <summary>
        /// The length of the point from the origin squared.
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
        /// Add two point and vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f operator +(Point2f v1, Vector2f v2)
        {
            return new Point2f(v1.x + v2.x, v1.y + v2.y);
        }

        /// <summary>
        /// Add two point and vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f operator +(Vector2f v1, Point2f v2)
        {
            return new Point2f(v1.x + v2.x, v1.y + v2.y);
        }

        /// <summary>
        /// Add two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f operator +(Point2f v1, Point2f v2)
        {
            return new Point2f(v1.x + v2.x, v1.y + v2.y);
        }

        /// <summary>
        /// Add point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f operator +(Point2f v1, REAL s)
        {
            return new Point2f(v1.x + s, v1.y + s);
        }

        /// <summary>
        /// Add point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f operator +(REAL s, Point2f v1)
        {
            return new Point2f(s + v1.x, s + v1.y);
        }

        /// <summary>
        /// Negate point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f operator -(Point2f v)
        {
            return new Point2f(-v.x, -v.y);
        }

        /// <summary>
        /// Subtract two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f operator -(Point2f v1, Point2f v2)
        {
            return new Point2f(v1.x - v2.x, v1.y - v2.y);
        }

        /// <summary>
        /// Subtract point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f operator -(Point2f v1, REAL s)
        {
            return new Point2f(v1.x - s, v1.y - s);
        }

        /// <summary>
        /// Subtract point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f operator -(REAL s, Point2f v1)
        {
            return new Point2f(s - v1.x, s - v1.y);
        }

        /// <summary>
        /// Multiply two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f operator *(Point2f v1, Point2f v2)
        {
            return new Point2f(v1.x * v2.x, v1.y * v2.y);
        }

        /// <summary>
        /// Multiply a point and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f operator *(Point2f v, REAL s)
        {
            return new Point2f(v.x * s, v.y * s);
        }

        /// <summary>
        /// Multiply a point and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f operator *(REAL s, Point2f v)
        {
            return new Point2f(v.x * s, v.y * s);
        }

        /// <summary>
        /// Divide two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f operator /(Point2f v1, Point2f v2)
        {
            return new Point2f(v1.x / v2.x, v1.y / v2.y);
        }

        /// <summary>
        /// Divide a point and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f operator /(Point2f v, REAL s)
        {
            return new Point2f(v.x / s, v.y / s);
        }

        /// <summary>
        /// Implict cast from vector.
        /// </summary>
        /// <param name="v">The vector to cast from</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point2f(Vector2f v)
        {
            return new Point2f(v.x, v.y);
        }

        /// <summary>
        /// Are these points equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Point2f v1, Point2f v2)
        {
            return (v1.x == v2.x && v1.y == v2.y);
        }

        /// <summary>
        /// Are these points not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Point2f v1, Point2f v2)
        {
            return (v1.x != v2.x || v1.y != v2.y);
        }

        /// <summary>
        /// Are these points equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (!(obj is Point2f)) return false;
            Point2f v = (Point2f)obj;
            return this == v;
        }

        /// <summary>
        /// Are these points equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Point2f v)
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
        /// Distance between two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static REAL Distance(Point2f v0, Point2f v1)
        {
            return MathUtil.Sqrt(SqrDistance(v0, v1));
        }

        /// <summary>
        /// Square distance between two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static REAL SqrDistance(Point2f v0, Point2f v1)
        {
            REAL x = v0.x - v1.x;
            REAL y = v0.y - v1.y;
            return x * x + y * y;
        }

        /// <summary>
        /// Direction between two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f Direction(Point2f v0, Point2f v1)
        {
            return (v1 - v0).Vector2f.Normalized;
        }

        /// <summary>
        /// The minimum value between s and each component in point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f Min(Point2f v, REAL s)
        {
            v.x = MathUtil.Min(v.x, s);
            v.y = MathUtil.Min(v.y, s);
            return v;
        }

        /// <summary>
        /// The minimum value between each component in points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f Min(Point2f v0, Point2f v1)
        {
            v0.x = MathUtil.Min(v0.x, v1.x);
            v0.y = MathUtil.Min(v0.y, v1.y);
            return v0;
        }

        /// <summary>
        /// The maximum value between s and each component in point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f Max(Point2f v, REAL s)
        {
            v.x = MathUtil.Max(v.x, s);
            v.y = MathUtil.Max(v.y, s);
            return v;
        }

        /// <summary>
        /// The maximum value between each component in points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f Max(Point2f v0, Point2f v1)
        {
            v0.x = MathUtil.Max(v0.x, v1.x);
            v0.y = MathUtil.Max(v0.y, v1.y);
            return v0;
        }

        /// <summary>
        /// Clamp each component to specified min and max.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f Clamp(Point2f v, REAL min, REAL max)
        {
            v.x = MathUtil.Max(MathUtil.Min(v.x, max), min);
            v.y = MathUtil.Max(MathUtil.Min(v.y, max), min);
            return v;
        }

        /// <summary>
        /// Clamp each component to specified min and max.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f Clamp(Point2f v, Point2f min, Point2f max)
        {
            v.x = MathUtil.Max(MathUtil.Min(v.x, max.x), min.x);
            v.y = MathUtil.Max(MathUtil.Min(v.y, max.y), min.y);
            return v;
        }

        /// <summary>
        /// Lerp between two points.
        /// </summary>
        public static Point2f Lerp(Point2f from, Point2f to, REAL t)
        {
            if (t < 0.0f) t = 0.0f;
            if (t > 1.0f) t = 1.0f;

            if (t == 0.0) return from;
            if (t == 1.0) return to;

            REAL t1 = 1.0f - t;
            var v = new Point2f();
            v.x = from.x * t1 + to.x * t;
            v.y = from.y * t1 + to.y * t;
            return v;
        }

        /// <summary>
        /// A rounded point.
        /// </summary>
        /// <param name="digits">The number of digits to round to.</param>
        /// <returns>The rounded point</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2f Rounded(int digits = 0)
        {
            REAL x = MathUtil.Round(this.x, digits);
            REAL y = MathUtil.Round(this.y, digits);
            return new Point2f(x, y);
        }

        /// <summary>
        /// Round the point.
        /// </summary>
        /// <param name="digits">The number of digits to round to.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Round(int digits = 0)
        {
            x = MathUtil.Round(x, digits);
            y = MathUtil.Round(y, digits);
        }

    }
}
