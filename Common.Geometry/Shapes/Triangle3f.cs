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
    public struct Triangle3f : IEquatable<Triangle3f>
    {

        public VECTOR3 A;

        public VECTOR3 B;

        public VECTOR3 C;

        public Triangle3f(VECTOR3 a, VECTOR3 b, VECTOR3 c)
        {
            A = a;
            B = b;
            C = c;
        }

        public Triangle3f(REAL ax, REAL ay, REAL az, REAL bx, REAL by, REAL bz, REAL cx, REAL cy, REAL cz)
        {
            A = new VECTOR3(ax, ay, az);
            B = new VECTOR3(bx, by, bz);
            C = new VECTOR3(cx, cy, cz);
        }

        public VECTOR3 Center
        {
            get { return (A + B + C) / 3.0f; }
        }


        /// <summary>
        /// Calculate the bounding box.
        /// </summary>
        public Box3f Bounds
        {
            get
            {
                var xmin = FMath.Min(A.x, B.x, C.x);
                var xmax = FMath.Max(A.x, B.x, C.x);
                var ymin = FMath.Min(A.y, B.y, C.y);
                var ymax = FMath.Max(A.y, B.y, C.y);
                var zmin = FMath.Min(A.z, B.y, C.y);
                var zmax = FMath.Max(A.z, B.z, C.z);

                return new Box3f(xmin, xmax, ymin, ymax, zmin, zmax);
            }
        }

        unsafe public VECTOR3 this[int i]
        {
            get
            {
                if ((uint)i >= 3)
                    throw new IndexOutOfRangeException("Triangle3f index out of range.");

                fixed (Triangle3f* array = &this) { return ((VECTOR3*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 3)
                    throw new IndexOutOfRangeException("Triangle3f index out of range.");

                fixed (VECTOR3* array = &A) { array[i] = value; }
            }
        }

        public static Triangle3f operator +(Triangle3f tri, REAL s)
        {
            return new Triangle3f(tri.A + s, tri.B + s, tri.C + s);
        }

        public static Triangle3f operator +(Triangle3f tri, VECTOR3 v)
        {
            return new Triangle3f(tri.A + v, tri.B + v, tri.C + v);
        }

        public static Triangle3f operator -(Triangle3f tri, REAL s)
        {
            return new Triangle3f(tri.A - s, tri.B - s, tri.C - s);
        }

        public static Triangle3f operator -(Triangle3f tri, VECTOR3 v)
        {
            return new Triangle3f(tri.A - v, tri.B - v, tri.C - v);
        }

        public static Triangle3f operator *(Triangle3f tri, REAL s)
        {
            return new Triangle3f(tri.A * s, tri.B * s, tri.C * s);
        }

        public static Triangle3f operator *(Triangle3f tri, VECTOR3 v)
        {
            return new Triangle3f(tri.A * v, tri.B * v, tri.C * v);
        }

        public static Triangle3f operator /(Triangle3f tri, REAL s)
        {
            return new Triangle3f(tri.A / s, tri.B / s, tri.C / s);
        }

        public static Triangle3f operator /(Triangle3f tri, VECTOR3 v)
        {
            return new Triangle3f(tri.A / v, tri.B / v, tri.C / v);
        }

        public static Triangle3f operator *(Triangle3f tri, MATRIX3 m)
        {
            return new Triangle3f(m * tri.A, m * tri.B, m * tri.C);
        }

        public static bool operator ==(Triangle3f t1, Triangle3f t2)
        {
            return t1.A == t2.A && t1.B == t2.B && t1.C == t2.C;
        }

        public static bool operator !=(Triangle3f t1, Triangle3f t2)
        {
            return t1.A != t2.A || t1.B != t2.B || t1.C != t2.C;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Triangle3f)) return false;
            Triangle3f tri = (Triangle3f)obj;
            return this == tri;
        }

        public bool Equals(Triangle3f tri)
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
            return string.Format("[Triangle3f: A={0}, B={1}, C={2}]", A, B, C);
        }

    }
}
