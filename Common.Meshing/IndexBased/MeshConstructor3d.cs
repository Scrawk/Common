using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Meshing.Constructors;

namespace Common.Meshing.IndexBased
{
    /// <summary>
    /// Indexed based mesh constructor.
    /// Supports triangle or edge meshes.
    /// </summary>
    public class MeshConstructor3d :
        IEdgeMeshConstructor<Mesh3d>,
        ITriangularMeshConstructor<Mesh3d>,
        ITetrahedralMeshConstructor<Mesh3d>
    {

        private Mesh3d m_mesh;

        private int m_vertexIndex;

        private int m_faceIndex;

        private int m_edgeIndex;

        private int m_faceVerts;

        /// <summary>
        /// Index based meshes do not support face connections.
        /// </summary>
        public bool SupportsFaceConnections { get { return false; } }

        /// <summary>
        /// If true mesh will have unique vertices for each face.
        /// </summary>
        public bool SplitFaces { get; set; }

        /// <summary>
        /// Push a new edge mesh.
        /// </summary>
        public void PushEdgeMesh(int numVertices, int numEdges)
        {
            if (m_mesh != null)
                throw new InvalidOperationException("Mesh under construction. Can not push new mesh.");

            m_faceVerts = 2;
            m_mesh = new Mesh3d(numVertices, numEdges * m_faceVerts);
        }

        /// <summary>
        /// Push a new triangle mesh.
        /// </summary>
        public void PushTriangularMesh(int numVertices, int numFaces)
        {
            if (m_mesh != null)
                throw new InvalidOperationException("Mesh under construction. Can not push new mesh.");

            m_faceVerts = 3;
            m_mesh = new Mesh3d(numVertices, numFaces * m_faceVerts);
        }

        /// <summary>
        /// Push a new tetrahedral mesh.
        /// </summary>
        public void PushTetrahedralMesh(int numVertices, int numFaces)
        {
            if (m_mesh != null)
                throw new InvalidOperationException("Mesh under construction. Can not push new mesh.");

            m_faceVerts = 4;
            m_mesh = new Mesh3d(numVertices, numFaces * m_faceVerts);
        }

        /// <summary>
        /// Return the created mesh and reset constructor.
        /// </summary>
        public Mesh3d PopMesh()
        {

            if (SplitFaces && m_faceVerts == 3)
            {
                var mesh = new Mesh3d(m_mesh.VerticesCount * 3, m_mesh.IndicesCount);

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

            Mesh3d tmp = m_mesh;
            Reset();

            return tmp;
        }

        /// <summary>
        /// Reset constructor after mesh created.
        /// </summary>
        private void Reset()
        {
            m_mesh = null;
            m_vertexIndex = 0;
            m_faceIndex = 0;
            m_edgeIndex = 0;
            m_faceVerts = 0;
        }

        /// <summary>
        /// Add a vertex to the mesh with this position.
        /// </summary>
        /// <param name="pos">The vertex position</param>
        public void AddVertex(Vector2d pos)
        {
            CheckMeshIsPushed();
            m_mesh.Positions[m_vertexIndex] = pos.xy0;
            m_vertexIndex++;
        }

        /// <summary>
        /// Add a vertex to the mesh with this position.
        /// </summary>
        /// <param name="pos">The vertex position</param>
        public void AddVertex(Vector3d pos)
        {
            CheckMeshIsPushed();
            m_mesh.Positions[m_vertexIndex] = pos;
            m_vertexIndex++;
        }

        /// <summary>
        /// Add a edge.
        /// </summary>
        /// <param name="i0">index of vertex 0</param>
        /// <param name="i1">index of vertex 1</param>
        public void AddEdge(int i0, int i1)
        {
            CheckMeshIsPushed();
            m_mesh.Indices[m_edgeIndex * 2 + 0] = i0;
            m_mesh.Indices[m_edgeIndex * 2 + 1] = i1;
            m_edgeIndex++;
        }

        /// <summary>
        /// Add a CCW triangle face.
        /// </summary>
        /// <param name="i0">index of vertex 0</param>
        /// <param name="i1">index of vertex 1</param>
        /// <param name="i2">index of vertex 2</param>
        public void AddFace(int i0, int i1, int i2)
        {
            CheckMeshIsPushed();
            m_mesh.Indices[m_faceIndex * 3 + 0] = i0;
            m_mesh.Indices[m_faceIndex * 3 + 1] = i1;
            m_mesh.Indices[m_faceIndex * 3 + 2] = i2;
            m_faceIndex++;
        }

        /// <summary>
        /// Add a tetrahedron face.
        /// </summary>
        /// <param name="i0">index of vertex 0</param>
        /// <param name="i1">index of vertex 1</param>
        /// <param name="i2">index of vertex 2</param>
        /// <param name="i3">index of vertex 3</param>
        public void AddFace(int i0, int i1, int i2, int i3)
        {
            CheckMeshIsPushed();
            m_mesh.Indices[m_faceIndex * 4 + 0] = i0;
            m_mesh.Indices[m_faceIndex * 4 + 1] = i1;
            m_mesh.Indices[m_faceIndex * 4 + 2] = i2;
            m_mesh.Indices[m_faceIndex * 4 + 3] = i3;
            m_faceIndex++;
        }

        /// <summary>
        /// Face connections not supported for indexable meshes.
        /// </summary>
        public void AddFaceConnection(int faceIndex, int i0, int i1, int i2)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Face connections not supported for indexable meshes.
        /// </summary>
        public void AddFaceConnection(int faceIndex, int i0, int i1, int i2, int i3)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Helper to throw exception if tring to create mesh
        /// with out pushing a mesh first.
        /// </summary>
        private void CheckMeshIsPushed()
        {
            if (m_mesh == null)
                throw new InvalidOperationException("Mesh has not been pushed.");
        }

    }
}
