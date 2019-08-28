using System;
using System.Runtime.InteropServices;

using Common.Core.Numerics;

using REAL = System.Single;
using VECTOR3 = Common.Core.Numerics.Vector3f;
using MATRIX3 = Common.Core.Numerics.Matrix3x3f;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Segment3f : IEquatable<Segment3f>
    {

        public VECTOR3 A;

        public VECTOR3 B;

        public Segment3f(VECTOR3 a, VECTOR3 b)
        {
            A = a;
            B = b;
        }

        public Segment3f(REAL ax, REAL ay, REAL az, REAL bx, REAL by, REAL bz)
        {
            A = new VECTOR3(ax, ay, az);
            B = new VECTOR3(bx, by, bz);
        }

        public VECTOR3 Center
        {
            get { return (A + B) * 0.5f; }
        }

        public REAL Length
        {
            get { return VECTOR3.Distance(A, B); }
        }

        public REAL SqrLength
        {
            get { return VECTOR3.SqrDistance(A, B); }
        }

        public Box3f Bounds
        {
            get
            {
                REAL xmin = Math.Min(A.x, B.x);
                REAL xmax = Math.Max(A.x, B.x);
                REAL ymin = Math.Min(A.y, B.y);
                REAL ymax = Math.Max(A.y, B.y);
                REAL zmin = Math.Min(A.z, B.z);
                REAL zmax = Math.Max(A.z, B.z);

                return new Box3f(xmin, xmax, ymin, ymax, zmin, zmax);
            }
        }

        unsafe public VECTOR3 this[int i]
        {
            get
            {
                if ((uint)i >= 2)
                    throw new IndexOutOfRangeException("Segment3f index out of range.");

                fixed (Segment3f* array = &this) { return ((VECTOR3*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 2)
                    throw new IndexOutOfRangeException("Segment3f index out of range.");

                fixed (VECTOR3* array = &A) { array[i] = value; }
            }
        }

        public static Segment3f operator +(Segment3f seg, REAL s)
        {
            return new Segment3f(seg.A + s, seg.B + s);
        }

        public static Segment3f operator +(Segment3f seg, VECTOR3 v)
        {
            return new Segment3f(seg.A + v, seg.B + v);
        }

        public static Segment3f operator -(Segment3f seg, REAL s)
        {
            return new Segment3f(seg.A - s, seg.B - s);
        }

        public static Segment3f operator -(Segment3f seg, VECTOR3 v)
        {
            return new Segment3f(seg.A - v, seg.B - v);
        }

        public static Segment3f operator *(Segment3f seg, REAL s)
        {
            return new Segment3f(seg.A * s, seg.B * s);
        }

        public static Segment3f operator *(Segment3f seg, VECTOR3 v)
        {
            return new Segment3f(seg.A * v, seg.B * v);
        }

        public static Segment3f operator /(Segment3f seg, REAL s)
        {
            return new Segment3f(seg.A / s, seg.B / s);
        }

        public static Segment3f operator /(Segment3f seg, VECTOR3 v)
        {
            return new Segment3f(seg.A / v, seg.B / v);
        }

        public static Segment3f operator *(Segment3f seg, MATRIX3 m)
        {
            return new Segment3f(m * seg.A, m * seg.B);
        }

        public static bool operator ==(Segment3f s1, Segment3f s2)
        {
            return s1.A == s2.A && s1.B == s2.B;
        }

        public static bool operator !=(Segment3f s1, Segment3f s2)
        {
            return s1.A != s2.A || s1.B != s2.B;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Segment3f)) return false;
            Segment3f seg = (Segment3f)obj;
            return this == seg;
        }

        public bool Equals(Segment3f seg)
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
            return string.Format("[Segment3f: A={0}, B={1}]", A, B);
        }

        /// <summary>
        /// The closest point on segment to point.
        /// </summary>
        /// <param name="p">point</param>
        public VECTOR3 Closest(VECTOR3 p)
        {
            REAL t;
            Closest(p, out t);
            return A + (B - A) * t;
        }

        /// <summary>
        /// The closest point on segment to point.
        /// </summary>
        /// <param name="p">point</param>
        /// <param name="t">closest point = A + t * (B - A)</param>
        public void Closest(VECTOR3 p, out REAL t)
        {
            t = 0.0f;
            VECTOR3 ab = B - A;
            VECTOR3 ap = p - A;

            REAL len = ab.x * ab.x + ab.y * ab.y;
            if (len < FMath.EPS) return;

            t = (ab.x * ap.x + ab.y * ap.y) / len;
            t = FMath.Clamp01(t);
        }

        /// <summary>
        /// Return the signed distance to the point. 
        /// Always positive.
        /// </summary>
        public REAL SignedDistance(VECTOR3 p)
        {
            return VECTOR3.Distance(Closest(p), p);
        }

    }
}

