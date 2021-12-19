﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Core.Numerics;

using REAL = System.Double;
using VECTOR3 = Common.Core.Numerics.Vector3d;
using VECTOR4 = Common.Core.Numerics.Vector4d;
using MATRIX3 = Common.Core.Numerics.Matrix3x3d;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Box3d : IEquatable<Box3d>, IShape3d
    {

        public VECTOR3 Min;

        public VECTOR3 Max;

        public Box3d(REAL min, REAL max)
        {
            Min = new VECTOR3(min);
            Max = new VECTOR3(max);
        }

        public Box3d(REAL minX, REAL maxX, REAL minY, REAL maxY, REAL minZ, REAL maxZ)
        {
            Min = new VECTOR3(minX, minY, minZ);
            Max = new VECTOR3(maxX, maxY, maxZ);
        }

        public Box3d(VECTOR3 min, VECTOR3 max)
        {
            Min = min;
            Max = max;
        }

        public VECTOR3 Center
        {
            get { return (Min + Max) * 0.5; }
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
                return 2.0 * (d.x * d.y + d.x * d.z + d.y * d.z);
            }
        }

        public Box3d Bounds => this;

        public static Box3d operator +(Box3d box, REAL s)
        {
            return new Box3d(box.Min + s, box.Max + s);
        }

        public static Box3d operator +(Box3d box, VECTOR3 v)
        {
            return new Box3d(box.Min + v, box.Max + v);
        }

        public static Box3d operator -(Box3d box, REAL s)
        {
            return new Box3d(box.Min - s, box.Max - s);
        }

        public static Box3d operator -(Box3d box, VECTOR3 v)
        {
            return new Box3d(box.Min - v, box.Max - v);
        }

        public static Box3d operator *(Box3d box, REAL s)
        {
            return new Box3d(box.Min * s, box.Max * s);
        }

        public static Box3d operator /(Box3d box, REAL s)
        {
            return new Box3d(box.Min / s, box.Max / s);
        }

        public static Box3d operator *(Box3d box, MATRIX3 m)
        {
            return new Box3d(m * box.Min, m * box.Max);
        }

        public static implicit operator Box3d(Box3f box)
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

        public void GetCorners(IList<VECTOR4> corners)
        {
            corners[0] = new VECTOR4(Min.x, Min.y, Min.z, 1);
            corners[1] = new VECTOR4(Max.x, Min.y, Min.z, 1);
            corners[2] = new VECTOR4(Max.x, Min.y, Max.z, 1);
            corners[3] = new VECTOR4(Min.x, Min.y, Max.z, 1);

            corners[4] = new VECTOR4(Min.x, Max.y, Min.z, 1);
            corners[5] = new VECTOR4(Max.x, Max.y, Min.z, 1);
            corners[6] = new VECTOR4(Max.x, Max.y, Max.z, 1);
            corners[7] = new VECTOR4(Min.x, Max.y, Max.z, 1);
        }

        /// <summary>
        /// Returns the bounding box containing this box and the given point.
        /// </summary>
        public static Box3d Enlarge(Box3d box, VECTOR3 p)
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
        /// Return a new box expanded by the amount.
        /// </summary>
        /// <param name="box">The box to expand.</param>
        /// <param name="amount">The amount to expand.</param>
        /// <returns>The expanded box.</returns>
        public static Box3d Expand(Box3d box, REAL amount)
        {
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
        public bool Contains(VECTOR3 p)
        {
            if (p.x > Max.x || p.x < Min.x) return false;
            if (p.y > Max.y || p.y < Min.y) return false;
            if (p.z > Max.z || p.z < Min.z) return false;
            return true;
        }

        /// <summary>
        /// Find the closest point to the box.
        /// If point inside box return point.
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

        /// <summary>
        /// Return the signed distance to the point. 
        /// If point is outside box field is positive.
        /// If point is inside box field is negative.
        /// </summary>
        public REAL SignedDistance(VECTOR3 p)
        {
            VECTOR3 d = (p - Center).Absolute - Size * 0.5;
            VECTOR3 max = VECTOR3.Max(d, 0);
            return max.Magnitude + Math.Min(MathUtil.Max(d.x, d.y, d.z), 0);
        }

        public static Box3d CalculateBounds(IList<VECTOR3> vertices)
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

            return new Box3d(min, max);
        }

        public static Box3d CalculateBounds(VECTOR3 a, VECTOR3 b)
        {
            REAL xmin = Math.Min(a.x, b.x);
            REAL xmax = Math.Max(a.x, b.x);
            REAL ymin = Math.Min(a.y, b.y);
            REAL ymax = Math.Max(a.y, b.y);
            REAL zmin = Math.Min(a.z, b.z);
            REAL zmax = Math.Max(a.z, b.z);

            return new Box3d(xmin, xmax, ymin, ymax, zmin, zmax);
        }
    }

}




















