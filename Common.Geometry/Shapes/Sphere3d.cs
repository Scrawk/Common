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

        public double Radius2
        {
            get { return Radius * Radius; }
        }

        public double Diameter
        {
            get { return Radius * 2.0; }
        }

        public double Area
        {
            get { return 4.0 / 3.0 * Math.PI * Radius * Radius * Radius; }
        }

        public double SurfaceArea
        {
            get { return 4.0 * Math.PI * Radius2; }
        }

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

        public Vector3d Closest(Vector3d p)
        {
            double dist = Vector3d.Distance(p, Center);
            return Center + Radius * dist;
        }

        public bool Contains(Vector3d p)
        {
            double r2 = Radius * Radius;
            return Vector3d.SqrDistance(Center, p) <= r2;
        }

        public bool Intersects(Sphere3d sphere)
        {
            double r = Radius + sphere.Radius;
            return Vector3d.SqrDistance(Center, sphere.Center) <= r * r;
        }

    }

}