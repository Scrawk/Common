﻿using System;
using System.Runtime.InteropServices;

using Common.Core.Numerics;

using REAL = System.Single;
using VECTOR2 = Common.Core.Numerics.Vector2f;
using POINT2 = Common.Core.Numerics.Point2f;
using CIRCLE2 = Common.Geometry.Shapes.Circle2f;
using SEGMENT2 = Common.Geometry.Shapes.Segment2f;
using BOX2 = Common.Geometry.Shapes.Box2f;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Ray2f : IEquatable<Ray2f>
    {

        public POINT2 Position;

        public VECTOR2 Direction;

        public Ray2f(POINT2 position, VECTOR2 direction)
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

        /// <summary>
        /// Get the position offset along the ray at t.
        /// </summary>
        /// <param name="t">The amount to offset.</param>
        /// <returns>The position at t.</returns>
        public POINT2 GetPosition(REAL t)
        {
            return Position + t * Direction;
        }

        /// <summary>
        /// Intersection point between two rays.
        /// Presumes there is a intersection.
        /// </summary>
        /// <param name="ray">The other ray.</param>
        /// <returns></returns>
        public POINT2 Intersection(Ray2f ray)
        {
            REAL s, t;
            Intersects(ray, out s, out t);
            return GetPosition(s);
        }

        /// <summary>
        /// Intersection between two rays.
        /// </summary>
        /// <param name="ray">The other ray</param>
        /// <param name="s">Intersection point = Position + s * Direction.</param>
        /// <param name="t">Intersection point = ray.Position + t * ray.Direction.</param>
        /// <returns>If rays intersect</returns>
        public bool Intersects(Ray2f ray, out REAL s, out REAL t)
        {
            s = 0;
            t = 0;
            REAL dx = ray.Position.x - Position.x;
            REAL dy = ray.Position.y - Position.y;

            REAL det = ray.Direction.x * Direction.y - ray.Direction.y * Direction.x;
            if (MathUtil.IsZero(det)) return false;

            s = (dy * ray.Direction.x - dx * ray.Direction.y) / det;
            t = (dy * Direction.x - dx * Direction.y) / det;

            return s > 0 && t > 0;
        }

        /// <summary>
        /// Intersection between ray and segment.
        /// </summary>
        /// <param name="seg">the segment</param>
        /// <param name="s">Intersection point = Position + s * Direction</param>
        /// <param name="t">Intersection point = A + t * (B - A)</param>
        /// <returns>If rays intersect</returns>
        public bool Intersects(SEGMENT2 seg, out REAL s, out REAL t)
        {
            s = t = 0;

            REAL dx = seg.A.x - Position.x;
            REAL dy = seg.A.y - Position.y;

            REAL len = POINT2.Distance(seg.A, seg.B);
            if (MathUtil.IsZero(len)) return false;

            VECTOR2 n1;
            n1.x = (seg.B.x - seg.A.x) / len;
            n1.y = (seg.B.y - seg.A.y) / len;

            REAL det = n1.x * Direction.y - n1.y * Direction.x;
            if (MathUtil.IsZero(det)) return false;

            s = (dy * n1.x - dx * n1.y) / det;
            t = (dy * Direction.x - dx * Direction.y) / det;
            t /= len;

            return s > 0 && t > 0 && t < 1.0;
        }

        /// <summary>
        /// Intersection between ray and circle.
        /// </summary>
        /// <param name="circle">the circle</param>
        /// <param name="t">Intersection point = Position + t * Direction</param>
        /// <returns>If rays intersect</returns>
        public bool Intersects(CIRCLE2 circle, out REAL t)
        {
            t = 0;
            VECTOR2 m = (Position - circle.Center).Vector2f;
            REAL b = VECTOR2.Dot(m, Direction);
            REAL c = VECTOR2.Dot(m, m) - circle.Radius2;

            if (c > 0.0 && b > 0.0) return false;

            REAL discr = b * b - c;
            if (discr < 0.0) return false;

            t = -b - MathUtil.Sqrt(discr);

            if (t < 0.0) t = 0;
            return true;
        }

        /// <summary>
        /// Intersection between ray and box.
        /// </summary>
        /// <param name="box">the box</param>
        /// <param name="t">Intersection point = Position + t * Direction</param>
        /// <returns>If rays intersect</returns>
        public bool Intersects(BOX2 box, out REAL t)
        {
            t = 0;
            REAL tmin = 0;
            REAL tmax = REAL.PositiveInfinity;

            for (int i = 0; i < 2; i++)
            {
                if (MathUtil.IsZero(Direction[i]))
                {
                    if (Position[i] < box.Min[i] || Position[i] > box.Max[i])
                        return false;
                }
                else
                {
                    REAL ood = 1.0f / Direction[i];
                    REAL t1 = (box.Min[i] - Position[i]) * ood;
                    REAL t2 = (box.Max[i] - Position[i]) * ood;

                    if (t1 > t2)
                    {
                        REAL tmp = t1;
                        t1 = t2;
                        t2 = tmp;
                    }

                    tmin = Math.Max(tmin, t1);
                    tmax = Math.Min(tmax, t2);

                    if (tmin > tmax) return false;
                }
            }

            t = tmin;
            return true;
        }

    }
}

