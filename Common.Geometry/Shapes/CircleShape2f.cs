using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Shapes
{
    public class CircleShape2f : IShape2f
    {
        public CircleShape2f(Vector2f centre, float radius)
        {
            Circle = new Circle2f(centre, radius);
        }

        public CircleShape2f(float x, float y, float radius)
        {
            Circle = new Circle2f(x, y, radius);
        }

        public CircleShape2f(Circle2f circle)
        {
            Circle = circle;
        }

        public Circle2f Circle;

        public Box2f Bounds => Circle.Bounds;

        public bool Contains(Vector2f p)
        {
            return Circle.Contains(p);
        }

        public bool Intersects(Box2f box)
        {
            return Circle.Intersects(box);
        }

        public Vector2f Closest(Vector2f p)
        {
            return Circle.Closest(p);
        }

        public float SignedDistance(Vector2f p)
        {
            return Circle.SignedDistance(p);
        }
    }
}
