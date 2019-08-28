using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Meshing.Constructors
{

    /// <summary>
    /// Create triangle meshes via mesh constructor.
    /// All triangles are CCW.
    /// </summary>
    public static class CreateTriangleMesh2
    {

        public static void FromTriangle<MESH>(ITriangleMeshConstructor<MESH> constructor, Vector2f A, Vector2f B, Vector2f C)
        {
            constructor.PushTriangleMesh(3, 1);

            constructor.AddVertex(A);
            constructor.AddVertex(B);
            constructor.AddVertex(C);
            constructor.AddFace(0, 1, 2);
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

        public static void FromCircle<MESH>(ITriangleMeshConstructor<MESH> constructor, Vector2f center, float radius, int segments)
        {
            constructor.PushTriangleMesh(segments + 1, segments * 3);
            constructor.AddVertex(center);

            float pi = FMath.PI;
            float fseg = segments;

            for (int i = 0; i < segments; i++)
            {
                float theta = 2.0f * pi * i / fseg;

                float x = -radius * FMath.Cos(theta);
                float y = -radius * FMath.Sin(theta);
                constructor.AddVertex(center + new Vector2f(x, y));
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

        public static void FromGrid<MESH>(ITriangleMeshConstructor<MESH> constructor, int width, int height, float scale)
        {

            int numVerts = width * height;
            int numFaces = (width-1) * (height-1) * 2;
            int width1 = width - 1;
            int height1 = height - 1;

            constructor.PushTriangleMesh(numVerts, numFaces);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                    constructor.AddVertex(new Vector2f(x, y) * scale);
            }

            int idx = 0;
            var faces = new int[width1 * height1, 2];

            for (int y = 0; y < height1; y++)
            {
                for (int x = 0; x < width1; x++)
                {
                    int i0 = x + y * width;
                    int i1 = (x + 1) + (y + 1) * width;
                    int i2 = x + (y + 1) * width;
                    int i3 = (x + 1) + y * width;

                    faces[x + y * width1, 0] = idx++;
                    faces[x + y * width1, 1] = idx++;

                    constructor.AddFace(i0, i1, i2);
                    constructor.AddFace(i0, i3, i1);
                }
            }

            if(constructor.SupportsFaceConnections)
            {
                for (int y = 0; y < height1; y++)
                {
                    for (int x = 0; x < width1; x++)
                    {
                        int i0 = (x + y * width1) * 2 + 0;
                        int i1 = (x + y * width1) * 2 + 1;
                        int n0, n1, n2, n3;

                        if (x > 0)
                            n0 = ((x - 1) + y * width1) * 2 + 1;
                        else
                            n0 = -1;

                        if (y > 0)
                            n1 = (x + (y - 1) * width1) * 2 + 0;
                        else
                            n1 = -1;

                        if (x < width1 - 1)
                            n2 = ((x + 1) + y * width1) * 2 + 0;
                        else
                            n2 = -1;

                        if (y < height1 - 1)
                            n3 = (x + (y + 1) * width1) * 2 + 1;
                        else
                            n3 = -1;

                        constructor.AddFaceConnection(i0, i1, n3, n0);
                        constructor.AddFaceConnection(i1, i0, n1, n2);
                    }
                }
            }
        }

    }
}
