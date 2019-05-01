using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Core.LinearAlgebra;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Box3f : IEquatable<Box3f>
    {

        public Vector3f Min;

        public Vector3f Max;

        public Box3f(float min, float max)
        {
            Min = new Vector3f(min);
            Max = new Vector3f(max);
        }

        public Box3f(float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
        {
            Min = new Vector3f(minX, minY, minZ);
            Max = new Vector3f(maxX, maxY, maxZ);
        }

        public Box3f(Vector3f min, Vector3f max)
        {
            Min = min;
            Max = max;
        }

        public Box3f(Vector3i min, Vector3i max)
        {
            Min = new Vector3f(min.x, min.y, min.z);
            Max = new Vector3f(max.x, max.y, max.z); ;
        }

        public Vector3f Center
        {
            get { return (Min + Max) * 0.5f; }
        }

        public Vector3f Size
        {
            get { return new Vector3f(Width, Height, Depth); }
        }

        public float Width
        {
            get { return Max.x - Min.x; }
        }

        public float Height
        {
            get { return Max.y - Min.y; }
        }

        public float Depth
        {
            get { return Max.z - Min.z; }
        }

        public float Area
        {
            get
            {
                return (Max.x - Min.x) * (Max.y - Min.y) * (Max.z - Min.z);
            }
        }

        public float SurfaceArea
        {
            get
            {
                Vector3f d = Max - Min;
                return 2.0f * (d.x * d.y + d.x * d.z + d.y * d.z);
            }
        }

        public static explicit operator Box3f(Box3d box)
        {
            return new Box3f((Vector3f)box.Min, (Vector3f)box.Max);
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

        public void GetCorners(IList<Vector3f> corners)
        {
            corners[0] = new Vector3f(Min.x, Min.y, Min.z);
            corners[1] = new Vector3f(Max.x, Min.y, Min.z);
            corners[2] = new Vector3f(Max.x, Min.y, Max.z);
            corners[3] = new Vector3f(Min.x, Min.y, Max.z);

            corners[4] = new Vector3f(Min.x, Max.y, Min.z);
            corners[5] = new Vector3f(Max.x, Max.y, Min.z);
            corners[6] = new Vector3f(Max.x, Max.y, Max.z);
            corners[7] = new Vector3f(Min.x, Max.y, Max.z);
        }

        public void GetCorners(IList<Vector4f> corners)
        {
            corners[0] = new Vector4f(Min.x, Min.y, Min.z, 1);
            corners[1] = new Vector4f(Max.x, Min.y, Min.z, 1);
            corners[2] = new Vector4f(Max.x, Min.y, Max.z, 1);
            corners[3] = new Vector4f(Min.x, Min.y, Max.z, 1);

            corners[4] = new Vector4f(Min.x, Max.y, Min.z, 1);
            corners[5] = new Vector4f(Max.x, Max.y, Min.z, 1);
            corners[6] = new Vector4f(Max.x, Max.y, Max.z, 1);
            corners[7] = new Vector4f(Min.x, Max.y, Max.z, 1);
        }

        /// <summary>
        /// Returns the bounding box containing this box and the given point.
        /// </summary>
        public void Enlarge(Vector3f p)
        {
            Min.x = Math.Min(Min.x, p.x);
            Min.y = Math.Min(Min.y, p.y);
            Min.z = Math.Min(Min.z, p.z);
            Max.x = Math.Max(Max.x, p.x);
            Max.y = Math.Max(Max.y, p.y);
            Max.z = Math.Max(Max.z, p.z);
        }

        /// <summary>
        /// Returns the bounding box containing this box and the given box.
        /// </summary>
        public void Enlarge(Box3f box)
        {
            Min.x = Math.Min(Min.x, box.Min.x);
            Min.y = Math.Min(Min.y, box.Min.y);
            Min.z = Math.Min(Min.z, box.Min.z);
            Max.x = Math.Max(Max.x, box.Max.x);
            Max.y = Math.Max(Max.y, box.Max.y);
            Max.z = Math.Max(Max.z, box.Max.z);
        }

        /// <summary>
        /// Returns the bounding box containing this box and the given box.
        /// </summary>
        public static Box3f Enlarge(Box3f box0, Box3f box1)
        {
            var box = box0;
            box.Enlarge(box1);
            return box;
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
        public bool Contains(Vector3f p)
        {
            if (p.x > Max.x || p.x < Min.x) return false;
            if (p.y > Max.y || p.y < Min.y) return false;
            if (p.z > Max.z || p.z < Min.z) return false;
            return true;
        }

        /// <summary>
        /// Returns the closest point on the box.
        /// </summary>
        public Vector3f Closest(Vector3f p)
        {
            Vector3f c;

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

        public static Box3f CalculateBounds(IList<Vector3f> vertices)
        {
            Vector3f min = Vector3f.PositiveInfinity;
            Vector3f max = Vector3f.NegativeInfinity;

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

        public static Box3f CalculateBounds(Vector3f a, Vector3f b)
        {
            float xmin = Math.Min(a.x, b.x);
            float xmax = Math.Max(a.x, b.x);
            float ymin = Math.Min(a.y, b.y);
            float ymax = Math.Max(a.y, b.y);
            float zmin = Math.Min(a.z, b.z);
            float zmax = Math.Max(a.z, b.z);

            return new Box3f(xmin, xmax, ymin, ymax, zmin, zmax);
        }
    }

}
