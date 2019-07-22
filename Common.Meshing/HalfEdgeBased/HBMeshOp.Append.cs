using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Meshing.HalfEdgeBased
{
    public static partial class HBMeshOp
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
        /// Append the contents of dest into source as a deep copy.
        /// </summary>
        /// <param name="source">mesh that get appended to</param>
        /// <param name="dest">mesh that gets appended from</param>
        /// <param name="incudeFaces">should the mesh faces also be appended</param>
        public static void Append<VERTEX, EDGE, FACE>(HBMesh<VERTEX, EDGE, FACE> source, HBMesh<VERTEX, EDGE, FACE> dest, bool incudeFaces)
            where VERTEX : HBVertex, new()
            where EDGE : HBEdge, new()
            where FACE : HBFace, new()
        {
            var Vertices = dest.Vertices;
            var Edges = dest.Edges;
            var Faces = dest.Faces;

            int vStart = Vertices.Count;
            int eStart = Edges.Count;
            int fStart = Faces.Count;

            source.TagAll();

            for (int i = 0; i < source.Vertices.Count; i++)
            {
                var v = new VERTEX();
                v.SetPosition(source.Vertices[i]);
                Vertices.Add(v);
            }

            for (int i = 0; i < source.Edges.Count; i++)
            {
                var e = new EDGE();
                Edges.Add(e);
            }

            if (incudeFaces)
            {
                for (int i = 0; i < source.Faces.Count; i++)
                {
                    var f = new FACE();
                    Faces.Add(f);
                }
            }

            for (int i = 0; i < source.Vertices.Count; i++)
            {
                var v0 = source.Vertices[i];
                var v1 = Vertices[vStart + i];

                var edge = (v0.Edge != null) ? Edges[eStart + v0.Edge.Tag] : null;
                v1.Edge = edge;
            }

            for (int i = 0; i < source.Edges.Count; i++)
            {
                var e0 = source.Edges[i];
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
                for (int i = 0; i < source.Faces.Count; i++)
                {
                    var f0 = source.Faces[i];
                    var f1 = Faces[fStart + i];

                    var edge = (f0.Edge != null) ? Edges[eStart + f0.Edge.Tag] : null;
                    f1.Edge = edge;
                }
            }
        }
    }
}
