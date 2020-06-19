using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Shapes
{
    public interface IShape2d
    {
        Box2d Bounds { get; }

        bool Contains(Vector2d p);

        bool Intersects(Box2d box);

        Vector2d Closest(Vector2d p);

        double SignedDistance(Vector2d p);
    }

    public abstract class Shape2d : IShape2d
    {
        public abstract Box2d Bounds { get; }

        public Vector2d Closest(Vector2d p)
        {
            throw new NotImplementedException();
        }

        public bool Contains(Vector2d p)
        {
            throw new NotImplementedException();
        }

        public bool Intersects(Box2d box)
        {
            throw new NotImplementedException();
        }

        public double SignedDistance(Vector2d p)
        {
            throw new NotImplementedException();
        }
    }
}
