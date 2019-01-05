using System;
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

        public float Radius2
        {
            get { return Radius * Radius; }
        }

        public float Diameter
        {
            get { return Radius * 2.0f; }
        }

        public float Area
        {
            get { return (float)Math.PI * Radius * Radius; }
        }

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
        /// Enlarge the circle so it contains the point p.
        /// </summary>
        public void Enlarge(Vector2f p)
        {
            Vector2f d = p - Center;
            float dist2 = d.SqrMagnitude;

            if (dist2 > Radius2)
            {
                float dist = (float)Math.Sqrt(dist2);
                float radius = (Radius + dist) * 0.5f;
                float k = (radius - Radius) / dist;

                Center += d * k;
                Radius = radius;
            }
        }

        public Vector2f Closest(Vector2f p)
        {
            float dist = Vector2f.Distance(Center, p);
            return Center + Radius * dist;
        }

        public bool Contains(Vector2f p)
        {
            float r2 = Radius * Radius;
            return Vector2f.SqrDistance(Center, p) <= r2;
        }

        public bool Intersects(Circle2f circle)
        {
            float r = Radius + circle.Radius;
            return Vector2f.SqrDistance(Center, circle.Center) <= r * r;
        }

    }
}