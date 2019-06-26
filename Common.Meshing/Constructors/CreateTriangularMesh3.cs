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

        public static HBMesh3f FromTriangle(Vector3f A, Vector3f B, Vector3f C)
        {
            var constructor = new HBMeshConstructor3f();
            FromTriangle(constructor, A, B, C);
            return constructor.PopMesh();
        }

        public static HBMesh3d FromTriangle(Vector3d A, Vector3d B, Vector3d C)
        {
            var constructor = new HBMeshConstructor3d();
            FromTriangle(constructor, A, B, C);
            return constructor.PopMesh();
        }

        public static void FromTriangle<MESH>(ITriangularMeshConstructor<MESH> constructor, Vector3d A, Vector3d B, Vector3d C)
        {
            constructor.PushTriangularMesh(3, 1);

            constructor.AddVertex(A);
            constructor.AddVertex(B);
            constructor.AddVertex(C);
            constructor.AddFace(0, 1, 2);
        }

    }
}
