﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using Common.Core.Numerics;

using REAL = System.Single;
using CIRCLE = Common.Geometry.Shapes.Circle2f;
using BOX = Common.Geometry.Shapes.Box2f;

namespace Common.Geometry.Points
{
    /// <summary>
    /// Interface for point in 2D space.
    /// </summary>
    public interface IPoint2f
    {
        REAL x { get; set; }
        REAL y { get; set; }
    }

    /// <summary>
    /// Generic helper class for common point operations.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class PointOps2f<T>
        where T : IPoint2f
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
            return x * x + y * y;
        }

        /// <summary>
        /// Does the circle contain the point.
        /// </summary>
        public static bool Contains(CIRCLE circle, T point)
        {
            var x = circle.Center.x - point.x;
            var y = circle.Center.y - point.y;
            return (x*x + y*y) <= circle.Radius2;
        }

        /// <summary>
        /// Does the box contain the point.
        /// </summary>
        public static bool Contains(BOX box, T point)
        {
            if (point.x > box.Max.x || point.x < box.Min.x) return false;
            if (point.y > box.Max.y || point.y < box.Min.y) return false;
            return true;
        }
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Point2f : IEquatable<Point2f>, IComparable<Point2f>, IPoint2f
    {
        private REAL _x, _y;

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

        /// <summary>
        /// A point all with the value v.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2f(REAL v)
        {
            _x = v;
            _y = v;
        }

        /// <summary>
        /// A point from the variables.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2f(REAL x, REAL y)
        {
            _x = x;
            _y = y;
        }

        /// <summary>
        /// A point from the varibles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2f(double x, double y)
        {
            _x = (float)x;
            _y = (float)y;
        }

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

                fixed (REAL* array = &_x) { array[i] = value; }
            }
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
            return new Point2f(v1.x + s, v1.y + s);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Point2f(Vector2d v)
        {
            return new Point2f((REAL)v.x, (REAL)v.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point2f(Vector2f v)
        {
            return new Point2f(v.x, v.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point2f(Vector2i v)
        {
            return new Point2f(v.x, v.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point2f(ValueTuple<REAL, REAL> v)
        {
            return new Point2f(v.Item1, v.Item2);
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
                return hash;
            }
        }

        /// <summary>
        /// Compare two points by axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(Point2f other)
        {
            if (x != other.x)
                return x < other.x ? -1 : 1;
            else if (y != other.y)
                return y < other.y ? -1 : 1;
            return 0;
        }

        /// <summary>
        /// Point as a string.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return string.Format("{0},{1}", x, y);
        }

        /// <summary>
        /// Point as a string.
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
            return MathUtil.SafeSqrt(SqrDistance(v0, v1));
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
        /// The minimum value between s and each component in point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f Min(Point2f v, REAL s)
        {
            v.x = Math.Min(v.x, s);
            v.y = Math.Min(v.y, s);
            return v;
        }

        /// <summary>
        /// The minimum value between each component in points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f Min(Point2f v0, Point2f v1)
        {
            v0.x = Math.Min(v0.x, v1.x);
            v0.y = Math.Min(v0.y, v1.y);
            return v0;
        }

        /// <summary>
        /// The maximum value between s and each component in point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f Max(Point2f v, REAL s)
        {
            v.x = Math.Max(v.x, s);
            v.y = Math.Max(v.y, s);
            return v;
        }

        /// <summary>
        /// The maximum value between each component in points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f Max(Point2f v0, Point2f v1)
        {
            v0.x = Math.Max(v0.x, v1.x);
            v0.y = Math.Max(v0.y, v1.y);
            return v0;
        }

        /// <summary>
        /// Clamp the each component to specified min and max.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f Clamp(Point2f p, REAL min, REAL max)
        {
            p.x = Math.Max(Math.Min(p.x, max), min);
            p.y = Math.Max(Math.Min(p.y, max), min);
            return p;
        }

        /// <summary>
        /// Clamp the each component to specified min and max.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f Clamp(Point2f p, Point2f min, Point2f max)
        {
            p.x = Math.Max(Math.Min(p.x, max.x), min.x);
            p.y = Math.Max(Math.Min(p.y, max.y), min.y);
            return p;
        }

        /// <summary>
        /// Lerp between two points.
        /// </summary>
        public static Point2f Lerp(Point2f from, Point2f to, REAL t)
        {
            if (t < 0.0f) t = 0.0f;
            if (t > 1.0f) t = 1.0f;

            if (t == 0.0f) return from;
            if (t == 1.0f) return to;

            REAL t1 = 1.0f - t;
            Point2f v = new Point2f();
            v.x = from.x * t1 + to.x * t;
            v.y = from.y * t1 + to.y * t;
            return v;
        }

        /// <summary>
        /// A rounded point.
        /// </summary>
        /// <param name="digits"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2f Rounded(int digits = 0)
        {
            x = MathUtil.Round(this.x, digits);
            y = MathUtil.Round(this.y, digits);
            return new Point2f(x, y);
        }

        /// <summary>
        /// Convert to vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2f ToVector2f()
        {
            return new Vector2f(x, y);
        }

    }

}


































