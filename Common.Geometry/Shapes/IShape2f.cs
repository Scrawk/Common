using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Shapes
{
    public interface IShape2f
    {
        Box2f Bounds { get; }

        bool Contains(Vector2f p);

        bool Intersects(Box2f box);

        Vector2f Closest(Vector2f p);

        float SignedDistance(Vector2f p);
    }

    public abstract class Shape2f : IShape2f
    {
        public abstract Box2f Bounds { get; }

        public Vector2f Closest(Vector2f p)
        {
            throw new NotImplementedException();
        }

        public bool Contains(Vector2f p)
        {
            throw new NotImplementedException();
        }

        public bool Intersects(Box2f box)
        {
            throw new NotImplementedException();
        }

        public float SignedDistance(Vector2f p)
        {
            throw new NotImplementedException();
        }
    }
}
