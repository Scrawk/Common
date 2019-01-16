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

        void AddVertex(Vector3f pos);

        void AddFace(TriangleIndex triangle);

        void AddFace(int i0, int i1, int i2);

        void AddEdge(EdgeIndex edge);

        void AddEdge(int i0, int i1);

        void AddFaceConnection(int faceIndex, TriangleIndex neighbors);

        void AddFaceConnection(int faceIndex, int i0, int i1, int i2);

        void AddEdgeConnection(EdgeConnection connection);

        void AddEdgeConnection(int edgeIndex, int previousIndex, int nextIndex, int oppositeIndex);

    }
}
