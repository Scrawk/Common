using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Core.Numerics;

using REAL = System.Single;
using VECTOR2 = Common.Core.Numerics.Vector2f;
using VECTOR3 = Common.Core.Numerics.Vector3f;
using MATRIX3 = Common.Core.Numerics.Matrix3x3f;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Box3f : IEquatable<Box3f>
    {

        public VECTOR3 Min;

        public VECTOR3 Max;

        public Box3f(REAL min, REAL max)
        {
            Min = new VECTOR3(min);
            Max = new VECTOR3(max);
        }

        public Box3f(REAL minX, REAL maxX, REAL minY, REAL maxY, REAL minZ, REAL maxZ)
        {
            Min = new VECTOR3(minX, minY, minZ);
            Max = new VECTOR3(maxX, maxY, maxZ);
        }

        public Box3f(VECTOR3 min, VECTOR3 max)
        {
            Min = min;
            Max = max;
        }

        public Box3f(Vector3i min, Vector3i max)
        {
            Min = new VECTOR3(min.x, min.y, min.z);
            Max = new VECTOR3(max.x, max.y, max.z); ;
        }

        public VECTOR3 Center
        {
            get { return (Min + Max) * 0.5f; }
        }

        public VECTOR3 Size
        {
            get { return new VECTOR3(Width, Height, Depth); }
        }

        public REAL Width
        {
            get { return Max.x - Min.x; }
        }

        public REAL Height
        {
            get { return Max.y - Min.y; }
        }

        public REAL Depth
        {
            get { return Max.z - Min.z; }
        }

        public REAL Area
        {
            get
            {
                return (Max.x - Min.x) * (Max.y - Min.y) * (Max.z - Min.z);
            }
        }

        public REAL SurfaceArea
        {
            get
            {
                VECTOR3 d = Max - Min;
                return 2.0f * (d.x * d.y + d.x * d.z + d.y * d.z);
            }
        }

        public static explicit operator Box3f(Box3d box)
        {
            return new Box3f((VECTOR3)box.Min, (VECTOR3)box.Max);
        }

        public static implicit operator Box3f(Box3i box)
        {
            return new Box3f(box.Min, box.Max);
        }

        public static bool operator ==(Box3f b1, Box3f b2)
        {
            return b1.Min == b2.Min && b1.Max == b2.Max;
        }

        public static bool operator !=(Box3f b1, Box3f b2)
        {
            return b1.Min != b2.Min || b1.Max != b2.Max;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Box3f)) return false;
            Box3f box = (Box3f)obj;
            return this == box;
        }

        public bool Equals(Box3f box)
        {
            return this == box;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ Min.GetHashCode();
                hash = (hash * 16777619) ^ Max.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return string.Format("[Box3f: Min={0}, Max={1}, Width={2}, Height={3}, Depth={4}]", Min, Max, Width, Height, Depth);
        }

        public void GetCorners(IList<VECTOR3> corners)
        {
            corners[0] = new VECTOR3(Min.x, Min.y, Min.z);
            corners[1] = new VECTOR3(Max.x, Min.y, Min.z);
            corners[2] = new VECTOR3(Max.x, Min.y, Max.z);
            corners[3] = new VECTOR3(Min.x, Min.y, Max.z);

            corners[4] = new VECTOR3(Min.x, Max.y, Min.z);
            corners[5] = new VECTOR3(Max.x, Max.y, Min.z);
            corners[6] = new VECTOR3(Max.x, Max.y, Max.z);
            corners[7] = new VECTOR3(Min.x, Max.y, Max.z);
        }

        public void GetCorners(IList<Vector4d> corners)
        {
            corners[0] = new Vector4d(Min.x, Min.y, Min.z, 1);
            corners[1] = new Vector4d(Max.x, Min.y, Min.z, 1);
            corners[2] = new Vector4d(Max.x, Min.y, Max.z, 1);
            corners[3] = new Vector4d(Min.x, Min.y, Max.z, 1);

            corners[4] = new Vector4d(Min.x, Max.y, Min.z, 1);
            corners[5] = new Vector4d(Max.x, Max.y, Min.z, 1);
            corners[6] = new Vector4d(Max.x, Max.y, Max.z, 1);
            corners[7] = new Vector4d(Min.x, Max.y, Max.z, 1);
        }

        /// <summary>
        /// Returns the bounding box containing this box and the given point.
        /// </summary>
        public static Box3f Enlarge(Box3f box, VECTOR3 p)
        {
            var b = new Box3f();
            b.Min.x = Math.Min(box.Min.x, p.x);
            b.Min.y = Math.Min(box.Min.y, p.y);
            b.Min.z = Math.Min(box.Min.z, p.z);
            b.Max.x = Math.Max(box.Max.x, p.x);
            b.Max.y = Math.Max(box.Max.y, p.y);
            b.Max.z = Math.Max(box.Max.z, p.z);
            return b;
        }

        /// <summary>
        /// Returns the bounding box containing this box and the given box.
        /// </summary>
        public static Box3f Enlarge(Box3f box0, Box3f box1)
        {
            var b = new Box3f();
            b.Min.x = Math.Min(box0.Min.x, box1.Min.x);
            b.Min.y = Math.Min(box0.Min.y, box1.Min.y);
            b.Min.z = Math.Min(box0.Min.z, box1.Min.z);
            b.Max.x = Math.Max(box0.Max.x, box1.Max.x);
            b.Max.y = Math.Max(box0.Max.y, box1.Max.y);
            b.Max.z = Math.Max(box0.Max.z, box1.Max.z);
            return b;
        }

        /// <summary>
        /// Enlarge the box by a given percent.
        /// </summary>
        public static Box3f Enlarge(Box3f box, REAL percent)
        {
            var amount = box.Size * percent * 0.5f;
            return new Box3f(box.Min - amount, box.Max + amount);
        }

        /// <summary>
        /// Returns true if this box intersects the other box.
        /// </summary>
        public bool Intersects(Box3f a)
        {
            if (Max.x < a.Min.x || Min.x > a.Max.x) return false;
            if (Max.y < a.Min.y || Min.y > a.Max.y) return false;
            if (Max.z < a.Min.z || Min.z > a.Max.z) return false;
            return true;
        }

        /// <summary>
        /// Does the box contain the other box.
        /// </summary>
        public bool Contains(Box3f a)
        {
            if (a.Max.x > Max.x || a.Min.x < Min.x) return false;
            if (a.Max.y > Max.y || a.Min.y < Min.y) return false;
            if (a.Max.z > Max.z || a.Min.z < Min.z) return false;
            return true;
        }

        /// <summary>
        /// Returns true if this bounding box contains the given point.
        /// </summary>
        public bool Contains(VECTOR3 p)
        {
            if (p.x > Max.x || p.x < Min.x) return false;
            if (p.y > Max.y || p.y < Min.y) return false;
            if (p.z > Max.z || p.z < Min.z) return false;
            return true;
        }

        /// <summary>
        /// Returns the closest point on the box.
        /// </summary>
        public VECTOR3 Closest(VECTOR3 p)
        {
            VECTOR3 c;

            if (p.x < Min.x)
                c.x = Min.x;
            else if (p.x > Max.x)
                c.x = Max.x;
            else
                c.x = p.x;

            if (p.y < Min.y)
                c.y = Min.y;
            else if (p.y > Max.y)
                c.y = Max.y;
            else
                c.y = p.y;

            if (p.z < Min.z)
                c.z = Min.z;
            else if (p.z > Max.z)
                c.z = Max.z;
            else
                c.z = p.z;

            return c;
        }

        public static Box3f CalculateBounds(IList<VECTOR3> vertices)
        {
            VECTOR3 min = VECTOR3.PositiveInfinity;
            VECTOR3 max = VECTOR3.NegativeInfinity;

            int count = vertices.Count;
            for (int i = 0; i < count; i++)
            {
                var v = vertices[i];
                if (v.x < min.x) min.x = v.x;
                if (v.y < min.y) min.y = v.y;
                if (v.z < min.z) min.z = v.z;

                if (v.x > max.x) max.x = v.x;
                if (v.y > max.y) max.y = v.y;
                if (v.z > max.z) max.z = v.z;
            }

            return new Box3f(min, max);
        }

        public static Box3f CalculateBounds(VECTOR3 a, VECTOR3 b)
        {
            REAL xmin = Math.Min(a.x, b.x);
            REAL xmax = Math.Max(a.x, b.x);
            REAL ymin = Math.Min(a.y, b.y);
            REAL ymax = Math.Max(a.y, b.y);
            REAL zmin = Math.Min(a.z, b.z);
            REAL zmax = Math.Max(a.z, b.z);

            return new Box3f(xmin, xmax, ymin, ymax, zmin, zmax);
        }
    }

}




















