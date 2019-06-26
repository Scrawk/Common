using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Meshing.HalfEdgeBased
{
    public static partial class HBOperations
    {

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
        private static void SetFrom(HBEdge edge, HBVertex vert)
        {
            edge.From = vert;
            vert.Edge = edge;
        }

        /// <summary>
        /// Sets the tag for the edge and its opposite.
        /// </summary>
        private static void TagEdgeAndOpposite(HBEdge edge, int tag)
        {
            if (edge == null) return;

            edge.Tag = tag;

            if (edge.Opposite != null)
                edge.Opposite.Tag = tag;
        }
    }
}
