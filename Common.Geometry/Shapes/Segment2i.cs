using System;
using System.Runtime.InteropServices;

using Common.Core.LinearAlgebra;
using Common.Core.Mathematics;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Segment2i : IEquatable<Segment2i>
    {

        public Vector2i A;

        public Vector2i B;

        public Segment2i(Vector2i a, Vector2i b)
        {
            A = a;
            B = b;
        }

        public Segment2i(int ax, int ay, int bx, int by)
        {
            A = new Vector2i(ax, ay);
            B = new Vector2i(bx, by);
        }

        public Vector2d Center
        {
            get { return (Vector2d)(A + B) * 0.5; }
        }

        public double Length
        {
            get { return Vector2i.Distance(A, B); }
        }

        public double SqrLength
        {
            get { return Vector2i.SqrDistance(A, B); }
        }

        public Vector2d Normal
        {
            get
            {
                return ((Vector2d)(B - A)).Normalized.PerpendicularCW;
            }
        }

        public Box2i Bounds
        {
            get
            {
                int xmin = Math.Min(A.x, B.x);
                int xmax = Math.Max(A.x, B.x);
                int ymin = Math.Min(A.y, B.y);
                int ymax = Math.Max(A.y, B.y);

                return new Box2i(xmin, xmax, ymin, ymax);
            }
        }

        public static bool operator ==(Segment2i s1, Segment2i s2)
        {
            return s1.A == s2.A && s1.B == s2.B;
        }

        public static bool operator !=(Segment2i s1, Segment2i s2)
        {
            return s1.A != s2.A || s1.B != s2.B;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Segment2i)) return false;
            Segment2i seg = (Segment2i)obj;
            return this == seg;
        }

        public bool Equals(Segment2i seg)
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
            return string.Format("[Segment2i: A={0}, B={1}]", A, B);
        }

        /// <summary>
        /// Does the two segments intersect.
        /// </summary>
        /// <param name="seg">other segment</param>
        public bool Intersects(Segment2i seg)
        {
            return Intersects(seg, out double t);
        }

        /// <summary>
        /// Does the two segments intersect.
        /// </summary>
        /// <param name="seg">other segment</param>
        /// <param name="t">Intersection point = A + t * (B - A)</param>
        /// <returns>If they intersect</returns>
        public bool Intersects(Segment2i seg, out double t)
        {
            int area1 = SignedTriArea(A, B, seg.B);
            int area2 = SignedTriArea(A, B, seg.A);
            t = 0;

            if (area1 * area2 < 0)
            {
                int area3 = SignedTriArea(seg.A, seg.B, A);
                int area4 = area3 + area2 - area1;

                if (area3 * area4 < 0)
                {
                    t = area3 / (double)(area3 - area4);
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
        public bool Intersects(Segment2i seg, out double s, out double t)
        {

            int area1 = SignedTriArea(A, B, seg.B);
            int area2 = SignedTriArea(A, B, seg.A);
            s = 0;
            t = 0;

            if (area1 * area2 < 0)
            {
                int area3 = SignedTriArea(seg.A, seg.B, A);
                int area4 = area3 + area2 - area1;

                if (area3 * area4 < 0)
                {
                    s = area3 / (area3 - area4);

                    int a2 = area2;
                    int a3 = area3;

                    area1 = SignedTriArea(seg.A, seg.B, B);
                    area2 = a3;
                    area3 = a2;
                    area4 = area3 + area2 - area1;
                    t = area3 / (double)(area3 - area4);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// The closest point on segment to point.
        /// </summary>
        /// <param name="p">point</param>
        public Vector2d Closest(Vector2i p)
        {
            double t;
            Closest(p, out t);
            return A + (Vector2d)(B - A) * t;
        }

        /// <summary>
        /// The closest point on segment to point.
        /// </summary>
        /// <param name="p">point</param>
        /// <param name="t">closest point = A + t * (B - A)</param>
        public void Closest(Vector2i p, out double t)
        {
            t = 0.0f;
            Vector2i ab = B - A;
            Vector2i ap = p - A;

            double len = ab.x * ab.x + ab.y * ab.y;
            if (len <= 0) return;

            t = (ab.x * ap.x + ab.y * ap.y) / len;
            t = DMath.Clamp01(t);
        }

        /// <summary>
        /// The closest segment spanning two other segments.
        /// </summary>
        /// <param name="seg">the other segment</param>
        public Segment2d Closest(Segment2i seg)
        {
            double s, t;
            Closest(seg, out s, out t);
            return new Segment2d(A + (Vector2d)(B - A) * s, seg.A + (Vector2d)(seg.B - seg.A) * t);
        }

        /// <summary>
        /// The closest segment spanning two other segments.
        /// </summary>
        /// <param name="seg">the other segment</param>
        /// <param name="s">closest point = A + s * (B - A)</param>
        /// <param name="t">other closest point = seg.A + t * (seg.B - seg.A)</param>
        public void Closest(Segment2i seg, out double s, out double t)
        {

            Vector2i ab0 = B - A;
            Vector2i ab1 = seg.B - seg.A;
            Vector2i a01 = A - seg.A;

            int d00 = Vector2i.Dot(ab0, ab0);
            int d11 = Vector2i.Dot(ab1, ab1);
            int d1 = Vector2i.Dot(ab1, a01);

            s = 0;
            t = 0;

            //Check if either or both segments degenerate into points.
            if (d00 <= 0 && d11 <= 0)
                return;

            if (d00 <= 0)
            {
                //First segment degenerates into a point.
                s = 0;
                t = DMath.Clamp01(d1 / (double)d11);
            }
            else
            {
                int c = Vector2i.Dot(ab0, a01);

                if (d11 <= 0)
                {
                    //Second segment degenerates into a point.
                    s = DMath.Clamp01(-c / (double)d00);
                    t = 0.0;
                }
                else
                {
                    //The generate non degenerate case starts here.
                    int d2 = Vector2i.Dot(ab0, ab1);
                    int denom = d00 * d11 - d2 * d2;

                    //if segments not parallel compute closest point and clamp to segment.
                    if (denom != 0)
                        s = DMath.Clamp01((d2 * d1 - c * d11) / (double)denom);
                    else
                        s = 0.0;

                    t = (d2 * s + d1) / (double)d11;

                    if (t < 0)
                    {
                        t = 0.0;
                        s = DMath.Clamp01(-c / (double)d00);
                    }
                    else if (t > 1)
                    {
                        t = 1.0;
                        s = DMath.Clamp01((d2 - c) / (double)d00);
                    }
                }
            }
        }

        private static int SignedTriArea(Vector2i a, Vector2i b, Vector2i c)
        {
            return (a.x - c.x) * (b.y - c.y) - (a.y - c.y) * (b.x - c.x);
        }

    }
}

