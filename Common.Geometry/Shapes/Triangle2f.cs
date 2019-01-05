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
            return string.Format("[Triangle2f: A={0}, B={1}, C={1}]", A, B, C);
        }

    }
}