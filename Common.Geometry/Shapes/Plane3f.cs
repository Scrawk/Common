using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Core.LinearAlgebra;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Plane3f : IEquatable<Plane3f>
    {

        public Vector3f Normal;

        public Vector3f Position;

        public Plane3f(Vector3f position, Vector3f normal)
        {
            Normal = normal;
            Position = position;
        }

        public Plane3f(Vector3f normal, float distance)
        {
            Normal = normal;
            Position = Normal * distance;
        }

        /// <summary>
        /// From three noncollinear points (ordered ccw).
        /// </summary>
        public Plane3f(Vector3f a, Vector3f b, Vector3f c)
        {
            Normal = Vector3f.Cross(b - a, c - a);
            Normal.Normalize();
            Position = Normal * Vector3f.Dot(Normal, a);
        }

        public static bool operator ==(Plane3f p1, Plane3f p2)
        {
            return p1.Position == p2.Position && p1.Normal == p2.Normal;
        }

        public static bool operator !=(Plane3f p1, Plane3f p2)
        {
            return p1.Position != p2.Position || p1.Normal != p2.Normal;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Plane3f)) return false;
            Plane3f plane = (Plane3f)obj;
            return this == plane;
        }

        public bool Equals(Plane3f plane)
        {
            return this == plane;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ Position.GetHashCode();
                hash = (hash * 16777619) ^ Normal.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return string.Format("[Plane3f: Positions{0}, Normal={1}]", Position, Normal);
        }

    }
    
}
