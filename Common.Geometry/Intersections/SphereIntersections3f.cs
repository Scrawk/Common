using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Core.Mathematics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Intersections
{
    public static class SphereIntersections3f
    {

        public static bool SphereContainsPoint(Sphere3f sphere, Vector3f p)
        {
            Vector3f diff = p - sphere.Center;
            return diff.SqrMagnitude < sphere.Radius2;
        }

    }
}
