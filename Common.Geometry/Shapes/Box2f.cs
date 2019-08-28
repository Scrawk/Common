using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Core.Numerics;

using REAL = System.Single;
using VECTOR2 = Common.Core.Numerics.Vector2f;
using VECTOR3 = Common.Core.Numerics.Vector3f;
using MATRIX2 = Common.Core.Numerics.Matrix2x2f;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Box2f : IEquatable<Box2f>
    {

        public VECTOR2 Min;

        public VECTOR2 Max;

        public Box2f(REAL min, REAL max)
        {
            Min = new VECTOR2(min);
            Max = new VECTOR2(max);
        }

        public Box2f(REAL minX, REAL maxX, REAL minY, REAL maxY)
        {
            Min = new VECTOR2(minX, minY);
            Max = new VECTOR2(maxX, maxY);
        }

        public Box2f(VECTOR2 min, VECTOR2 max)
        {
            Min = min;
            Max = max;
        }

        public Box2f(Vector2i min, Vector2i max)
        {
            Min = new VECTOR2(min.x, min.y);
            Max = new VECTOR2(max.x, max.y);
        }

        public VECTOR2 Corner00
        {
            get { return Min; }
        }

        public VECTOR2 Corner10
        {
            get { return new VECTOR2(Max.x, Min.y); }
        }

        public VECTOR2 Corner11
        {
            get { return Max; }
        }

        public VECTOR2 Corner01
        {
            get { return new VECTOR2(Min.x, Max.y); }
        }

        public VECTOR2 Center
        {
            get { return (Min + Max) * 0.5f; }
        }

        public VECTOR2 Size
        {
            get { return new VECTOR2(Width, Height); }
        }

        public REAL Width
        {
            get { return Max.x - Min.x; }
        }

        public REAL Height
        {
            get { return Max.y - Min.y; }
        }

        public REAL Area
        {
            get { return (Max.x - Min.x) * (Max.y - Min.y); }
        }

        public static Box2f operator +(Box2f box, REAL s)
        {
            return new Box2f(box.Min + s, box.Max + s);
        }

        public static Box2f operator +(Box2f box, VECTOR2 v)
        {
            return new Box2f(box.Min + v, box.Max + v);
        }

        public static Box2f operator -(Box2f box, REAL s)
        {
            return new Box2f(box.Min - s, box.Max - s);
        }

        public static Box2f operator -(Box2f box, VECTOR2 v)
        {
            return new Box2f(box.Min - v, box.Max - v);
        }

        public static Box2f operator *(Box2f box, REAL s)
        {
            return new Box2f(box.Min * s, box.Max * s);
        }

        public static Box2f operator *(Box2f box, VECTOR2 v)
        {
            return new Box2f(box.Min * v, box.Max * v);
        }

        public static Box2f operator /(Box2f box, REAL s)
        {
            return new Box2f(box.Min / s, box.Max / s);
        }

        public static Box2f operator /(Box2f box, VECTOR2 v)
        {
            return new Box2f(box.Min / v, box.Max / v);
        }

        public static Box2f operator *(Box2f box, MATRIX2 m)
        {
            return new Box2f(m * box.Min, m * box.Max);
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

        public void GetCorners(IList<VECTOR2> corners)
        {
            corners[0] = new VECTOR2(Min.x, Min.y);
            corners[1] = new VECTOR2(Max.x, Min.y);
            corners[2] = new VECTOR2(Max.x, Max.y);
            corners[3] = new VECTOR2(Min.x, Max.y);
        }

        public void GetCornersXZ(IList<VECTOR3> corners, REAL y = 0)
        {
            corners[0] = new VECTOR3(Min.x, y, Min.y);
            corners[1] = new VECTOR3(Max.x, y, Min.y);
            corners[2] = new VECTOR3(Max.x, y, Max.y);
            corners[3] = new VECTOR3(Min.x, y, Max.y);
        }

        /// <summary>
        /// Returns the bounding box containing this box and the given point.
        /// </summary>
        public static Box2f Enlarge(Box2f box, VECTOR2 p)
        {
            var b = new Box2f();
            b.Min.x = Math.Min(box.Min.x, p.x);
            b.Min.y = Math.Min(box.Min.y, p.y);
            b.Max.x = Math.Max(box.Max.x, p.x);
            b.Max.y = Math.Max(box.Max.y, p.y);
            return b;
        }

        /// <summary>
        /// Returns the bounding box containing this box and the given box.
        /// </summary>
        public static Box2f Enlarge(Box2f box0, Box2f box1)
        {
            var b = new Box2f();
            b.Min.x = Math.Min(box0.Min.x, box1.Min.x);
            b.Min.y = Math.Min(box0.Min.y, box1.Min.y);
            b.Max.x = Math.Max(box0.Max.x, box1.Max.x);
            b.Max.y = Math.Max(box0.Max.y, box1.Max.y);
            return b;
        }

        /// <summary>
        /// Enlarge the box by a given percent.
        /// </summary>
        public static Box2f Enlarge(Box2f box, REAL percent)
        {
            var amount = box.Size * percent * 0.5f;
            return new Box2f(box.Min - amount, box.Max + amount);
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
        public bool Contains(VECTOR2 p)
        {
            if (p.x > Max.x || p.x < Min.x) return false;
            if (p.y > Max.y || p.y < Min.y) return false;
            return true;
        }

        /// <summary>
        /// Find the closest point to the box.
        /// If point inside box return point.
        /// </summary>
        public VECTOR2 Closest(VECTOR2 p)
        {
            VECTOR2 c;

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

        /// <summary>
        /// Return the signed distance to the point. 
        /// If point is outside box field is positive.
        /// If point is inside box field is negative.
        /// </summary>
        public REAL SignedDistance(VECTOR2 p)
        {
            VECTOR2 d = (p - Center).Absolute - Size * 0.5f;
            VECTOR2 max = VECTOR2.Max(d, 0);
            return max.Magnitude + Math.Min(Math.Max(d.x, d.y), 0.0f);
        }

        public static Box2f CalculateBounds(IList<VECTOR2> vertices)
        {
            VECTOR2 min = VECTOR2.PositiveInfinity;
            VECTOR2 max = VECTOR2.NegativeInfinity;

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

        public static Box2f CalculateBounds(VECTOR2 a, VECTOR2 b)
        {
            REAL xmin = Math.Min(a.x, b.x);
            REAL xmax = Math.Max(a.x, b.x);
            REAL ymin = Math.Min(a.y, b.y);
            REAL ymax = Math.Max(a.y, b.y);

            return new Box2f(xmin, xmax, ymin, ymax);
        }

    }

}

















