using System;
using System.Runtime.InteropServices;

using Common.Core.Numerics;

using REAL = System.Single;
using VECTOR3 = Common.Core.Numerics.Vector3f;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Ray3f : IEquatable<Ray3f>
    {

        public VECTOR3 Position;

        public VECTOR3 Direction;

        public Ray3f(VECTOR3 position, VECTOR3 direction)
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

        /// <summary>
        /// Intersection between ray and sphere.
        /// </summary>
        /// <param name="sphere">the sphere</param>
        /// <param name="t">Intersection point = Position + t * Direction</param>
        /// <returns>If rays intersect</returns>
        public bool Intersects(Sphere3f sphere, out REAL t)
        {
            t = 0;
            VECTOR3 m = Position - sphere.Center;

            REAL b = VECTOR3.Dot(m, Direction);
            REAL c = VECTOR3.Dot(m, m) - sphere.Radius2;

            if (c > 0.0 && b > 0.0) return false;

            REAL discr = b * b - c;
            if (discr < 0.0) return false;

            t = -b - FMath.Sqrt(discr);

            if (t < 0) t = 0;
            return true;
        }

    }
}

