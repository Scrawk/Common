using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using Common.Core.Numerics;

using REAL = System.Single;
using CIRCLE = Common.Geometry.Shapes.Sphere3f;
using BOX = Common.Geometry.Shapes.Box3f;

namespace Common.Geometry.Points
{
    public interface IPoint3f
    {
        REAL x { get; set; }
        REAL y { get; set; }
        REAL z { get; set; }
    }

    /// <summary>
    /// Generic helper class for common point operations.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class PointOps3f<T>
        where T : IPoint3f
    {
        /// <summary>
        /// The distance between two points.
        /// </summary>
        public static REAL Distance(T p0, T p1)
        {
            return MathUtil.SafeSqrt(SqrDistance(p0, p1));
        }

        /// <summary>
        /// The square distance between two points.
        /// </summary>
        public static REAL SqrDistance(T p0, T p1)
        {
            var x = p0.x - p1.x;
            var y = p0.y - p1.y;
            var z = p0.z - p1.z;
            return x * x + y * y + z * z;
        }

        /// <summary>
        /// Does the circle contain the point.
        /// </summary>
        public static bool Contains(CIRCLE circle, T point)
        {
            var x = circle.Center.x - point.x;
            var y = circle.Center.y - point.y;
            var z = circle.Center.z - point.z;
            return (x*x + y*y + z*z) <= circle.Radius2;
        }

        /// <summary>
        /// Does the box contain the point.
        /// </summary>
        public static bool Contains(BOX box, T point)
        {
            if (point.x > box.Max.x || point.x < box.Min.x) return false;
            if (point.y > box.Max.y || point.y < box.Min.y) return false;
            if (point.z > box.Max.z || point.z < box.Min.z) return false;
            return true;
        }
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Point3f : IEquatable<Point3f>, IComparable<Point3f>
    {
        private REAL _x, _y, _z;

        public REAL x
        {
            get => _x;
            set => _x = value;
        }

        public REAL y
        {
            get => _y;
            set => _y = value;
        }

        public REAL z
        {
            get => _z;
            set => _z = value;
        }

        /// <summary>
        /// A point all with the value v.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3f(REAL v)
        {
            _x = v;
            _y = v;
            _z = v;
        }

        /// <summary>
        /// A point from the varibles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3f(REAL x, REAL y, REAL z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        /// <summary>
        /// A point from the varibles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3f(double x, double y, double z)
        {
            _x = (float)x;
            _y = (float)y;
            _z = (float)z;
        }

        unsafe public REAL this[int i]
        {
            get
            {
                if ((uint)i >= 3)
                    throw new IndexOutOfRangeException("Point3f index out of range.");

                fixed (Point3f* array = &this) { return ((REAL*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 3)
                    throw new IndexOutOfRangeException("Point3f index out of range.");

                fixed (REAL* array = &_x) { array[i] = value; }
            }
        }

        /// <summary>
        /// Add two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3f operator +(Point3f v1, Point3f v2)
        {
            return new Point3f(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        /// <summary>
        /// Add point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3f operator +(Point3f v1, REAL s)
        {
            return new Point3f(v1.x + s, v1.y + s, v1.z + s);
        }

        /// <summary>
        /// Add point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3f operator +(REAL s, Point3f v1)
        {
            return new Point3f(v1.x + s, v1.y + s, v1.z + s);
        }

        /// <summary>
        /// Negate point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3f operator -(Point3f v)
        {
            return new Point3f(-v.x, -v.y, -v.z);
        }

        /// <summary>
        /// Subtract two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3f operator -(Point3f v1, Point3f v2)
        {
            return new Point3f(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        /// <summary>
        /// Subtract point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3f operator -(Point3f v1, REAL s)
        {
            return new Point3f(v1.x - s, v1.y - s, v1.z - s);
        }

        /// <summary>
        /// Subtract point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3f operator -(REAL s, Point3f v1)
        {
            return new Point3f(s - v1.x, s - v1.y, s - v1.z);
        }

        /// <summary>
        /// Multiply two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3f operator *(Point3f v1, Point3f v2)
        {
            return new Point3f(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        /// <summary>
        /// Multiply a point and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3f operator *(Point3f v, REAL s)
        {
            return new Point3f(v.x * s, v.y * s, v.z * s);
        }

        /// <summary>
        /// Multiply a point and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3f operator *(REAL s, Point3f v)
        {
            return new Point3f(v.x * s, v.y * s, v.z * s);
        }

        /// <summary>
        /// Divide two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3f operator /(Point3f v1, Point3f v2)
        {
            return new Point3f(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
        }

        /// <summary>
        /// Divide a point and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3f operator /(Point3f v, REAL s)
        {
            return new Point3f(v.x / s, v.y / s, v.z / s);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Point3f(Vector3d v)
        {
            return new Point3f((REAL)v.x, (REAL)v.y, (REAL)v.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point3f(Vector3i v)
        {
            return new Point3f(v.x, v.y, v.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point3f(Vector3f v)
        {
            return new Point3f(v.x, v.y, v.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point3f(ValueTuple<REAL, REAL, REAL> v)
        {
            return new Point3f(v.Item1, v.Item2, v.Item3);
        }

        /// <summary>
        /// Are these points equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Point3f v1, Point3f v2)
        {
            return (v1.x == v2.x && v1.y == v2.y && v1.z == v2.z);
        }

        /// <summary>
        /// Are these points not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Point3f v1, Point3f v2)
        {
            return (v1.x != v2.x || v1.y != v2.y || v1.z != v2.z);
        }

        /// <summary>
        /// Are these points equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (!(obj is Point3f)) return false;
            Point3f v = (Point3f)obj;
            return this == v;
        }

        /// <summary>
        /// Are these points equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Point3f v)
        {
            return this == v;
        }

        /// <summary>
        /// Points hash code. 
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
        /// Compare two points by axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(Point3f other)
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
        /// Point as a string.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return string.Format("{0},{1},{2}", x, y, z);
        }

        /// <summary>
        /// Point as a string.
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
        public static REAL Distance(Point3f v0, Point3f v1)
        {
            return MathUtil.SafeSqrt(SqrDistance(v0, v1));
        }

        /// <summary>
        /// Square distance between two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static REAL SqrDistance(Point3f v0, Point3f v1)
        {
            REAL x = v0.x - v1.x;
            REAL y = v0.y - v1.y;
            REAL z = v0.z - v1.z;
            return x * x + y * y + z * z;
        }

        /// <summary>
        /// The minimum value between s and each component in point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3f Min(Point3f v, REAL s)
        {
            v.x = Math.Min(v.x, s);
            v.y = Math.Min(v.y, s);
            v.z = Math.Min(v.z, s);
            return v;
        }

        /// <summary>
        /// The minimum value between each component in points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3f Min(Point3f v0, Point3f v1)
        {
            v0.x = Math.Min(v0.x, v1.x);
            v0.y = Math.Min(v0.y, v1.y);
            v0.z = Math.Min(v0.z, v1.z);
            return v0;
        }

        /// <summary>
        /// The maximum value between s and each component in point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3f Max(Point3f v, REAL s)
        {
            v.x = Math.Max(v.x, s);
            v.y = Math.Max(v.y, s);
            v.z = Math.Max(v.z, s);
            return v;
        }

        /// <summary>
        /// The maximum value between each component in points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3f Max(Point3f v0, Point3f v1)
        {
            v0.x = Math.Max(v0.x, v1.x);
            v0.y = Math.Max(v0.y, v1.y);
            v0.z = Math.Max(v0.z, v1.z);
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
            z = Math.Max(Math.Min(z, max), min);
        }

        /// <summary>
        /// Clamp the each component to specified min and max.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clamp(Point3f min, Point3f max)
        {
            x = Math.Max(Math.Min(x, max.x), min.x);
            y = Math.Max(Math.Min(y, max.y), min.y);
            z = Math.Max(Math.Min(z, max.z), min.z);
        }

        /// <summary>
        /// Lerp between two points.
        /// </summary>
        public static Point3f Lerp(Point3f from, Point3f to, REAL t)
        {
            if (t < 0.0f) t = 0.0f;
            if (t > 1.0f) t = 1.0f;

            if (t == 0.0f) return from;
            if (t == 1.0f) return to;

            REAL t1 = 1.0f - t;
            Point3f v = new Point3f();
            v.x = from.x * t1 + to.x * t;
            v.y = from.y * t1 + to.y * t;
            v.z = from.z * t1 + to.z * t;
            return v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Round(int digits = 0)
        {
            x = (REAL)Math.Round(x, digits);
            y = (REAL)Math.Round(y, digits);
            z = (REAL)Math.Round(z, digits);
        }

    }


}


































