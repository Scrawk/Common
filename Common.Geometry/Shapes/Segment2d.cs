using System;
using System.Runtime.InteropServices;

using Common.Core.LinearAlgebra;

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

    }
}

