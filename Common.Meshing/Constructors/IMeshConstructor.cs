using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Meshing.Descriptors;

namespace Common.Meshing.Constructors
{
    public interface IMeshConstructor<MESH>
    {

        bool SupportsEdges { get; }

        bool SupportsEdgeConnections { get; }

        bool SupportsFaces { get; }

        bool SupportsFaceConnections { get; }

        void PushTriangleMesh(int numVertices, int numFaces);

        void PushEdgeMesh(int numVertices, int numEdges);

        MESH PopMesh();

        void AddVertex(Vector2f pos);

        void AddFace(TriangleIndex triangle);

        void AddEdge(EdgeIndex edge);

        void AddFaceConnection(int faceIndex, TriangleIndex neighbors);

        void AddEdgeConnection(EdgeConnection connection);

    }
}
