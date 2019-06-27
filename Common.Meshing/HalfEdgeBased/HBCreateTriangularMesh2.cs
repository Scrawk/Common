using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Meshing.Constructors;

namespace Common.Meshing.HalfEdgeBased
{
    public static class HBCreateTriangularMesh2
    {
        public static HBMesh2d FromTriangle(Vector2d A, Vector2d B, Vector2d C)
        {
            var constructor = new HBMeshConstructor2d();
            CreateTriangularMesh2.FromTriangle(constructor, A, B, C);
            return constructor.PopMesh();
        }

        public static HBMesh2d FromBox(Vector2d min, Vector2d max)
        {
            var constructor = new HBMeshConstructor2d();
            CreateTriangularMesh2.FromBox(constructor, min, max);
            return constructor.PopMesh();
        }

        public static HBMesh2d FromCircle(Vector2d center, double radius, int segments)
        {
            var constructor = new HBMeshConstructor2d();
            CreateTriangularMesh2.FromCircle(constructor, center, radius, segments);
            return constructor.PopMesh();
        }

        public static HBMesh2d FromGrid(int width, int height, double scale)
        {
            var constructor = new HBMeshConstructor2d();
            CreateTriangularMesh2.FromGrid(constructor, width, height, scale);
            return constructor.PopMesh();
        }
    }
}
