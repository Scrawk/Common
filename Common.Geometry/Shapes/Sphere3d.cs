using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Core.LinearAlgebra;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Sphere3d : IEquatable<Sphere3d>
    {

        public Vector3d Center;

        public double Radius;

        public Sphere3d(Vector3d center, double radius)
        {
            Center = center;
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
        /// The spheres diameter.
        /// </summary>
        public double Diameter
        {
            get { return Radius * 2.0; }
        }

        /// <summary>
        /// The spheres area.
        /// </summary>
        public double Area
        {
            get { return 4.0 / 3.0 * Math.PI * Radius * Radius * Radius; }
        }

        /// <summary>
        /// The spheres surface area.
        /// </summary>
        public double SurfaceArea
        {
            get { return 4.0 * Math.PI * Radius2; }
        }

        /// <summary>
        /// Calculate the bounding box.
        /// </summary>
        public Box3d Bounds
        {
            get
            {
                double xmin = Center.x - Radius;
                double xmax = Center.x + Radius;
                double ymin = Center.y - Radius;
                double ymax = Center.y + Radius;
                double zmin = Center.z - Radius;
                double zmax = Center.z + Radius;

                return new Box3d(xmin, xmax, ymin, ymax, zmin, zmax);
            }
        }

        public static bool operator ==(Sphere3d s1, Sphere3d s2)
        {
            return s1.Center == s2.Center && s1.Radius == s2.Radius;
        }

        public static bool operator !=(Sphere3d s1, Sphere3d s2)
        {
            return s1.Center != s2.Center || s1.Radius != s2.Radius;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Sphere3d)) return false;
            Sphere3d sphere = (Sphere3d)obj;
            return this == sphere;
        }

        public bool Equals(Sphere3d sphere)
        {
            return this == sphere;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ Center.GetHashCode();
                hash = (hash * 16777619) ^ Radius.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return string.Format("[Sphere3d: Center={0}, Radius={1}]", Center, Radius);
        }

        /// <summary>
        /// Enlarge the sphere so it contains the point p.
        /// </summary>
        public void Enlarge(Vector3d p)
        {
            Vector3d d = p - Center;
            double dist2 = d.SqrMagnitude;

            if (dist2 > Radius2)
            {
                double dist = Math.Sqrt(dist2);
                double radius = (Radius + dist) * 0.5;
                double k = (radius - Radius) / dist;

                Center += d * k;
                Radius = radius;
            }
        }

        /// <summary>
        /// Find the closest point on the spheres 
        /// surface to the point.
        /// </summary>
        /// <param name="p">a point that is not equal to the spheres center</param>
        /// <returns>The closest point on the surface</returns>
        public Vector3d Closest(Vector3d p)
        {
            Vector3d n = (Center - p).Normalized;
            if (n == Vector3d.Zero) n = Vector3d.UnitX;
            return Center + Radius * n;
        }

        /// <summary>
        /// Does the sphere contain the point.
        /// </summary>
        /// <param name="p">The point</param>
        /// <returns>true if sphere contains point</returns>
        public bool Contains(Vector3d p)
        {
            return Vector3d.SqrDistance(Center, p) <= Radius2;
        }

        /// <summary>
        /// Does this sphere intersect with the other sphere.
        /// </summary>
        /// <param name="sphere">The other sphere</param>
        /// <returns>True if the spheres intersect</returns>
        public bool Intersects(Sphere3d sphere)
        {
            double r = Radius + sphere.Radius;
            return Vector3d.SqrDistance(Center, sphere.Center) <= r * r;
        }

        /// <summary>
        /// Creates a sphere that has both points on its surface.
        /// </summary>
        public static Sphere3d CircumSphere(Vector3d p0, Vector3d p1)
        {
            var centre = (p0 + p1) * 0.5;
            var radius = Vector3d.Distance(p0, p1) * 0.5;
            var bounds = new Sphere3d(centre, radius);
            return bounds;
        }

        /// <summary>
        /// Creates a sphere that has all 4 points on its surface.
        /// From MathWorld: http://mathworld.wolfram.com/Circumsphere.html.
        /// Fails if the points are colinear.
        /// </summary>
        public static Sphere3d CircumSphere(Vector3d p0, Vector3d p1, Vector3d p2, Vector3d p3)
        {
            var m = new Matrix4x4d();

            // x, y, z, 1
            m.SetRow(0, new Vector4d(p0, 1));
            m.SetRow(1, new Vector4d(p1, 1));
            m.SetRow(2, new Vector4d(p2, 1));
            m.SetRow(3, new Vector4d(p3, 1));
            double a = m.Determinant;

            // size, y, z, 1
            m.SetColumn(0, new Vector4d(p0.SqrMagnitude, p1.SqrMagnitude, p2.SqrMagnitude, p3.SqrMagnitude));
            double dx = m.Determinant;

            // size, x, z, 1
            m.SetColumn(1, new Vector4d(p0.x, p1.x, p2.x, p3.x));
            double dy = -m.Determinant;

            // size, x, y, 1
            m.SetColumn(2, new Vector4d(p0.y, p1.y, p2.y, p3.y));
            double dz = m.Determinant;

            // size, x, y, z
            m.SetColumn(3, new Vector4d(p0.z, p1.z, p2.z, p3.z));
            double c = m.Determinant;

            double s = -1.0f / (2.0f * a);

            var circumCenter = new Vector3d(s * dx, s * dy, s * dz);
            double radius = Math.Abs(s) * Math.Sqrt(dx * dx + dy * dy + dz * dz - 4 * a * c);

            return new Sphere3d(circumCenter, radius);
        }

        /// <summary>
        /// Creates a sphere that contains all three points.
        /// </summary>
        public static Sphere3d CalculateBounds(Vector3d p0, Vector3d p1, Vector3d p2)
        {
            var bounds = CircumSphere(p0, p1);
            bounds.Enlarge(p2);
            return bounds;
        }

        /// <summary>
        /// Calculate the minimum bounding sphere that contains 
        /// all the points in the list.
        /// </summary>
        public static Sphere3d CalculateBounds(IList<Vector3d> points)
        {
            var idx = ExtremePoints(points);

            var bounds = CircumSphere(points[idx.x], points[idx.y]);

            int count = points.Count;
            for (int i = 2; i < count; i++)
                bounds.Enlarge(points[i]);

            return bounds;
        }

        /// <summary>
        /// Finds which axis contains the two most extreme points
        /// </summary>
        private static Vector2i ExtremePoints(IList<Vector3d> points)
        {
            Vector3i min = new Vector3i();
            Vector3i max = new Vector3i();

            int count = points.Count;
            for (int i = 0; i < count; i++)
            {
                var v = points[i];
                if (v.x < points[min.x].x) min.x = i;
                if (v.y < points[min.y].y) min.y = i;
                if (v.z < points[min.z].z) min.z = i;

                if (v.x > points[max.x].x) max.x = i;
                if (v.y > points[max.y].y) max.y = i;
                if (v.z > points[max.z].z) max.z = i;
            }

            var d2x = Vector3d.SqrDistance(points[max.x], points[min.x]);
            var d2y = Vector3d.SqrDistance(points[max.y], points[min.y]);
            var d2z = Vector3d.SqrDistance(points[max.z], points[min.z]);

            if (d2x > d2y && d2x > d2z)
                return new Vector2i(min.x, max.x);
            else if (d2y > d2z)
                return new Vector2i(min.y, max.y);
            else
                return new Vector2i(min.z, max.z);
        }

    }

}