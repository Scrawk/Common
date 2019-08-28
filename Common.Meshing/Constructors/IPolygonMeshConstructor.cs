using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Meshing.Constructors
{
    public interface IPolygonMeshConstructor<MESH>
    {

        bool SupportsFaceConnections { get; }

        void PushPolygonMesh(int numVertices, int numFaces);

        MESH PopMesh();

        void AddVertex(Vector2f pos);

        void AddVertex(Vector3f pos);

        void AddFace(IList<int> vertList);

        void AddFace(int vertStart, int numVertices);

        void AddFaceConnection(int faceIndex, IList<int> neigbours);

    }
}
