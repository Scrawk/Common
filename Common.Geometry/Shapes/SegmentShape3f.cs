using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Shapes
{
    public class SegmentShape3f : IShape3f
    {
        public SegmentShape3f(Vector3f a, Vector3f b)
        {
            Segment = new Segment3f(a, b);
        }

        public SegmentShape3f(float ax, float ay, float az, float bx, float by, float bz)
        {
            Segment = new Segment3f(ax, ay, az, bx, by, bz);
        }

        public SegmentShape3f(Segment3f sphere)
        {
            Segment = sphere;
        }

        public Segment3f Segment;

        public Box3f Bounds => Segment.Bounds;

        public bool Contains(Vector3f p)
        {
            return false;
        }

        public bool Intersects(Box3f box)
        {
            return Segment.Intersects(box);
        }

        public Vector3f Closest(Vector3f p)
        {
            return Segment.Closest(p);
        }

        public float SignedDistance(Vector3f p)
        {
            return Segment.SignedDistance(p);
        }
    }
}
