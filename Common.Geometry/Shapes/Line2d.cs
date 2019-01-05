using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Core.LinearAlgebra;
using Common.Core.Mathematics;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Line2d : IEquatable<Line2d>
    {
        public Vector2d Origin;

        public Vector2d Direction;

        public Line2d(Vector2d origin, Vector2d direction)
        {
            Origin = origin;
            Direction = direction;
        }

        public static implicit operator Line2d(Line2f line)
        {
            return new Line2d(line.Origin, line.Direction);
        }

        public static bool operator ==(Line2d i1, Line2d i2)
        {
            return i1.Origin == i2.Origin && i1.Direction == i2.Direction;
        }

        public static bool operator !=(Line2d i1, Line2d i2)
        {
            return i1.Origin != i2.Origin || i1.Direction != i2.Direction;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Line2d)) return false;
            Line2d line = (Line2d)obj;
            return this == line;
        }

        public bool Equals(Line2d line)
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
            return string.Format("[Line2d: Origin={0}, Direction={1}]", Origin, Direction);
        }

        /// <summary>
        /// parameter is distance along Line
        /// </summary>
        public Vector2d PointAt(double d)
        {
            return Origin + d * Direction;
        }

        public double Project(Vector2d p)
        {
            return Vector2d.Dot(p - Origin, Direction);
        }

        public double Distance(Vector2d p)
        {
            return DMath.SafeSqrt(SqrDistance(p));
        }

        public double SqrDistance(Vector2d p)
        {
            double t = Project(p);
            Vector2d proj = Origin + t * Direction;
            return Vector2d.SqrDistance(proj, p);
        }

        public Vector2d ClosestPoint(Vector2d p)
        {
            double t = Project(p);
            return Origin + t * Direction;
        }

        /// <summary>
        /// Returns:
        ///   +1, on right of line
        ///   -1, on left of line
        ///    0, on the line
        /// </summary>
        public int WhichSide(Vector2d test, double tol = 0)
        {
            double x0 = test.x - Origin.x;
            double y0 = test.y - Origin.y;
            double x1 = Direction.x;
            double y1 = Direction.y;
            double det = x0 * y1 - x1 * y0;
            return (det > tol ? +1 : (det < -tol ? -1 : 0));
        }

        /// <summary>
        /// Calculate intersection point between this line and another one.
        /// Returns false if lines are parallel.
        /// </summary>
        public bool IntersectionPoint(Line2d other, out Vector2d point)
        {
            Vector2d diff = other.Origin - Origin;
            double D0DotPerpD1 = Vector2d.Dot(Direction, other.Direction.PerpendicularCW);

            if (Math.Abs(D0DotPerpD1) > DMath.EPS)
            {
                double invD0DotPerpD1 = 1.0 / D0DotPerpD1;
                double diffDotPerpD1 = Vector2d.Dot(diff, other.Direction.PerpendicularCW);
                double s = diffDotPerpD1 * invD0DotPerpD1;
                point = Origin + s * Direction;
                return true;
            }
            else
            {
                // Lines are parallel.
                point = Vector2d.Zero;
                return false;
            }
        }

        public static Line2d FromPoints(Vector2d p0, Vector2d p1)
        {
            return new Line2d(p0, (p1 - p0).Normalized);
        }

    }

}
