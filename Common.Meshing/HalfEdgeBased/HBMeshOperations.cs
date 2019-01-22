using System;
using System.Collections.Generic;

namespace Common.Meshing.HalfEdgeBased
{
    public static class HBMeshOperations
    {

        /// <summary>
        /// Splits a edge.
        /// </summary>
        /// <param name="mesh">parent mesh</param>
        /// <param name="edge">the edge to split</param>
        /// <param name="t">the point to split at</param>
        public static void SplitEdge<VERTEX, EDGE, FACE>(HBMesh<VERTEX, EDGE, FACE> mesh, EDGE edge, float t = 0.5f)
            where VERTEX : HBVertex, new()
            where EDGE : HBEdge, new()
            where FACE : HBFace, new()
        {
            var right0 = edge;
            var left0 = edge.Opposite;
            var leftPrevious = left0.Previous;
            var rightNext = right0.Next;

            var from = right0.From;
            var to = left0.From;
            var mid = from.Interpolate(to, t);

            EDGE right1, left1;
            NewEdge(out right1, out left1);

            SetVertex(right1, mid);
            InsertBetween(right1, right0, rightNext);
            right1.Face = right0.Face;

            SetVertex(left1, to);
            InsertBetween(left1, leftPrevious, left0);
            left1.Face = left0.Face;

            left0.From = mid;

            mesh.Edges.Add(right1);
            mesh.Edges.Add(left1);
            mesh.Vertices.Add(mid as VERTEX);
        }

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
            List<EDGE> edges = null;
            foreach (var edge in mesh.Edges)
            {
                if (edge.Opposite == null)
                {
                    if (edge.Next == null)
                        throw new InvalidOperationException("Edge not closed.");

                    var opp = NewEdge(edge);
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

            mesh.Edges.AddRange(edges);
        }

        /// <summary>
        /// Create a new edge from two half edges.
        /// </summary>
        private static void NewEdge<EDGE>(out EDGE e1, out EDGE e2)
            where EDGE : HBEdge, new()
        {
            e1 = new EDGE();
            e2 = new EDGE();
            e1.Opposite = e2;
            e2.Opposite = e1;
        }

        /// <summary>
        /// Create a new edge from two half edges.
        /// </summary>
        private static EDGE NewEdge<EDGE>(EDGE edge)
            where EDGE : HBEdge, new()
        {
            var opp = new EDGE();
            opp.Opposite = edge;
            edge.Opposite = opp;
            return opp;
        }

        /// <summary>
        /// Inserts edge between previous and next.
        /// </summary>
        private static void InsertBetween(HBEdge edge, HBEdge previous, HBEdge next)
        {
            edge.Previous = previous;
            previous.Next = edge;
            edge.Next = next;
            next.Previous = edge;
        }

        /// <summary>
        /// Sets the edges vertex and vertex edge.
        /// </summary>
        private static void SetVertex(HBEdge edge, HBVertex vert)
        {
            edge.From = vert;
            vert.Edge = edge;
        }

    }
}
