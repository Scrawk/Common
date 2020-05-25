using System;
using System.Runtime.InteropServices;

using Common.Core.Numerics;

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

        /// <summary>
        /// Intersection between two rays.
        /// </summary>
        /// <param name="ray">other ray</param>
        /// <param name="s">Intersection point = Position + s * Direction</param>
        /// <param name="t">Intersection point = ray.Position + t * ray.Direction</param>
        /// <returns>If rays intersect</returns>
        public bool Intersects(Ray2f ray, out float s, out float t)
        {
            s = 0;
            t = 0;
            float dx = ray.Position.x - Position.x;
            float dy = ray.Position.y - Position.y;

            float det = Direction.x * Direction.y - ray.Direction.y * ray.Direction.x;
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
        public bool Intersects(Segment2f seg, out float s, out float t)
        {
            s = t = 0;

            float dx = seg.A.x - Position.x;
            float dy = seg.A.y - Position.y;

            float len = Vector2f.Distance(seg.A, seg.B);
            if (MathUtil.IsZero(len)) return false;

            Vector2f n1;
            n1.x = (seg.B.x - seg.A.x) / len;
            n1.y = (seg.B.y - seg.A.y) / len;

            float det = n1.x * Direction.y - n1.y * Direction.x;
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
        public bool Intersects(Circle2f circle, out float t)
        {
            t = 0;
            Vector2f m = Position - circle.Center;
            float b = Vector2f.Dot(m, Direction);
            float c = Vector2f.Dot(m, m) - circle.Radius2;

            if (c > 0.0 && b > 0.0) return false;

            float discr = b * b - c;
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
        public bool Intersects(Box2f box, out float t)
        {
            t = 0;
            float tmin = 0;
            float tmax = float.PositiveInfinity;

            for (int i = 0; i < 2; i++)
            {
                if (MathUtil.IsZero(Direction[i]))
                {
                    if (Position[i] < box.Min[i] || Position[i] > box.Max[i])
                        return false;
                }
                else
                {
                    float ood = 1.0f / Direction[i];
                    float t1 = (box.Min[i] - Position[i]) * ood;
                    float t2 = (box.Max[i] - Position[i]) * ood;

                    if (t1 > t2)
                    {
                        float tmp = t1;
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

