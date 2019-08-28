using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Meshing.Constructors
{
    public interface ITetrahedronMeshConstructor<MESH>
    {
        bool SupportsFaceConnections { get; }

        void PushTetrahedronMesh(int numVertices, int numFaces);

        MESH PopMesh();

        void AddVertex(Vector3f pos);

        void AddFace(int i0, int i1, int i2, int i3);

        void AddFaceConnection(int faceIndex, int i0, int i1, int i2, int i4);
    }
}
