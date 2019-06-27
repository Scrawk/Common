using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Core.LinearAlgebra;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Box3d : IEquatable<Box3d>
    {

        public Vector3d Min;

        public Vector3d Max;

        public Box3d(double min, double max)
        {
            Min = new Vector3d(min);
            Max = new Vector3d(max);
        }

        public Box3d(double minX, double maxX, double minY, double maxY, double minZ, double maxZ)
        {
            Min = new Vector3d(minX, minY, minZ);
            Max = new Vector3d(maxX, maxY, maxZ);
        }

        public Box3d(Vector3d min, Vector3d max)
        {
            Min = min;
            Max = max;
        }

        public Box3d(Vector3i min, Vector3i max)
        {
            Min = new Vector3d(min.x, min.y, min.z);
            Max = new Vector3d(max.x, max.y, max.z); ;
        }

        public Vector3d Center 
        { 
            get { return (Min + Max) * 0.5f; } 
        }

        public Vector3d Size 
        { 
            get { return new Vector3d(Width, Height, Depth); } 
        }

        public double Width 
        { 
            get { return Max.x - Min.x; } 
        }

        public double Height 
        { 
            get { return Max.y - Min.y; } 
        }

        public double Depth 
        { 
            get { return Max.z - Min.z; } 
        }

        public double Area
        {
            get
            {
                return (Max.x - Min.x) * (Max.y - Min.y) * (Max.z - Min.z);
            }
        }

        public double SurfaceArea
        {
            get
            {
                Vector3d d = Max - Min;
                return 2.0 * (d.x * d.y + d.x * d.z + d.y * d.z);
            }
        }

        public static implicit operator Box3d(Box3f box)
        {
            return new Box3d(box.Min, box.Max);
        }

        public static implicit operator Box3d(Box3i box)
        {
            return new Box3d(box.Min, box.Max);
        }

        public static bool operator ==(Box3d b1, Box3d b2)
        {
            return b1.Min == b2.Min && b1.Max == b2.Max;
        }

        public static bool operator !=(Box3d b1, Box3d b2)
        {
            return b1.Min != b2.Min || b1.Max != b2.Max;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Box3d)) return false;
            Box3d box = (Box3d)obj;
            return this == box;
        }

        public bool Equals(Box3d box)
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
            return string.Format("[Box3d: Min={0}, Max={1}, Width={2}, Height={3}, Depth={4}]", Min, Max, Width, Height, Depth);
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
        public static Box3d Enlarge(Box3d box, Vector3d p)
        {
            var b = new Box3d();
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
        public static Box3d Enlarge(Box3d box0, Box3d box1)
        {
            var b = new Box3d();
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
        public static Box3d Enlarge(Box3d box, double percent)
        {
            var amount = box.Size * percent * 0.5;
            return new Box3d(box.Min - amount, box.Max + amount);
        }

        /// <summary>
        /// Returns true if this box intersects the other box.
        /// </summary>
        public bool Intersects(Box3d a)
        {
            if (Max.x < a.Min.x || Min.x > a.Max.x) return false;
            if (Max.y < a.Min.y || Min.y > a.Max.y) return false;
            if (Max.z < a.Min.z || Min.z > a.Max.z) return false;
            return true;
        }

        /// <summary>
        /// Does the box contain the other box.
        /// </summary>
        public bool Contains(Box3d a)
        {
            if (a.Max.x > Max.x || a.Min.x < Min.x) return false;
            if (a.Max.y > Max.y || a.Min.y < Min.y) return false;
            if (a.Max.z > Max.z || a.Min.z < Min.z) return false;
            return true;
        }

        /// <summary>
        /// Returns true if this bounding box contains the given point.
        /// </summary>
        public bool Contains(Vector3d p)
        {
            if (p.x > Max.x || p.x < Min.x) return false;
            if (p.y > Max.y || p.y < Min.y) return false;
            if (p.z > Max.z || p.z < Min.z) return false;
            return true;
        }

        /// <summary>
        /// Returns the closest point on the box.
        /// </summary>
        public Vector3d Closest(Vector3d p)
        {
            Vector3d c;

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

        public static Box3d CalculateBounds(IList<Vector3d> vertices)
        {
            Vector3d min = Vector3d.PositiveInfinity;
            Vector3d max = Vector3d.NegativeInfinity;

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

            return new Box3d(min, max);
        }

        public static Box3d CalculateBounds(Vector3d a, Vector3d b)
        {
            double xmin = Math.Min(a.x, b.x);
            double xmax = Math.Max(a.x, b.x);
            double ymin = Math.Min(a.y, b.y);
            double ymax = Math.Max(a.y, b.y);
            double zmin = Math.Min(a.z, b.z);
            double zmax = Math.Max(a.z, b.z);

            return new Box3d(xmin, xmax, ymin, ymax, zmin, zmax);
        }
    }

}




















