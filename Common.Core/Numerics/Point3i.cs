﻿using System;
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
        public Point2d xy => new Point2d(x, y);

        /// <summary>
        /// 3D point to 4D point with w as 0.
        /// </summary>
        public Point4d xyz0 => new Point4d(x, y, z, 0);

        /// <summary>
        /// 3D point to 4D point with w as 1.
        /// </summary>
        public Point4d xyz1 => new Point4d(x, y, z, 1);

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
        /// Point as vector.
        /// </summary>
        public Vector3d Vector3d => new Vector3d(x, y, z);

        /// <summary>
        /// Point as vector.
        /// </summary>
        public Vector4d Vector4d => new Vector4d(x, y, z, 1);

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (!(obj is Point3i)) return false;
            Point3i v = (Point3i)obj;
            return this == v;
        }

        /// <summary>
        /// Are these points equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Point3i v)
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
        /// Distance between two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Distance(Point3i v0, Point3i v1)
        {
            return MathUtil.Sqrt(SqrDistance(v0, v1));
        }

        /// <summary>
        /// Square distance between two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static REAL SqrDistance(Point3i v0, Point3i v1)
        {
            REAL x = v0.x - v1.x;
            REAL y = v0.y - v1.y;
            REAL z = v0.z - v1.z;
            return x * x + y * y + z * z;
        }

        /// <summary>
        /// Direction between two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d Direction(Point3i v0, Point3i v1)
        {
            return (v1 - v0).Vector3d.Normalized;
        }

        /// <summary>
        /// The minimum value between s and each component in point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3i Clamp(Point3i v, Point3i min, Point3i max)
        {
            v.x = MathUtil.Max(MathUtil.Min(v.x, max.x), min.x);
            v.y = MathUtil.Max(MathUtil.Min(v.y, max.y), min.y);
            v.z = MathUtil.Max(MathUtil.Min(v.z, max.z), min.z);
            return v;
        }

    }
}
