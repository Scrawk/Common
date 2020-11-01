using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Core.Numerics;

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
            get { return (Vector2f)(Min + Max) * 0.5f; } 
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

        public static Box2i operator +(Box2i box, int s)
        {
            return new Box2i(box.Min + s, box.Max + s);
        }

        public static Box2i operator +(Box2i box, Vector2i v)
        {
            return new Box2i(box.Min + v, box.Max + v);
        }

        public static Box2i operator -(Box2i box, int s)
        {
            return new Box2i(box.Min - s, box.Max - s);
        }

        public static Box2i operator -(Box2i box, Vector2i v)
        {
            return new Box2i(box.Min - v, box.Max - v);
        }

        public static Box2i operator *(Box2i box, int s)
        {
            return new Box2i(box.Min * s, box.Max * s);
        }

        public static Box2i operator /(Box2i box, int s)
        {
            return new Box2i(box.Min / s, box.Max / s);
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

        public IEnumerable<Vector2i> EnumeratePerimeter()
        {
            for(int x = Min.x; x < Max.x; x++)
                yield return new Vector2i(x, Min.y);

            for (int y = Min.y; y < Max.y; y++)
                yield return new Vector2i(Max.x, y);

            for (int x = Max.x; x > Min.x; x--)
                yield return new Vector2i(x, Max.y);

            for (int y = Max.y; y > Min.y; y--)
                yield return new Vector2i(Min.x, y);
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

        public void GetCorners(IList<Vector2f> corners)
        {
            corners[0] = new Vector2f(Min.x, Min.y);
            corners[1] = new Vector2f(Max.x, Min.y);
            corners[2] = new Vector2f(Max.x, Max.y);
            corners[3] = new Vector2f(Min.x, Max.y);
        }

        public void GetCornersXZ(IList<Vector3f> corners, int y = 0)
        {
            corners[0] = new Vector3f(Min.x, y, Min.y);
            corners[1] = new Vector3f(Max.x, y, Min.y);
            corners[2] = new Vector3f(Max.x, y, Max.y);
            corners[3] = new Vector3f(Min.x, y, Max.y);
        }

        /// <summary>
        /// Returns the bounding box containing this box and the given point.
        /// </summary>
        public static Box2i Enlarge(Box2i box, Vector2i p)
        {
            var b = new Box2i();
            b.Min.x = Math.Min(box.Min.x, p.x);
            b.Min.y = Math.Min(box.Min.y, p.y);
            b.Max.x = Math.Max(box.Max.x, p.x);
            b.Max.y = Math.Max(box.Max.y, p.y);
            return b;
        }

        /// <summary>
        /// Returns the bounding box containing this box and the given box.
        /// </summary>
        public static Box2i Enlarge(Box2i box0, Box2i box1)
        {
            var b = new Box2i();
            b.Min.x = Math.Min(box0.Min.x, box1.Min.x);
            b.Min.y = Math.Min(box0.Min.y, box1.Min.y);
            b.Max.x = Math.Max(box0.Max.x, box1.Max.x);
            b.Max.y = Math.Max(box0.Max.y, box1.Max.y);
            return b;
        }

        /// <summary>
        /// Return a new box expanded by the amount.
        /// </summary>
        /// <param name="box">The box to expand.</param>
        /// <param name="amount">The amount to expand.</param>
        /// <returns>The expanded box.</returns>
        public static Box2i Expand(Box2i box, int amount)
        {
            return new Box2i(box.Min - amount, box.Max + amount);
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
        /// Find the closest point to the box.
        /// If point inside box return point.
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

















