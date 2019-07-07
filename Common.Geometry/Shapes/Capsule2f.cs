using System;
using System.Runtime.InteropServices;

using Common.Core.Numerics;

using REAL = System.Single;
using VECTOR2 = Common.Core.Numerics.Vector2f;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Capsule2f : IEquatable<Capsule2f>
    {
        public VECTOR2 A;

        public VECTOR2 B;

        public REAL Radius;

        public Capsule2f(VECTOR2 a, VECTOR2 b, REAL radius)
        {
            A = a;
            B = b;
            Radius = radius;
        }

        public Capsule2f(REAL ax, REAL ay, REAL bx, REAL by, REAL radius)
        {
            A = new VECTOR2(ax, ay);
            B = new VECTOR2(bx, by);
            Radius = radius;
        }

        public VECTOR2 Center
        {
            get { return (A + B) * 0.5f; }
        }

        /// <summary>
        /// Calculate the bounding box.
        /// </summary>
        public Box2f Bounds
        {
            get
            {
                REAL xmin = Math.Min(A.x, B.x) - Radius;
                REAL xmax = Math.Max(A.x, B.x) + Radius;
                REAL ymin = Math.Min(A.y, B.y) - Radius;
                REAL ymax = Math.Max(A.y, B.y) + Radius;

                return new Box2f(xmin, xmax, ymin, ymax);
            }
        }

        public static bool operator ==(Capsule2f c1, Capsule2f c2)
        {
            return c1.Radius == c2.Radius && c1.A == c2.A && c1.B == c2.B;
        }

        public static bool operator !=(Capsule2f c1, Capsule2f c2)
        {
            return c1.Radius != c2.Radius || c1.A != c2.A || c1.A != c2.A;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Capsule2f)) return false;
            Capsule2f cap = (Capsule2f)obj;
            return this == cap;
        }

        public bool Equals(Capsule2f cap)
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
            return string.Format("[Capsule2f: A={0}, B={1}, Radius={2}]", A, B, Radius);
        }

        public bool Contains(VECTOR2 p)
        {
            REAL r2 = Radius * Radius;

            VECTOR2 ap = p - A;

            if (ap.x * ap.x + ap.y * ap.y <= r2) return true;

            VECTOR2 bp = p - B.x;

            if (bp.x * bp.x + bp.y * bp.y <= r2) return true;

            VECTOR2 ab = B - A;

            REAL t = (ab.x * A.x + ab.y * A.y) / (ab.x * ab.x + ab.y * ab.y);

            if (t < 0.0) t = 0.0f;
            if (t > 1.0) t = 1.0f;

            p = p - (A + t * ab);

            if (p.x * p.x + p.y * p.y <= r2) return true;

            return false;
        }

    }
}