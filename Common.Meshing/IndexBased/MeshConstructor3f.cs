using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Meshing.Constructors;

namespace Common.Meshing.IndexBased
{
    public class MeshConstructor3f : IEdgeMeshConstructor<Mesh3f>, ITriangleMeshConstructor<Mesh3f>
    {

        public bool SupportsFaceConnections { get { return false; } }

        public bool SplitFaces { get; set; }

        private Mesh3f m_mesh;

        private int m_vertexIndex;

        private int m_faceIndex;

        private int m_edgeIndex;

        public void PushTriangleMesh(int numVertices, int numFaces)
        {
            if (m_mesh != null)
                throw new InvalidOperationException("Mesh under construction. Can not push new mesh.");

            m_mesh = new Mesh3f(numVertices, numFaces * 3);
        }

        public void PushEdgeMesh(int numVertices, int numEdges)
        {
            if (m_mesh != null)
                throw new InvalidOperationException("Mesh under construction. Can not push new mesh.");

            m_mesh = new Mesh3f(numVertices, numEdges * 2);
        }

        public Mesh3f PopMesh()
        {

            if (SplitFaces)
            {
                var mesh = new Mesh3f(m_mesh.VerticesCount * 3, m_mesh.IndicesCount);

                var indices = m_mesh.Indices;
                var positions = m_mesh.Positions;

                for (int i = 0; i < m_mesh.IndicesCount / 3; i++)
                {
                    mesh.Positions[i * 3 + 0] = positions[indices[i * 3 + 0]];
                    mesh.Positions[i * 3 + 1] = positions[indices[i * 3 + 1]];
                    mesh.Positions[i * 3 + 2] = positions[indices[i * 3 + 2]];

                    mesh.Indices[i * 3 + 0] = i * 3 + 0;
                    mesh.Indices[i * 3 + 1] = i * 3 + 1;
                    mesh.Indices[i * 3 + 2] = i * 3 + 2;
                }

                m_mesh = mesh;
            }

            Mesh3f tmp = m_mesh;
            Reset();

            return tmp;
        }

        private void Reset()
        {
            m_mesh = null;
            m_vertexIndex = 0;
            m_faceIndex = 0;
            m_edgeIndex = 0;
        }

        public void AddVertex(Vector2f pos)
        {
            m_mesh.Positions[m_vertexIndex] = pos.xy0;
            m_vertexIndex++;
        }

        public void AddVertex(Vector3f pos)
        {
            m_mesh.Positions[m_vertexIndex] = pos;
            m_vertexIndex++;
        }

        public void AddFace(int i0, int i1, int i2)
        {
            m_mesh.Indices[m_faceIndex * 3 + 0] = i0;
            m_mesh.Indices[m_faceIndex * 3 + 1] = i1;
            m_mesh.Indices[m_faceIndex * 3 + 2] = i2;
            m_faceIndex++;
        }

        public void AddEdge(int i0, int i1)
        {
            m_mesh.Indices[m_edgeIndex * 2 + 0] = i0;
            m_mesh.Indices[m_edgeIndex * 2 + 1] = i1;
            m_edgeIndex++;
        }

        public void AddFaceConnection(int faceIndex, int i0, int i1, int i2)
        {
            throw new NotSupportedException();
        }

    }
}
