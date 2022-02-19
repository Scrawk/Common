﻿using System;
using System.Runtime.InteropServices;

using Common.Core.Numerics;

using REAL = System.Single;
using POINT3 = Common.Core.Numerics.Point3f;
using VECTOR3 = Common.Core.Numerics.Vector3f;
using MATRIX3 = Common.Core.Numerics.Matrix3x3f;
using MATRIX4 = Common.Core.Numerics.Matrix4x4f;

namespace Common.Core.Shapes
{
    /// <summary>
    /// A 3D ray struct represented by a position and a direction.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Ray3f : IEquatable<Ray3f>
    {
        /// <summary>
        /// The rays position.
        /// </summary>
        public POINT3 Position;

        /// <summary>
        /// The rays direction.
        /// Might not be normalized.
        /// </summary>
        public VECTOR3 Direction;

        /// <summary>
        /// Construct a ray from a point and the direction.
        /// </summary>
        /// <param name="position">The rays position.</param>
        /// <param name="direction">The rays direction (will be normalized)</param>
        public Ray3f(POINT3 position, VECTOR3 direction)
        {
            Position = position;
            Direction = direction.Normalized;
        }

        public static Ray3f operator *(MATRIX3 m, Ray3f ray)
        {
            return new Ray3f(m * ray.Position, m * ray.Direction);
        }

        public static Ray3f operator *(MATRIX4 m, Ray3f ray)
        {
            return new Ray3f(m * ray.Position, m * ray.Direction);
        }

        public static explicit operator Ray3f(Ray3d ray)
        {
            return new Ray3f((POINT3)ray.Position, (VECTOR3)ray.Direction);
        }

        /// <summary>
        /// Check if the two rays are equal.
        /// </summary>
        /// <param name="r1">The first ray.</param>
        /// <param name="r2">The second ray.</param>
        /// <returns>True if the two rays are equal.</returns>
        public static bool operator ==(Ray3f r1, Ray3f r2)
        {
            return r1.Position == r2.Position && r1.Direction == r2.Direction;
        }

        /// <summary>
        /// Check if the two rays are not equal.
        /// </summary>
        /// <param name="r1">The first ray.</param>
        /// <param name="r2">The second ray.</param>
        /// <returns>True if the two rays are not equal.</returns>
        public static bool operator !=(Ray3f r1, Ray3f r2)
        {
            return r1.Position != r2.Position || r1.Direction != r2.Direction;
        }

        /// <summary>
        /// Is the ray equal to this object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>Is the ray equal to this object.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Ray3f)) return false;
            Ray3f ray = (Ray3f)obj;
            return this == ray;
        }

        /// <summary>
        /// Is the ray equal to the other ray.
        /// </summary>
        /// <param name="ray">The over ray.</param>
        /// <returns>Is the ray equal to the other ray.</returns>
        public bool Equals(Ray3f ray)
        {
            return this == ray;
        }

        /// <summary>
        /// The rays hashcode.
        /// </summary>
        /// <returns>The rays hashcode.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)MathUtil.HASH_PRIME_1;
                hash = (hash * MathUtil.HASH_PRIME_2) ^ Position.GetHashCode();
                hash = (hash * MathUtil.HASH_PRIME_2) ^ Direction.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// The rays as a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[Ray3f: Position={0}, Direction={1}]", Position, Direction);
        }

        /// <summary>
        /// The rays directions magnidute.
        /// </summary>
        public REAL Magnitude => Direction.Magnitude;

        /// <summary>
        /// The rays directions square magnidute.
        /// </summary>
        public REAL SqrMagnitude => Direction.SqrMagnitude;

        /// <summary>
        /// Get the position offset along the ray at t.
        /// </summary>
        /// <param name="t">The amount to offset.</param>
        /// <returns>The position at t.</returns>
        public POINT3 GetPosition(REAL t)
        {
            return Position + (t * Direction);
        }

        /// <summary>
        /// Normalize the lines direction.
        /// </summary>
        public void Normalize()
        {
            Direction.Normalize();
        }

        /// <summary>
        /// Round the rays position and direction.
        /// </summary>
        /// <param name="digits">number of digits to round to.</param>
        public void Round(int digits)
        {
            Position = Position.Rounded(digits);
            Direction = Direction.Rounded(digits);
        }
    }
}

