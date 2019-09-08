using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Shapes
{
    public interface IShape3f
    {
        Box3f Bounds { get; }

        bool Contains(Vector3f p);

        Vector3f Closest(Vector3f p);

        float SignedDistance(Vector3f p);
    }
}
