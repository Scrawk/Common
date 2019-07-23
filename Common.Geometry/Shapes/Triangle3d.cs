using System;
using System.Runtime.InteropServices;

using Common.Core.Numerics;

using REAL = System.Double;
using VECTOR3 = Common.Core.Numerics.Vector3d;
using MATRIX3 = Common.Core.Numerics.Matrix3x3d;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Triangle3d : IEquatable<Triangle3d>
    {

        public VECTOR3 A;

        public VECTOR3 B;

        public VECTOR3 C;

        public Triangle3d(VECTOR3 a, VECTOR3 b, VECTOR3 c)
        {
            A = a;
            B = b;
            C = c;
        }

        public Triangle3d(REAL ax, REAL ay, REAL az, REAL bx, REAL by, REAL bz, REAL cx, REAL cy, REAL cz)
        {
            A = new VECTOR3(ax, ay, az);
            B = new VECTOR3(bx, by, bz);
            C = new VECTOR3(cx, cy, cz);
        }

        public VECTOR3 Center
        {
            get { return (A + B + C) / 3.0; }
        }


        /// <summary>
        /// Calculate the bounding box.
        /// </summary>
        public Box3d Bounds
        {
            get
            {
                var xmin = DMath.Min(A.x, B.x, C.x);
                var xmax = DMath.Max(A.x, B.x, C.x);
                var ymin = DMath.Min(A.y, B.y, C.y);
                var ymax = DMath.Max(A.y, B.y, C.y);
                var zmin = DMath.Min(A.z, B.y, C.y);
                var zmax = DMath.Max(A.z, B.z, C.z);

                return new Box3d(xmin, xmax, ymin, ymax, zmin, zmax);
            }
        }

        unsafe public VECTOR3 this[int i]
        {
            get
            {
                if ((uint)i >= 3)
                    throw new IndexOutOfRangeException("Triangle3d index out of range.");

                fixed (Triangle3d* array = &this) { return ((VECTOR3*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 3)
                    throw new IndexOutOfRangeException("Triangle3d index out of range.");

                fixed (VECTOR3* array = &A) { array[i] = value; }
            }
        }

        public static Triangle3d operator +(Triangle3d tri, REAL s)
        {
            return new Triangle3d(tri.A + s, tri.B + s, tri.C + s);
        }

        public static Triangle3d operator +(Triangle3d tri, VECTOR3 v)
        {
            return new Triangle3d(tri.A + v, tri.B + v, tri.C + v);
        }

        public static Triangle3d operator -(Triangle3d tri, REAL s)
        {
            return new Triangle3d(tri.A - s, tri.B - s, tri.C - s);
        }

        public static Triangle3d operator -(Triangle3d tri, VECTOR3 v)
        {
            return new Triangle3d(tri.A - v, tri.B - v, tri.C - v);
        }

        public static Triangle3d operator *(Triangle3d tri, REAL s)
        {
            return new Triangle3d(tri.A * s, tri.B * s, tri.C * s);
        }

        public static Triangle3d operator *(Triangle3d tri, VECTOR3 v)
        {
            return new Triangle3d(tri.A * v, tri.B * v, tri.C * v);
        }

        public static Triangle3d operator /(Triangle3d tri, REAL s)
        {
            return new Triangle3d(tri.A / s, tri.B / s, tri.C / s);
        }

        public static Triangle3d operator /(Triangle3d tri, VECTOR3 v)
        {
            return new Triangle3d(tri.A / v, tri.B / v, tri.C / v);
        }

        public static Triangle3d operator *(Triangle3d tri, MATRIX3 m)
        {
            return new Triangle3d(m * tri.A, m * tri.B, m * tri.C);
        }

        //public static implicit operator Triangle3d(Triangle3f tri)
        //{
        //    return new Triangle3d(tri.A, tri.B, tri.C);
        //}

        public static bool operator ==(Triangle3d t1, Triangle3d t2)
        {
            return t1.A == t2.A && t1.B == t2.B && t1.C == t2.C;
        }

        public static bool operator !=(Triangle3d t1, Triangle3d t2)
        {
            return t1.A != t2.A || t1.B != t2.B || t1.C != t2.C;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Triangle3d)) return false;
            Triangle3d tri = (Triangle3d)obj;
            return this == tri;
        }

        public bool Equals(Triangle3d tri)
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
            return string.Format("[Triangle3d: A={0}, B={1}, C={2}]", A, B, C);
        }

    }
}
