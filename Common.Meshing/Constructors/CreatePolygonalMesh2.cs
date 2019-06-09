using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Meshing.HalfEdgeBased;

namespace Common.Meshing.Constructors
{
    /// <summary>
    /// Create general meshes via mesh constructor.
    /// All faces are CCW.
    /// </summary>
    public static class CreatePolygonalMesh2
    {

        public static HBMesh2f FromTriangle(Vector2f A, Vector2f B, Vector2f C)
        {
            var constructor = new HBMeshConstructor2f();
            FromTriangle(constructor, A, B, C);
            return constructor.PopMesh();
        }

        public static HBMesh2d FromTriangle(Vector2d A, Vector2d B, Vector2d C)
        {
            var constructor = new HBMeshConstructor2d();
            FromTriangle(constructor, A, B, C);
            return constructor.PopMesh();
        }

        public static void FromTriangle<MESH>(IPolygonalMeshConstructor<MESH> constructor, Vector2d A, Vector2d B, Vector2d C)
        {
            constructor.PushPolygonalMesh(3, 1);

            constructor.AddVertex(A);
            constructor.AddVertex(B);
            constructor.AddVertex(C);
            constructor.AddFace(0, 3);
        }

        public static HBMesh2f FromBox(Vector2f min, Vector2f max)
        {
            var constructor = new HBMeshConstructor2f();
            FromBox(constructor, min, max);
            return constructor.PopMesh();
        }

        public static HBMesh2d FromBox(Vector2d min, Vector2d max)
        {
            var constructor = new HBMeshConstructor2d();
            FromBox(constructor, min, max);
            return constructor.PopMesh();
        }

        public static void FromBox<MESH>(IPolygonalMeshConstructor<MESH> constructor, Vector2d min, Vector2d max)
        {
            constructor.PushPolygonalMesh(4, 1);

            constructor.AddVertex(min);
            constructor.AddVertex(new Vector2d(max.x, min.y));
            constructor.AddVertex(max);
            constructor.AddVertex(new Vector2d(min.x, max.y));

            constructor.AddFace(0, 4);
        }

        public static HBMesh2f FromCircle(Vector2f center, float radius, int segments)
        {
            var constructor = new HBMeshConstructor2f();
            FromCircle(constructor, center, radius, segments);
            return constructor.PopMesh();
        }

        public static HBMesh2d FromCircle(Vector2d center, double radius, int segments)
        {
            var constructor = new HBMeshConstructor2d();
            FromCircle(constructor, center, radius, segments);
            return constructor.PopMesh();
        }

        public static void FromCircle<MESH>(IPolygonalMeshConstructor<MESH> constructor, Vector2d center, double radius, int segments)
        {
            constructor.PushPolygonalMesh(segments, 1);

            double pi = Math.PI;
            double fseg = segments;

            for (int i = 0; i < segments; i++)
            {
                double theta = 2.0f * pi * i / fseg;

                double x = -radius * Math.Cos(theta);
                double y = -radius * Math.Sin(theta);

                constructor.AddVertex(center + new Vector2d(x, y));
            }

            constructor.AddFace(0, segments);
        }

    }
}
