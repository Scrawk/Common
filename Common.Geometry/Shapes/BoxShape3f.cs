using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Shapes
{
    public class BoxShape3f : IShape3f
    {
        public BoxShape3f(float min, float max)
        {
            Box = new Box3f(min, max);
        }

        public BoxShape3f(float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
        {
            Box = new Box3f(minX, maxX, minY, maxY, minZ, maxZ);
        }

        public BoxShape3f(Vector3f min, Vector3f max)
        {
            Box = new Box3f(min, max);
        }

        public BoxShape3f(Box3f box)
        {
            Box = box;
        }

        public Box3f Box;

        public Box3f Bounds => Box;

        public bool Contains(Vector3f p)
        {
            return Box.Contains(p);
        }

        public bool Intersects(Box3f box)
        {
            return Box.Intersects(box);
        }

        public Vector3f Closest(Vector3f p)
        {
            return Box.Closest(p);
        }

        public float SignedDistance(Vector3f p)
        {
            return Box.SignedDistance(p);
        }
    }
}
