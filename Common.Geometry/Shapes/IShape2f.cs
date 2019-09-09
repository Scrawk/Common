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
}
