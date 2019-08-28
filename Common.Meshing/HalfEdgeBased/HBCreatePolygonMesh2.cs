using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Meshing.Constructors;

namespace Common.Meshing.HalfEdgeBased
{
    public static class HBCreatePolygonMesh2
    {
        public static HBMesh2f FromBox(Vector2f min, Vector2f max)
        {
            var constructor = new HBMeshConstructor2f();
            CreatePolygonMesh2.FromBox(constructor, min, max);
            return constructor.PopMesh();
        }

        public static HBMesh2f FromCircle(Vector2f center, float radius, int segments)
        {
            var constructor = new HBMeshConstructor2f();
            CreatePolygonMesh2.FromCircle(constructor, center, radius, segments);
            return constructor.PopMesh();
        }
    }
}
