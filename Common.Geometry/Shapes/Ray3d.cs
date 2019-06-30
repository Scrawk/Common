using System;
using System.Runtime.InteropServices;

using Common.Core.Numerics;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Ray3d : IEquatable<Ray3d>
    {

        public Vector3f Position;

        public Vector3f Direction;

        public Ray3d(Vector3f position, Vector3f direction)
        {
            Position = position;
            Direction = direction;
        }

        public static bool operator ==(Ray3d r1, Ray3d r2)
        {
            return r1.Position == r2.Position && r1.Direction == r2.Direction;
        }

        public static bool operator !=(Ray3d r1, Ray3d r2)
        {
            return r1.Position != r2.Position || r1.Direction != r2.Direction;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Ray3d)) return false;
            Ray3d ray = (Ray3d)obj;
            return this == ray;
        }

        public bool Equals(Ray3d ray)
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
            return string.Format("[Ray3d: Position={0}, Direction={1}]", Position, Direction);
        }

        /// <summary>
        /// Intersection between ray and sphere.
        /// </summary>
        /// <param name="sphere">the sphere</param>
        /// <param name="t">Intersection point = Position + t * Direction</param>
        /// <returns>If rays intersect</returns>
        public bool Intersects(Sphere3d sphere, out double t)
        {
            t = 0;
            Vector3d m = Position - sphere.Center;

            double b = Vector3d.Dot(m, Direction);
            double c = Vector3d.Dot(m, m) - sphere.Radius2;

            if (c > 0.0f && b > 0.0f) return false;

            double discr = b * b - c;
            if (discr < 0.0f) return false;

            t = -b - Math.Sqrt(discr);

            if (t < 0) t = 0;
            return true;
        }

    }
}

