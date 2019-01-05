using System;
using System.Runtime.InteropServices;

using Common.Core.LinearAlgebra;
using Common.Core.Mathematics;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Segment2d : IEquatable<Segment2d>
    {

        public Vector2d A;

        public Vector2d B;

        public Segment2d(Vector2d a, Vector2d b)
        {
            A = a;
            B = b;
        }

        public Segment2d(double ax, double ay, double bx, double by)
        {
            A = new Vector2d(ax, ay);
            B = new Vector2d(bx, by);
        }

        public Vector2d Center
        {
            get { return (A + B) / 2.0; }
        }

        public double Length
        {
            get { return Vector2d.Distance(A, B); }
        }

        public double SqrLength
        {
            get { return Vector2d.SqrDistance(A, B); }
        }

        public Vector2d Normal
        {
            get
            {
                return (B - A).Normalized.PerpendicularCW;
            }
        }

        public Box2d Bounds
        {
            get
            {
                double xmin = Math.Min(A.x, B.x);
                double xmax = Math.Max(A.x, B.x);
                double ymin = Math.Min(A.y, B.y);
                double ymax = Math.Max(A.y, B.y);

                return new Box2d(xmin, xmax, ymin, ymax);
            }
        }

        public static bool operator ==(Segment2d s1, Segment2d s2)
        {
            return s1.A == s2.A && s1.B == s2.B;
        }

        public static bool operator !=(Segment2d s1, Segment2d s2)
        {
            return s1.A != s2.A || s1.B != s2.B;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Segment2d)) return false;
            Segment2d seg = (Segment2d)obj;
            return this == seg;
        }

        public bool Equals(Segment2d seg)
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
            return string.Format("[Segment2d: A={0}, B={1}]", A, B);
        }


        /// <summary>
        /// Does the two segments intersect.
        /// </summary>
        /// <param name="seg">other segment</param>
        /// <param name="t">Intersection point = A + t * (B - A)</param>
        /// <returns>If they intersect</returns>
        public bool Intersects(Segment2d seg, out double t)
        {
            double area1 = SignedTriArea(A, B, seg.B);
            double area2 = SignedTriArea(A, B, seg.A);
            t = 0.0f;

            if (area1 * area2 < 0.0)
            {
                double area3 = SignedTriArea(seg.A, seg.B, A);
                double area4 = area3 + area2 - area1;

                if (area3 * area4 < 0.0)
                {
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
        /// <param name="t">closest point = A + t * (B - A)</param>
        public void Closest(Vector2d p, out double t)
        {
            t = 0.0f;
            Vector2d ab = B - A;
            Vector2d ap = p - A;

            double len = ab.x * ab.x + ab.y * ab.y;
            if (len < DMath.EPS) return;

            t = (ab.x * ap.x + ab.y * ap.y) / len;

            if (t < 0.0f) t = 0.0f;
            if (t > 1.0f) t = 1.0f;
        }

        /// <summary>
        /// The closest segment spanning two other segments.
        /// </summary>
        /// <param name="seg">the other segment</param>
        /// <param name="s">closest point = A + s * (B - A)</param>
        /// <param name="t">other closest point = seg.A + t * (seg.B - seg.A)</param>
        public void Closest(Segment2d seg, out double s, out double t)
        {

            Vector2d ab0 = B - A;
            Vector2d ab1 = seg.B - seg.A;
            Vector2d a01 = A - seg.A;

            double d00 = Vector2d.Dot(ab0, ab0);
            double d11 = Vector2d.Dot(ab1, ab1);
            double d1 = Vector2d.Dot(ab1, a01);

            s = 0;
            t = 0;

            //Check if either or both segments degenerate into points.
            if (d00 < DMath.EPS && d11 < DMath.EPS)
                return;

            if (d00 < DMath.EPS)
            {
                //First segment degenerates into a point.
                s = 0;
                t = DMath.Clamp01(d1 / d11);
            }
            else
            {
                double c = Vector2d.Dot(ab0, a01);

                if (d11 < DMath.EPS)
                {
                    //Second segment degenerates into a point.
                    s = DMath.Clamp01(-c / d00);
                    t = 0;
                }
                else
                {
                    //The generate non degenerate case starts here.
                    double d2 = Vector2d.Dot(ab0, ab1);
                    double denom = d00 * d11 - d2 * d2;

                    //if segments not parallel compute closest point and clamp to segment.
                    if (!DMath.IsZero(denom))
                        s = DMath.Clamp01((d2 * d1 - c * d11) / denom);
                    else
                        s = 0;

                    t = (d2 * s + d1) / d11;

                    if (t < 0.0f)
                    {
                        t = 0.0f;
                        s = DMath.Clamp01(-c / d00);
                    }
                    else if (t > 1.0f)
                    {
                        t = 1.0f;
                        s = DMath.Clamp01((d2 - c) / d00);
                    }
                }
            }
        }

        private static double SignedTriArea(Vector2d a, Vector2d b, Vector2d c)
        {
            return (a.x - c.x) * (b.y - c.y) - (a.y - c.y) * (b.x - c.x);
        }

    }
}

