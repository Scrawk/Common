using System;
using System.Runtime.InteropServices;

using Common.Core.LinearAlgebra;

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

        public Box2f Bounds
        {
            get
            {
                float xmin = Math.Min(A.x, Math.Min(B.x, C.x));
                float xmax = Math.Max(A.x, Math.Max(B.x, C.x));
                float ymin = Math.Min(A.y, Math.Min(B.y, C.y));
                float ymax = Math.Max(A.y, Math.Max(B.y, C.y));

                return new Box2f(xmin, xmax, ymin, ymax);
            }
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