using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Core.Numerics;

using REAL = System.Single;
using VECTOR2 = Common.Core.Numerics.Vector2d;
using VECTOR3 = Common.Core.Numerics.Vector3f;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Plane3f : IEquatable<Plane3f>
    {

        public VECTOR3 Normal;

        public VECTOR3 Position;

        public Plane3f(VECTOR3 position, VECTOR3 normal)
        {
            Normal = normal;
            Position = position;
        }

        public Plane3f(VECTOR3 normal, REAL distance)
        {
            Normal = normal;
            Position = Normal * distance;
        }

        /// <summary>
        /// From three noncollinear points (ordered ccw).
        /// </summary>
        public Plane3f(VECTOR3 a, VECTOR3 b, VECTOR3 c)
        {
            Normal = VECTOR3.Cross(b - a, c - a);
            Normal.Normalize();
            Position = Normal * VECTOR3.Dot(Normal, a);
        }

        public REAL Distance
        {
            get { return Position.Magnitude; }
        }

        public REAL SqrDistance
        {
            get { return Position.SqrMagnitude; }
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

        public VECTOR3 Closest(VECTOR3 p)
        {
            REAL t = VECTOR3.Dot(Normal, p) - Distance;
            return p - t * Normal;
        }

    }

}
