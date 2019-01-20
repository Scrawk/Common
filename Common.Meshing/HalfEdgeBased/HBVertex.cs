using System;
using System.Xml;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Meshing.HalfEdgeBased
{
    /// <summary>
    /// A half edge vertex. Presumes edges are connected in CCW order.
    /// </summary>
    public class HBVertex
    {
        /// <summary>
        /// The vertex edge.
        /// </summary>
        public HBEdge Edge { get; set; }

        public HBVertex()
        {

        }

        public virtual void Initialize(Vector2f pos)
        {
    
        }

        public virtual void Initialize(Vector3f pos)
        {

        }

        public virtual void Transform(Matrix2x2f m)
        {

        }

        public virtual void Transform(Matrix3x3f m)
        {

        }

        public EDGE GetEdge<EDGE>() where EDGE : HBEdge
        {
            if (Edge == null) return null;

            EDGE edge = Edge as EDGE;
            if (edge == null)
                throw new InvalidCastException("Edge is not a " + typeof(EDGE));

            return edge;
        }

        public string ToString(HBMesh<HBVertex, HBEdge, HBFace> mesh)
        {
            return string.Format("[HBVertex: Edge={0}]", mesh.IndexOf(Edge));
        }

        /// <summary>
        /// The number of edges connecting to this vertex.
        /// Edges must have a opposite member.
        /// </summary>
        public int EdgeCount
        {
            get
            {
                HBEdge start = Edge;
                HBEdge e = start;
                int count = 0;

                do
                {
                    if (e == null) return count;
                    count++;
                    if (e.Opposite == null) return count;
                    e = e.Opposite.Previous;
                }
                while (!ReferenceEquals(start, e));

                return count;
            }
        }

        /// <summary>
        /// Enumerate all edges connected to this vertex.
        /// Edges must have a opposite member.
        /// </summary>
        /// <param name="ccw"></param>
        /// <returns></returns>
        public IEnumerable<HBEdge> EnumerateEdges(bool ccw = true)
        {
            HBEdge start = Edge;
            HBEdge e = start;

            do
            {
                if (e == null) yield break;
                yield return e;

                if(ccw)
                {
                    if (e.Opposite == null) yield break;
                    e = e.Opposite.Previous;
                }
                else
                {
                    if (e.Next == null) yield break;
                    e = e.Next.Opposite;
                }
            }
            while (!ReferenceEquals(start, e));
        }

        /// <summary>
        /// Clear vertex of all connections.
        /// </summary>
        public virtual void Clear()
        {
            Edge = null;
        }

        /// <summary>
        /// Will remove edge from vertex.
        /// If edge is this vertexs edge the vertex will 
        /// connect to any other edge connecting to vertex.
        /// </summary>
        /// <param name="edge"></param>
        public void RemoveEdge(HBEdge edge)
        {
            if (Edge == null) return;
            if (!ReferenceEquals(edge, Edge)) return;

            HBEdge tmp = null;
            foreach (var e in EnumerateEdges())
            {
                if (!ReferenceEquals(edge, e))
                {
                    tmp = e;
                    break;
                }
            }

            Edge = tmp;
        }

    }
}
