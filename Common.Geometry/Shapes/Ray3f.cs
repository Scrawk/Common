using System;
using System.Runtime.InteropServices;

using Common.Core.LinearAlgebra;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Ray3f : IEquatable<Ray3f>
    {

        public Vector3f Position;

        public Vector3f Direction;

        public Ray3f(Vector3f position, Vector3f direction)
        {
            Position = position;
            Direction = direction;
        }

        public static bool operator ==(Ray3f r1, Ray3f r2)
        {
            return r1.Position == r2.Position && r1.Direction == r2.Direction;
        }

        public static bool operator !=(Ray3f r1, Ray3f r2)
        {
            return r1.Position != r2.Position || r1.Direction != r2.Direction;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Ray3f)) return false;
            Ray3f ray = (Ray3f)obj;
            return this == ray;
        }

        public bool Equals(Ray3f ray)
        {
            return this == ray;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ Position.GetHashCode();
                hash = (hash * 16777619) ^ Direction.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return string.Format("[Ray3f: Position={0}, Direction={1}]", Position, Direction);
        }

    }
}

