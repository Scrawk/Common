using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Core.Numerics;

using REAL = System.Single;
using VECTOR2 = Common.Core.Numerics.Vector2f;
using BOX2 = Common.Geometry.Shapes.Box2f;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Circle2f : IEquatable<Circle2f>, IShape2f
    {
        public VECTOR2 Center;

        public REAL Radius;

        public Circle2f(VECTOR2 centre, REAL radius)
        {
            Center = centre;
            Radius = radius;
        }

        public Circle2f(REAL x, REAL y, REAL radius)
        {
            Center = new VECTOR2(x, y);
            Radius = radius;
        }

        /// <summary>
        /// The squared radius.
        /// </summary>
        public REAL Radius2
        {
            get { return Radius * Radius; }
        }

        /// <summary>
        /// The circles diameter.
        /// </summary>
        public REAL Diameter
        {
            get { return Radius * 2.0f; }
        }

        /// <summary>
        /// The circles area.
        /// </summary>
        public REAL Area
        {
            get { return MathUtil.PI * Radius * Radius; }
        }

        /// <summary>
        /// the circles circumference.
        /// </summary>
        public REAL Circumference
        {
            get { return MathUtil.PI * Radius * 2.0f; }
        }

        /// <summary>
        /// Calculate the bounding box.
        /// </summary>
        public BOX2 Bounds
        {
            get
            {
                REAL xmin = Center.x - Radius;
                REAL xmax = Center.x + Radius;
                REAL ymin = Center.y - Radius;
                REAL ymax = Center.y + Radius;

                return new BOX2(xmin, xmax, ymin, ymax);
            }
        }

        public static bool operator ==(Circle2f c1, Circle2f c2)
        {
            return c1.Radius == c2.Radius && c1.Center == c2.Center;
        }

        public static bool operator !=(Circle2f c1, Circle2f c2)
        {
            return c1.Radius != c2.Radius || c1.Center != c2.Center;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Circle2f)) return false;
            Circle2f cir = (Circle2f)obj;
            return this == cir;
        }

        public bool Equals(Circle2f cir)
        {
            return this == cir;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ Radius.GetHashCode();
                hash = (hash * 16777619) ^ Center.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return string.Format("[Circle2f: Center={0}, Radius={1}]", Center, Radius);
        }

        /// <summary>
        /// Find the closest point to the circle.
        /// If point inside circle return point.
        /// </summary>
        public VECTOR2 Closest(VECTOR2 p)
        {
            VECTOR2 d = Center - p;
            if (d.SqrMagnitude <= Radius2) return p;
            return Center + Radius * d.Normalized;
        }

        /// <summary>
        /// Return the signed distance to the point. 
        /// If point is outside circle field is positive.
        /// If point is inside circle field is negative.
        /// </summary>
        public REAL SignedDistance(VECTOR2 p)
        {
            p = p - Center;
            return p.Magnitude - Radius;
        }

        /// <summary>
        /// Does the circle contain the point.
        /// </summary>
        /// <param name="p">The point</param>
        /// <returns>true if circle contains point</returns>
        public bool Contains(VECTOR2 p)
        {
            return VECTOR2.SqrDistance(Center, p) <= Radius2;
        }

        /// <summary>
        /// Does the circle fully contain the box.
        /// </summary>
        public bool Contains(BOX2 box)
        {
            if (!Contains(box.Corner00)) return false;
            if (!Contains(box.Corner01)) return false;
            if (!Contains(box.Corner10)) return false;
            if (!Contains(box.Corner11)) return false;
            return true;
        }

        /// <summary>
        /// Does this circle intersect with the other circle.
        /// </summary>
        /// <param name="circle">The other circle</param>
        /// <returns>True if the circles intersect</returns>
        public bool Intersects(Circle2f circle)
        {
            REAL r = Radius + circle.Radius;
            return VECTOR2.SqrDistance(Center, circle.Center) <= r * r;
        }

        /// <summary>
        /// Does the circle intersect the box.
        /// </summary>
        public bool Intersects(BOX2 box)
        {
            var p = box.Closest(Center);
            return VECTOR2.SqrDistance(p, Center) <= Radius2;
        }

        /// <summary>
        /// Enlarge the circle so it contains the point p.
        /// </summary>
        public static Circle2f Enlarge(Circle2f cir, VECTOR2 p)
        {
            VECTOR2 d = p - cir.Center;
            REAL dist2 = d.SqrMagnitude;

            if (dist2 > cir.Radius2)
            {
                REAL dist = MathUtil.Sqrt(dist2);
                REAL radius = (cir.Radius + dist) * 0.5f;
                REAL k = (radius - cir.Radius) / dist;

                cir.Center += d * k;
                cir.Radius = radius;
            }

            return cir;
        }

        /// <summary>
        /// Returns true if the point d is inside the circle defined by the points a, b, c.
        /// </summary>
        public static bool InCircle(VECTOR2 a, VECTOR2 b, VECTOR2 c, VECTOR2 d)
        {
            return (a.x * a.x + a.y * a.y) * Triangle2f.CrossProductArea(b, c, d) -
                    (b.x * b.x + b.y * b.y) * Triangle2f.CrossProductArea(a, c, d) +
                    (c.x * c.x + c.y * c.y) * Triangle2f.CrossProductArea(a, b, d) -
                    (d.x * d.x + d.y * d.y) * Triangle2f.CrossProductArea(a, b, c) > 0;
        }

        /// <summary>
        /// Creates a circle that has both points on its circumference.
        /// </summary>
        public static Circle2f CircumCircle(VECTOR2 p0, VECTOR2 p1)
        {
            var centre = (p0 + p1) * 0.5f;
            var radius = VECTOR2.Distance(p0, p1) * 0.5f;
            var bounds = new Circle2f(centre, radius);
            return bounds;
        }

        /// <summary>
        /// Creates a circle that has all 3 points on its circumference.
        /// From MathWorld: http://mathworld.wolfram.com/Circumcircle.html.
        /// Fails if the points are colinear.
        /// </summary>
        public static Circle2f CircumCircle(VECTOR2 p0, VECTOR2 p1, VECTOR2 p2)
        {
            var m = new Matrix3x3f();

            // x, y, 1
            m.SetRow(0, new Vector3f(p0.x, p0.y, 1));
            m.SetRow(1, new Vector3f(p1.x, p1.y, 1));
            m.SetRow(2, new Vector3f(p2.x, p2.y, 1));
            REAL a = m.Determinant;

            // size, y, 1
            m.SetColumn(0, new Vector3f(p0.SqrMagnitude, p1.SqrMagnitude, p2.SqrMagnitude));
            REAL dx = -m.Determinant;

            // size, x, 1
            m.SetColumn(1, new Vector3f(p0.x, p1.x, p2.x));
            REAL dy = m.Determinant;

            // size, x, y
            m.SetColumn(2, new Vector3f(p0.y, p1.y, p2.y));
            REAL c = -m.Determinant;

            REAL s = -1.0f / (2.0f * a);

            var circumCenter = new VECTOR2(s * dx, s * dy);
            REAL radius = Math.Abs(s) * MathUtil.Sqrt(dx * dx + dy * dy - 4.0f * a * c);

            return new Circle2f(circumCenter, radius);
        }

        /// <summary>
        /// Creates a circle that contains all three point.
        /// </summary>
        public static Circle2f CalculateBounds(VECTOR2 p0, VECTOR2 p1, VECTOR2 p2)
        {
            var bounds = CircumCircle(p0, p1);
            return Enlarge(bounds, p2);
        }

        /// <summary>
        /// Calculate the bounding circle that contains 
        /// all the points in the list.
        /// </summary>
        public static Circle2f CalculateBounds(IList<VECTOR2> points)
        {
            var idx = ExtremePoints(points);

            var bounds = CircumCircle(points[idx.x], points[idx.y]);

            int count = points.Count;
            for (int i = 2; i < count; i++)
                bounds = Enlarge(bounds, points[i]);

            return bounds;
        }

        /// <summary>
        /// Finds which axis contains the two most extreme points
        /// </summary>
        private static Vector2i ExtremePoints(IList<VECTOR2> points)
        {
            Vector2i min = new Vector2i();
            Vector2i max = new Vector2i();

            int count = points.Count;
            for (int i = 0; i < count; i++)
            {
                var v = points[i];
                if (v.x < points[min.x].x) min.x = i;
                if (v.y < points[min.y].y) min.y = i;

                if (v.x > points[max.x].x) max.x = i;
                if (v.y > points[max.y].y) max.y = i;
            }

            var d2x = VECTOR2.SqrDistance(points[max.x], points[min.x]);
            var d2y = VECTOR2.SqrDistance(points[max.y], points[min.y]);

            if (d2x > d2y)
                return new Vector2i(min.x, max.x);
            else
                return new Vector2i(min.y, max.y);
        }
    }
}