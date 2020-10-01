﻿using System;
using System.Runtime.InteropServices;

using Common.Core.Numerics;

using REAL = System.Single;
using VECTOR2 = Common.Core.Numerics.Vector2f;
using BOX2 = Common.Geometry.Shapes.Box2f;

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
        /// Calculate the bounding box.
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
            var seg = new Segment2f(A, B);
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
            var c = Closest(box.Center);
            return SignedDistance(c) <= Radius;
        }

        /// <summary>
        /// The signed distance to the point.
        /// </summary>
        public REAL SignedDistance(VECTOR2 p)
        {
            var seg = new Segment2f(A, B);
	        return seg.SignedDistance(p) - Radius;
        }

    }
}