using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Shapes
{
    public class SegmentShape2f : IShape2f
    {
        public SegmentShape2f(Vector2f a, Vector2f b)
        {
            Segment = new Segment2f(a, b);
        }

        public SegmentShape2f(float ax, float ay, float bx, float by)
        {
            Segment = new Segment2f(ax, ay, bx, by);
        }

        public SegmentShape2f(Segment2f segment)
        {
            Segment = segment;
        }

        public Segment2f Segment;

        public Box2f Bounds => Segment.Bounds;

        public bool Contains(Vector2f p)
        {
            return false;
        }

        public bool Intersects(Box2f box)
        {
            return Segment.Intersects(box);
        }

        public Vector2f Closest(Vector2f p)
        {
            return Segment.Closest(p);
        }

        public float SignedDistance(Vector2f p)
        {
            return Segment.SignedDistance(p);
        }
    }
}
