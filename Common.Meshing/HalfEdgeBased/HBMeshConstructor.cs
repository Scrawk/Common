﻿using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Core.Mathematics;
using Common.Meshing.Constructors;

namespace Common.Meshing.HalfEdgeBased
{

    /// <summary>
    /// Constructor for HBMesh2f
    /// </summary>
    public class HBMeshConstructor2f : HBMeshConstructor<HBVertex2f, HBEdge, HBFace>
    {
        protected override void NewMesh(int numVertices, int numEdges, int numFaces)
        {
            Mesh = new HBMesh2f(numVertices, numEdges, numFaces);
        }

        public new HBMesh2f PopMesh()
        {
            if (AddBoundary)
                Mesh.AddBoundaryEdges();

            var tmp = Mesh;
            Mesh = null;
            return tmp as HBMesh2f;
        }
    }

    /// <summary>
    /// Constructor for HBMesh3f
    /// </summary>
    public class HBMeshConstructor3f : HBMeshConstructor<HBVertex3f, HBEdge, HBFace>
    {
        protected override void NewMesh(int numVertices, int numEdges, int numFaces)
        {
            Mesh = new HBMesh3f(numVertices, numEdges, numFaces);
        }

        public new HBMesh3f PopMesh()
        {
            if (AddBoundary)
                Mesh.AddBoundaryEdges();

            var tmp = Mesh;
            Mesh = null;
            return tmp as HBMesh3f;
        }
    }

    /// <summary>
    /// Half edge based mesh constructor.
    /// Supports triangle or general meshes.
    /// </summary>
    public class HBMeshConstructor<VERTEX, EDGE, FACE> : 
            ITriangleMeshConstructor<HBMesh<VERTEX, EDGE, FACE>>,
            IGeneralMeshConstructor<HBMesh<VERTEX, EDGE, FACE>>
            where VERTEX : HBVertex, new()
            where EDGE : HBEdge, new()
            where FACE : HBFace, new()
    {

        protected HBMesh<VERTEX, EDGE, FACE> Mesh;

        /// <summary>
        /// Does a half edge mesh support face connections.
        /// Done via edge opposite member.
        /// </summary>
        public bool SupportsFaceConnections { get { return true; } }

        /// <summary>
        /// Add a boundary so all edges have a opposite member.
        /// </summary>
        public bool AddBoundary = true;

        /// <summary>
        /// Create a triangle mesh. All faces are triangles.
        /// </summary>
        public void PushTriangleMesh(int numVertices, int numFaces)
        {
            NewMesh(numVertices, numFaces * 3 * 2, numFaces);
        }

        /// <summary>
        /// Create a general mesh. Faces can have any number of edges.
        /// </summary>
        public void PushGeneralMesh(int numVertices, int numFaces)
        {
            NewMesh(numVertices, 0, numFaces);
        }

        /// <summary>
        /// Create a new mesh. Allows parent class to make different mesh type.
        /// </summary>
        protected virtual void NewMesh(int numVertices, int numEdges, int numFaces)
        {
            if (Mesh != null)
                throw new InvalidOperationException("Mesh under construction. Can not push a new mesh.");

            Mesh = new HBMesh<VERTEX, EDGE, FACE>(numVertices, numEdges, numFaces);
        }

        /// <summary>
        /// Remove and return finished mesh.
        /// </summary>
        public HBMesh<VERTEX, EDGE, FACE> PopMesh()
        {
            if (AddBoundary)
                Mesh.AddBoundaryEdges();

            var tmp = Mesh;
            Mesh = null;
            return tmp;
        }

        /// <summary>
        /// Add a vertex to the mesh with this position.
        /// </summary>
        /// <param name="pos">The vertex position</param>
        public void AddVertex(Vector2f pos)
        {
            CheckMeshIsPushed();
            VERTEX v = new VERTEX();
            v.Initialize(pos);
            Mesh.Vertices.Add(v);
        }

        /// <summary>
        /// Add a vertex to the mesh with this position.
        /// </summary>
        /// <param name="pos">The vertex position</param>
        public void AddVertex(Vector3f pos)
        {
            CheckMeshIsPushed();
            VERTEX v = new VERTEX();
            v.Initialize(pos);
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

            var e0 = new EDGE();
            var e1 = new EDGE();
            var e2 = new EDGE();

            v0.Edge = e0;
            v1.Edge = e1;
            v2.Edge = e2;

            var face = new FACE();
            face.Edge = e0;

            e0.Set(v0, face, e2, e1, null);
            e1.Set(v1, face, e0, e2, null);
            e2.Set(v2, face, e1, e0, null);

            Mesh.Faces.Add(face);
            Mesh.Edges.Add(e0);
            Mesh.Edges.Add(e1);
            Mesh.Edges.Add(e2);
        }

        /// <summary>
        /// Add a CCW general face.
        /// </summary>
        /// <param name="vertList">A list of vertices in the face</param>
        public void AddFace(IList<int> vertList)
        {
            CheckMeshIsPushed();
            int count = vertList.Count;
            var face = new FACE();
            var edges = new List<EDGE>(vertList.Count);

            for (int i = 0; i < count; i++)
            {
                var v = Mesh.Vertices[vertList[i]];
                var e = new EDGE();
                v.Edge = e;
                edges.Add(e);
            }

            face.Edge = edges[0];
            Mesh.Faces.Add(face);

            for (int i = 0; i < count; i++)
            {
                var e = edges[i];
                var v = Mesh.Vertices[vertList[i]];
                var previous = edges[IMath.Wrap(i - 1, count)];
                var next = edges[IMath.Wrap(i + 1, count)];

                e.Set(v, face, previous, next, null);
                Mesh.Edges.Add(e);
            }

        }

        /// <summary>
        /// Add a CCW general face. Presumes that 
        /// vertices are orders from start index to start + num index.
        /// </summary>
        /// <param name="vertStart">The index of the start vert</param>
        /// <param name="numVertices">The number of vertices in face</param>
        public void AddFace(int vertStart, int numVertices)
        {
            CheckMeshIsPushed();
            int count = numVertices;
            var face = new FACE();
            var edges = new List<EDGE>(numVertices);

            for (int i = 0; i < count; i++)
            {
                var v = Mesh.Vertices[vertStart + i];
                var e = new EDGE();
                v.Edge = e;
                edges.Add(e);
            }

            face.Edge = edges[0];
            Mesh.Faces.Add(face);

            for (int i = 0; i < count; i++)
            {
                var e = edges[i];
                var v = Mesh.Vertices[vertStart + i];
                var previous = edges[IMath.Wrap(i - 1, count)];
                var next = edges[IMath.Wrap(i + 1, count)];

                e.Set(v, face, previous, next, null);
                Mesh.Edges.Add(e);
            }

        }

        /// <summary>
        /// Add a triangle face connection.
        /// Will find and connect faces by joining the face edges opposite member.
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

            if (i0 != -1)
            {
                foreach (var edge in face.Edge.EnumerateEdges())
                    if (SetOppositeEdge(edge, Mesh.Faces[i0])) break;
            }

            if (i1 != -1)
            {
                foreach (var edge in face.Edge.EnumerateEdges())
                    if (SetOppositeEdge(edge, Mesh.Faces[i1])) break;
            }

            if (i2 != -1)
            {
                foreach (var edge in face.Edge.EnumerateEdges())
                    if (SetOppositeEdge(edge, Mesh.Faces[i2])) break;
            }
        }

        /// <summary>
        /// Add a general face connection.
        /// Will find and connect faces by joining the face edges opposite member.
        /// A index of -1 means face has no neighbour.
        /// </summary>
        /// <param name="faceIndex">The index of the face</param>
        /// <param name="neighbours">index of faces neighbours</param>
        public void AddFaceConnection(int faceIndex, IList<int> neighbours)
        {
            var face = Mesh.Faces[faceIndex];

            foreach(var n in neighbours)
            {
                if (n == -1) continue;

                foreach (var edge in face.Edge.EnumerateEdges())
                    if (SetOppositeEdge(edge, Mesh.Faces[n])) break;
            }
        }

        /// <summary>
        /// Find and set the edges opposite member.
        /// </summary>
        /// <returns>true if found</returns>
        private bool SetOppositeEdge(HBEdge edge, HBFace neighbour)
        {
            if (neighbour == null) return false;

            if (edge == null)
                throw new NullReferenceException("Edge is null.");

            if (edge.From == null)
                throw new NullReferenceException("Edge has null vertex.");

            if (neighbour.Edge == null)
                throw new NullReferenceException("Neighbour has null edge.");

            var from = edge.From;
            var to = edge.Next.From;

            foreach (var nedge in neighbour.Edge.EnumerateEdges())
            {
                if (nedge.From == null)
                    throw new NullReferenceException("Neighbour edge has null vertex.");

                //if nedge and edge share the same vertices but 
                //in opposite order then they must be opposite edges
                if(ReferenceEquals(from, nedge.Next.From) &&
                   ReferenceEquals(to, nedge.From))
                {
                    edge.Opposite = nedge;
                    nedge.Opposite = edge;
                    return true;
                }
            }

            return false;
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
