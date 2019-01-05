﻿using System;
using System.Runtime.InteropServices;

using Common.Core.LinearAlgebra;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Ray2f : IEquatable<Ray2f>
    {

        public Vector2f Position;

        public Vector2f Direction;

        public Ray2f(Vector2f position, Vector2f direction)
        {
            Position = position;
            Direction = direction;
        }

        public static bool operator ==(Ray2f r1, Ray2f r2)
        {
            return r1.Position == r2.Position && r1.Direction == r2.Direction;
        }

        public static bool operator !=(Ray2f r1, Ray2f r2)
        {
            return r1.Position != r2.Position || r1.Direction != r2.Direction;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Ray2f)) return false;
            Ray2f ray = (Ray2f)obj;
            return this == ray;
        }

        public bool Equals(Ray2f ray)
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
            return string.Format("[Ray2f: Position={0}, Direction={1}]", Position, Direction);
        }

    }
}

