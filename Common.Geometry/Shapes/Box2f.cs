using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Core.LinearAlgebra;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Box2f : IEquatable<Box2f>
    {

        public Vector2f Min;

        public Vector2f Max;

        public Box2f(float min, float max)
        {
            Min = new Vector2f(min);
            Max = new Vector2f(max);
        }

        public Box2f(float minX, float maxX, float minY, float maxY)
        {
            Min = new Vector2f(minX, minY);
            Max = new Vector2f(maxX, maxY);
        }

        public Box2f(Vector2f min, Vector2f max)
        {
            Min = min;
            Max = max;
        }

        public Box2f(Vector2i min, Vector2i max)
        {
            Min = new Vector2f(min.x, min.y);
            Max = new Vector2f(max.x, max.y);
        }

        public Vector2f Corner00
        {
            get { return Min; }
        }

        public Vector2f Corner10
        {
            get { return new Vector2f(Max.x, Min.y); }
        }

        public Vector2f Corner11
        {
            get { return Max; }
        }

        public Vector2f Corner01
        {
            get { return new Vector2f(Min.x, Max.y); }
        }

        public Vector2f Center
        {
            get { return (Min + Max) * 0.5f; }
        }

        public Vector2f Size
        {
            get { return new Vector2f(Width, Height); }
        }

        public float Width
        {
            get { return Max.x - Min.x; }
        }

        public float Height
        {
            get { return Max.y - Min.y; }
        }

        public float Area
        {
            get { return (Max.x - Min.x) * (Max.y - Min.y); }
        }

        public static explicit operator Box2f(Box2d box)
        {
            return new Box2f((Vector2f)box.Min, (Vector2f)box.Max);
        }

        public static implicit operator Box2f(Box2i box)
        {
            return new Box2f(box.Min, box.Max);
        }

        public static bool operator ==(Box2f b1, Box2f b2)
        {
            return b1.Min == b2.Min && b1.Max == b2.Max;
        }

        public static bool operator !=(Box2f b1, Box2f b2)
        {
            return b1.Min != b2.Min || b1.Max != b2.Max;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Box2f)) return false;
            Box2f box = (Box2f)obj;
            return this == box;
        }

        public bool Equals(Box2f box)
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
            return string.Format("[Box2f: Min={0}, Max={1}, Width={2}, Height={3}]", Min, Max, Width, Height);
        }

        public void GetCorners(IList<Vector2f> corners)
        {
            corners[0] = new Vector2f(Min.x, Min.y);
            corners[1] = new Vector2f(Max.x, Min.y);
            corners[2] = new Vector2f(Max.x, Max.y);
            corners[3] = new Vector2f(Min.x, Max.y);
        }

        public void GetCornersXZ(IList<Vector3f> corners, float y = 0)
        {
            corners[0] = new Vector3f(Min.x, y, Min.y);
            corners[1] = new Vector3f(Max.x, y, Min.y);
            corners[2] = new Vector3f(Max.x, y, Max.y);
            corners[3] = new Vector3f(Min.x, y, Max.y);
        }

        /// <summary>
        /// Returns the bounding box containing this box and the given point.
        /// </summary>
        public void Enlarge(Vector2f p)
        {
            Min.x = Math.Min(Min.x, p.x);
            Min.y = Math.Min(Min.y, p.y);
            Max.x = Math.Max(Max.x, p.x);
            Max.y = Math.Max(Max.y, p.y);
        }

        /// <summary>
        /// Returns the bounding box containing this box and the given box.
        /// </summary>
        public void Enlarge(Box2f box)
        {
            Min.x = Math.Min(Min.x, box.Min.x);
            Min.y = Math.Min(Min.y, box.Min.y);
            Max.x = Math.Max(Max.x, box.Max.x);
            Max.y = Math.Max(Max.y, box.Max.y);
        }

        /// <summary>
        /// Returns the bounding box containing this box and the given box.
        /// </summary>
        public Box2f Enlarge(Box2f box0, Box2f box1)
        {
            var box = box0;
            box.Enlarge(box1);
            return box;
        }

        /// <summary>
        /// Returns true if this box intersects the other box.
        /// </summary>
        public bool Intersects(Box2f a)
        {
            if (Max.x < a.Min.x || Min.x > a.Max.x) return false;
            if (Max.y < a.Min.y || Min.y > a.Max.y) return false;
            return true;
        }

        /// <summary>
        /// Does the box contain the other box.
        /// </summary>
        public bool Contains(Box2f a)
        {
            if (a.Max.x > Max.x || a.Min.x < Min.x) return false;
            if (a.Max.y > Max.y || a.Min.y < Min.y) return false;
            return true;
        }

        /// <summary>
        /// Does the box contain the point.
        /// </summary>
        public bool Contains(Vector2f p)
        {
            if (p.x > Max.x || p.x < Min.x) return false;
            if (p.y > Max.y || p.y < Min.y) return false;
            return true;
        }

        /// <summary>
        /// Returns the closest point on the box.
        /// </summary>
        public Vector2f Closest(Vector2f p)
        {
            Vector2f c;

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

        public static Box2f CalculateBounds(IList<Vector2f> vertices)
        {
            Vector2f min = Vector2f.PositiveInfinity;
            Vector2f max = Vector2f.NegativeInfinity;

            int count = vertices.Count;
            for (int i = 0; i < count; i++)
            {
                var v = vertices[i];
                if (v.x < min.x) min.x = v.x;
                if (v.y < min.y) min.y = v.y;

                if (v.x > max.x) max.x = v.x;
                if (v.y > max.y) max.y = v.y;
            }

            return new Box2f(min, max);
        }

        public static Box2f CalculateBounds(Vector2f a, Vector2f b)
        {
            float xmin = Math.Min(a.x, b.x);
            float xmax = Math.Max(a.x, b.x);
            float ymin = Math.Min(a.y, b.y);
            float ymax = Math.Max(a.y, b.y);

            return new Box2f(xmin, xmax, ymin, ymax);
        }

    }

}

















