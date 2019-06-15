using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Core.LinearAlgebra;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Box3i : IEquatable<Box3i>
    {
        public Vector3i Min;

        public Vector3i Max;

        public Box3i(int min, int max)
        {
            Min = new Vector3i(min);
            Max = new Vector3i(max);
        }

        public Box3i(int minX, int maxX, int minY, int maxY, int minZ, int maxZ)
        {
            Min = new Vector3i(minX, minY, minZ);
            Max = new Vector3i(maxX, maxY, maxZ);
        }

        public Box3i(Vector3i min, Vector3i max)
        {
            Min = min;
            Max = max;
        }

        public Vector3f Center 
        { 
            get { return new Vector3f((Min + Max).x * 0.5f, (Min + Max).y * 0.5f, (Min + Max).z * 0.5f); } 
        }

        public Vector3i Size 
        { 
            get { return new Vector3i(Width, Height, Depth); } 
        }

        public int Width 
        { 
            get { return Max.x - Min.x; } 
        }

        public int Height 
        { 
            get { return Max.y - Min.y; } 
        }

        public int Depth 
        { 
            get { return Max.z - Min.z; } 
        }

        public int Area
        {
            get
            {
                return (Max.x - Min.x) * (Max.y - Min.y) * (Max.z - Min.z);
            }
        }

        public int SurfaceArea
        {
            get
            {
                Vector3i d = Max - Min;
                return 2 * (d.x * d.y + d.x * d.z + d.y * d.z);
            }
        }

        public static explicit operator Box3i(Box3f box)
        {
            return new Box3i((Vector3i)box.Min, (Vector3i)box.Max);
        }

        public static explicit operator Box3i(Box3d box)
        {
            return new Box3i((Vector3i)box.Min, (Vector3i)box.Max);
        }

        public static bool operator ==(Box3i b1, Box3i b2)
        {
            return b1.Min == b2.Min && b1.Max == b2.Max;
        }

        public static bool operator !=(Box3i b1, Box3i b2)
        {
            return b1.Min != b2.Min || b1.Max != b2.Max;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Box3i)) return false;
            Box3i box = (Box3i)obj;
            return this == box;
        }

        public bool Equals(Box3i box)
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
            return string.Format("[Box3i: Min={0}, Max={1}, Width={2}, Height={3}, Depth={4}]", Min, Max, Width, Height, Depth);
        }

        public void GetCorners(IList<Vector3i> corners)
        {
            corners[0] = new Vector3i(Min.x, Min.y, Min.z);
            corners[1] = new Vector3i(Max.x, Min.y, Min.z);
            corners[2] = new Vector3i(Max.x, Min.y, Max.z);
            corners[3] = new Vector3i(Min.x, Min.y, Max.z);

            corners[4] = new Vector3i(Min.x, Max.y, Min.z);
            corners[5] = new Vector3i(Max.x, Max.y, Min.z);
            corners[6] = new Vector3i(Max.x, Max.y, Max.z);
            corners[7] = new Vector3i(Min.x, Max.y, Max.z);
        }

        public void GetCorners(IList<Vector3d> corners)
        {
            corners[0] = new Vector3d(Min.x, Min.y, Min.z);
            corners[1] = new Vector3d(Max.x, Min.y, Min.z);
            corners[2] = new Vector3d(Max.x, Min.y, Max.z);
            corners[3] = new Vector3d(Min.x, Min.y, Max.z);

            corners[4] = new Vector3d(Min.x, Max.y, Min.z);
            corners[5] = new Vector3d(Max.x, Max.y, Min.z);
            corners[6] = new Vector3d(Max.x, Max.y, Max.z);
            corners[7] = new Vector3d(Min.x, Max.y, Max.z);
        }

        public void GetCorners(IList<Vector4i> corners)
        {
            corners[0] = new Vector4i(Min.x, Min.y, Min.z, 1);
            corners[1] = new Vector4i(Max.x, Min.y, Min.z, 1);
            corners[2] = new Vector4i(Max.x, Min.y, Max.z, 1);
            corners[3] = new Vector4i(Min.x, Min.y, Max.z, 1);

            corners[4] = new Vector4i(Min.x, Max.y, Min.z, 1);
            corners[5] = new Vector4i(Max.x, Max.y, Min.z, 1);
            corners[6] = new Vector4i(Max.x, Max.y, Max.z, 1);
            corners[7] = new Vector4i(Min.x, Max.y, Max.z, 1);
        }

        /// <summary>
        /// Returns the bounding box containing this box and the given point.
        /// </summary>
        public static Box3i Enlarge(Box3i box, Vector3i p)
        {
            var b = new Box3i();
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
        public static Box3i Enlarge(Box3i box0, Box3i box1)
        {
            var b = new Box3i();
            b.Min.x = Math.Min(box0.Min.x, box1.Min.x);
            b.Min.y = Math.Min(box0.Min.y, box1.Min.y);
            b.Min.z = Math.Min(box0.Min.z, box1.Min.z);
            b.Max.x = Math.Max(box0.Max.x, box1.Max.x);
            b.Max.y = Math.Max(box0.Max.y, box1.Max.y);
            b.Max.z = Math.Max(box0.Max.z, box1.Max.z);
            return b;
        }

        /// <summary>
        /// Returns true if this box intersects the other box.
        /// </summary>
        public bool Intersects(Box3i a)
        {
            if (Max.x < a.Min.x || Min.x > a.Max.x) return false;
            if (Max.y < a.Min.y || Min.y > a.Max.y) return false;
            if (Max.z < a.Min.z || Min.z > a.Max.z) return false;
            return true;
        }

        /// <summary>
        /// Does the box contain the other box.
        /// </summary>
        public bool Contains(Box3i a)
        {
            if (a.Max.x > Max.x || a.Min.x < Min.x) return false;
            if (a.Max.y > Max.y || a.Min.y < Min.y) return false;
            if (a.Max.z > Max.z || a.Min.z < Min.z) return false;
            return true;
        }

        /// <summary>
        /// Returns true if this bounding box contains the given point.
        /// </summary>
        public bool Contains(Vector3i p)
        {
            if (p.x > Max.x || p.x < Min.x) return false;
            if (p.y > Max.y || p.y < Min.y) return false;
            if (p.z > Max.z || p.z < Min.z) return false;
            return true;
        }

        /// <summary>
        /// Returns the closest point on the box.
        /// </summary>
        public Vector3i Closest(Vector3i p)
        {
            Vector3i c;

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

        public static Box3i CalculateBounds(IList<Vector3i> vertices)
        {
            Vector3i min = Vector3i.MaxInt;
            Vector3i max = Vector3i.MinInt;

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

            return new Box3i(min, max);
        }

        public static Box3i CalculateBounds(Vector3i a, Vector3i b)
        {
            int xmin = Math.Min(a.x, b.x);
            int xmax = Math.Max(a.x, b.x);
            int ymin = Math.Min(a.y, b.y);
            int ymax = Math.Max(a.y, b.y);
            int zmin = Math.Min(a.z, b.z);
            int zmax = Math.Max(a.z, b.z);

            return new Box3i(xmin, xmax, ymin, ymax, zmin, zmax);
        }

    }

}




















