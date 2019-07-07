using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Core.Numerics;
using Common.Meshing.HalfEdgeBased;

namespace Common.Meshing.Constructors
{

    /// <summary>
    /// Create triangle meshes via mesh constructor.
    /// All triangles are CCW.
    /// </summary>
    public static class CreateTriangularMesh3
    {
        /// <summary>
        /// Create a triangle were a,b,c are ccw vertices.
        /// </summary>
        public static void FromTriangle<MESH>(ITriangleMeshConstructor<MESH> constructor,
            Vector3d A, Vector3d B, Vector3d C)
        {
            constructor.PushTriangleMesh(3, 1);

            constructor.AddVertex(A);
            constructor.AddVertex(B);
            constructor.AddVertex(C);
            constructor.AddFace(0, 1, 2);
        }

        public static void FromTertahedron<MESH>(ITriangleMeshConstructor<MESH> constructor,
            Vector3d A, Vector3d B, Vector3d C, Vector3d D)
        {
            constructor.PushTriangleMesh(4, 1);

            constructor.AddVertex(A);
            constructor.AddVertex(B);
            constructor.AddVertex(C);
            constructor.AddVertex(D);

            constructor.AddFace(0, 1, 2);
            constructor.AddFace(0, 3, 1);
            constructor.AddFace(1, 3, 2);
            constructor.AddFace(0, 2, 3);

            if (constructor.SupportsFaceConnections)
            {
                constructor.AddFaceConnection(0, 1, 2, 3);
                constructor.AddFaceConnection(1, 3, 2, 0);
                constructor.AddFaceConnection(2, 1, 3, 0);
                constructor.AddFaceConnection(3, 0, 2, 1);
            }
        }

        public static void FromBox<MESH>(ITriangleMeshConstructor<MESH> constructor, Vector3d min, Vector3d max)
        {
            constructor.PushTriangleMesh(8, 12);

            constructor.AddVertex(new Vector3d(min.x, min.y, min.z));
            constructor.AddVertex(new Vector3d(max.x, min.y, min.z));
            constructor.AddVertex(new Vector3d(max.x, min.y, max.z));
            constructor.AddVertex(new Vector3d(min.x, min.y, max.z));
            constructor.AddVertex(new Vector3d(min.x, max.y, min.z));
            constructor.AddVertex(new Vector3d(max.x, max.y, min.z));
            constructor.AddVertex(new Vector3d(max.x, max.y, max.z));
            constructor.AddVertex(new Vector3d(min.x, max.y, max.z));

            constructor.AddFace(0, 1, 2);
            constructor.AddFace(2, 3, 0);

            constructor.AddFace(1, 5, 6);
            constructor.AddFace(6, 2, 1);

            constructor.AddFace(4, 0, 3);
            constructor.AddFace(3, 7, 4);

            constructor.AddFace(4, 5, 1);
            constructor.AddFace(1, 0, 4);

            constructor.AddFace(3, 2, 6);
            constructor.AddFace(6, 7, 3);

            constructor.AddFace(7, 6, 5);
            constructor.AddFace(5, 4, 7);

            if (constructor.SupportsFaceConnections)
            {
                constructor.AddFaceConnection(0, 7, 3, 1);
                constructor.AddFaceConnection(1, 8, 4, 0);

                constructor.AddFaceConnection(2, 6, 10, 3);
                constructor.AddFaceConnection(3, 8, 0, 2);

                constructor.AddFaceConnection(4, 7, 1, 5);
                constructor.AddFaceConnection(5, 9, 11, 4);

                constructor.AddFaceConnection(6, 1, 2, 7);
                constructor.AddFaceConnection(7, 0, 4, 6);

                constructor.AddFaceConnection(8, 1, 3, 9);
                constructor.AddFaceConnection(9, 10, 5, 8);

                constructor.AddFaceConnection(10, 9, 2, 11);
                constructor.AddFaceConnection(11, 6, 5, 10);
            }
        }

        public static void FromMesh<MESH>(ITriangleMeshConstructor<MESH> constructor, IList<Vector3d> positions, IList<int> indices, bool ccw)
        {
            int numPositions = positions.Count;
            int numTriangles = indices.Count / 3;

            constructor.PushTriangleMesh(numPositions, numTriangles);

            Dictionary<Vector2i, int> edges = null;
            if (constructor.SupportsFaceConnections)
                edges = new Dictionary<Vector2i, int>(numTriangles * 3);

            for (int i = 0; i < numPositions; i++)
                constructor.AddVertex(positions[i]);

            for (int i = 0; i < numTriangles; i++)
            {
                int a = indices[i * 3 + 0];
                int b = indices[i * 3 + 1];
                int c = indices[i * 3 + 2];

                if (!ccw)
                {
                    int tmp = a;
                    a = c;
                    c = tmp;
                }

                constructor.AddFace(a, b, c);

                if (edges != null)
                {
                    edges.Add(new Vector2i(a, b), i);
                    edges.Add(new Vector2i(b, c), i);
                    edges.Add(new Vector2i(c, a), i);
                }
            }

            if (edges != null)
            {
                for (int i = 0; i < numTriangles; i++)
                {
                    int a = indices[i * 3 + 0];
                    int b = indices[i * 3 + 1];
                    int c = indices[i * 3 + 2];

                    if (!ccw)
                    {
                        int tmp = a;
                        a = c;
                        c = tmp;
                    }

                    int n0, n1, n2;
                    n0 = n1 = n2 = -1;

                    edges.TryGetValue(new Vector2i(b, a), out n0);
                    edges.TryGetValue(new Vector2i(c, b), out n1);
                    edges.TryGetValue(new Vector2i(a, c), out n2);

                    constructor.AddFaceConnection(i, n0, n1, n2);
                }
            }

        }

        public static void FromIcosahedron<MESH>(ITriangleMeshConstructor<MESH> constructor, double scale)
        {
            constructor.PushTriangleMesh(12, 20);

            var s = scale;
            var t = (1.0 + Math.Sqrt(5.0)) / 2.0 * scale;

            constructor.AddVertex(new Vector3d(-s, t, 0));
            constructor.AddVertex(new Vector3d(s, t, 0));
            constructor.AddVertex(new Vector3d(-s, -t, 0));
            constructor.AddVertex(new Vector3d(s, -t, 0));

            constructor.AddVertex(new Vector3d(0, -s, t));
            constructor.AddVertex(new Vector3d(0, s, t));
            constructor.AddVertex(new Vector3d(0, -s, -t));
            constructor.AddVertex(new Vector3d(0, s, -t));

            constructor.AddVertex(new Vector3d(t, 0, -s));
            constructor.AddVertex(new Vector3d(t, 0, s));
            constructor.AddVertex(new Vector3d(-t, 0, -s));
            constructor.AddVertex(new Vector3d(-t, 0, s));


            // 5 faces around point 0
            constructor.AddFace(0, 11, 5); //0
            constructor.AddFace(0, 5, 1); //1
            constructor.AddFace(0, 1, 7); //2
            constructor.AddFace(0, 7, 10); //3
            constructor.AddFace(0, 10, 11); //4

            // 5 adjacent faces
            constructor.AddFace(1, 5, 9); //5
            constructor.AddFace(5, 11, 4); //6
            constructor.AddFace(11, 10, 2); //7
            constructor.AddFace(10, 7, 6); //8
            constructor.AddFace(7, 1, 8); //9

            // 5 faces around point 3
            constructor.AddFace(3, 9, 4); //10
            constructor.AddFace(3, 4, 2); //11
            constructor.AddFace(3, 2, 6); //12
            constructor.AddFace(3, 6, 8); //13
            constructor.AddFace(3, 8, 9); //14

            // 5 adjacent faces
            constructor.AddFace(4, 9, 5); //15
            constructor.AddFace(2, 4, 11); //16
            constructor.AddFace(6, 2, 10); //17
            constructor.AddFace(8, 6, 7); //18
            constructor.AddFace(9, 8, 1); //19

            if (constructor.SupportsFaceConnections)
            {
                constructor.AddFaceConnection(0, 1, 4, 6);
                constructor.AddFaceConnection(1, 0, 5, 2);
                constructor.AddFaceConnection(2, 3, 1, 9);
                constructor.AddFaceConnection(3, 4, 2, 8);
                constructor.AddFaceConnection(4, 3, 7, 0);

                constructor.AddFaceConnection(5, 1, 15, 19);
                constructor.AddFaceConnection(6, 0, 16, 15);
                constructor.AddFaceConnection(7, 4, 17, 16);
                constructor.AddFaceConnection(8, 18, 17, 3);
                constructor.AddFaceConnection(9, 2, 19, 18);

                constructor.AddFaceConnection(10, 15, 11, 14);
                constructor.AddFaceConnection(11, 10, 16, 12);
                constructor.AddFaceConnection(12, 11, 17, 13);
                constructor.AddFaceConnection(13, 12, 18, 14);
                constructor.AddFaceConnection(14, 13, 19, 10);

                constructor.AddFaceConnection(15, 11, 5, 10);
                constructor.AddFaceConnection(16, 7, 11, 6);
                constructor.AddFaceConnection(17, 8, 12, 7);
                constructor.AddFaceConnection(18, 8, 9, 13);
                constructor.AddFaceConnection(19, 9, 5, 14);
            }
        }

    }
}
