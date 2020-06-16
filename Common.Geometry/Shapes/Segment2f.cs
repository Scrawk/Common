﻿using System;
using System.Runtime.InteropServices;

using Common.Core.Numerics;

using REAL = System.Single;
using VECTOR2 = Common.Core.Numerics.Vector2f;
using BOX2 = Common.Geometry.Shapes.Box2f;
using MATRIX2 = Common.Core.Numerics.Matrix2x2f;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Segment2f : IEquatable<Segment2f>, IShape2f
    {

        public VECTOR2 A;

        public VECTOR2 B;

        /// <summary>
        /// Alternative names for a and b to make
        /// it more clear when comparing two segments 
        /// named ab and cd.
        /// </summary>
        public VECTOR2 C => A;
        public VECTOR2 D => B;

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

        public VECTOR2 Center => (A + B) * 0.5f;

        public REAL Length => VECTOR2.Distance(A, B);

        public REAL SqrLength => VECTOR2.SqrDistance(A, B);

        public VECTOR2 Normal => (B - A).Normalized.PerpendicularCW;

        public REAL LeftMost => Math.Min(A.x, B.x);

        public REAL RightMost => Math.Max(A.x, B.x);

        public REAL BottomMost => Math.Min(A.y, B.y);

        public REAL TopMost => Math.Max(A.y, B.y);

        public Segment2f Flipped => new Segment2f(B, A);

        public BOX2 Bounds
        {
            get
            {
                REAL xmin = Math.Min(A.x, B.x);
                REAL xmax = Math.Max(A.x, B.x);
                REAL ymin = Math.Min(A.y, B.y);
                REAL ymax = Math.Max(A.y, B.y);

                return new BOX2(xmin, xmax, ymin, ymax);
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
        /// Does the point line on the segemnts.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool Contains(VECTOR2 p)
        {
            var c = Closest(p);
            return VECTOR2.AlmostEqual(c, p);
        }

        /// <summary>
        /// Does the point line on the segemnts.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool Contains(VECTOR2 p, REAL eps)
        {
            var c = Closest(p);
            return VECTOR2.AlmostEqual(c, p, eps);
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
            REAL area1 = Triangle2f.CrossProductArea(A, B, seg.B);
            REAL area2 = Triangle2f.CrossProductArea(A, B, seg.A);
            t = 0.0f;

            if (area1 * area2 < 0.0f)
            {
                REAL area3 = Triangle2f.CrossProductArea(seg.A, seg.B, A);
                REAL area4 = area3 + area2 - area1;

                if (area3 * area4 < 0.0f)
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

            REAL area1 = Triangle2f.CrossProductArea(A, B, seg.B);
            REAL area2 = Triangle2f.CrossProductArea(A, B, seg.A);
            s = 0.0f;
            t = 0.0f;

            if (area1 * area2 < 0.0f)
            {
                REAL area3 = Triangle2f.CrossProductArea(seg.A, seg.B, A);
                REAL area4 = area3 + area2 - area1;

                if (area3 * area4 < 0.0f)
                {
                    s = area3 / (area3 - area4);

                    REAL a2 = area2;
                    REAL a3 = area3;

                    area1 = Triangle2f.CrossProductArea(seg.A, seg.B, B);
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
        /// Does the segment intersect this box.
        /// </summary>
        public bool Intersects(BOX2 box)
        {
            VECTOR2 c = box.Center;
            VECTOR2 e = box.Max - c; //Box half length extents
            VECTOR2 m = Center;
            VECTOR2 d = B - m; //Segment halflength vector.
            m = m - c; //translate box and segment to origin.

            //try world coordinate axes as seperating axes.
            REAL adx = Math.Abs(d.x);
            if (Math.Abs(m.x) > e.x + adx) return false;
            REAL ady = Math.Abs(d.y);
            if (Math.Abs(m.y) > e.y + ady) return false;

            //add in an epsilon term to counteract arithmetic errors 
            //when segment is near parallel to a coordinate axis.
            adx += MathUtil.EPS;
            ady += MathUtil.EPS;

            if (Math.Abs(m.x * d.y - m.y * d.x) > e.x * ady + e.y * adx) return false;

            return true;
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
            if (MathUtil.IsZero(len)) return;

            t = (ab.x * ap.x + ab.y * ap.y) / len;
            t = MathUtil.Clamp01(t);
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
            if (MathUtil.IsZero(d00) && MathUtil.IsZero(d11))
                return;

            if (MathUtil.IsZero(d00))
            {
                //First segment degenerates into a point.
                s = 0;
                t = MathUtil.Clamp01(d1 / d11);
            }
            else
            {
                REAL c = VECTOR2.Dot(ab0, a01);

                if (MathUtil.IsZero(d11))
                {
                    //Second segment degenerates into a point.
                    s = MathUtil.Clamp01(-c / d00);
                    t = 0;
                }
                else
                {
                    //The generate non degenerate case starts here.
                    REAL d2 = VECTOR2.Dot(ab0, ab1);
                    REAL denom = d00 * d11 - d2 * d2;

                    //if segments not parallel compute closest point and clamp to segment.
                    if (!MathUtil.IsZero(denom))
                        s = MathUtil.Clamp01((d2 * d1 - c * d11) / denom);
                    else
                        s = 0;

                    t = (d2 * s + d1) / d11;

                    if (t < 0.0f)
                    {
                        t = 0.0f;
                        s = MathUtil.Clamp01(-c / d00);
                    }
                    else if (t > 1.0f)
                    {
                        t = 1.0f;
                        s = MathUtil.Clamp01((d2 - c) / d00);
                    }
                }
            }
        }

    }
}

