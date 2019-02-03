using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Core.Mathematics;

namespace Common.Meshing.HalfEdgeBased
{
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
            for (int i = 0; i < Vertices.Count; i++)
                Vertices[i].Clear();

            for (int i = 0; i < Edges.Count; i++)
                Edges[i].Clear();

            for (int i = 0; i < Faces.Count; i++)
                Faces[i].Clear();

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
        /// Applies the vertex index as a tag.
        /// </summary>
        public void TagVertices()
        {
            for (int i = 0; i < Vertices.Count; i++)
                Vertices[i].Tag = i;
        }

        /// <summary>
        /// Sets all vertex tags.
        /// </summary>
        public void TagVertices(int tag)
        {
            for (int i = 0; i < Vertices.Count; i++)
                Vertices[i].Tag = tag;
        }

        /// <summary>
        /// Applies the edge index as a tag.
        /// </summary>
        public void TagEdges()
        {
            for (int i = 0; i < Edges.Count; i++)
                Edges[i].Tag = i;
        }

        /// <summary>
        /// Sets all edge tags.
        /// </summary>
        public void TagEdges(int tag)
        {
            for (int i = 0; i < Edges.Count; i++)
                Edges[i].Tag = tag;
        }

        /// <summary>
        /// Applies the face index as a tag.
        /// </summary>
        public void TagFaces()
        {
            for (int i = 0; i < Faces.Count; i++)
                Faces[i].Tag = i;
        }

        /// <summary>
        /// Sets all face tags.
        /// </summary>
        public void TagFaces(int tag)
        {
            for (int i = 0; i < Faces.Count; i++)
                Faces[i].Tag = tag;
        }

        public void TagAll()
        {
            TagVertices();
            TagEdges();
            TagFaces();
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
            for (int i = 0; i < Edges.Count; i++)
            {
                var edge = Edges[i];

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

            for (int i = 0; i < edges.Count; i++)
            {
                var edge = edges[i];
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
        public void AppendMesh(HBMesh<VERTEX, EDGE, FACE> mesh, bool incudeFaces = true)
        {
            int vStart = Vertices.Count;
            int eStart = Edges.Count;
            int fStart = Faces.Count;

            mesh.TagAll();

            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                var v = new VERTEX();
                v.Initialize(mesh.Vertices[i]);
                Vertices.Add(v);
            }

            for (int i = 0; i < mesh.Edges.Count; i++)
            {
                var e = new EDGE();
                Edges.Add(e);
            }

            if (incudeFaces)
            {
                for (int i = 0; i < mesh.Faces.Count; i++)
                {
                    var f = new FACE();
                    Faces.Add(f);
                }
            }

            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                var v0 = mesh.Vertices[i];
                var v1 = Vertices[vStart + i];

                var edge = (v0.Edge != null) ? Edges[eStart + v0.Edge.Tag] : null;
                v1.Edge = edge;
            }

            for (int i = 0; i < mesh.Edges.Count; i++)
            {
                var e0 = mesh.Edges[i];
                var e1 = Edges[eStart + i];

                var from = (e0.From != null) ? Vertices[vStart + e0.From.Tag] : null;
                var face = (incudeFaces && e0.Face != null) ? Faces[fStart + e0.Face.Tag] : null;
                var previous = (e0.Previous != null) ? Edges[eStart + e0.Previous.Tag] : null;
                var next = (e0.Next != null) ? Edges[eStart + e0.Next.Tag] : null;
                var opposite = (e0.Opposite != null) ? Edges[eStart + e0.Opposite.Tag]  : null;

                e1.Set(from, face, previous, next, opposite);
            }

            if (incudeFaces)
            {
                for (int i = 0; i < mesh.Faces.Count; i++)
                {
                    var f0 = mesh.Faces[i];
                    var f1 = Faces[fStart + i];

                    var edge = (f0.Edge != null) ? Edges[eStart + f0.Edge.Tag] : null;
                    f1.Edge = edge;
                }
            }
        }

        /// <summary>
        /// Creates a index list representing the vertices of each face.
        /// </summary>
        /// <param name="faceVertices">The number of vertices each face has</param>
        /// <param name="indices">list representing the vertices of each face</param>
        public void GetFaceIndices(List<int> indices, int faceVertices = 3)
        {
            if (faceVertices < 3)
                throw new ArgumentException("faceVertices can not be less than 3.");

            TagVertices();
            List<VERTEX> vertices = new List<VERTEX>(faceVertices);

            int count = Faces.Count;
            for (int i = 0; i < count; i++)
            {
                var face = Faces[i];

                vertices.Clear();
                face.GetVertices(vertices);

                if (vertices.Count != faceVertices)
                    throw new InvalidOperationException("Face does not contain the required number of vertices.");

                for (int j = 0; j < faceVertices; j++)
                    indices.Add(vertices[j].Tag);
            }
        }

        /// <summary>
        /// Creates a index list representing the vertices of each edge.
        /// </summary>
        /// <param name="indices">list representing the vertices of each edge</param>
        public void GetEdgeIndices(List<int> indices)
        {
            TagVertices();
            TagEdges(0);

            int count = Edges.Count;
            for (int i = 0; i < count; i++)
            {
                var edge = Edges[i];
                if (edge.Tag == 1) continue;
                if (edge.Opposite == null) continue;

                indices.Add(edge.From.Tag);
                indices.Add(edge.To.Tag);

                edge.Tag = 1;
                edge.Opposite.Tag = 1;
            }
        }

    }
}
