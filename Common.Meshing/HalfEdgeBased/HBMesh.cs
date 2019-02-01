using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Meshing.HalfEdgeBased
{

    /// <summary>
    /// HBMesh with Vector2f as vertices.
    /// </summary>
    public class HBMesh2f : HBMesh<HBVertex2f, HBEdge, HBFace>
    {
        public HBMesh2f() { }

        public HBMesh2f(int numVertices, int numEdges, int numFaces)
            : base(numVertices, numEdges, numFaces)
        {

        }
    }

    /// <summary>
    /// HBMesh with Vector3f as vertices.
    /// </summary>
    public class HBMesh3f : HBMesh<HBVertex3f, HBEdge, HBFace>
    {
        public HBMesh3f() { }

        public HBMesh3f(int numVertices, int numEdges, int numFaces)
            : base(numVertices, numEdges, numFaces)
        {

        }
    }

    /// <summary>
    /// A half edge based mesh.
    /// </summary>
    public class HBMesh<VERTEX, EDGE, FACE>
            where VERTEX : HBVertex, new()
            where EDGE : HBEdge, new()
            where FACE : HBFace, new()
    {

        /// <summary>
        /// All the vertices in the mesh.
        /// </summary>
        public List<VERTEX> Vertices { get; private set; }

        /// <summary>
        /// All the edges in the mesh.
        /// </summary>
        public List<EDGE> Edges { get; private set; }

        /// <summary>
        /// All the faces in the mesh.
        /// </summary>
        public List<FACE> Faces { get; private set; }

        public HBMesh()
        {
            Vertices = new List<VERTEX>();
            Edges = new List<EDGE>();
            Faces = new List<FACE>();
        }

        public HBMesh(int numVertices, int numEdges, int numFaces)
        {
            Vertices = new List<VERTEX>(numVertices);
            Edges = new List<EDGE>(numEdges);
            Faces = new List<FACE>(numFaces);
        }

        /// <summary>
        /// Convert mesh to string.
        /// </summary>
        /// <returns>Mesh as string</returns>
        public override string ToString()
        {
            return string.Format("[HBMesh: Vertices={0}, Edges={1}, Faces={2}]", 
                Vertices.Count, Edges.Count, Faces.Count);
        }

        /// <summary>
        /// Clear mesh.
        /// </summary>
        public void Clear()
        {
            foreach (var v in Vertices)
                v.Clear();

            foreach (var e in Edges)
                e.Clear();

            foreach (var f in Faces)
                f.Clear();

            Vertices.Clear();
            Edges.Clear();
            Faces.Clear();
        }

        /// <summary>
        /// Find the index of this vertex.
        /// </summary>
        /// <returns>The vertex index or -1 if not found.</returns>
        public int IndexOf(HBVertex vertex)
        {
            VERTEX v = vertex as VERTEX;
            if (v == null) return -1;
            return Vertices.IndexOf(v);
        }

        /// <summary>
        /// Find the index of this edge.
        /// </summary>
        /// <returns>The edge index or -1 if not found.</returns>
        public int IndexOf(HBEdge edge)
        {
            EDGE e = edge as EDGE;
            if (e == null) return -1;
            return Edges.IndexOf(e);
        }

        /// <summary>
        /// Find the index of this face.
        /// </summary>
        /// <returns>The face index or -1 if not found.</returns>
        public int IndexOf(HBFace face)
        {
            FACE f = face as FACE;
            if (f == null) return -1;
            return Faces.IndexOf(f);
        }

        /// <summary>
        /// Creates a new vertex, adds it to 
        /// vertex list add returns.
        /// </summary>
        /// <returns>The new vertex</returns>
        public VERTEX NewVertex()
        {
            var v = new VERTEX();
            Vertices.Add(v);
            return v;
        }

        /// <summary>
        /// Creates a new edge, adds it to 
        /// edge list add returns.
        /// </summary>
        /// <returns>The new edge</returns>
        public EDGE NewEdge()
        {
            var e = new EDGE();
            Edges.Add(e);
            return e;
        }

        /// <summary>
        /// Creates a new face, adds it to 
        /// face list add returns.
        /// </summary>
        /// <returns>The new face</returns>
        public FACE NewFace()
        {
            var f = new FACE();
            Faces.Add(f);
            return f;
        }

        /// <summary>
        /// Find the edge that uses these two vertices.
        /// Presumes all edges have opposites.
        /// </summary>
        /// <param name="from">The from vertex</param>
        /// <param name="to">The to vertex</param>
        /// <returns>The edge index or null if not found</returns>
        public EDGE FindEdge(HBVertex from, HBVertex to)
        {
            for (int i = 0; i < Edges.Count; i++)
            {
                var edge = Edges[i];
                if (edge.From == null) continue;
                if (edge.To == null) continue;

                if (ReferenceEquals(edge.From, from) &&
                   ReferenceEquals(edge.To, to))
                    return edge;
            }

            return null;
        }

        /// <summary>
        /// Fill the mesh with vertices, faces and edges.
        /// Connections will need to be set manually.
        /// </summary>
        public void Fill(int numVertices, int numEdges, int numFaces) 
        {
            Clear();

            Vertices.Capacity = numVertices;
            Edges.Capacity = numEdges;
            Faces.Capacity = numFaces;

            for (int i = 0; i < numVertices; i++)
                Vertices.Add(new VERTEX());

            for (int i = 0; i < numEdges; i++)
                Edges.Add(new EDGE());

            for (int i = 0; i < numFaces; i++)
                Faces.Add(new FACE());
        }

        /// <summary>
        /// Remove all faces.
        /// Will clear faces from all edges.
        /// </summary>
        public void RemoveFaces()
        {
            Faces.Clear();

            int numEdges = Edges.Count;
            for (int i = 0; i < numEdges; i++)
                Edges[i].Face = null;
        }

        /// <summary>
        /// Add opposite edges to all edges that dont have one.
        /// These edges would be considered to be the boundary edges.
        /// Presumes all edges are closed.
        /// </summary>
        public void AddBoundaryEdges()
        {
            List<EDGE> edges = null;
            foreach (var edge in Edges)
            {
                if (edge.Opposite == null)
                {
                    if (edge.Next == null)
                        throw new InvalidOperationException("Edge not closed.");

                    var opp = new EDGE();
                    opp.Opposite = edge;
                    edge.Opposite = opp;
                    opp.From = edge.Next.From;

                    if (edges == null)
                        edges = new List<EDGE>();

                    edges.Add(opp);
                }
            }

            if (edges == null) return;

            foreach (var edge in edges)
            {
                var from = edge.From;
                var to = edge.To;

                EDGE next = null;
                EDGE previous = null;

                foreach (var e in edges)
                {
                    if (ReferenceEquals(edge, e)) continue;

                    //if edge e is going from this edges to vertex
                    //it must be the next edge
                    if (next == null && ReferenceEquals(to, e.From))
                        next = e;

                    //if edge e is going to this edges vertex 
                    //it must be the previous edge
                    if (previous == null && ReferenceEquals(from, e.To))
                        previous = e;

                    if (next != null && previous != null)
                        break;
                }

                if (next == null)
                    throw new NullReferenceException("Failed to find next edge.");

                if (previous == null)
                    throw new NullReferenceException("Failed to find previous edge.");

                edge.Next = next;
                edge.Previous = previous;
            }

            Edges.AddRange(edges);
        }

        /// <summary>
        /// Append the contents of another mesh
        /// on to this mesh as a deep copy.
        /// </summary>
        /// <param name="mesh"></param>
        public void Append(HBMesh<VERTEX, EDGE, FACE> mesh, bool incudeFaces = true)
        {
            int vStart = Vertices.Count;
            int eStart = Edges.Count;
            int fStart = Faces.Count;

            foreach (var other in mesh.Vertices)
            {
                var v = new VERTEX();
                v.Initialize(other);
                Vertices.Add(v);
            }

            foreach (var other in mesh.Edges)
            {
                var e = new EDGE();
                Edges.Add(e);
            }

            if (incudeFaces)
            {
                foreach (var other in mesh.Faces)
                {
                    var f = new FACE();
                    Faces.Add(f);
                }
            }

            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                var v0 = mesh.Vertices[i];
                var v1 = Vertices[vStart + i];

                int edgeIndex = mesh.IndexOf(v0.Edge);

                var edge = (edgeIndex != -1) ? Edges[eStart + edgeIndex] : null;
                v1.Edge = edge;
            }

            for (int i = 0; i < mesh.Edges.Count; i++)
            {
                var e0 = mesh.Edges[i];
                var e1 = Edges[eStart + i];

                int faceIndex = (incudeFaces) ? mesh.IndexOf(e0.Face) : -1;
                int oppIndex = mesh.IndexOf(e0.Opposite);
                int fromIndex = mesh.IndexOf(e0.From);
                int previousIndex = mesh.IndexOf(e0.Previous);
                int nextIndex = mesh.IndexOf(e0.Next);

                var from = (fromIndex != -1) ? Vertices[vStart + fromIndex] : null;
                var face = (faceIndex != -1) ? Faces[fStart + faceIndex] : null;
                var previous = (previousIndex != -1) ? Edges[eStart + previousIndex] : null;
                var next = (nextIndex != -1) ? Edges[eStart + nextIndex] : null;
                var opposite = (oppIndex != -1) ? Edges[eStart + oppIndex]  : null;

                e1.Set(from, face, previous, next, opposite);
            }

            if (incudeFaces)
            {
                for (int i = 0; i < mesh.Faces.Count; i++)
                {
                    var f0 = mesh.Faces[i];
                    var f1 = Faces[fStart + i];

                    int edgeIndex = mesh.IndexOf(f0.Edge);

                    var edge = (edgeIndex != -1) ? Edges[eStart + edgeIndex] : null;
                    f1.Edge = edge;
                }
            }
        }

    }
}
