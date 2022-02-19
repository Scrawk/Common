﻿using System;
using System.Runtime.InteropServices;

using Common.Core.Numerics;

using REAL = System.Single;
using POINT3 = Common.Core.Numerics.Point3f;
using VECTOR3 = Common.Core.Numerics.Vector3f;
using BOX3 = Common.Core.Shapes.Box3f;
using MATRIX3 = Common.Core.Numerics.Matrix3x3f;
using MATRIX4 = Common.Core.Numerics.Matrix4x4f;

namespace Common.Core.Shapes
{
    /// <summary>
    /// A 3D triangle.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Triangle3f : IEquatable<Triangle3f>
    {
        /// <summary>
        /// The triangles first point.
        /// </summary>
        public POINT3 A;

        /// <summary>
        /// The triangles second point.
        /// </summary>
        public POINT3 B;

        /// <summary>
        /// The triangles third point.
        /// </summary>
        public POINT3 C;

        /// <summary>
        /// Create a new triangle.
        /// </summary>
        /// <param name="a">The first point.</param>
        /// <param name="b">The second point.</param>
        /// <param name="c">The third point.</param>
        public Triangle3f(POINT3 a, POINT3 b, POINT3 c)
        {
            A = a;
            B = b;
            C = c;
        }

        /// <summary>
        /// Create a new triangle.
        /// </summary>
        /// <param name="ax">The first points x value.</param>
        /// <param name="ay">The first points y value.</param>
        /// <param name="az">The first points z value.</param>
        /// <param name="bx">The second points x value.</param>
        /// <param name="by">The second points y value.</param>
        /// <param name="bz">The second points z value.</param>
        /// <param name="cx">The third points x value.</param>
        /// <param name="cy">The third points y value.</param>
        /// <param name="cz">The third points z value.</param>
        public Triangle3f(REAL ax, REAL ay, REAL az, REAL bx, REAL by, REAL bz, REAL cx, REAL cy, REAL cz)
        {
            A = new POINT3(ax, ay, az);
            B = new POINT3(bx, by, bz);
            C = new POINT3(cx, cy, cz);
        }

        /// <summary>
        /// The bounding box of the triangle.
        /// </summary>
        public BOX3 Bounds
        {
            get
            {
                var xmin = MathUtil.Min(A.x, B.x, C.x);
                var xmax = MathUtil.Max(A.x, B.x, C.x);
                var ymin = MathUtil.Min(A.y, B.y, C.y);
                var ymax = MathUtil.Max(A.y, B.y, C.y);
                var zmin = MathUtil.Min(A.z, B.z, C.z);
                var zmax = MathUtil.Max(A.z, B.z, C.z);

                return new BOX3(new POINT3(xmin, ymin, zmin), new POINT3(xmax, ymax, zmin));
            }
        }

        /// <summary>
        /// Array acess to the triangles points.
        /// </summary>
        /// <param name="i">The index of the point to access (0-2)</param>
        /// <returns>The point at index i.</returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        unsafe public POINT3 this[int i]
        {
            get
            {
                if ((uint)i >= 3)
                    throw new IndexOutOfRangeException("Triangle3f index out of range.");

                fixed (Triangle3f* array = &this) { return ((POINT3*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 3)
                    throw new IndexOutOfRangeException("Triangle3f index out of range.");

                fixed (POINT3* array = &A) { array[i] = value; }
            }
        }

        public static Triangle3f operator +(Triangle3f tri, REAL s)
        {
            return new Triangle3f(tri.A + s, tri.B + s, tri.C + s);
        }

        public static Triangle3f operator +(Triangle3f tri, POINT3 v)
        {
            return new Triangle3f(tri.A + v, tri.B + v, tri.C + v);
        }

        public static Triangle3f operator -(Triangle3f tri, REAL s)
        {
            return new Triangle3f(tri.A - s, tri.B - s, tri.C - s);
        }

        public static Triangle3f operator -(Triangle3f tri, POINT3 v)
        {
            return new Triangle3f(tri.A - v, tri.B - v, tri.C - v);
        }

        public static Triangle3f operator *(Triangle3f tri, REAL s)
        {
            return new Triangle3f(tri.A * s, tri.B * s, tri.C * s);
        }

        public static Triangle3f operator /(Triangle3f tri, REAL s)
        {
            return new Triangle3f(tri.A / s, tri.B / s, tri.C / s);
        }

        public static Triangle3f operator *(MATRIX3 m, Triangle3f tri)
        {
            return new Triangle3f(m * tri.A, m * tri.B, m * tri.C);
        }

        public static Triangle3f operator *(MATRIX4 m, Triangle3f tri)
        {
            return new Triangle3f(m * tri.A, m * tri.B, m * tri.C);
        }

        public static explicit operator Triangle3f(Triangle3d tri)
        {
            return new Triangle3f((POINT3)tri.A, (POINT3)tri.B, (POINT3)tri.C);
        }

        public static bool operator ==(Triangle3f t1, Triangle3f t2)
        {
            return t1.A == t2.A && t1.B == t2.B && t1.C == t2.C;
        }

        public static bool operator !=(Triangle3f t1, Triangle3f t2)
        {
            return t1.A != t2.A || t1.B != t2.B || t1.C != t2.C;
        }

        /// <summary>
        /// Is the triangle equal to this object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>Is the triangle equal to this object.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Triangle3f)) return false;
            Triangle3f tri = (Triangle3f)obj;
            return this == tri;
        }

        /// <summary>
        /// Is the triangle equal to the other triangle.
        /// </summary>
        /// <param name="tri">The other triangle.</param>
        /// <returns>Is the triangle equal to the other triangle.</returns>
        public bool Equals(Triangle3f tri)
        {
            return this == tri;
        }

        /// <summary>
        /// The triangles hash code.
        /// </summary>
        /// <returns>The triangles hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)MathUtil.HASH_PRIME_1;
                hash = (hash * MathUtil.HASH_PRIME_2) ^ A.GetHashCode();
                hash = (hash * MathUtil.HASH_PRIME_2) ^ B.GetHashCode();
                hash = (hash * MathUtil.HASH_PRIME_2) ^ C.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// The triangle as a string.
        /// </summary>
        /// <returns>The triangle as a string.</returns>
        public override string ToString()
        {
            return string.Format("[Triangle3f: A={0}, B={1}, C={2}]", A, B, C);
        }

        /// <summary>
        /// Round the triangles points.
        /// </summary>
        /// <param name="digits">number of digits to round to.</param>
        public void Round(int digits)
        {
            A = A.Rounded(digits);
            B = B.Rounded(digits);
            C = C.Rounded(digits);
        }

    }
}