using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
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

        public static void FromTriangle<MESH>(ITriangleMeshConstructor<MESH> constructor, Vector2f A, Vector2f B, Vector2f C)
        {
            constructor.PushTriangleMesh(3, 1);

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

        public static void FromBox<MESH>(ITriangleMeshConstructor<MESH> constructor, Vector2f min, Vector2f max)
        {
            constructor.PushTriangleMesh(4, 2);

            constructor.AddVertex(min);
            constructor.AddVertex(new Vector2f(max.x, min.y));
            constructor.AddVertex(max);
            constructor.AddVertex(new Vector2f(min.x, max.y));

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

        public static void FromCircle<MESH>(ITriangleMeshConstructor<MESH> constructor, Vector2f center, float radius, int segments)
        {
            constructor.PushTriangleMesh(segments + 1, segments * 3);
            constructor.AddVertex(center);

            float pi = (float)Math.PI;
            float fseg = segments;

            for (int i = 0; i < segments; i++)
            {
                float theta = 2.0f * pi * i / fseg;

                float x = -radius * (float)Math.Cos(theta);
                float y = -radius * (float)Math.Sin(theta);
                constructor.AddVertex(center + new Vector2f(x, y));
            }

            for (int i = 0; i < segments; i++)
            {
                int i0 = 0;
                int i1 = i + 1;
                int i2 = (i + 1) % segments + 1;
                constructor.AddFace(i0, i1, i2);
            }

            if (constructor.SupportsFaceConnections)
            {
                for (int i = 0; i < segments; i++)
                {
                    int i0 = mod(i - 1, segments);
                    int i1 = -1;
                    int i2 = (i + 1) % segments;
                    constructor.AddFaceConnection(i, i0, i1, i2);
                }
            }

        }

        private static int mod(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }
    }
}
