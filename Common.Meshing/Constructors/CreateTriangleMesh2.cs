using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Core.Mathematics;
using Common.Meshing.HalfEdgeBased;

namespace Common.Meshing.Constructors
{

    /// <summary>
    /// Create triangle meshes via mesh constructor.
    /// All triangles are CCW.
    /// </summary>
    public static class CreateTriangleMesh2
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

        public static void FromTriangle<MESH>(ITriangularMeshConstructor<MESH> constructor, Vector2d A, Vector2d B, Vector2d C)
        {
            constructor.PushTriangularMesh(3, 1);

            constructor.AddVertex(A);
            constructor.AddVertex(B);
            constructor.AddVertex(C);
            constructor.AddFace(0, 1, 2);
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

        public static void FromBox<MESH>(ITriangularMeshConstructor<MESH> constructor, Vector2d min, Vector2d max)
        {
            constructor.PushTriangularMesh(4, 2);

            constructor.AddVertex(min);
            constructor.AddVertex(new Vector2d(max.x, min.y));
            constructor.AddVertex(max);
            constructor.AddVertex(new Vector2d(min.x, max.y));

            constructor.AddFace(0, 1, 2);
            constructor.AddFace(2, 3, 0);

            if (constructor.SupportsFaceConnections)
            {
                constructor.AddFaceConnection(0, 1, -1, -1);
                constructor.AddFaceConnection(1, 0, -1, -1);
            }
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

        public static void FromCircle<MESH>(ITriangularMeshConstructor<MESH> constructor, Vector2d center, double radius, int segments)
        {
            constructor.PushTriangularMesh(segments + 1, segments * 3);
            constructor.AddVertex(center);

            double pi = Math.PI;
            double fseg = segments;

            for (int i = 0; i < segments; i++)
            {
                double theta = 2.0f * pi * i / fseg;

                double x = -radius * Math.Cos(theta);
                double y = -radius * Math.Sin(theta);
                constructor.AddVertex(center + new Vector2d(x, y));
            }

            for (int i = 0; i < segments; i++)
            {
                int i0 = 0;
                int i1 = i + 1;
                int i2 = IMath.Wrap(i + 1, segments) + 1;
                constructor.AddFace(i0, i1, i2);
            }

            if (constructor.SupportsFaceConnections)
            {
                for (int i = 0; i < segments; i++)
                {
                    int i0 = IMath.Wrap(i - 1, segments);
                    int i1 = -1;
                    int i2 = IMath.Wrap(i + 1, segments);
                    constructor.AddFaceConnection(i, i0, i1, i2);
                }
            }

        }

    }
}
