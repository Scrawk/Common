using System;
using System.Runtime.InteropServices;

using Common.Core.Numerics;

using REAL = System.Single;
using VECTOR2 = Common.Core.Numerics.Vector2f;
using MATRIX2 = Common.Core.Numerics.Matrix2x2f;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Segment2f : IEquatable<Segment2f>
    {

        public VECTOR2 A;

        public VECTOR2 B;

        public Segment2f(VECTOR2 a, VECTOR2 b)
        {
            A = a;
            B = b;
        }

        public Segment2f(REAL ax, REAL ay, REAL bx, REAL by)
        {
            A = new VECTOR2(ax, ay);
            B = new VECTOR2(bx, by);
        }

        public VECTOR2 Center
        {
            get { return (A + B) * 0.5f; }
        }

        public REAL Length
        {
            get { return VECTOR2.Distance(A, B); }
        }

        public REAL SqrLength
        {
            get { return VECTOR2.SqrDistance(A, B); }
        }

        public VECTOR2 Normal
        {
            get
            {
                return (B - A).Normalized.PerpendicularCW;
            }
        }

        public Box2f Bounds
        {
            get
            {
                REAL xmin = Math.Min(A.x, B.x);
                REAL xmax = Math.Max(A.x, B.x);
                REAL ymin = Math.Min(A.y, B.y);
                REAL ymax = Math.Max(A.y, B.y);

                return new Box2f(xmin, xmax, ymin, ymax);
            }
        }

        unsafe public VECTOR2 this[int i]
        {
            get
            {
                if ((uint)i >= 2)
                    throw new IndexOutOfRangeException("Segment2f index out of range.");

                fixed (Segment2f* array = &this) { return ((VECTOR2*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 2)
                    throw new IndexOutOfRangeException("Segment2f index out of range.");

                fixed (VECTOR2* array = &A) { array[i] = value; }
            }
        }


        public static Segment2f operator +(Segment2f seg, REAL s)
        {
            return new Segment2f(seg.A + s, seg.B + s);
        }

        public static Segment2f operator +(Segment2f seg, VECTOR2 v)
        {
            return new Segment2f(seg.A + v, seg.B + v);
        }

        public static Segment2f operator -(Segment2f seg, REAL s)
        {
            return new Segment2f(seg.A - s, seg.B - s);
        }

        public static Segment2f operator -(Segment2f seg, VECTOR2 v)
        {
            return new Segment2f(seg.A - v, seg.B - v);
        }

        public static Segment2f operator *(Segment2f seg, REAL s)
        {
            return new Segment2f(seg.A * s, seg.B * s);
        }

        public static Segment2f operator *(Segment2f seg, VECTOR2 v)
        {
            return new Segment2f(seg.A * v, seg.B * v);
        }

        public static Segment2f operator /(Segment2f seg, REAL s)
        {
            return new Segment2f(seg.A / s, seg.B / s);
        }

        public static Segment2f operator /(Segment2f seg, VECTOR2 v)
        {
            return new Segment2f(seg.A / v, seg.B / v);
        }

        public static Segment2f operator *(Segment2f seg, MATRIX2 m)
        {
            return new Segment2f(m * seg.A, m * seg.B);
        }

        public static explicit operator Segment2f(Segment2d seg)
        {
            return new Segment2f((VECTOR2)seg.A, (VECTOR2)seg.B);
        }

        public static bool operator ==(Segment2f s1, Segment2f s2)
        {
            return s1.A == s2.A && s1.B == s2.B;
        }

        public static bool operator !=(Segment2f s1, Segment2f s2)
        {
            return s1.A != s2.A || s1.B != s2.B;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Segment2f)) return false;
            Segment2f seg = (Segment2f)obj;
            return this == seg;
        }

        public bool Equals(Segment2f seg)
        {
            return this == seg;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ A.GetHashCode();
                hash = (hash * 16777619) ^ B.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return string.Format("[Segment2f: A={0}, B={1}]", A, B);
        }

        /// <summary>
        /// Does the two segments intersect.
        /// </summary>
        /// <param name="seg">other segment</param>
        public bool Intersects(Segment2f seg)
        {
            return Intersects(seg, out REAL t);
        }

        /// <summary>
        /// Do the two segments intersect.
        /// </summary>
        /// <param name="seg">other segment</param>
        /// <param name="t">Intersection point = A + t * (B - A)</param>
        /// <returns>If they intersect</returns>
        public bool Intersects(Segment2f seg, out REAL t)
        {
            REAL area1 = SignedTriArea(A, B, seg.B);
            REAL area2 = SignedTriArea(A, B, seg.A);
            t = 0.0f;

            if (area1 * area2 < 0.0)
            {
                REAL area3 = SignedTriArea(seg.A, seg.B, A);
                REAL area4 = area3 + area2 - area1;

                if (area3 * area4 < 0.0)
                {
                    t = area3 / (area3 - area4);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Do the two segments intersect.
        /// </summary>
        /// <param name="seg">other segment</param>
        /// <param name="s">Intersection point = A + s * (B - A)</param>
        /// <param name="t">Intersection point = seg.A + t * (seg.B - seg.A)</param>
        /// <returns>If they intersect</returns>
        public bool Intersects(Segment2f seg, out REAL s, out REAL t)
        {

            REAL area1 = SignedTriArea(A, B, seg.B);
            REAL area2 = SignedTriArea(A, B, seg.A);
            s = 0.0f;
            t = 0.0f;

            if (area1 * area2 < 0.0)
            {
                REAL area3 = SignedTriArea(seg.A, seg.B, A);
                REAL area4 = area3 + area2 - area1;

                if (area3 * area4 < 0.0)
                {
                    s = area3 / (area3 - area4);

                    REAL a2 = area2;
                    REAL a3 = area3;

                    area1 = SignedTriArea(seg.A, seg.B, B);
                    area2 = a3;
                    area3 = a2;
                    area4 = area3 + area2 - area1;
                    t = area3 / (area3 - area4);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// The closest point on segment to point.
        /// </summary>
        /// <param name="p">point</param>
        public VECTOR2 Closest(VECTOR2 p)
        {
            REAL t;
            Closest(p, out t);
            return A + (B - A) * t;
        }

        /// <summary>
        /// The closest point on segment to point.
        /// </summary>
        /// <param name="p">point</param>
        /// <param name="t">closest point = A + t * (B - A)</param>
        public void Closest(VECTOR2 p, out REAL t)
        {
            t = 0.0f;
            VECTOR2 ab = B - A;
            VECTOR2 ap = p - A;

            REAL len = ab.x * ab.x + ab.y * ab.y;
            if (len < FMath.EPS) return;

            t = (ab.x * ap.x + ab.y * ap.y) / len;
            t = FMath.Clamp01(t);
        }

        /// <summary>
        /// Return the signed distance to the point. 
        /// Always positive.
        /// </summary>
        public REAL SignedDistance(VECTOR2 p)
        {
            return VECTOR2.Distance(Closest(p), p);
        }

        /// <summary>
        /// The closest segment spanning two other segments.
        /// </summary>
        /// <param name="seg">the other segment</param>
        public Segment2f Closest(Segment2f seg)
        {
            REAL s, t;
            Closest(seg, out s, out t);
            return new Segment2f(A + (B - A) * s, seg.A + (seg.B - seg.A) * t);
        }

        /// <summary>
        /// The closest segment spanning two other segments.
        /// </summary>
        /// <param name="seg">the other segment</param>
        /// <param name="s">closest point = A + s * (B - A)</param>
        /// <param name="t">other closest point = seg.A + t * (seg.B - seg.A)</param>
        public void Closest(Segment2f seg, out REAL s, out REAL t)
        {

            VECTOR2 ab0 = B - A;
            VECTOR2 ab1 = seg.B - seg.A;
            VECTOR2 a01 = A - seg.A;

            REAL d00 = VECTOR2.Dot(ab0, ab0);
            REAL d11 = VECTOR2.Dot(ab1, ab1);
            REAL d1 = VECTOR2.Dot(ab1, a01);

            s = 0;
            t = 0;

            //Check if either or both segments degenerate into points.
            if (d00 < FMath.EPS && d11 < FMath.EPS)
                return;

            if (d00 < FMath.EPS)
            {
                //First segment degenerates into a point.
                s = 0;
                t = FMath.Clamp01(d1 / d11);
            }
            else
            {
                REAL c = VECTOR2.Dot(ab0, a01);

                if (d11 < FMath.EPS)
                {
                    //Second segment degenerates into a point.
                    s = FMath.Clamp01(-c / d00);
                    t = 0;
                }
                else
                {
                    //The generate non degenerate case starts here.
                    REAL d2 = VECTOR2.Dot(ab0, ab1);
                    REAL denom = d00 * d11 - d2 * d2;

                    //if segments not parallel compute closest point and clamp to segment.
                    if (!FMath.IsZero(denom))
                        s = FMath.Clamp01((d2 * d1 - c * d11) / denom);
                    else
                        s = 0;

                    t = (d2 * s + d1) / d11;

                    if (t < 0.0f)
                    {
                        t = 0.0f;
                        s = FMath.Clamp01(-c / d00);
                    }
                    else if (t > 1.0f)
                    {
                        t = 1.0f;
                        s = FMath.Clamp01((d2 - c) / d00);
                    }
                }
            }
        }

        private static REAL SignedTriArea(VECTOR2 a, VECTOR2 b, VECTOR2 c)
        {
            return (a.x - c.x) * (b.y - c.y) - (a.y - c.y) * (b.x - c.x);
        }

    }
}

