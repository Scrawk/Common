using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Core.LinearAlgebra;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Circle2f : IEquatable<Circle2f>
    {
        public Vector2f Center;

        public float Radius;

        public Circle2f(Vector2f centre, float radius)
        {
            Center = centre;
            Radius = radius;
        }

        public Circle2f(float x, float y, float radius)
        {
            Center = new Vector2f(x, y);
            Radius = radius;
        }

        /// <summary>
        /// The squared radius.
        /// </summary>
        public float Radius2
        {
            get { return Radius * Radius; }
        }

        /// <summary>
        /// The circles diameter.
        /// </summary>
        public float Diameter
        {
            get { return Radius * 2.0f; }
        }

        /// <summary>
        /// The circles area.
        /// </summary>
        public float Area
        {
            get { return (float)Math.PI * Radius * Radius; }
        }

        /// <summary>
        /// the circles circumference.
        /// </summary>
        public float Circumference
        {
            get { return (float)Math.PI * Radius * 2.0f; }
        }

        /// <summary>
        /// Calculate the bounding box.
        /// </summary>
        public Box2f Bounds
        {
            get
            {
                float xmin = Center.x - Radius;
                float xmax = Center.x + Radius;
                float ymin = Center.y - Radius;
                float ymax = Center.y + Radius;

                return new Box2f(xmin, xmax, ymin, ymax);
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
        /// Find the closest point on the circles 
        /// circumference to the point.
        /// </summary>
        /// <param name="p">a point that is not equal to the circles center</param>
        /// <returns>The closest point on the circumference</returns>
        public Vector2f Closest(Vector2f p)
        {
            Vector2f n = (Center - p).Normalized;
            if (n == Vector2f.Zero) n = Vector2f.UnitX;
            return Center + Radius * n;
        }

        /// <summary>
        /// Does the circle contain the point.
        /// </summary>
        /// <param name="p">The point</param>
        /// <returns>true if circle contains point</returns>
        public bool Contains(Vector2f p)
        {
            return Vector2f.SqrDistance(Center, p) <= Radius2;
        }

        /// <summary>
        /// Does this circle intersect with the other circle.
        /// </summary>
        /// <param name="circle">The other circle</param>
        /// <returns>True if the circles intersect</returns>
        public bool Intersects(Circle2f circle)
        {
            float r = Radius + circle.Radius;
            return Vector2f.SqrDistance(Center, circle.Center) <= r * r;
        }

        /// <summary>
        /// Enlarge the circle so it contains the point p.
        /// </summary>
        public static Circle2f Enlarge(Circle2f cir, Vector2f p)
        {
            Vector2f d = p - cir.Center;
            float dist2 = d.SqrMagnitude;

            if (dist2 > cir.Radius2)
            {
                float dist = (float)Math.Sqrt(dist2);
                float radius = (cir.Radius + dist) * 0.5f;
                float k = (radius - cir.Radius) / dist;

                cir.Center += d * k;
                cir.Radius = radius;
            }

            return cir;
        }

        /// <summary>
        /// Creates a circle that has both points on its circumference.
        /// </summary>
        public static Circle2f CircumCircle(Vector2f p0, Vector2f p1)
        {
            var centre = (p0 + p1) * 0.5f;
            var radius = Vector2f.Distance(p0, p1) * 0.5f;
            var bounds = new Circle2f(centre, radius);
            return bounds;
        }

        /// <summary>
        /// Creates a circle that has all 3 points on its circumference.
        /// From MathWorld: http://mathworld.wolfram.com/Circumcircle.html.
        /// Fails if the points are colinear.
        /// </summary>
        public static Circle2f CircumCircle(Vector2f p0, Vector2f p1, Vector2f p2)
        {
            var m = new Matrix3x3f();

            // x, y, 1
            m.SetRow(0, new Vector3f(p0, 1));
            m.SetRow(1, new Vector3f(p1, 1));
            m.SetRow(2, new Vector3f(p2, 1));
            float a = m.Determinant;

            // size, y, 1
            m.SetColumn(0, new Vector3f(p0.SqrMagnitude, p1.SqrMagnitude, p2.SqrMagnitude));
            float dx = -m.Determinant;

            // size, x, 1
            m.SetColumn(1, new Vector3f(p0.x, p1.x, p2.x));
            float dy = m.Determinant;

            // size, x, y
            m.SetColumn(2, new Vector3f(p0.y, p1.y, p2.y));
            float c = -m.Determinant;

            float s = -1.0f / (2.0f * a);

            var circumCenter = new Vector2f(s * dx, s * dy);
            float radius = Math.Abs(s) * (float)Math.Sqrt(dx * dx + dy * dy - 4.0 * a * c);

            return new Circle2f(circumCenter, radius);
        }

        /// <summary>
        /// Creates a circle that contains all three point.
        /// </summary>
        public static Circle2f CalculateBounds(Vector2f p0, Vector2f p1, Vector2f p2)
        {
            var bounds = CircumCircle(p0, p1);
            return Enlarge(bounds, p2);
        }

        /// <summary>
        /// Calculate the minimum bounding circle that contains 
        /// all the points in the list.
        /// </summary>
        public static Circle2f CalculateBounds(IList<Vector2f> points)
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
        private static Vector2i ExtremePoints(IList<Vector2f> points)
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

            var d2x = Vector2f.SqrDistance(points[max.x], points[min.x]);
            var d2y = Vector2f.SqrDistance(points[max.y], points[min.y]);

            if(d2x > d2y)
                return new Vector2i(min.x, max.x);
            else
                return new Vector2i(min.y, max.y);
        }
    }
}