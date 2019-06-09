using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Meshing.Constructors
{
    public interface ITetrahedralMeshConstructor<MESH>
    {
        bool SupportsFaceConnections { get; }

        void PushTetrahedralMesh(int numVertices, int numFaces);

        MESH PopMesh();

        void AddVertex(Vector3d pos);

        void AddFace(int i0, int i1, int i2, int i3);

        void AddFaceConnection(int faceIndex, int i0, int i1, int i2, int i4);
    }
}
