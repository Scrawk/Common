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

        public virtual void AddFace(TriangleIndex triangle)
        {

        }

        public virtual void AddEdge(EdgeIndex edge)
        {

        }

        public virtual void AddFaceConnection(int faceIndex, TriangleIndex neighbors)
        {

        }

        public virtual void AddEdgeConnection(EdgeConnection connection)
        {

        }

    }
}
