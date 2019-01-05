using System;
using System.Runtime.InteropServices;

using Common.Core.LinearAlgebra;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Capsule2f : IEquatable<Capsule2f>
    {
        public Vector2f A;

        public Vector2f B;

        public float Radius;

        public Capsule2f(Vector2f a, Vector2f b, float radius)
        {
            A = a;
            B = b;
            Radius = radius;
        }

        public Capsule2f(float ax, float ay, float bx, float by, float radius)
        {
            A = new Vector2f(ax, ay);
            B = new Vector2f(bx, by);
            Radius = radius;
        }

        public Vector2f Center
        {
            get { return (A + B) / 2.0f; }
        }

        /// <summary>
        /// Calculate the bounding box.
        /// </summary>
        public Box2f Bounds
        {
            get
            {
                float xmin = Math.Min(A.x, B.x) - Radius;
                float xmax = Math.Max(A.x, B.x) + Radius;
                float ymin = Math.Min(A.y, B.y) - Radius;
                float ymax = Math.Max(A.y, B.y) + Radius;

                return new Box2f(xmin, xmax, ymin, ymax);
            }
        }

        public static bool operator ==(Capsule2f c1, Capsule2f c2)
        {
            return c1.Radius == c2.Radius && c1.A == c2.A && c1.B == c2.B;
        }

        public static bool operator !=(Capsule2f c1, Capsule2f c2)
        {
            return c1.Radius != c2.Radius || c1.A != c2.A || c1.A != c2.A;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Capsule2f)) return false;
            Capsule2f cap = (Capsule2f)obj;
            return this == cap;
        }

        public bool Equals(Capsule2f cap)
        {
            return this == cap;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ Radius.GetHashCode();
                hash = (hash * 16777619) ^ A.GetHashCode();
                hash = (hash * 16777619) ^ B.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return string.Format("[Capsule2f: A={0}, B={1}, Radius={2}]", A, B, Radius);
        }

    }
}