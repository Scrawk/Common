using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Meshing.Constructors;

namespace Common.Meshing.FaceBased
{

    /// <summary>
    /// Constructor for HBMesh2f
    /// </summary>
    public class FBMeshConstructor2f : FBMeshConstructor<FBMesh2f, FBVertex2f, FBFace>
    {

    }

    /// <summary>
    /// Constructor for HBMesh3f
    /// </summary>
    public class FBMeshConstructor3f : FBMeshConstructor<FBMesh3f, FBVertex3f, FBFace>
    {

    }

    /// <summary>
    /// Constructor for HBMesh2f
    /// </summary>
    public class FBMeshConstructor2d : FBMeshConstructor<FBMesh2d, FBVertex2d, FBFace>
    {

    }

    /// <summary>
    /// Constructor for HBMesh3f
    /// </summary>
    public class FBMeshConstructor3d : FBMeshConstructor<FBMesh3d, FBVertex3d, FBFace>
    {

    }

    /// <summary>
    /// Face based mesh constructor.
    /// Supports triangle or polygon meshes.
    /// </summary>
    public class FBMeshConstructor<MESH, VERTEX, FACE> :
           ITriangularMeshConstructor<MESH>,
           IPolygonalMeshConstructor<MESH>,
           ITetrahedralMeshConstructor<MESH>
           where MESH : FBMesh<VERTEX, FACE>, new()
           where VERTEX : FBVertex, new()
           where FACE : FBFace, new()
    {

        private MESH Mesh;

        /// <summary>
        /// Does a face mesh support face connections.
        /// </summary>
        public bool SupportsFaceConnections => true;

        /// <summary>
        /// Create a triangle mesh. All faces are triangles.
        /// </summary>
        public void PushTriangularMesh(int numVertices, int numFaces)
        {
            if (Mesh != null)
                throw new InvalidOperationException("Mesh under construction. Can not push new mesh.");

            NewMesh(numVertices, numFaces);
        }

        /// <summary>
        /// Create a polygon mesh. Faces can have any number of edges.
        /// </summary>
        public void PushPolygonalMesh(int numVertices, int numFaces)
        {
            if (Mesh != null)
                throw new InvalidOperationException("Mesh under construction. Can not push new mesh.");

            NewMesh(numVertices, numFaces);
        }

        /// <summary>
        /// Create a tetrahedral mesh. All faces are tetrahedron.
        /// </summary>
        public void PushTetrahedralMesh(int numVertices, int numFaces)
        {
            if (Mesh != null)
                throw new InvalidOperationException("Mesh under construction. Can not push new mesh.");

            NewMesh(numVertices, numFaces);
        }

        /// <summary>
        /// Create a new mesh.
        /// </summary>
        private void NewMesh(int numVertices, int numFaces)
        {
            if (Mesh != null)
                throw new InvalidOperationException("Mesh under construction. Can not push a new mesh.");

            Mesh = new MESH();
            Mesh.Vertices.Capacity = numVertices;
            Mesh.Faces.Capacity = numFaces;
        }

        /// <summary>
        /// Remove and return finished mesh.
        /// </summary>
        public MESH PopMesh()
        {
            var tmp = Mesh;
            Mesh = null;
            return tmp;
        }

        /// <summary>
        /// Add a vertex to the mesh with this position.
        /// </summary>
        /// <param name="pos">The vertex position</param>
        public void AddVertex(Vector2d pos)
        {
            CheckMeshIsPushed();
            VERTEX v = new VERTEX();
            v.SetPosition(pos.xy0);
            Mesh.Vertices.Add(v);
        }

        /// <summary>
        /// Add a vertex to the mesh with this position.
        /// </summary>
        /// <param name="pos">The vertex position</param>
        public void AddVertex(Vector3d pos)
        {
            CheckMeshIsPushed();
            VERTEX v = new VERTEX();
            v.SetPosition(pos);
            Mesh.Vertices.Add(v);
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
            var v0 = Mesh.Vertices[i0];
            var v1 = Mesh.Vertices[i1];
            var v2 = Mesh.Vertices[i2];

            FACE face = new FACE();
            face.SetVerticesSize(3);

            v0.AddFace(face);
            v1.AddFace(face);
            v2.AddFace(face);

            face.Vertices[0] = v0;
            face.Vertices[1] = v1;
            face.Vertices[2] = v2;

            Mesh.Faces.Add(face);
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
            var v0 = Mesh.Vertices[i0];
            var v1 = Mesh.Vertices[i1];
            var v2 = Mesh.Vertices[i2];
            var v3 = Mesh.Vertices[i3];

            FACE face = new FACE();
            face.SetVerticesSize(4);

            v0.AddFace(face);
            v1.AddFace(face);
            v2.AddFace(face);
            v3.AddFace(face);

            face.Vertices[0] = v0;
            face.Vertices[1] = v1;
            face.Vertices[2] = v2;
            face.Vertices[3] = v3;

            Mesh.Faces.Add(face);
        }

        /// <summary>
        /// Add a CCW polygon face.
        /// </summary>
        /// <param name="vertList">A list of vertices in the face</param>
        public void AddFace(IList<int> vertList)
        {
            CheckMeshIsPushed();
            int count = vertList.Count;
            FACE face = new FACE();
            face.SetVerticesSize(count);

            for(int i = 0; i < count; i++)
            {
                var v = Mesh.Vertices[vertList[i]];
                v.AddFace(face);
                face.Vertices[i] = v;
            }

            Mesh.Faces.Add(face);
        }

        /// <summary>
        /// Add a CCW polygon face. Presumes that 
        /// vertices are orders from start index to start + num index.
        /// </summary>
        /// <param name="vertStart">The index of the start vert</param>
        /// <param name="numVertices">The number of vertices in face</param>
        public void AddFace(int vertStart, int numVertices)
        {
            CheckMeshIsPushed();
            int count = numVertices;
            FACE face = new FACE();
            face.SetVerticesSize(count);

            for (int i = 0; i < count; i++)
            {
                var v = Mesh.Vertices[vertStart + i];
                v.AddFace(face);
                face.Vertices[i] = v;
            }

            Mesh.Faces.Add(face);
        }

        /// <summary>
        /// Add a triangle face connection.
        /// Will find and connect faces by set the neighbours.
        /// A index of -1 means face has no neighbour.
        /// </summary>
        /// <param name="faceIndex">The index of the face</param>
        /// <param name="i0">index of faces neighbour 0</param>
        /// <param name="i1">index of faces neighbour 1</param>
        /// <param name="i2">index of faces neighbour 2</param>
        public void AddFaceConnection(int faceIndex, int i0, int i1, int i2)
        {
            CheckMeshIsPushed();
            var face = Mesh.Faces[faceIndex];
            var f0 = (i0 != -1) ? Mesh.Faces[i0] : null;
            var f1 = (i1 != -1) ? Mesh.Faces[i1] : null;
            var f2 = (i2 != -1) ? Mesh.Faces[i2] : null;

            face.Neighbours[0] = f0;
            face.Neighbours[1] = f1;
            face.Neighbours[2] = f2;
        }

        /// <summary>
        /// Add a tetrahedral face connection.
        /// Will find and connect faces by set the neighbours.
        /// A index of -1 means face has no neighbour.
        /// </summary>
        /// <param name="faceIndex">The index of the face</param>
        /// <param name="i0">index of faces neighbour 0</param>
        /// <param name="i1">index of faces neighbour 1</param>
        /// <param name="i2">index of faces neighbour 2</param>
        /// <param name="i3">index of faces neighbour 3</param>
        public void AddFaceConnection(int faceIndex, int i0, int i1, int i2, int i3)
        {
            CheckMeshIsPushed();
            var face = Mesh.Faces[faceIndex];
            var f0 = (i0 != -1) ? Mesh.Faces[i0] : null;
            var f1 = (i1 != -1) ? Mesh.Faces[i1] : null;
            var f2 = (i2 != -1) ? Mesh.Faces[i2] : null;
            var f3 = (i3 != -1) ? Mesh.Faces[i3] : null;

            face.Neighbours[0] = f0;
            face.Neighbours[1] = f1;
            face.Neighbours[2] = f2;
            face.Neighbours[3] = f3;
        }

        /// <summary>
        /// Add a general face connection.
        /// Will find and connect faces by set the neighbours.
        /// A index of -1 means face has no neighbour.
        /// </summary>
        /// <param name="faceIndex">The index of the face</param>
        /// <param name="neighbours">index of faces neighbours</param>
        public void AddFaceConnection(int faceIndex, IList<int> neighbours)
        {
            CheckMeshIsPushed();
            int count = neighbours.Count;
            var face = Mesh.Faces[faceIndex];

            for (int i = 0; i < count; i++)
            {
                int n = neighbours[i];
                if (n == -1) continue;
                face.Neighbours[i] = Mesh.Faces[n];
            }

        }

        /// <summary>
        /// Helper to throw exception if tring to create mesh
        /// with out pushing a mesh first.
        /// </summary>
        private void CheckMeshIsPushed()
        {
            if (Mesh == null)
                throw new InvalidOperationException("Mesh has not been pushed.");
        }

    }
}
