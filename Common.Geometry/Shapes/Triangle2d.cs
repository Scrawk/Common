using System;
using System.Runtime.InteropServices;

using Common.Core.Numerics;

using REAL = System.Double;
using VECTOR2 = Common.Core.Numerics.Vector2d;
using VECTOR3 = Common.Core.Numerics.Vector3d;
using BOX2 = Common.Geometry.Shapes.Box2d;
using MATRIX2 = Common.Core.Numerics.Matrix2x2d;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Triangle2d : IEquatable<Triangle2d>, IShape2d
    {

        public VECTOR2 A;

        public VECTOR2 B;

        public VECTOR2 C;

        public Triangle2d(VECTOR2 a, VECTOR2 b, VECTOR2 c)
        {
            A = a;
            B = b;
            C = c;
        }

        public Triangle2d(REAL ax, REAL ay, REAL bx, REAL by, REAL cx, REAL cy)
        {
            A = new VECTOR2(ax, ay);
            B = new VECTOR2(bx, by);
            C = new VECTOR2(cx, cy);
        }

        public VECTOR2 Center
        {
            get { return (A + B + C) / 3.0; }
        }

        public bool IsCCW
        {
            get { return SignedArea > 0; }
        }

        public REAL Area
        {
            get { return Math.Abs(SignedArea); }
        }

        public REAL SignedArea
        {
            get { return ((A.x - C.x) * (B.y - C.y) - (A.y - C.y) * (B.x - C.x)) * 0.5; }
        }

        /// <summary>
        /// The side lengths are given as
        /// a = sqrt((cx - bx)^2 + (cy - by)^2) -- side BC opposite of A
        /// b = sqrt((cx - ax)^2 + (cy - ay)^2) -- side CA opposite of B
        /// c = sqrt((ax - bx)^2 + (ay - by)^2) -- side AB opposite of C
        /// </summary>
        public VECTOR3 SideLengths
        {
            get
            {
                var a = MathUtil.Sqrt(MathUtil.Sqr(C.x - B.x) + MathUtil.Sqr(C.y - B.y));
                var b = MathUtil.Sqrt(MathUtil.Sqr(C.x - A.x) + MathUtil.Sqr(C.y - A.y));
                var c = MathUtil.Sqrt(MathUtil.Sqr(A.x - B.x) + MathUtil.Sqr(A.y - B.y));
                return new VECTOR3(a, b, c);
            }
        }

        /// <summary>
        /// The side lengths are given as
        /// ang_a = acos((b^2 + c^2 - a^2)  / (2 * b * c)) -- angle at A
        /// ang_b = acos((c^2 + a^2 - b^2)  / (2 * c * a)) -- angle at B
        /// ang_c = acos((a^2 + b^2 - c^2)  / (2 * a * b)) -- angle at C
        /// </summary>
        public VECTOR3 Angles
        {
            get
            {
                var len = SideLengths;
                var a2 = len.a * len.a;
                var b2 = len.b * len.b;
                var c2 = len.c * len.c;
                var a = MathUtil.Acos((b2 + c2 - a2) * (2 * len.b * len.c));
                var b = MathUtil.Acos((c2 + a2 - b2) * (2 * len.c * len.a));
                var c = MathUtil.Acos((a2 + b2 - c2) * (2 * len.a * len.b));
                return new VECTOR3(a, b, c);
            }
        }

        /// <summary>
        /// The semiperimeter is given as
        /// s = (a + b + c) / 2
        /// </summary>
        public REAL Semiperimeter
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
        public REAL Inradius
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
        public REAL Circumradius
        {
            get
            {
                return SideLengths.Product / (4 * Area);
            }
        }

        /// <summary>
        /// The altitudes are given as
        ///   alt_a = 2 * D / a -- altitude above side a
        ///   alt_b = 2 * D / b -- altitude above side b
        ///   alt_c = 2 * D / c -- altitude above side c
        /// </summary>
        public VECTOR3 Altitudes
        {
            get
            {
                var a = 2 * Area / SideLengths.a;
                var b = 2 * Area / SideLengths.b;
                var c = 2 * Area / SideLengths.c;
                return new VECTOR3(a, b, c);
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
        public REAL AspectRatio
        {
            get
            {
                return Circumradius / (2 * Inradius);
            }
        }

        public BOX2 Bounds
        {
            get
            {
                var xmin = MathUtil.Min(A.x, B.x, C.x);
                var xmax = MathUtil.Max(A.x, B.x, C.x);
                var ymin = MathUtil.Min(A.y, B.y, C.y);
                var ymax = MathUtil.Max(A.y, B.y, C.y);

                return new BOX2(xmin, xmax, ymin, ymax);
            }
        }

        unsafe public VECTOR2 this[int i]
        {
            get
            {
                if ((uint)i >= 3)
                    throw new IndexOutOfRangeException("Triangle2d index out of range.");

                fixed (Triangle2d* array = &this) { return ((VECTOR2*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 3)
                    throw new IndexOutOfRangeException("Triangle2d index out of range.");

                fixed (VECTOR2* array = &A) { array[i] = value; }
            }
        }

        public static Triangle2d operator +(Triangle2d tri, REAL s)
        {
            return new Triangle2d(tri.A + s, tri.B + s, tri.C + s);
        }

        public static Triangle2d operator +(Triangle2d tri, VECTOR2 v)
        {
            return new Triangle2d(tri.A + v, tri.B + v, tri.C + v);
        }

        public static Triangle2d operator -(Triangle2d tri, REAL s)
        {
            return new Triangle2d(tri.A - s, tri.B - s, tri.C - s);
        }

        public static Triangle2d operator -(Triangle2d tri, VECTOR2 v)
        {
            return new Triangle2d(tri.A - v, tri.B - v, tri.C - v);
        }

        public static Triangle2d operator *(Triangle2d tri, REAL s)
        {
            return new Triangle2d(tri.A * s, tri.B * s, tri.C * s);
        }

        public static Triangle2d operator *(Triangle2d tri, VECTOR2 v)
        {
            return new Triangle2d(tri.A * v, tri.B * v, tri.C * v);
        }

        public static Triangle2d operator /(Triangle2d tri, REAL s)
        {
            return new Triangle2d(tri.A / s, tri.B / s, tri.C / s);
        }

        public static Triangle2d operator /(Triangle2d tri, VECTOR2 v)
        {
            return new Triangle2d(tri.A / v, tri.B / v, tri.C / v);
        }

        public static Triangle2d operator *(Triangle2d tri, MATRIX2 m)
        {
            return new Triangle2d(m * tri.A, m * tri.B, m * tri.C);
        }

        public static implicit operator Triangle2d(Triangle2f tri)
        {
            return new Triangle2d(tri.A, tri.B, tri.C);
        }

        public static bool operator ==(Triangle2d t1, Triangle2d t2)
        {
            return t1.A == t2.A && t1.B == t2.B && t1.C == t2.C;
        }

        public static bool operator !=(Triangle2d t1, Triangle2d t2)
        {
            return t1.A != t2.A || t1.B != t2.B || t1.C != t2.C;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Triangle2d)) return false;
            Triangle2d tri = (Triangle2d)obj;
            return this == tri;
        }

        public bool Equals(Triangle2d tri)
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
            return string.Format("[Triangle2d: A={0}, B={1}, C={2}]", A, B, C);
        }

        /// <summary>
        /// The cross product area which is the 
        /// same as double the signed area.
        /// </summary>
        public static REAL CrossProductArea(VECTOR2 a, VECTOR2 b, VECTOR2 c)
        {
            return (a.x - c.x) * (b.y - c.y) - (a.y - c.y) * (b.x - c.x);
        }

        /// <summary>
        /// Return th barycentric coordinates
        /// with respect to p.
        /// </summary>
        public VECTOR3 Barycentric(VECTOR2 p)
        {
            VECTOR2 v0 = B - A, v1 = C - A, v2 = p - A;
            REAL d00 = VECTOR2.Dot(v0, v0);
            REAL d01 = VECTOR2.Dot(v0, v1);
            REAL d11 = VECTOR2.Dot(v1, v1);
            REAL d20 = VECTOR2.Dot(v2, v0);
            REAL d21 = VECTOR2.Dot(v2, v1);
            REAL denom = d00 * d11 - d01 * d01;
            REAL v = (d11 * d20 - d01 * d21) / denom;
            REAL w = (d00 * d21 - d01 * d20) / denom;
            REAL u = 1.0 - v - w;
            return new VECTOR3(u, v, w);
        }

        /// <summary>
        /// Find the closest point to the triangle.
        /// If point inside triangle return point.
        /// </summary>
        public VECTOR2 Closest(VECTOR2 p)
        {
            VECTOR2 ab = B - A;
            VECTOR2 ac = C - A;
            VECTOR2 ap = p - A;

            // Check if P in vertex region outside A
            REAL d1 = VECTOR2.Dot(ab, ap);
            REAL d2 = VECTOR2.Dot(ac, ap);
            if (d1 <= 0.0 && d2 <= 0.0)
            {
                // barycentric coordinates (1,0,0)
                return A;
            }

            REAL v, w;

            // Check if P in vertex region outside B
            VECTOR2 bp = p - B;
            REAL d3 = VECTOR2.Dot(ab, bp);
            REAL d4 = VECTOR2.Dot(ac, bp);
            if (d3 >= 0.0 && d4 <= d3)
            {
                // barycentric coordinates (0,1,0)
                return B;
            }

            // Check if P in edge region of AB, if so return projection of P onto AB
            REAL vc = d1 * d4 - d3 * d2;
            if (vc <= 0.0f && d1 >= 0.0f && d3 <= 0.0f)
            {
                v = d1 / (d1 - d3);
                // barycentric coordinates (1-v,v,0)
                return A + v * ab;
            }

            // Check if P in vertex region outside C
            VECTOR2 cp = p - C;
            REAL d5 = VECTOR2.Dot(ab, cp);
            REAL d6 = VECTOR2.Dot(ac, cp);
            if (d6 >= 0.0 && d5 <= d6)
            {
                // barycentric coordinates (0,0,1)
                return C;
            }

            // Check if P in edge region of AC, if so return projection of P onto AC
            REAL vb = d5 * d2 - d1 * d6;
            if (vb <= 0.0 && d2 >= 0.0 && d6 <= 0.0)
            {
                w = d2 / (d2 - d6);
                // barycentric coordinates (1-w,0,w)
                return A + w * ac;
            }

            // Check if P in edge region of BC, if so return projection of P onto BC
            REAL va = d3 * d6 - d5 * d4;
            if (va <= 0.0 && (d4 - d3) >= 0.0 && (d5 - d6) >= 0.0)
            {
                w = (d4 - d3) / ((d4 - d3) + (d5 - d6));
                // barycentric coordinates (0,1-w,w)
                return B + w * (C - B);
            }

            // P inside face region. Compute Q through its barycentric coordinates (u,v,w)
            REAL denom = 1.0 / (va + vb + vc);
            v = vb * denom;
            w = vc * denom;

            // = u*a + v*b + w*c, u = va * denom = 1.0f - v - w
            return A + ab * v + ac * w;
        }

        /// <summary>
        /// Returns the signed distance to surface of triangle.
        /// </summary>
        /// <returns>positive if outside triangle, negative if inside and 0 on boundary</returns>
        public REAL SignedDistance(VECTOR2 p)
        {
            VECTOR2 center = Center;
            p = p - center;
            VECTOR2 a = A - center;
            VECTOR2 b = B - center;
            VECTOR2 c = C - center;

            VECTOR2 e0 = b - a, e1 = c - b, e2 = a - c;
            VECTOR2 v0 = p - a, v1 = p - b, v2 = p - c;

            VECTOR2 pq0 = v0 - e0 * MathUtil.Clamp01(VECTOR2.Dot(v0, e0) / VECTOR2.Dot(e0, e0));
            VECTOR2 pq1 = v1 - e1 * MathUtil.Clamp01(VECTOR2.Dot(v1, e1) / VECTOR2.Dot(e1, e1));
            VECTOR2 pq2 = v2 - e2 * MathUtil.Clamp01(VECTOR2.Dot(v2, e2) / VECTOR2.Dot(e2, e2));

            REAL s = Math.Sign(e0.x * e2.y - e0.y * e2.x);

            VECTOR2 d0 = new VECTOR2(VECTOR2.Dot(pq0, pq0), s * (v0.x * e0.y - v0.y * e0.x));
            VECTOR2 d1 = new VECTOR2(VECTOR2.Dot(pq1, pq1), s * (v1.x * e1.y - v1.y * e1.x));
            VECTOR2 d2 = new VECTOR2(VECTOR2.Dot(pq2, pq2), s * (v2.x * e2.y - v2.y * e2.x));

            VECTOR2 d = new VECTOR2();
            d.x = MathUtil.Min(d0.x, d1.x, d2.x);
            d.y = MathUtil.Min(d0.y, d1.y, d2.y);

            return -MathUtil.Sqrt(d.x) * Math.Sign(d.y);
        }

        /// <summary>
        /// Does triangle contain point.
        /// </summary>
        /// <param name="p">point</param>
        /// <returns>true if triangle contains point</returns>
        public bool Contains(VECTOR2 p)
        {
            REAL pab = VECTOR2.Cross(p - A, B - A);
            REAL pbc = VECTOR2.Cross(p - B, C - B);

            if (Math.Sign(pab) != Math.Sign(pbc)) return false;

            REAL pca = VECTOR2.Cross(p - C, A - C);

            if (Math.Sign(pab) != Math.Sign(pca)) return false;

            return true;
        }

        /// <summary>
        /// Does triangle contain point.
        /// Asumes triangle is CCW;
        /// </summary>
        /// <param name="p">point</param>
        /// <returns>true if triangle contains point</returns>
        public bool ContainsCCW(VECTOR2 p)
        {
            if (VECTOR2.Cross(p - A, B - A) > 0.0) return false;
            if (VECTOR2.Cross(p - B, C - B) > 0.0) return false;
            if (VECTOR2.Cross(p - C, A - C) > 0.0) return false;

            return true;
        }

        /// <summary>
        /// Does the triangle intersect this box.
        /// </summary>
        public bool Intersects(Box2d box)
        {
            throw new NotImplementedException();
        }

    }
}