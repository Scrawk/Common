using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Core.Mathematics;
using Common.Meshing.Constructors;

namespace Common.Meshing.HalfEdgeBased
{
    /// <summary>
    /// Half edge based mesh constructor.
    /// Supports edge, triangle or general meshes.
    /// </summary>
    public class HBMeshConstructor<VERTEX, EDGE, FACE> : 
            IEdgeMeshConstructor<HBMesh<VERTEX, EDGE, FACE>>,
            ITriangleMeshConstructor<HBMesh<VERTEX, EDGE, FACE>>,
            IGeneralMeshConstructor<HBMesh<VERTEX, EDGE, FACE>>
            where VERTEX : HBVertex, new()
            where EDGE : HBEdge, new()
            where FACE : HBFace, new()
    {

        /// <summary>
        /// The mesh object under construction.
        /// </summary>
        private HBMesh<VERTEX, EDGE, FACE> Mesh { get; set; }

        /// <summary>
        /// Does a half edge mesh support edge connections.
        /// </summary>
        public bool SupportsEdgeConnections { get { return true; } }

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
        /// <param name="numVertices"></param>
        /// <param name="numFaces"></param>
        public void PushTriangleMesh(int numVertices, int numFaces)
        {
            Mesh = new HBMesh<VERTEX, EDGE, FACE>(numVertices, numFaces * 3 * 2, numFaces);
        }

        /// <summary>
        /// Create a edge mesh. Edge meshs have no faces.
        /// </summary>
        /// <param name="numVertices"></param>
        /// <param name="numEdges"></param>
        public void PushEdgeMesh(int numVertices, int numEdges)
        {
            Mesh = new HBMesh<VERTEX, EDGE, FACE>(numVertices, numEdges, 0);
        }

        /// <summary>
        /// Create a general mesh. Faces can have any number of edges.
        /// </summary>
        /// <param name="numVertices"></param>
        /// <param name="numFaces"></param>
        public void PushGeneralMesh(int numVertices, int numFaces)
        {
            Mesh = new HBMesh<VERTEX, EDGE, FACE>(numVertices, 0, numFaces);
        }

        /// <summary>
        /// Remove and return finished mesh.
        /// </summary>
        /// <returns></returns>
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

            foreach (var i in vertList)
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
        /// Add a edge to mesh.
        /// </summary>
        /// <param name="i0">index of vertex 0</param>
        /// <param name="i1">index of vertex 1</param>
        public void AddEdge(int i0, int i1)
        {
            var v0 = Mesh.Vertices[i0];
            var v1 = Mesh.Vertices[i1];

            var e0 = new EDGE();
            var e1 = new EDGE();

            e0.Opposite = e1;
            e1.Opposite = e0;

            v0.Edge = e0;
            e0.Vertex = v0;

            v1.Edge = e1;
            e1.Vertex = v1;

            Mesh.Edges.Add(e0);
            Mesh.Edges.Add(e1);
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

            if (edge.Vertex == null)
                throw new NullReferenceException("Edge has null vertex.");

            if (neighbour.Edge == null)
                throw new NullReferenceException("Neighbor has null edge.");

            var v0 = edge.Vertex;
            var v1 = edge.Previous.Vertex;

            foreach (var nedge in neighbour.Edge.EnumerateEdges())
            {
                if (nedge.Vertex == null)
                    throw new NullReferenceException("Neighbor edge has null vertex.");

                if(ReferenceEquals(v0, nedge.Previous.Vertex) &&
                   ReferenceEquals(v1, nedge.Vertex))
                {
                    edge.Opposite = nedge;
                    nedge.Opposite = edge;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Connect a edge to its other edge members.
        /// </summary>
        /// <param name="edgeIndex">The index of the edge to connect</param>
        /// <param name="previousIndex">The index of this edges previous member</param>
        /// <param name="nextIndex">The index of this edges next member</param>
        /// <param name="oppositeIndex">The index of this edges opposite member</param>
        public void AddEdgeConnection(int edgeIndex, int previousIndex, int nextIndex, int oppositeIndex)
        {
            var edge = Mesh.Edges[edgeIndex];
            var previous = (previousIndex != -1) ? Mesh.Edges[previousIndex] : null;
            var next = (nextIndex != -1) ? Mesh.Edges[nextIndex] : null;

            edge.Previous = previous;
            edge.Next = next;
        }

    }
}
