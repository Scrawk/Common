using System;
using System.Runtime.InteropServices;

using Common.Core.Numerics;

using REAL = System.Single;
using VECTOR2 = Common.Core.Numerics.Vector2f;
using BOX2 = Common.Geometry.Shapes.Box2f;
using SEGMENT2 = Common.Geometry.Shapes.Segment2f;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Capsule2f : IEquatable<Capsule2f>, IShape2f
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

        /// <summary>
        /// The center position of the capsule.
        /// </summary>
        public VECTOR2 Center => (A + B) * 0.5f;

        /// <summary>
        /// The capsules squared radius at the end points.
        /// </summary>
        public REAL Radius2 => Radius * Radius;

        /// <summary>
        /// The capsules diameter at the end points.
        /// </summary>
        public REAL Diameter => Radius * 2.0f;

        /// <summary>
        /// The segment made from the capsules two points.
        /// </summary>
        public SEGMENT2 Segment => new SEGMENT2(A, B);

        /// <summary>
        /// The capsules bounding box.
        /// </summary>
        public BOX2 Bounds
        {
            get
            {
                REAL xmin = Math.Min(A.x, B.x) - Radius;
                REAL xmax = Math.Max(A.x, B.x) + Radius;
                REAL ymin = Math.Min(A.y, B.y) - Radius;
                REAL ymax = Math.Max(A.y, B.y) + Radius;

                return new BOX2(xmin, xmax, ymin, ymax);
            }
        }

        /// <summary>
        /// Access point a or b by index.
        /// </summary>
        /// <param name="i">A index of 0 or 1.</param>
        /// <returns>The point at index i.</returns>
        unsafe public VECTOR2 this[int i]
        {
            get
            {
                if ((uint)i >= 2)
                    throw new IndexOutOfRangeException("Capsule2f index out of range.");

                fixed (Capsule2f* array = &this) { return ((VECTOR2*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 2)
                    throw new IndexOutOfRangeException("Capsule2f index out of range.");

                fixed (VECTOR2* array = &A) { array[i] = value; }
            }
        }

        /// <summary>
        /// Cast the capsule.
        /// </summary>
        /// <param name="cap"></param>
        //public static explicit operator Capsule2f(Capsule2d cap)
        //{
        //    return new Capsule2f((Vector2f)cap.A, (Vector2f)cap.B, (float)cap.Radius);
        //}

        /// <summary>
        /// Are these two capsules equal.
        /// </summary>
        public static bool operator ==(Capsule2f c1, Capsule2f c2)
        {
            return c1.Radius == c2.Radius && c1.A == c2.A && c1.B == c2.B;
        }

        /// <summary>
        /// Are these two capsules not equal.
        /// </summary>
        public static bool operator !=(Capsule2f c1, Capsule2f c2)
        {
            return c1.Radius != c2.Radius || c1.A != c2.A || c1.A != c2.A;
        }

        /// <summary>
        /// Are these two capsules equal.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is Capsule2f)) return false;
            Capsule2f cap = (Capsule2f)obj;
            return this == cap;
        }

        /// <summary>
        /// Are these two capsules equal.
        /// </summary>
        public bool Equals(Capsule2f cap)
        {
            return this == cap;
        }

        /// <summary>
        /// The capsules hash code.
        /// </summary>
        /// <returns></returns>
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
        /// The capsules description.
        /// </summary>
        public override string ToString()
        {
            return string.Format("[Capsule2f: A={0}, B={1}, Radius={2}]", A, B, Radius);
        }

        /// <summary>
        /// Does the capsule contain the point.
        /// </summary>
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

        /// <summary>
        /// Find the closest point to the capsule
        /// If point inside capsule return point.
        /// </summary>
        public VECTOR2 Closest(VECTOR2 p)
        {
            var seg = Segment;
            REAL sd = seg.SignedDistance(p) - Radius;

            if (sd <= 0)
                return p;
            else
            {
                var c = seg.Closest(p);
                return (c - p).Normalized * Radius;
            }
        }

        /// <summary>
        /// Does the capsule intersect with the box.
        /// </summary>
        public bool Intersects(BOX2 box)
        {
            var a = box.Closest(A);
            if (SignedDistance(a) <= Radius)
                return true;

            var b = box.Closest(B);
            if (SignedDistance(b) <= Radius)
                return true;

            return false;
        }

        /// <summary>
        /// Does the capsule intersect with the capsule.
        /// </summary>
        public bool Intersects(Capsule2f capsule)
        {
            var closest = Segment.Closest(capsule.Segment);
            return closest.Length <= Radius + capsule.Radius;
        }

        /// <summary>
        /// The signed distance to the point.
        /// </summary>
        public REAL SignedDistance(VECTOR2 p)
        {
	        return Segment.SignedDistance(p) - Radius;
        }

    }
}