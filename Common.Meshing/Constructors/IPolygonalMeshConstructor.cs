using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Meshing.Constructors
{
    public interface IPolygonalMeshConstructor<MESH>
    {

        bool SupportsFaceConnections { get; }

        void PushPolygonalMesh(int numVertices, int numFaces);

        MESH PopMesh();

        void AddVertex(Vector2d pos);

        void AddVertex(Vector3d pos);

        void AddFace(IList<int> vertList);

        void AddFace(int vertStart, int numVertices);

        void AddFaceConnection(int faceIndex, IList<int> neigbours);

    }
}
