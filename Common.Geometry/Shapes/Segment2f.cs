using System;
using System.Runtime.InteropServices;

using Common.Core.LinearAlgebra;
using Common.Core.Mathematics;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Segment2f : IEquatable<Segment2f>
    {

        public Vector2f A;

        public Vector2f B;

        public Segment2f(Vector2f a, Vector2f b)
        {
            A = a;
            B = b;
        }

        public Segment2f(float ax, float ay, float bx, float by)
        {
            A = new Vector2f(ax, ay);
            B = new Vector2f(bx, by);
        }

        public Vector2f Center
        {
            get { return (A + B) / 2.0f; }
        }

        public float Length
        {
            get { return Vector2f.Distance(A, B); }
        }

        public float SqrLength
        {
            get { return Vector2f.SqrDistance(A, B); }
        }

        public Vector2f Normal
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
                float xmin = Math.Min(A.x, B.x);
                float xmax = Math.Max(A.x, B.x);
                float ymin = Math.Min(A.y, B.y);
                float ymax = Math.Max(A.y, B.y);

                return new Box2f(xmin, xmax, ymin, ymax);
            }
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
        /// <param name="t">Intersection point = A + t * (B - A)</param>
        /// <returns>If they intersect</returns>
        public bool Intersects(Segment2f seg, out float t)
        {
            float area1 = SignedTriArea(A, B, seg.B);
            float area2 = SignedTriArea(A, B, seg.A);
            t = 0.0f;

            if (area1 * area2 < 0.0)
            {
                float area3 = SignedTriArea(seg.A, seg.B, A);
                float area4 = area3 + area2 - area1;

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
        public void Closest(Vector2f p, out float t)
        {
            t = 0.0f;
            Vector2f ab = B - A;
            Vector2f ap = p - A;

            float len = ab.x * ab.x + ab.y * ab.y;
            if (len < FMath.EPS) return;

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
        public void Closest(Segment2f seg, out float s, out float t)
        {

            Vector2f ab0 = B - A;
            Vector2f ab1 = seg.B - seg.A;
            Vector2f a01 = A - seg.A;

            float d00 = Vector2f.Dot(ab0, ab0);
            float d11 = Vector2f.Dot(ab1, ab1);
            float d1 = Vector2f.Dot(ab1, a01);

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
                float c = Vector2f.Dot(ab0, a01);

                if (d11 < FMath.EPS)
                {
                    //Second segment degenerates into a point.
                    s = FMath.Clamp01(-c / d00);
                    t = 0;
                }
                else
                {
                    //The generate non degenerate case starts here.
                    float d2 = Vector2f.Dot(ab0, ab1);
                    float denom = d00 * d11 - d2 * d2;

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

        private static float SignedTriArea(Vector2f a, Vector2f b, Vector2f c)
        {
            return (a.x - c.x) * (b.y - c.y) - (a.y - c.y) * (b.x - c.x);
        }

    }
}

