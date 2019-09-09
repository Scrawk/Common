using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Shapes
{
    public class BoxShape2f : IShape2f
    {
        public BoxShape2f(float min, float max)
        {
            Box = new Box2f(min, max);
        }

        public BoxShape2f(float minX, float maxX, float minY, float maxY)
        {
            Box = new Box2f(minX, maxX, minY, maxY);
        }

        public BoxShape2f(Vector2f min, Vector2f max)
        {
            Box = new Box2f(min, max);
        }

        public BoxShape2f(Box2f box)
        {
            Box = box;
        }

        public Box2f Box;

        public Box2f Bounds => Box;

        public bool Contains(Vector2f p)
        {
            return Box.Contains(p);
        }

        public bool Intersects(Box2f box)
        {
            return Box.Intersects(box);
        }

        public Vector2f Closest(Vector2f p)
        {
            return Box.Closest(p);
        }

        public float SignedDistance(Vector2f p)
        {
            return Box.SignedDistance(p);
        }
    }
}
