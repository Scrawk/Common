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
    public static class CreateTriangularMesh3
    {

        public static void FromTriangle<MESH>(ITriangularMeshConstructor<MESH> constructor, Vector3d A, Vector3d B, Vector3d C)
        {
            constructor.PushTriangularMesh(3, 1);

            constructor.AddVertex(A);
            constructor.AddVertex(B);
            constructor.AddVertex(C);
            constructor.AddFace(0, 1, 2);
        }

        public static void FromBox<MESH>(ITriangularMeshConstructor<MESH> constructor, Vector3d min, Vector3d max)
        {
            constructor.PushTriangularMesh(8, 12);

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

    }
}
