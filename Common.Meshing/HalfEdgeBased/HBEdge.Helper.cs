using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Meshing.HalfEdgeBased
{
    public sealed partial class HBEdge
    {

        /// <summary>
        /// Create a new edge from two half edges.
        /// </summary>
        public static void NewEdge(out HBEdge e1, out HBEdge e2)
        {
            e1 = new HBEdge();
            e2 = new HBEdge();
            e1.Opposite = e2;
            e2.Opposite = e1;
        }

        /// <summary>
        /// Inserts edge between previous and next.
        /// </summary>
        public static void InsertBetween(HBEdge edge, HBEdge previous, HBEdge next)
        {
            edge.Previous = previous;
            previous.Next = edge;
            edge.Next = next;
            next.Previous = edge;
        }

        /// <summary>
        /// Sets the edges vertex and vertex edge.
        /// </summary>
        public static void SetFrom(HBEdge edge, HBVertex vert)
        {
            edge.From = vert;
            vert.Edge = edge;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void SetNext(HBEdge edge, HBEdge next)
        {
            edge.Next = next;
            next.Previous = edge;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void SetPrevious(HBEdge edge, HBEdge previous)
        {
            edge.Previous = previous;
            previous.Next = edge;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void SetFace(HBEdge edge, HBFace face)
        {
            edge.Face = face;
            face.Edge = edge;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void SetFaces(HBEdge edge, HBFace face)
        {
            foreach (var e in edge.EnumerateEdges())
                SetFace(e, face);
        }

        /// <summary>
        /// Sets the tag for the edge and its opposite.
        /// </summary>
        public static void TagEdgeAndOpposite(HBEdge edge, int tag)
        {
            if (edge == null) return;

            edge.Tag = tag;

            if (edge.Opposite != null)
                edge.Opposite.Tag = tag;
        }
    }
}
