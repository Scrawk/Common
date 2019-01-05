using System;
using System.Runtime.InteropServices;

using Common.Core.LinearAlgebra;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Capsule2d : IEquatable<Capsule2d>
    {
        public Vector2d A;

        public Vector2d B;

        public double Radius;

        public Capsule2d(Vector2d a, Vector2d b, double radius)
        {
            A = a;
            B = b;
            Radius = radius;
        }

        public Capsule2d(double ax, double ay, double bx, double by, double radius)
		{
            A = new Vector2d(ax, ay);
            B = new Vector2d(bx, by);
            Radius = radius;
		}

        public Vector2d Center
        {
            get { return (A + B) / 2.0; }
        }

        /// <summary>
        /// Calculate the bounding box.
        /// </summary>
        public Box2d Bounds
        {
            get
            {
                double xmin = Math.Min(A.x, B.x) - Radius;
                double xmax = Math.Max(A.x, B.x) + Radius;
                double ymin = Math.Min(A.y, B.y) - Radius;
                double ymax = Math.Max(A.y, B.y) + Radius;

                return new Box2d(xmin, xmax, ymin, ymax);
            }
        }

        public static bool operator ==(Capsule2d c1, Capsule2d c2)
        {
            return c1.Radius == c2.Radius && c1.A == c2.A && c1.B == c2.B;
        }

        public static bool operator !=(Capsule2d c1, Capsule2d c2)
        {
            return c1.Radius != c2.Radius || c1.A != c2.A || c1.A != c2.A;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Capsule2d)) return false;
            Capsule2d cap = (Capsule2d)obj;
            return this == cap;
        }

        public bool Equals(Capsule2d cap)
        {
            return this == cap;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ Radius.GetHashCode();
                hash = (hash * 16777619) ^ A.GetHashCode();
                hash = (hash * 16777619) ^ B.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return string.Format("[Capsule2d: A={0}, B={1}, Radius={2}]", A, B, Radius);
        }

        public bool Contains(Vector2d p)
        {
            double r2 = Radius * Radius;

            Vector2d ap = p - A;

            if (ap.x * ap.x + ap.y * ap.y <= r2) return true;

            Vector2d bp = p - B.x;

            if (bp.x * bp.x + bp.y * bp.y <= r2) return true;

            Vector2d ab = B - A;

            double t = (ab.x * A.x + ab.y * A.y) / (ab.x * ab.x + ab.y * ab.y);

            if (t < 0.0) t = 0.0;
            if (t > 1.0) t = 1.0;

            p = p - (A + t * ab);

            if (p.x * p.x + p.y * p.y <= r2) return true;

            return false;
        }

    }
}