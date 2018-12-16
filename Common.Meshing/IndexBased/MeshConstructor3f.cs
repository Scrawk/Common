using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Meshing.Constructors;
using Common.Meshing.Descriptors;

namespace Common.Meshing.IndexBased
{
    public class MeshConstructor3f : MeshConstructor<Mesh3f>
    {

        public override bool SupportsEdges { get { return true; } }

        public override bool SupportsEdgeConnections { get { return false; } }

        public override bool SupportsFaces { get { return true; } }

        public override bool SupportsFaceConnections { get { return false; } }

        private Mesh3f m_mesh;

        private int m_vertexIndex;

        private int m_faceIndex;

        private int m_edgeIndex;

        public override void PushTriangleMesh(int numVertices, int numFaces)
        {
            m_mesh = new Mesh3f(numVertices, numFaces * 3);
        }

        public override void PushEdgeMesh(int numVertices, int numEdges)
        {
            m_mesh = new Mesh3f(numVertices, numEdges * 2);
        }

        public override Mesh3f PopMesh()
        {
            Mesh3f tmp = m_mesh;
            m_mesh = null;

            ResetIndex();

            return tmp;
        }

        private void ResetIndex()
        {
            m_vertexIndex = 0;
            m_faceIndex = 0;
            m_edgeIndex = 0;
        }

        public override void AddVertex(Vector3f pos)
        {
            m_mesh.Positions[m_vertexIndex] = pos;
            m_vertexIndex++;
        }

        public override void AddFace(TriangleIndex triangle)
        {
            m_mesh.Indices[m_faceIndex * 3 + 0] = triangle.i0;
            m_mesh.Indices[m_faceIndex * 3 + 1] = triangle.i1;
            m_mesh.Indices[m_faceIndex * 3 + 2] = triangle.i2;
            m_faceIndex++;
        }

        public override void AddEdge(EdgeIndex edge)
        {
            m_mesh.Indices[m_edgeIndex * 2 + 0] = edge.i0;
            m_mesh.Indices[m_edgeIndex * 2 + 1] = edge.i1;
            m_edgeIndex++;
        }
    }
}
