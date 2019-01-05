using System;
using System.Runtime.InteropServices;

using Common.Core.LinearAlgebra;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Ray2d : IEquatable<Ray2d>
    {

        public Vector2d Position;

        public Vector2d Direction;

        public Ray2d(Vector2d position, Vector2d direction)
        {
            Position = position;
            Direction = direction;
        }

        public static bool operator ==(Ray2d r1, Ray2d r2)
        {
            return r1.Position == r2.Position && r1.Direction == r2.Direction;
        }

        public static bool operator !=(Ray2d r1, Ray2d r2)
        {
            return r1.Position != r2.Position || r1.Direction != r2.Direction;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Ray2d)) return false;
            Ray2d ray = (Ray2d)obj;
            return this == ray;
        }

        public bool Equals(Ray2d ray)
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
            return string.Format("[Ray2d: Position={0}, Direction={1}]", Position, Direction);
        }

    }
}

