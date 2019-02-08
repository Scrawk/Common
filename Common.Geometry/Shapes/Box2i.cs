using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Core.LinearAlgebra;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Box2i : IEquatable<Box2i>
    {

        public Vector2i Min;

        public Vector2i Max;

        public Box2i(int min, int max)
        {
            Min = new Vector2i(min);
            Max = new Vector2i(max);
        }

        public Box2i(int minX, int maxX, int minY, int maxY)
        {
            Min = new Vector2i(minX, minY);
            Max = new Vector2i(maxX, maxY);
        }

        public Box2i(Vector2i min, Vector2i max)
        {
            Min = min;
            Max = max;
        }

        public Vector2i Corner00
        {
            get { return Min; }
        }

        public Vector2i Corner10
        {
            get { return new Vector2i(Max.x, Min.y); }
        }

        public Vector2i Corner11
        {
            get { return Max; }
        }

        public Vector2i Corner01
        {
            get { return new Vector2i(Min.x, Max.y); }
        }

        public Vector2f Center 
        { 
            get { return new Vector2f((Min.x + Max.x) * 0.5f, (Min.y + Max.y) * 0.5f); } 
        }

        public Vector2i Size 
        { 
            get { return new Vector2i(Width, Height); } 
        }

        public int Width 
        { 
            get { return Max.x - Min.x; } 
        }

        public int Height 
        { 
            get { return Max.y - Min.y; } 
        }

        public int Area 
        { 
            get { return (Max.x - Min.x) * (Max.y - Min.y); } 
        }

        public static explicit operator Box2i(Box2f box)
        {
            return new Box2i((Vector2i)box.Min, (Vector2i)box.Max);
        }

        public static explicit operator Box2i(Box2d box)
        {
            return new Box2i((Vector2i)box.Min, (Vector2i)box.Max);
        }

        public static bool operator ==(Box2i b1, Box2i b2)
        {
            return b1.Min == b2.Min && b1.Max == b2.Max;
        }

        public static bool operator !=(Box2i b1, Box2i b2)
        {
            return b1.Min != b2.Min || b1.Max != b2.Max;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Box2i)) return false;
            Box2i box = (Box2i)obj;
            return this == box;
        }

        public bool Equals(Box2i box)
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
            return string.Format("[Box2i: Min={0}, Max={1}, Width={2}, Height={3}]", Min, Max, Width, Height);
        }

        public void GetCorners(IList<Vector2i> corners)
        {
            corners[0] = new Vector2i(Min.x, Min.y);
            corners[1] = new Vector2i(Max.x, Min.y);
            corners[2] = new Vector2i(Max.x, Max.y);
            corners[3] = new Vector2i(Min.x, Max.y);
        }

        public void GetCornersXZ(IList<Vector3i> corners, int y = 0)
        {
            corners[0] = new Vector3i(Min.x, y, Min.y);
            corners[1] = new Vector3i(Max.x, y, Min.y);
            corners[2] = new Vector3i(Max.x, y, Max.y);
            corners[3] = new Vector3i(Min.x, y, Max.y);
        }

        /// <summary>
        /// Returns the bounding box containing this box and the given point.
        /// </summary>
        public void Enlarge(Vector2i p)
        {
            Min.x = Math.Min(Min.x, p.x);
            Min.y = Math.Min(Min.y, p.y);
            Max.x = Math.Max(Max.x, p.x);
            Max.y = Math.Max(Max.y, p.y);
        }

        /// <summary>
        /// Returns the bounding box containing this box and the given box.
        /// </summary>
        public void Enlarge(Box2i box)
        {
            Min.x = Math.Min(Min.x, box.Min.x);
            Min.y = Math.Min(Min.y, box.Min.y);
            Max.x = Math.Max(Max.x, box.Max.x);
            Max.y = Math.Max(Max.y, box.Max.y);
        }

        /// <summary>
        /// Returns true if this box intersects the other box.
        /// </summary>
        public bool Intersects(Box2i a)
        {
            if (Max.x < a.Min.x || Min.x > a.Max.x) return false;
            if (Max.y < a.Min.y || Min.y > a.Max.y) return false;
            return true;
        }

        /// <summary>
        /// Does the box contain the other box.
        /// </summary>
        public bool Contains(Box2i a)
        {
            if (a.Max.x > Max.x || a.Min.x < Min.x) return false;
            if (a.Max.y > Max.y || a.Min.y < Min.y) return false;
            return true;
        }

        /// <summary>
        /// Does the box contain the point.
        /// </summary>
        public bool Contains(Vector2i p)
        {
            if (p.x > Max.x || p.x < Min.x) return false;
            if (p.y > Max.y || p.y < Min.y) return false;
            return true;
        }

        /// <summary>
        /// Returns the closest point on the box.
        /// </summary>
        public Vector2i Closest(Vector2i p)
        {
            Vector2i c;

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

        public static Box2i CalculateBounds(IList<Vector2i> vertices)
        {
            Vector2i min = Vector2i.MaxInt;
            Vector2i max = Vector2i.MinInt;

            int count = vertices.Count;
            for (int i = 0; i < count; i++)
            {
                var v = vertices[i];
                if (v.x < min.x) min.x = v.x;
                if (v.y < min.y) min.y = v.y;

                if (v.x > max.x) max.x = v.x;
                if (v.y > max.y) max.y = v.y;
            }

            return new Box2i(min, max);
        }

        public static Box2i CalculateBounds(Vector2i a, Vector2i b)
        {
            int xmin = Math.Min(a.x, b.x);
            int xmax = Math.Max(a.x, b.x);
            int ymin = Math.Min(a.y, b.y);
            int ymax = Math.Max(a.y, b.y);

            return new Box2i(xmin, xmax, ymin, ymax);
        }

    }

}

















