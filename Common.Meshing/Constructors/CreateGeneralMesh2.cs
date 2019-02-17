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
    public static class CreateGeneralMesh2
    {

        public static HBMesh2f FromTriangle(Vector2f A, Vector2f B, Vector2f C)
        {
            var constructor = new HBMeshConstructor2f();
            FromTriangle(constructor, A, B, C);
            return constructor.PopMesh();
        }

        public static void FromTriangle<MESH>(IPolygonMeshConstructor<MESH> constructor, Vector2f A, Vector2f B, Vector2f C)
        {
            constructor.PushPolygonMesh(3, 1);

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

        public static void FromBox<MESH>(IPolygonMeshConstructor<MESH> constructor, Vector2f min, Vector2f max)
        {
            constructor.PushPolygonMesh(4, 1);

            constructor.AddVertex(min);
            constructor.AddVertex(new Vector2f(max.x, min.y));
            constructor.AddVertex(max);
            constructor.AddVertex(new Vector2f(min.x, max.y));

            constructor.AddFace(0, 4);
        }

        public static HBMesh2f FromCircle(Vector2f center, float radius, int segments)
        {
            var constructor = new HBMeshConstructor2f();
            FromCircle(constructor, center, radius, segments);
            return constructor.PopMesh();
        }

        public static void FromCircle<MESH>(IPolygonMeshConstructor<MESH> constructor, Vector2f center, float radius, int segments)
        {
            constructor.PushPolygonMesh(segments, 1);

            float pi = (float)Math.PI;
            float fseg = segments;

            for (int i = 0; i < segments; i++)
            {
                float theta = 2.0f * pi * i / fseg;

                float x = -radius * (float)Math.Cos(theta);
                float y = -radius * (float)Math.Sin(theta);

                constructor.AddVertex(center + new Vector2f(x, y));
            }

            constructor.AddFace(0, segments);
        }

    }
}
