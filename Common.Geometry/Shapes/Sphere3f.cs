﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Core.Numerics;

using REAL = System.Single;
using VECTOR2 = Common.Core.Numerics.Vector2f;
using VECTOR3 = Common.Core.Numerics.Vector3f;
using VECTOR4 = Common.Core.Numerics.Vector4f;
using MATRIX4 = Common.Core.Numerics.Matrix4x4f;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Sphere3f : IEquatable<Sphere3f>
    {

        public VECTOR3 Center;

        public REAL Radius;

        public Sphere3f(VECTOR3 center, REAL radius)
        {
            Center = center;
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
        /// The spheres diameter.
        /// </summary>
        public REAL Diameter
        {
            get { return Radius * 2.0f; }
        }

        /// <summary>
        /// The spheres area.
        /// </summary>
        public REAL Area
        {
            get { return 4.0f / 3.0f * FMath.PI * Radius * Radius * Radius; }
        }

        /// <summary>
        /// The spheres surface area.
        /// </summary>
        public REAL SurfaceArea
        {
            get { return 4.0f * FMath.PI * Radius2; }
        }

        /// <summary>
        /// Calculate the bounding box.
        /// </summary>
        public Box3f Bounds
        {
            get
            {
                REAL xmin = Center.x - Radius;
                REAL xmax = Center.x + Radius;
                REAL ymin = Center.y - Radius;
                REAL ymax = Center.y + Radius;
                REAL zmin = Center.z - Radius;
                REAL zmax = Center.z + Radius;

                return new Box3f(xmin, xmax, ymin, ymax, zmin, zmax);
            }
        }

        public static bool operator ==(Sphere3f s1, Sphere3f s2)
        {
            return s1.Center == s2.Center && s1.Radius == s2.Radius;
        }

        public static bool operator !=(Sphere3f s1, Sphere3f s2)
        {
            return s1.Center != s2.Center || s1.Radius != s2.Radius;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Sphere3f)) return false;
            Sphere3f sphere = (Sphere3f)obj;
            return this == sphere;
        }

        public bool Equals(Sphere3f sphere)
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
            return string.Format("[Sphere3f: Center={0}, Radius={1}]", Center, Radius);
        }

        /// <summary>
        /// Enlarge the sphere so it contains the point p.
        /// </summary>
        public void Enlarge(VECTOR3 p)
        {
            VECTOR3 d = p - Center;
            REAL dist2 = d.SqrMagnitude;

            if (dist2 > Radius2)
            {
                REAL dist = FMath.Sqrt(dist2);
                REAL radius = (Radius + dist) * 0.5f;
                REAL k = (radius - Radius) / dist;

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
        public VECTOR3 Closest(VECTOR3 p)
        {
            VECTOR3 n = (Center - p).Normalized;
            if (n == VECTOR3.Zero) n = VECTOR3.UnitX;
            return Center + Radius * n;
        }

        /// <summary>
        /// Does the sphere contain the point.
        /// </summary>
        /// <param name="p">The point</param>
        /// <returns>true if sphere contains point</returns>
        public bool Contains(VECTOR3 p)
        {
            return VECTOR3.SqrDistance(Center, p) <= Radius2;
        }

        /// <summary>
        /// Does this sphere intersect with the other sphere.
        /// </summary>
        /// <param name="sphere">The other sphere</param>
        /// <returns>True if the spheres intersect</returns>
        public bool Intersects(Sphere3f sphere)
        {
            REAL r = Radius + sphere.Radius;
            return VECTOR3.SqrDistance(Center, sphere.Center) <= r * r;
        }

        /// <summary>
        /// Creates a sphere that has both points on its surface.
        /// </summary>
        public static Sphere3f CircumSphere(VECTOR3 p0, VECTOR3 p1)
        {
            var centre = (p0 + p1) * 0.5f;
            var radius = VECTOR3.Distance(p0, p1) * 0.5f;
            var bounds = new Sphere3f(centre, radius);
            return bounds;
        }

        /// <summary>
        /// Creates a sphere that has all 4 points on its surface.
        /// From MathWorld: http://mathworld.wolfram.com/Circumsphere.html.
        /// Fails if the points are colinear.
        /// </summary>
        public static Sphere3f CircumSphere(VECTOR3 p0, VECTOR3 p1, VECTOR3 p2, VECTOR3 p3)
        {
            var m = new MATRIX4();

            // x, y, z, 1
            m.SetRow(0, new VECTOR4(p0, 1));
            m.SetRow(1, new VECTOR4(p1, 1));
            m.SetRow(2, new VECTOR4(p2, 1));
            m.SetRow(3, new VECTOR4(p3, 1));
            REAL a = m.Determinant;

            // size, y, z, 1
            m.SetColumn(0, new VECTOR4(p0.SqrMagnitude, p1.SqrMagnitude, p2.SqrMagnitude, p3.SqrMagnitude));
            REAL dx = m.Determinant;

            // size, x, z, 1
            m.SetColumn(1, new VECTOR4(p0.x, p1.x, p2.x, p3.x));
            REAL dy = -m.Determinant;

            // size, x, y, 1
            m.SetColumn(2, new VECTOR4(p0.y, p1.y, p2.y, p3.y));
            REAL dz = m.Determinant;

            // size, x, y, z
            m.SetColumn(3, new VECTOR4(p0.z, p1.z, p2.z, p3.z));
            REAL c = m.Determinant;

            REAL s = -1.0f / (2.0f * a);

            var circumCenter = new VECTOR3(s * dx, s * dy, s * dz);
            REAL radius = Math.Abs(s) * FMath.Sqrt(dx * dx + dy * dy + dz * dz - 4.0f * a * c);

            return new Sphere3f(circumCenter, radius);
        }

        /// <summary>
        /// Creates a sphere that contains all three points.
        /// </summary>
        public static Sphere3f CalculateBounds(VECTOR3 p0, VECTOR3 p1, VECTOR3 p2)
        {
            var bounds = CircumSphere(p0, p1);
            bounds.Enlarge(p2);
            return bounds;
        }

        /// <summary>
        /// Calculate the minimum bounding sphere that contains 
        /// all the points in the list.
        /// </summary>
        public static Sphere3f CalculateBounds(IList<VECTOR3> points)
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
        private static Vector2i ExtremePoints(IList<VECTOR3> points)
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

            var d2x = VECTOR3.SqrDistance(points[max.x], points[min.x]);
            var d2y = VECTOR3.SqrDistance(points[max.y], points[min.y]);
            var d2z = VECTOR3.SqrDistance(points[max.z], points[min.z]);

            if (d2x > d2y && d2x > d2z)
                return new Vector2i(min.x, max.x);
            else if (d2y > d2z)
                return new Vector2i(min.y, max.y);
            else
                return new Vector2i(min.z, max.z);
        }

    }

}