using System;
using System.Runtime.InteropServices;

using Common.Core.Numerics;

using REAL = System.Single;
using VECTOR2 = Common.Core.Numerics.Vector2f;
using VECTOR3 = Common.Core.Numerics.Vector3f;
using MATRIX2 = Common.Core.Numerics.Matrix2x2f;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Triangle2f : IEquatable<Triangle2f>
    {

        public VECTOR2 A;

        public VECTOR2 B;

        public VECTOR2 C;

        public Triangle2f(VECTOR2 a, VECTOR2 b, VECTOR2 c)
        {
            A = a;
            B = b;
            C = c;
        }

        public Triangle2f(REAL ax, REAL ay, REAL bx, REAL by, REAL cx, REAL cy)
        {
            A = new VECTOR2(ax, ay);
            B = new VECTOR2(bx, by);
            C = new VECTOR2(cx, cy);
        }

        public VECTOR2 Center
        {
            get { return (A + B + C) / 3.0f; }
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
            get { return (A.x - C.x) * (B.y - C.y) - (A.y - C.y) * (B.x - C.x); }
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
                var a = FMath.Sqrt(FMath.Sqr(C.x - B.x) + FMath.Sqr(C.y - B.y));
                var b = FMath.Sqrt(FMath.Sqr(C.x - A.x) + FMath.Sqr(C.y - A.y));
                var c = FMath.Sqrt(FMath.Sqr(A.x - B.x) + FMath.Sqr(A.y - B.y));
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
                var a = FMath.Acos((b2 + c2 - a2) * (2 * len.b * len.c));
                var b = FMath.Acos((c2 + a2 - b2) * (2 * len.c * len.a));
                var c = FMath.Acos((a2 + b2 - c2) * (2 * len.a * len.b));
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
                return SideLengths.Mul / (4 * Area);
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

        public Box2f Bounds
        {
            get
            {
                REAL xmin = Math.Min(A.x, Math.Min(B.x, C.x));
                REAL xmax = Math.Max(A.x, Math.Max(B.x, C.x));
                REAL ymin = Math.Min(A.y, Math.Min(B.y, C.y));
                REAL ymax = Math.Max(A.y, Math.Max(B.y, C.y));

                return new Box2f(xmin, xmax, ymin, ymax);
            }
        }

        unsafe public VECTOR2 this[int i]
        {
            get
            {
                if ((uint)i >= 3)
                    throw new IndexOutOfRangeException("Triangle2f index out of range.");

                fixed (Triangle2f* array = &this) { return ((VECTOR2*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 3)
                    throw new IndexOutOfRangeException("Triangle2f index out of range.");

                fixed (VECTOR2* array = &A) { array[i] = value; }
            }
        }

        public static Triangle2f operator +(Triangle2f tri, REAL s)
        {
            return new Triangle2f(tri.A + s, tri.B + s, tri.C + s);
        }

        public static Triangle2f operator +(Triangle2f tri, VECTOR2 v)
        {
            return new Triangle2f(tri.A + v, tri.B + v, tri.C + v);
        }

        public static Triangle2f operator -(Triangle2f tri, REAL s)
        {
            return new Triangle2f(tri.A - s, tri.B - s, tri.C - s);
        }

        public static Triangle2f operator -(Triangle2f tri, VECTOR2 v)
        {
            return new Triangle2f(tri.A - v, tri.B - v, tri.C - v);
        }

        public static Triangle2f operator *(Triangle2f tri, REAL s)
        {
            return new Triangle2f(tri.A * s, tri.B * s, tri.C * s);
        }

        public static Triangle2f operator *(Triangle2f tri, VECTOR2 v)
        {
            return new Triangle2f(tri.A * v, tri.B * v, tri.C * v);
        }

        public static Triangle2f operator /(Triangle2f tri, REAL s)
        {
            return new Triangle2f(tri.A / s, tri.B / s, tri.C / s);
        }

        public static Triangle2f operator /(Triangle2f tri, VECTOR2 v)
        {
            return new Triangle2f(tri.A / v, tri.B / v, tri.C / v);
        }

        public static Triangle2f operator *(Triangle2f tri, MATRIX2 m)
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
        /// Closest point on triangle.
        /// </summary>
        /// <param name="p">point</param>
        /// <returns>closest point</returns>
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
            if (d6 >= 0.0f && d5 <= d6)
            {
                // barycentric coordinates (0,0,1)
                return C;
            }

            // Check if P in edge region of AC, if so return projection of P onto AC
            REAL vb = d5 * d2 - d1 * d6;
            if (vb <= 0.0f && d2 >= 0.0f && d6 <= 0.0f)
            {
                w = d2 / (d2 - d6);
                // barycentric coordinates (1-w,0,w)
                return A + w * ac;
            }

            // Check if P in edge region of BC, if so return projection of P onto BC
            REAL va = d3 * d6 - d5 * d4;
            if (va <= 0.0f && (d4 - d3) >= 0.0f && (d5 - d6) >= 0.0f)
            {
                w = (d4 - d3) / ((d4 - d3) + (d5 - d6));
                // barycentric coordinates (0,1-w,w)
                return B + w * (C - B);
            }

            // P inside face region. Compute Q through its barycentric coordinates (u,v,w)
            REAL denom = 1.0f / (va + vb + vc);
            v = vb * denom;
            w = vc * denom;

            // = u*a + v*b + w*c, u = va * denom = 1.0f - v - w
            return A + ab * v + ac * w;
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

    }
}