using System;
using System.Runtime.InteropServices;

using Common.Core.Numerics;
using Common.Core.Numerics;

using REAL = System.Double;
using VECTOR2 = Common.Core.Numerics.Vector2d;
using VECTOR3 = Common.Core.Numerics.Vector3d;

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
                var xmin = Math.Min(A.x, Math.Min(B.x, C.x));
                var xmax = Math.Max(A.x, Math.Max(B.x, C.x));
                var ymin = Math.Min(A.y, Math.Min(B.y, C.y));
                var ymax = Math.Max(A.y, Math.Max(B.y, C.y));
                var zmin = Math.Min(A.z, Math.Min(B.z, C.z));
                var zmax = Math.Max(A.z, Math.Max(B.z, C.z));

                return new Box3d(xmin, xmax, ymin, ymax, zmin, zmax);
            }
        }

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