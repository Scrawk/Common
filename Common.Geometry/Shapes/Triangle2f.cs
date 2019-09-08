using System;
using System.Runtime.InteropServices;

using Common.Core.Numerics;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Triangle2f : IEquatable<Triangle2f>
    {

        public Vector2f A;

        public Vector2f B;

        public Vector2f C;

        public Triangle2f(Vector2f a, Vector2f b, Vector2f c)
        {
            A = a;
            B = b;
            C = c;
        }

        public Triangle2f(float ax, float ay, float bx, float by, float cx, float cy)
        {
            A = new Vector2f(ax, ay);
            B = new Vector2f(bx, by);
            C = new Vector2f(cx, cy);
        }

        public Vector2f Center
        {
            get { return (A + B + C) / 3.0f; }
        }

        public bool IsCCW
        {
            get { return SignedArea > 0; }
        }

        public float Area
        {
            get { return Math.Abs(SignedArea); }
        }

        public float SignedArea
        {
            get { return (A.x - C.x) * (B.y - C.y) - (A.y - C.y) * (B.x - C.x); }
        }

        /// <summary>
        /// The side lengths are given as
        /// a = sqrt((cx - bx)^2 + (cy - by)^2) -- side BC opposite of A
        /// b = sqrt((cx - ax)^2 + (cy - ay)^2) -- side CA opposite of B
        /// c = sqrt((ax - bx)^2 + (ay - by)^2) -- side AB opposite of C
        /// </summary>
        public Vector3f SideLengths
        {
            get
            {
                var a = FMath.Sqrt(FMath.Sqr(C.x - B.x) + FMath.Sqr(C.y - B.y));
                var b = FMath.Sqrt(FMath.Sqr(C.x - A.x) + FMath.Sqr(C.y - A.y));
                var c = FMath.Sqrt(FMath.Sqr(A.x - B.x) + FMath.Sqr(A.y - B.y));
                return new Vector3f(a, b, c);
            }
        }

        /// <summary>
        /// The side lengths are given as
        /// ang_a = acos((b^2 + c^2 - a^2)  / (2 * b * c)) -- angle at A
        /// ang_b = acos((c^2 + a^2 - b^2)  / (2 * c * a)) -- angle at B
        /// ang_c = acos((a^2 + b^2 - c^2)  / (2 * a * b)) -- angle at C
        /// </summary>
        public Vector3f Angles
        {
            get
            {
                var len = SideLengths;
                var a2 = len.a * len.a;
                var b2 = len.b * len.b;
                var c2 = len.c * len.c;
                var a = FMath.Acos((b2 + c2 - a2) * (2 * len.b * len.c));
                var b = FMath.Acos((c2 + a2 - b2) * (2 * len.c * len.a));
                var c = FMath.Acos((a2 + b2 - c2) * (2 * len.a * len.b));
                return new Vector3f(a, b, c);
            }
        }

        /// <summary>
        /// The semiperimeter is given as
        /// s = (a + b + c) / 2
        /// </summary>
        public float Semiperimeter
        {
            get
            {
                return SideLengths.Sum / 2;
            }
        }

        /// <summary>
        /// The inradius is given as
        ///   r = D / s
        /// </summary>
        public float Inradius
        {
            get
            {
                return Area / Semiperimeter;
            }
        }

        /// <summary>
        /// The circumradius is given as
        ///   R = a * b * c / (4 * D)
        /// </summary>
        public float Circumradius
        {
            get
            {
                return SideLengths.Mul / (4 * Area);
            }
        }

        /// <summary>
        /// The altitudes are given as
        ///   alt_a = 2 * D / a -- altitude above side a
        ///   alt_b = 2 * D / b -- altitude above side b
        ///   alt_c = 2 * D / c -- altitude above side c
        /// </summary>
        public Vector3f Altitudes
        {
            get
            {
                var a = 2 * Area / SideLengths.a;
                var b = 2 * Area / SideLengths.b;
                var c = 2 * Area / SideLengths.c;
                return new Vector3f(a, b, c);
            }
        }

        /// <summary>
        /// The aspect ratio may be given as the ratio of the longest to the
        /// shortest edge or, more commonly as the ratio of the circumradius 
        /// to twice the inradius
        ///   ar = R / (2 * r)
        ///      = a * b * c / (8 * (s - a) * (s - b) * (s - c))
        ///      = a * b * c / ((b + c - a) * (c + a - b) * (a + b - c))
        /// </summary>
        public float AspectRatio
        {
            get
            {
                return Circumradius / (2 * Inradius);
            }
        }

        public Box2f Bounds
        {
            get
            {
                var xmin = FMath.Min(A.x, B.x, C.x);
                var xmax = FMath.Max(A.x, B.x, C.x);
                var ymin = FMath.Min(A.y, B.y, C.y);
                var ymax = FMath.Max(A.y, B.y, C.y);

                return new Box2f(xmin, xmax, ymin, ymax);
            }
        }

        unsafe public Vector2f this[int i]
        {
            get
            {
                if ((uint)i >= 3)
                    throw new IndexOutOfRangeException("Triangle2f index out of range.");

                fixed (Triangle2f* array = &this) { return ((Vector2f*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 3)
                    throw new IndexOutOfRangeException("Triangle2f index out of range.");

                fixed (Vector2f* array = &A) { array[i] = value; }
            }
        }

        public static Triangle2f operator +(Triangle2f tri, float s)
        {
            return new Triangle2f(tri.A + s, tri.B + s, tri.C + s);
        }

        public static Triangle2f operator +(Triangle2f tri, Vector2f v)
        {
            return new Triangle2f(tri.A + v, tri.B + v, tri.C + v);
        }

        public static Triangle2f operator -(Triangle2f tri, float s)
        {
            return new Triangle2f(tri.A - s, tri.B - s, tri.C - s);
        }

        public static Triangle2f operator -(Triangle2f tri, Vector2f v)
        {
            return new Triangle2f(tri.A - v, tri.B - v, tri.C - v);
        }

        public static Triangle2f operator *(Triangle2f tri, float s)
        {
            return new Triangle2f(tri.A * s, tri.B * s, tri.C * s);
        }

        public static Triangle2f operator *(Triangle2f tri, Vector2f v)
        {
            return new Triangle2f(tri.A * v, tri.B * v, tri.C * v);
        }

        public static Triangle2f operator /(Triangle2f tri, float s)
        {
            return new Triangle2f(tri.A / s, tri.B / s, tri.C / s);
        }

        public static Triangle2f operator /(Triangle2f tri, Vector2f v)
        {
            return new Triangle2f(tri.A / v, tri.B / v, tri.C / v);
        }

        public static Triangle2f operator *(Triangle2f tri, Matrix2x2f m)
        {
            return new Triangle2f(m * tri.A, m * tri.B, m * tri.C);
        }

        public static bool operator ==(Triangle2f t1, Triangle2f t2)
        {
            return t1.A == t2.A && t1.B == t2.B && t1.C == t2.C;
        }

        public static bool operator !=(Triangle2f t1, Triangle2f t2)
        {
            return t1.A != t2.A || t1.B != t2.B || t1.C != t2.C;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Triangle2f)) return false;
            Triangle2f tri = (Triangle2f)obj;
            return this == tri;
        }

        public bool Equals(Triangle2f tri)
        {
            return this == tri;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ A.GetHashCode();
                hash = (hash * 16777619) ^ B.GetHashCode();
                hash = (hash * 16777619) ^ C.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return string.Format("[Triangle2f: A={0}, B={1}, C={2}]", A, B, C);
        }

        /// <summary>
        /// Return th barycentric coordinates
        /// with respect to p.
        /// </summary>
        public Vector3f Barycentric(Vector2f p)
        {
            Vector2f v0 = B - A, v1 = C - A, v2 = p - A;
            float d00 = Vector2f.Dot(v0, v0);
            float d01 = Vector2f.Dot(v0, v1);
            float d11 = Vector2f.Dot(v1, v1);
            float d20 = Vector2f.Dot(v2, v0);
            float d21 = Vector2f.Dot(v2, v1);
            float denom = d00 * d11 - d01 * d01;
            float v = (d11 * d20 - d01 * d21) / denom;
            float w = (d00 * d21 - d01 * d20) / denom;
            float u = 1.0f - v - w;
            return new Vector3f(u, v, w);
        }

        /// <summary>
        /// Find the closest point to the triangle.
        /// If point inside triangle return point.
        /// </summary>
        public Vector2f Closest(Vector2f p)
        {
            Vector2f ab = B - A;
            Vector2f ac = C - A;
            Vector2f ap = p - A;

            // Check if P in vertex region outside A
            float d1 = Vector2f.Dot(ab, ap);
            float d2 = Vector2f.Dot(ac, ap);
            if (d1 <= 0.0 && d2 <= 0.0)
            {
                // barycentric coordinates (1,0,0)
                return A;
            }

            float v, w;

            // Check if P in vertex region outside B
            Vector2f bp = p - B;
            float d3 = Vector2f.Dot(ab, bp);
            float d4 = Vector2f.Dot(ac, bp);
            if (d3 >= 0.0 && d4 <= d3)
            {
                // barycentric coordinates (0,1,0)
                return B;
            }

            // Check if P in edge region of AB, if so return projection of P onto AB
            float vc = d1 * d4 - d3 * d2;
            if (vc <= 0.0f && d1 >= 0.0f && d3 <= 0.0f)
            {
                v = d1 / (d1 - d3);
                // barycentric coordinates (1-v,v,0)
                return A + v * ab;
            }

            // Check if P in vertex region outside C
            Vector2f cp = p - C;
            float d5 = Vector2f.Dot(ab, cp);
            float d6 = Vector2f.Dot(ac, cp);
            if (d6 >= 0.0f && d5 <= d6)
            {
                // barycentric coordinates (0,0,1)
                return C;
            }

            // Check if P in edge region of AC, if so return projection of P onto AC
            float vb = d5 * d2 - d1 * d6;
            if (vb <= 0.0f && d2 >= 0.0f && d6 <= 0.0f)
            {
                w = d2 / (d2 - d6);
                // barycentric coordinates (1-w,0,w)
                return A + w * ac;
            }

            // Check if P in edge region of BC, if so return projection of P onto BC
            float va = d3 * d6 - d5 * d4;
            if (va <= 0.0f && (d4 - d3) >= 0.0f && (d5 - d6) >= 0.0f)
            {
                w = (d4 - d3) / ((d4 - d3) + (d5 - d6));
                // barycentric coordinates (0,1-w,w)
                return B + w * (C - B);
            }

            // P inside face region. Compute Q through its barycentric coordinates (u,v,w)
            float denom = 1.0f / (va + vb + vc);
            v = vb * denom;
            w = vc * denom;

            // = u*a + v*b + w*c, u = va * denom = 1.0f - v - w
            return A + ab * v + ac * w;
        }

        public float SignedDistance(Vector2f p)
        {
            Vector2f center = Center;
            p = p - center;
            Vector2f a = A - center;
            Vector2f b = B - center;
            Vector2f c = C - center;

            Vector2f e0 = b - a, e1 = c - b, e2 = a - c;
            Vector2f v0 = p - a, v1 = p - b, v2 = p - c;

            Vector2f pq0 = v0 - e0 * FMath.Clamp01(Vector2f.Dot(v0, e0) / Vector2f.Dot(e0, e0));
            Vector2f pq1 = v1 - e1 * FMath.Clamp01(Vector2f.Dot(v1, e1) / Vector2f.Dot(e1, e1));
            Vector2f pq2 = v2 - e2 * FMath.Clamp01(Vector2f.Dot(v2, e2) / Vector2f.Dot(e2, e2));

            float s = Math.Sign(e0.x * e2.y - e0.y * e2.x);

            Vector2f d0 = new Vector2f(Vector2f.Dot(pq0, pq0), s * (v0.x * e0.y - v0.y * e0.x));
            Vector2f d1 = new Vector2f(Vector2f.Dot(pq1, pq1), s * (v1.x * e1.y - v1.y * e1.x));
            Vector2f d2 = new Vector2f(Vector2f.Dot(pq2, pq2), s * (v2.x * e2.y - v2.y * e2.x));

            Vector2f d = new Vector2f();
            d.x = FMath.Min(d0.x, d1.x, d2.x);
            d.y = FMath.Min(d0.y, d1.y, d2.y);

            return -FMath.Sqrt(d.x) * Math.Sign(d.y);
        }

        /// <summary>
        /// Does triangle contain point.
        /// </summary>
        /// <param name="p">point</param>
        /// <returns>true if triangle contains point</returns>
        public bool Contains(Vector2f p)
        {
            float pab = Vector2f.Cross(p - A, B - A);
            float pbc = Vector2f.Cross(p - B, C - B);

            if (Math.Sign(pab) != Math.Sign(pbc)) return false;

            float pca = Vector2f.Cross(p - C, A - C);

            if (Math.Sign(pab) != Math.Sign(pca)) return false;

            return true;
        }

        /// <summary>
        /// Does triangle contain point.
        /// Asumes triangle is CCW;
        /// </summary>
        /// <param name="p">point</param>
        /// <returns>true if triangle contains point</returns>
        public bool ContainsCCW(Vector2f p)
        {
            if (Vector2f.Cross(p - A, B - A) > 0.0) return false;
            if (Vector2f.Cross(p - B, C - B) > 0.0) return false;
            if (Vector2f.Cross(p - C, A - C) > 0.0) return false;

            return true;
        }

    }
}