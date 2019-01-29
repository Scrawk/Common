using System;
using System.Collections.Generic;

namespace Common.Meshing.HalfEdgeBased
{
    public static class HBMeshOperations
    {

        /// <summary>
        /// Splits a edge. Creates a new vertex at split 
        /// position os edge and connects newly created edges.
        /// </summary>
        /// <param name="mesh">parent mesh</param>
        /// <param name="edge">the edge to split</param>
        /// <param name="t">the point to split at</param>
        public static VERTEX SplitEdge<VERTEX, EDGE, FACE>(HBMesh<VERTEX, EDGE, FACE> mesh, EDGE edge, float t = 0.5f)
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

            VERTEX v = mid as VERTEX;

            mesh.Edges.Add(right1);
            mesh.Edges.Add(left1);
            mesh.Vertices.Add(v);

            return v;
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
