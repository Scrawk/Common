using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Meshing.Constructors;

namespace Common.Meshing.HalfEdgeBased
{
    public static class HBCreatePolygonalMesh2
    {
        public static HBMesh2d FromBox(Vector2d min, Vector2d max)
        {
            var constructor = new HBMeshConstructor2d();
            CreatePolygonalMesh2.FromBox(constructor, min, max);
            return constructor.PopMesh();
        }

        public static HBMesh2d FromCircle(Vector2d center, double radius, int segments)
        {
            var constructor = new HBMeshConstructor2d();
            CreatePolygonalMesh2.FromCircle(constructor, center, radius, segments);
            return constructor.PopMesh();
        }
    }
}
