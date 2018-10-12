﻿using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Core.Mathematics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Intersections
{
    public static class RayIntersections2f
    {

        public static bool RayIntersectsRay(Ray2f ray0, Ray2f ray1, out Vector2f point)
        {
            float s, t;
            if(RayIntersectsRay(ray0, ray1, out s,  out t))
            {
                point = ray0.Position + ray0.Direction * s;
                return true;
            }
            else
            {
                point = Vector2f.Zero;
                return false;
            }
        }

        public static bool RayIntersectsRay(Ray2f ray0, Ray2f ray1, out float s, out float t)
        {
            s = t = 0;

            float dx = ray1.Position.x - ray0.Position.x;
            float dy = ray1.Position.y - ray0.Position.y;

            float det = ray1.Direction.x * ray0.Direction.y - ray1.Direction.y * ray0.Direction.x;
            if (FMath.IsZero(det)) return false;

            s = (dy * ray1.Direction.x - dx * ray1.Direction.y) / det;
            t = (dy * ray0.Direction.x - dx * ray0.Direction.y) / det;

            return s > 0 && t > 0;
        }

        public static bool RayIntersectsSegment(Ray2f ray, Segment2f seg, out Vector2f point)
        {
            float s, t;
            if (RayIntersectsSegment(ray, seg, out s, out t))
            {
                point = ray.Position + ray.Direction * s;
                return true;
            }
            else
            {
                point = Vector2f.Zero;
                return false;
            }
        }

        public static bool RayIntersectsSegment(Ray2f ray, Segment2f seg, out float s, out float t)
        {
            s = t = 0;

            float dx = seg.A.x - ray.Position.x;
            float dy = seg.A.y - ray.Position.y;

            float len = Vector2f.Distance(seg.A, seg.B);
            if (FMath.IsZero(len)) return false;

            Vector2f n1;
            n1.x = (seg.B.x - seg.A.x) / len;
            n1.y = (seg.B.y - seg.A.y) / len;

            float det = n1.x * ray.Direction.y - n1.y * ray.Direction.x;
            if (FMath.IsZero(det)) return false;

            s = (dy * n1.x - dx * n1.y) / det;
            t = (dy * ray.Direction.x - dx * ray.Direction.y) / det;
            t /= len;

            return s > 0 && t > 0 && t < 1.0f;
        }

        public static bool RayIntersectsCircle(Ray2f ray, Circle2f circle, out Vector2f point)
        {
            float t;
            if (RayIntersectsCircle(ray, circle, out t))
            {
                point = ray.Position + ray.Direction * t;
                return true;
            }
            else
            {
                point = Vector2f.Zero;
                return false;
            }
        }

        public static bool RayIntersectsCircle(Ray2f ray, Circle2f circle, out float t)
        {
            t = 0;
            Vector2f m = ray.Position - circle.Center;
            float b = Vector2f.Dot(m, ray.Direction);
            float c = Vector2f.Dot(m, m) - circle.Radius2;

            if (c > 0.0f && b > 0.0f) return false;

            float discr = b * b - c;
            if (discr < 0.0f) return false;

            t = -b - (float)Math.Sqrt(discr);

            if (t < 0.0f) t = 0;
            return true;
        }

        public static bool RayIntersectsBox(Ray2f ray, Box2f box, out Vector2f point)
        {
            float t;
            if (RayIntersectsBox(ray, box, out t))
            {
                point = ray.Position + ray.Direction * t;
                return true;
            }
            else
            {
                point = Vector2f.Zero;
                return false;
            }
        }

        public static bool RayIntersectsBox(Ray2f ray, Box2f box, out float t)
        {
            t = 0;
            float tmin = 0;
            float tmax = float.PositiveInfinity;

            for (int i = 0; i < 2; i++)
            {
                if(Math.Abs(ray.Direction[i]) < FMath.EPS)
                {
                    if (ray.Position[i] < box.Min[i] || ray.Position[i] > box.Max[i])
                        return false;
                }
                else
                {
                    float ood = 1.0f / ray.Direction[i];
                    float t1 = (box.Min[i] - ray.Position[i]) * ood;
                    float t2 = (box.Max[i] - ray.Position[i]) * ood;

                    if(t1 > t2)
                    {
                        float tmp = t1;
                        t1 = t2;
                        t2 = tmp;
                    }

                    tmin = Math.Max(tmin, t1);
                    tmax = Math.Min(tmax, t1);

                    if (tmin > tmax) return false;
                }
            }

            t = tmin;
            return true;
        }

    }
}
