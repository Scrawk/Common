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
        /// Find the edge that uses these two vertices.
        /// Presumes all edges have opposites.
        /// </summary>
        /// <param name="v0">The from vertex</param>
        /// <param name="v1">The to vertex</param>
        /// <returns>The edge index or null if not found</returns>
        public EDGE FindEdge(HBVertex v0, HBVertex v1)
        {
            for (int i = 0; i < Edges.Count; i++)
            {
                var edge = Edges[i];
                if (edge.From == null) continue;
                if (edge.To == null) continue;

                if (ReferenceEquals(edge.From, v0) &&
                   ReferenceEquals(edge.To, v1))
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
        /// Remove a face.
        /// Will clear face from all edges.
        /// </summary>
        public void RemoveFace(FACE face)
        {
            Faces.Remove(face);

            int numEdges = Edges.Count;
            for (int i = 0; i < numEdges; i++)
            {
                if (ReferenceEquals(face, Edges[i].Face))
                    Edges[i].Face = null;
            }
        }

    }
}
