﻿using System;
using System.Runtime.InteropServices;

using Common.Core.LinearAlgebra;

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

    }
}

