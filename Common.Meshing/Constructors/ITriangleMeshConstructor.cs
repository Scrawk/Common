using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Meshing.Constructors
{
    public interface ITriangleMeshConstructor<MESH>
    {

        bool SupportsFaceConnections { get; }

        void PushTriangleMesh(int numVertices, int numFaces);

        MESH PopMesh();

        void AddVertex(Vector2f pos);

        void AddVertex(Vector3f pos);

        void AddFace(int i0, int i1, int i2);

        void AddFaceConnection(int faceIndex, int i0, int i1, int i2);

    }
}
