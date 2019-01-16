using System;
using System.Collections.Generic;
using Common.Core.LinearAlgebra;

using Common.Meshing.Descriptors;

namespace Common.Meshing.Constructors
{
    public abstract class MeshConstructor<MESH> : IMeshConstructor<MESH>
    {

        public abstract bool SupportsEdges { get; }

        public abstract bool SupportsEdgeConnections { get; }

        public abstract bool SupportsFaces { get; }

        public abstract bool SupportsFaceConnections { get; }

        public virtual void PushTriangleMesh(int numVertices, int numFaces)
        {
            throw new NotSupportedException("Mesh does not support faces.");
        }

        public virtual void PushEdgeMesh(int numVertices, int numEdges)
        {
            throw new NotSupportedException("Mesh does not support edges.");
        }

        public abstract MESH PopMesh();

        public virtual void AddVertex(Vector2f pos)
        {

        }

        public virtual void AddVertex(Vector3f pos)
        {

        }

        public void AddFace(TriangleIndex triangle)
        {
            AddFace(triangle.i0, triangle.i1, triangle.i2);
        }

        public virtual void AddFace(int i0, int i1, int i2)
        {

        }

        public void AddEdge(EdgeIndex edge)
        {
            AddEdge(edge.i0, edge.i1);
        }

        public virtual void AddEdge(int i0, int i1)
        {

        }

        public void AddFaceConnection(int faceIndex, TriangleIndex neighbors)
        {
            AddFaceConnection(faceIndex, neighbors.i0, neighbors.i1, neighbors.i2);
        }

        public virtual void AddFaceConnection(int faceIndex, int i0, int i1, int i2)
        {
            
        }

        public void AddEdgeConnection(EdgeConnection connection)
        {
            AddEdgeConnection(connection.Edge, connection.Previous, connection.Next, connection.Opposite);
        }

        public virtual void AddEdgeConnection(int edgeIndex, int previousIndex, int nextIndex, int oppositeIndex)
        {

        }

    }
}
