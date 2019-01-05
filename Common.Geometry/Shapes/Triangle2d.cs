using System;
using System.Runtime.InteropServices;

using Common.Core.LinearAlgebra;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Triangle2d : IEquatable<Triangle2d>
    {

        public Vector2d A;

        public Vector2d B;

        public Vector2d C;

        public Triangle2d(Vector2d a, Vector2d b, Vector2d c)
        {
            A = a;
            B = b;
            C = c;
        }

        public Triangle2d(double ax, double ay, double bx, double by, double cx, double cy)
        {
            A = new Vector2d(ax, ay);
            B = new Vector2d(bx, by);
            C = new Vector2d(cx, cy);
        }

        public Vector2d Center
        {
            get { return (A + B + C) / 3.0; }
        }

        /// <summary>
        /// Calculate the bounding box.
        /// </summary>
        public Box2d Bounds
        {
            get
            {
                double xmin = Math.Min(A.x, Math.Min(B.x, C.x));
                double xmax = Math.Max(A.x, Math.Max(B.x, C.x));
                double ymin = Math.Min(A.y, Math.Min(B.y, C.y));
                double ymax = Math.Max(A.y, Math.Max(B.y, C.y));

                return new Box2d(xmin, xmax, ymin, ymax);
            }
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
            return string.Format("[Triangle2d: A={0}, B={1}, C={1}]", A, B, C);
        }

        /// <summary>
        /// Closest point on triangle.
        /// </summary>
        /// <param name="p">point</param>
        /// <returns>closest point</returns>
        public Vector2d Closest(Vector2d p)
        {
            Vector2d ab = B - A;
            Vector2d ac = C - A;
            Vector2d ap = p - A;

            // Check if P in vertex region outside A
            double d1 = Vector2d.Dot(ab, ap);
            double d2 = Vector2d.Dot(ac, ap);
            if (d1 <= 0.0 && d2 <= 0.0)
            {
                // barycentric coordinates (1,0,0)
                return A;
            }

            double v, w;

            // Check if P in vertex region outside B
            Vector2d bp = p - B;
            double d3 = Vector2d.Dot(ab, bp);
            double d4 = Vector2d.Dot(ac, bp);
            if (d3 >= 0.0 && d4 <= d3)
            {
                // barycentric coordinates (0,1,0)
                return B;
            }

            // Check if P in edge region of AB, if so return projection of P onto AB
            double vc = d1 * d4 - d3 * d2;
            if (vc <= 0.0 && d1 >= 0.0 && d3 <= 0.0)
            {
                v = d1 / (d1 - d3);
                // barycentric coordinates (1-v,v,0)
                return A + v * ab;
            }

            // Check if P in vertex region outside C
            Vector2d cp = p - C;
            double d5 = Vector2d.Dot(ab, cp);
            double d6 = Vector2d.Dot(ac, cp);
            if (d6 >= 0.0 && d5 <= d6)
            {
                // barycentric coordinates (0,0,1)
                return C;
            }

            // Check if P in edge region of AC, if so return projection of P onto AC
            double vb = d5 * d2 - d1 * d6;
            if (vb <= 0.0 && d2 >= 0.0 && d6 <= 0.0)
            {
                w = d2 / (d2 - d6);
                // barycentric coordinates (1-w,0,w)
                return A + w * ac;
            }

            // Check if P in edge region of BC, if so return projection of P onto BC
            double va = d3 * d6 - d5 * d4;
            if (va <= 0.0 && (d4 - d3) >= 0.0 && (d5 - d6) >= 0.0)
            {
                w = (d4 - d3) / ((d4 - d3) + (d5 - d6));
                // barycentric coordinates (0,1-w,w)
                return B + w * (C - B);
            }

            // P inside face region. Compute Q through its barycentric coordinates (u,v,w)
            double denom = 1.0f / (va + vb + vc);
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
        public bool Contains(Vector2d p)
        {
            double pab = Vector2d.Cross(p - A, B - A);
            double pbc = Vector2d.Cross(p - B, C - B);

            if (Math.Sign(pab) != Math.Sign(pbc)) return false;

            double pca = Vector2d.Cross(p - C, A - C);

            if (Math.Sign(pab) != Math.Sign(pca)) return false;

            return true;
        }

        /// <summary>
        /// Does triangle contain point.
        /// Asumes triangle is CCW;
        /// </summary>
        /// <param name="p">point</param>
        /// <returns>true if triangle contains point</returns>
        public bool ContainsCCW(Vector2d p)
        {
            if (Vector2d.Cross(p - A, B - A) > 0.0) return false;
            if (Vector2d.Cross(p - B, C - B) > 0.0) return false;
            if (Vector2d.Cross(p - C, A - C) > 0.0) return false;

            return true;
        }

    }
}