using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Core.LinearAlgebra;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Circle2d : IEquatable<Circle2d>
    {
        public Vector2d Center;

        public double Radius;

        public Circle2d(Vector2d centre, double radius)
        {
            Center = centre;
            Radius = radius;
        }

        public Circle2d(double x, double y, double radius)
        {
            Center = new Vector2d(x, y);
            Radius = radius;
        }

        /// <summary>
        /// The squared radius.
        /// </summary>
        public double Radius2
        {
            get { return Radius * Radius; }
        }

        /// <summary>
        /// The circles diameter.
        /// </summary>
        public double Diameter
        {
            get { return Radius * 2.0; }
        }

        /// <summary>
        /// The circles area.
        /// </summary>
        public double Area
        {
            get { return Math.PI * Radius * Radius; }
        }

        /// <summary>
        /// the circles circumference.
        /// </summary>
        public double Circumference
        {
            get { return Math.PI * Radius * 2.0; }
        }

        /// <summary>
        /// Calculate the bounding box.
        /// </summary>
        public Box2d Bounds
        {
            get
            {
                double xmin = Center.x - Radius;
                double xmax = Center.x + Radius;
                double ymin = Center.y - Radius;
                double ymax = Center.y + Radius;

                return new Box2d(xmin, xmax, ymin, ymax);
            }
        }

        public static bool operator ==(Circle2d c1, Circle2d c2)
        {
            return c1.Radius == c2.Radius && c1.Center == c2.Center;
        }

        public static bool operator !=(Circle2d c1, Circle2d c2)
        {
            return c1.Radius != c2.Radius || c1.Center != c2.Center;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Circle2d)) return false;
            Circle2d cir = (Circle2d)obj;
            return this == cir;
        }

        public bool Equals(Circle2d cir)
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
            return string.Format("[Circle2d: Center={0}, Radius={1}]", Center, Radius);
        }

        /// <summary>
        /// Find the closest point on the circles 
        /// circumference to the point.
        /// </summary>
        /// <param name="p">a point that is not equal to the circles center</param>
        /// <returns>The closest point on the circumference</returns>
        public Vector2d Closest(Vector2d p)
        {
            Vector2d n = (Center - p).Normalized;
            if (n == Vector2d.Zero) n = Vector2d.UnitX;
            return Center + Radius * n;
        }

        /// <summary>
        /// Does the circle contain the point.
        /// </summary>
        /// <param name="p">The point</param>
        /// <returns>true if circle contains point</returns>
        public bool Contains(Vector2d p)
        {
            return Vector2d.SqrDistance(Center, p) <= Radius2;
        }

        /// <summary>
        /// Does this circle intersect with the other circle.
        /// </summary>
        /// <param name="circle">The other circle</param>
        /// <returns>True if the circles intersect</returns>
        public bool Intersects(Circle2d circle)
        {
            double r = Radius + circle.Radius;
            return Vector2d.SqrDistance(Center, circle.Center) <= r * r;
        }

        /// <summary>
        /// Enlarge the circle so it contains the point p.
        /// </summary>
        public static Circle2d Enlarge(Circle2d cir, Vector2d p)
        {
            Vector2d d = p - cir.Center;
            double dist2 = d.SqrMagnitude;

            if (dist2 > cir.Radius2)
            {
                double dist = Math.Sqrt(dist2);
                double radius = (cir.Radius + dist) * 0.5;
                double k = (radius - cir.Radius) / dist;

                cir.Center += d * k;
                cir.Radius = radius;
            }

            return cir;
        }

        /// <summary>
        /// Creates a circle that has both points on its circumference.
        /// </summary>
        public static Circle2d CircumCircle(Vector2d p0, Vector2d p1)
        {
            var centre = (p0 + p1) * 0.5;
            var radius = Vector2d.Distance(p0, p1) * 0.5;
            var bounds = new Circle2d(centre, radius);
            return bounds;
        }

        /// <summary>
        /// Creates a circle that has all 3 points on its circumference.
        /// From MathWorld: http://mathworld.wolfram.com/Circumcircle.html.
        /// Fails if the points are colinear.
        /// </summary>
        public static Circle2d CircumCircle(Vector2d p0, Vector2d p1, Vector2d p2)
        {
            var m = new Matrix3x3d();

            // x, y, 1
            m.SetRow(0, new Vector3d(p0.x, p0.y, 1));
            m.SetRow(1, new Vector3d(p1.x, p1.y, 1));
            m.SetRow(2, new Vector3d(p2.x, p2.y, 1));
            double a = m.Determinant;

            // size, y, 1
            m.SetColumn(0, new Vector3d(p0.SqrMagnitude, p1.SqrMagnitude, p2.SqrMagnitude));
            double dx = -m.Determinant;

            // size, x, 1
            m.SetColumn(1, new Vector3d(p0.x, p1.x, p2.x));
            double dy = m.Determinant;

            // size, x, y
            m.SetColumn(2, new Vector3d(p0.y, p1.y, p2.y));
            double c = -m.Determinant;

            double s = -1.0 / (2.0 * a);

            var circumCenter = new Vector2d(s * dx, s * dy);
            double radius = Math.Abs(s) * Math.Sqrt(dx * dx + dy * dy - 4.0 * a * c);

            return new Circle2d(circumCenter, radius);
        }

        /// <summary>
        /// Creates a circle that contains all three point.
        /// </summary>
        public static Circle2d CalculateBounds(Vector2d p0, Vector2d p1, Vector2d p2)
        {
            var bounds = CircumCircle(p0, p1);
            return Enlarge(bounds, p2);
        }

        /// <summary>
        /// Calculate the minimum bounding circle that contains 
        /// all the points in the list.
        /// </summary>
        public static Circle2d CalculateBounds(IList<Vector2d> points)
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
        private static Vector2i ExtremePoints(IList<Vector2d> points)
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

            var d2x = Vector2d.SqrDistance(points[max.x], points[min.x]);
            var d2y = Vector2d.SqrDistance(points[max.y], points[min.y]);

            if (d2x > d2y)
                return new Vector2i(min.x, max.x);
            else
                return new Vector2i(min.y, max.y);
        }
    }
}