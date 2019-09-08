using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Shapes
{
    public class TriangleShape2f : IShape2f
    {
        public TriangleShape2f(Vector2f a, Vector2f b, Vector2f c)
        {
            Triangle = new Triangle2f(a, b, c);
        }

        public TriangleShape2f(float ax, float ay, float bx, float by, float cx, float cy)
        {
            Triangle = new Triangle2f(ax, ay, bx, by, cx, cy);
        }

        public TriangleShape2f(Triangle2f triangle)
        {
            Triangle = triangle;
        }

        public Triangle2f Triangle;

        public Box2f Bounds => Triangle.Bounds;

        public bool Contains(Vector2f p)
        {
            return Triangle.Contains(p);
        }

        public Vector2f Closest(Vector2f p)
        {
            return Triangle.Closest(p);
        }

        public float SignedDistance(Vector2f p)
        {
            return Triangle.SignedDistance(p);
        }
    }
}
