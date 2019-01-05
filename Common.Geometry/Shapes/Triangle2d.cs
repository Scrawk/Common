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

    }
}