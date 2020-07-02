using System;
using System.Runtime.InteropServices;

using Common.Core.Numerics;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Capsule2f : IEquatable<Capsule2f>, IShape2f
    {
        public Vector2f A;

        public Vector2f B;

        public float Radius;

        public Capsule2f(Vector2f a, Vector2f b, float radius)
        {
            A = a;
            B = b;
            Radius = radius;
        }

        public Capsule2f(float ax, float ay, float bx, float by, float radius)
        {
            A = new Vector2f(ax, ay);
            B = new Vector2f(bx, by);
            Radius = radius;
        }

        /// <summary>
        /// The center position of the capsule.
        /// </summary>
        public Vector2f Center
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
                float xmin = Math.Min(A.x, B.x) - Radius;
                float xmax = Math.Max(A.x, B.x) + Radius;
                float ymin = Math.Min(A.y, B.y) - Radius;
                float ymax = Math.Max(A.y, B.y) + Radius;

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

        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            return string.Format("[Capsule2f: A={0}, B={1}, Radius={2}]", A, B, Radius);
        }

        /// <summary>
        /// Does the capsule contain the point.
        /// </summary>
        public bool Contains(Vector2f p)
        {
            float r2 = Radius * Radius;

            Vector2f ap = p - A;

            if (ap.x * ap.x + ap.y * ap.y <= r2) return true;

            Vector2f bp = p - B.x;

            if (bp.x * bp.x + bp.y * bp.y <= r2) return true;

            Vector2f ab = B - A;

            float t = (ab.x * A.x + ab.y * A.y) / (ab.x * ab.x + ab.y * ab.y);

            if (t < 0.0) t = 0.0f;
            if (t > 1.0) t = 1.0f;

            p = p - (A + t * ab);

            if (p.x * p.x + p.y * p.y <= r2) return true;

            return false;
        }

        /// <summary>
        /// The closest point to the surface of the capsule.
        /// </summary>
        public Vector2f Closest(Vector2f p)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Does the capsule intersect with the box.
        /// </summary>
        public bool Intersects(Box2f box)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The signed distance to the point.
        /// </summary>
        public float SignedDistance(Vector2f p)
        {
            var seg = new Segment2f(A, B);
	    return seg.SignedDistance(p) - Radius;
        }

    }
}