using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Core.LinearAlgebra;
using Common.Core.Mathematics;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Line2f : IEquatable<Line2f>
    {
        public Vector2f Origin;

        public Vector2f Direction;

        public Line2f(Vector2f origin, Vector2f direction)
        {
            Origin = origin;
            Direction = direction;
        }

        public static explicit operator Line2f(Line2d line)
        {
            return new Line2f((Vector2f)line.Origin, (Vector2f)line.Direction);
        }

        public static bool operator ==(Line2f i1, Line2f i2)
        {
            return i1.Origin == i2.Origin && i1.Direction == i2.Direction;
        }

        public static bool operator !=(Line2f i1, Line2f i2)
        {
            return i1.Origin != i2.Origin || i1.Direction != i2.Direction;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Line2f)) return false;
            Line2f line = (Line2f)obj;
            return this == line;
        }

        public bool Equals(Line2f line)
        {
            return this == line;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ Origin.GetHashCode();
                hash = (hash * 16777619) ^ Direction.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return string.Format("[Line2f: Origin={0}, Direction={1}]", Origin, Direction);
        }

        /// <summary>
        /// parameter is distance along Line
        /// </summary>
        public Vector2f PointAt(float d)
        {
            return Origin + d * Direction;
        }

        public float Project(Vector2f p)
        {
            return Vector2f.Dot(p - Origin, Direction);
        }

        public float Distance(Vector2f p)
        {
            return FMath.SafeSqrt(SqrDistance(p));
        }

        public float SqrDistance(Vector2f p)
        {
            float t = Project(p);
            Vector2f proj = Origin + t * Direction;
            return Vector2f.SqrDistance(proj, p);
        }

        public Vector2f ClosestPoint(Vector2f p)
        {
            float t = Project(p);
            return Origin + t * Direction;
        }

        /// <summary>
        /// Returns:
        ///   +1, on right of line
        ///   -1, on left of line
        ///    0, on the line
        /// </summary>
        public int WhichSide(Vector2f test, float tol = 0)
        {
            float x0 = test.x - Origin.x;
            float y0 = test.y - Origin.y;
            float x1 = Direction.x;
            float y1 = Direction.y;
            float det = x0 * y1 - x1 * y0;
            return (det > tol ? +1 : (det < -tol ? -1 : 0));
        }

        /// <summary>
        /// Calculate intersection point between this line and another one.
        /// Returns false if lines are parallel.
        /// </summary>
        public bool IntersectionPoint(Line2f other, out Vector2f point)
        {
            Vector2f diff = other.Origin - Origin;
            float D0DotPerpD1 = Vector2f.Dot(Direction, other.Direction.PerpendicularCW);

            if (Math.Abs(D0DotPerpD1) > FMath.EPS)
            {
                float invD0DotPerpD1 = 1.0f / D0DotPerpD1;
                float diffDotPerpD1 = Vector2f.Dot(diff, other.Direction.PerpendicularCW);
                float s = diffDotPerpD1 * invD0DotPerpD1;
                point = Origin + s * Direction;
                return true;
            }
            else
            {
                // Lines are parallel.
                point = Vector2f.Zero;
                return false;
            }
        }

        public static Line2f FromPoints(Vector2f p0, Vector2f p1)
        {
            return new Line2f(p0, (p1 - p0).Normalized);
        }

    }

}
