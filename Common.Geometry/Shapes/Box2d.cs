﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Core.LinearAlgebra;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Box2d : IEquatable<Box2d>
    {

        public Vector2d Min;

        public Vector2d Max;

        public Box2d(double min, double max)
        {
            Min = new Vector2d(min);
            Max = new Vector2d(max);
        }

        public Box2d(double minX, double maxX, double minY, double maxY)
        {
            Min = new Vector2d(minX, minY);
            Max = new Vector2d(maxX, maxY);
        }

        public Box2d(Vector2d min, Vector2d max)
        {
            Min = min;
            Max = max;
        }

        public Box2d(Vector2i min, Vector2i max)
        {
            Min = new Vector2d(min.x, min.y);
            Max = new Vector2d(max.x, max.y);
        }

        public Vector2d Corner00
        {
            get { return Min; }
        }

        public Vector2d Corner10
        {
            get { return new Vector2d(Max.x, Min.y); }
        }

        public Vector2d Corner11
        {
            get { return Max; }
        }

        public Vector2d Corner01
        {
            get { return new Vector2d(Min.x, Max.y); }
        }

        public Vector2d Center 
        { 
            get { return (Min + Max) * 0.5; } 
        }

        public Vector2d Size 
        { 
            get { return new Vector2d(Width, Height); } 
        }

        public double Width 
        { 
            get { return Max.x - Min.x; } 
        }

        public double Height 
        { 
            get { return Max.y - Min.y; } 
        }

        public double Area 
        { 
            get { return (Max.x - Min.x) * (Max.y - Min.y); } 
        }

        public static implicit operator Box2d(Box2f box)
        {
            return new Box2d(box.Min, box.Max);
        }

        public static implicit operator Box2d(Box2i box)
        {
            return new Box2d(box.Min, box.Max);
        }

        public static bool operator ==(Box2d b1, Box2d b2)
        {
            return b1.Min == b2.Min && b1.Max == b2.Max;
        }

        public static bool operator !=(Box2d b1, Box2d b2)
        {
            return b1.Min != b2.Min || b1.Max != b2.Max;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Box2d)) return false;
            Box2d box = (Box2d)obj;
            return this == box;
        }

        public bool Equals(Box2d box)
        {
            return this == box;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ Min.GetHashCode();
                hash = (hash * 16777619) ^ Max.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return string.Format("[Box2d: Min={0}, Max={1}, Width={2}, Height={3}]", Min, Max, Width, Height);
        }

        public void GetCorners(IList<Vector2d> corners)
        {
            corners[0] = new Vector2d(Min.x, Min.y);
            corners[1] = new Vector2d(Max.x, Min.y);
            corners[2] = new Vector2d(Max.x, Max.y);
            corners[3] = new Vector2d(Min.x, Max.y);
        }

        public void GetCornersXZ(IList<Vector3d> corners, double y = 0)
        {
            corners[0] = new Vector3d(Min.x, y, Min.y);
            corners[1] = new Vector3d(Max.x, y, Min.y);
            corners[2] = new Vector3d(Max.x, y, Max.y);
            corners[3] = new Vector3d(Min.x, y, Max.y);
        }

        /// <summary>
        /// Returns the bounding box containing this box and the given point.
        /// </summary>
        public static Box2d Enlarge(Box2d box, Vector2d p)
        {
            var b = new Box2d();
            b.Min.x = Math.Min(box.Min.x, p.x);
            b.Min.y = Math.Min(box.Min.y, p.y);
            b.Max.x = Math.Max(box.Max.x, p.x);
            b.Max.y = Math.Max(box.Max.y, p.y);
            return b;
        }

        /// <summary>
        /// Returns the bounding box containing this box and the given box.
        /// </summary>
        public static Box2d Enlarge(Box2d box0, Box2d box1)
        {
            var b = new Box2d();
            b.Min.x = Math.Min(box0.Min.x, box1.Min.x);
            b.Min.y = Math.Min(box0.Min.y, box1.Min.y);
            b.Max.x = Math.Max(box0.Max.x, box1.Max.x);
            b.Max.y = Math.Max(box0.Max.y, box1.Max.y);
            return b;
        }

        /// <summary>
        /// Enlarge the box by a given percent.
        /// </summary>
        public static Box2d Enlarge(Box2d box, double percent)
        {
            var amount = box.Size * percent;
            return new Box2d(box.Min - amount, box.Max + amount);
        }

        /// <summary>
        /// Returns true if this box intersects the other box.
        /// </summary>
        public bool Intersects(Box2d a)
        {
            if (Max.x < a.Min.x || Min.x > a.Max.x) return false;
            if (Max.y < a.Min.y || Min.y > a.Max.y) return false;
            return true;
        }

        /// <summary>
        /// Does the box contain the other box.
        /// </summary>
        public bool Contains(Box2d a)
        {
            if (a.Max.x > Max.x || a.Min.x < Min.x) return false;
            if (a.Max.y > Max.y || a.Min.y < Min.y) return false;
            return true;
        }

        /// <summary>
        /// Does the box contain the point.
        /// </summary>
        public bool Contains(Vector2d p)
        {
            if (p.x > Max.x || p.x < Min.x) return false;
            if (p.y > Max.y || p.y < Min.y) return false;
            return true;
        }

        /// <summary>
        /// Returns the closest point on the box.
        /// </summary>
        public Vector2d Closest(Vector2d p)
        {
            Vector2d c;

            if (p.x < Min.x)
                c.x = Min.x;
            else if (p.x > Max.x)
                c.x = Max.x;
            else
                c.x = p.x;

            if (p.y < Min.y)
                c.y = Min.y;
            else if (p.y > Max.y)
                c.y = Max.y;
            else
                c.y = p.y;

            return c;
        }

        public static Box2d CalculateBounds(IList<Vector2d> vertices)
        {
            Vector2d min = Vector2d.PositiveInfinity;
            Vector2d max = Vector2d.NegativeInfinity;

            int count = vertices.Count;
            for (int i = 0; i < count; i++)
            {
                var v = vertices[i];
                if (v.x < min.x) min.x = v.x;
                if (v.y < min.y) min.y = v.y;

                if (v.x > max.x) max.x = v.x;
                if (v.y > max.y) max.y = v.y;
            }

            return new Box2d(min, max);
        }

        public static Box2d CalculateBounds(Vector2d a, Vector2d b)
        {
            double xmin = Math.Min(a.x, b.x);
            double xmax = Math.Max(a.x, b.x);
            double ymin = Math.Min(a.y, b.y);
            double ymax = Math.Max(a.y, b.y);

            return new Box2d(xmin, xmax, ymin, ymax);
        }

    }

}

















