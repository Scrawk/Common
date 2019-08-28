using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Meshing.Constructors;

namespace Common.Meshing.HalfEdgeBased
{
    public static class HBCreateTriangleMesh2
    {
        public static HBMesh2f FromTriangle(Vector2f A, Vector2f B, Vector2f C)
        {
            var constructor = new HBMeshConstructor2f();
            CreateTriangleMesh2.FromTriangle(constructor, A, B, C);
            return constructor.PopMesh();
        }

        public static HBMesh2f FromBox(Vector2f min, Vector2f max)
        {
            var constructor = new HBMeshConstructor2f();
            CreateTriangleMesh2.FromBox(constructor, min, max);
            return constructor.PopMesh();
        }

        public static HBMesh2f FromCircle(Vector2f center, float radius, int segments)
        {
            var constructor = new HBMeshConstructor2f();
            CreateTriangleMesh2.FromCircle(constructor, center, radius, segments);
            return constructor.PopMesh();
        }

        public static HBMesh2f FromGrid(int width, int height, float scale)
        {
            var constructor = new HBMeshConstructor2f();
            CreateTriangleMesh2.FromGrid(constructor, width, height, scale);
            return constructor.PopMesh();
        }
    }
}
