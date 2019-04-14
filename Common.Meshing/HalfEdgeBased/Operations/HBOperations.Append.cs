using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Meshing.HalfEdgeBased
{
    public static partial class HBOperations
    {
        /// <summary>
        /// Add opposite edges to all edges that dont have one.
        /// These edges would be considered to be the boundary edges.
        /// Presumes all edges are closed.
        /// </summary>
        public static void AddBoundaryEdges<VERTEX, EDGE, FACE>(HBMesh<VERTEX, EDGE, FACE> mesh)
            where VERTEX : HBVertex, new()
            where EDGE : HBEdge, new()
            where FACE : HBFace, new()
        {
            var Vertices = mesh.Vertices;
            var Edges = mesh.Edges;
            var Faces = mesh.Faces;

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
        /// Append the contents of mesh1 into mesh0 as a deep copy.
        /// </summary>
        /// <param name="mesh0">mesh that get appended to</param>
        /// <param name="mesh1">mesh that gets appended from</param>
        /// <param name="incudeFaces">should the mesh faces also be appended</param>
        public static void Append<VERTEX, EDGE, FACE>(HBMesh<VERTEX, EDGE, FACE> mesh0, HBMesh<VERTEX, EDGE, FACE> mesh1, bool incudeFaces = true)
            where VERTEX : HBVertex, new()
            where EDGE : HBEdge, new()
            where FACE : HBFace, new()
        {
            var Vertices = mesh0.Vertices;
            var Edges = mesh0.Edges;
            var Faces = mesh0.Faces;

            int vStart = Vertices.Count;
            int eStart = Edges.Count;
            int fStart = Faces.Count;

            mesh1.TagAll();

            for (int i = 0; i < mesh1.Vertices.Count; i++)
            {
                var v = new VERTEX();
                v.Initialize(mesh1.Vertices[i]);
                Vertices.Add(v);
            }

            for (int i = 0; i < mesh1.Edges.Count; i++)
            {
                var e = new EDGE();
                Edges.Add(e);
            }

            if (incudeFaces)
            {
                for (int i = 0; i < mesh1.Faces.Count; i++)
                {
                    var f = new FACE();
                    Faces.Add(f);
                }
            }

            for (int i = 0; i < mesh1.Vertices.Count; i++)
            {
                var v0 = mesh1.Vertices[i];
                var v1 = Vertices[vStart + i];

                var edge = (v0.Edge != null) ? Edges[eStart + v0.Edge.Tag] : null;
                v1.Edge = edge;
            }

            for (int i = 0; i < mesh1.Edges.Count; i++)
            {
                var e0 = mesh1.Edges[i];
                var e1 = Edges[eStart + i];

                var from = (e0.From != null) ? Vertices[vStart + e0.From.Tag] : null;
                var face = (incudeFaces && e0.Face != null) ? Faces[fStart + e0.Face.Tag] : null;
                var previous = (e0.Previous != null) ? Edges[eStart + e0.Previous.Tag] : null;
                var next = (e0.Next != null) ? Edges[eStart + e0.Next.Tag] : null;
                var opposite = (e0.Opposite != null) ? Edges[eStart + e0.Opposite.Tag] : null;

                e1.Set(from, face, previous, next, opposite);
            }

            if (incudeFaces)
            {
                for (int i = 0; i < mesh1.Faces.Count; i++)
                {
                    var f0 = mesh1.Faces[i];
                    var f1 = Faces[fStart + i];

                    var edge = (f0.Edge != null) ? Edges[eStart + f0.Edge.Tag] : null;
                    f1.Edge = edge;
                }
            }
        }
    }
}
