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

        public virtual void Initialize(HBVertex vertex)
        {

        }

        public virtual void Initialize(Vector2f pos)
        {
    
        }

        public virtual void Initialize(Vector3f pos)
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

        /// <summary>
        /// Convert vertex to string.
        /// </summary>
        /// <param name="mesh">Parent mesh</param>
        /// <returns>Vertex as string</returns>
        public virtual string ToString<VERTEX, EDGE, FACE>(HBMesh<VERTEX, EDGE, FACE> mesh)
            where VERTEX : HBVertex, new()
            where EDGE : HBEdge, new()
            where FACE : HBFace, new()
        {
            return string.Format("[HBVertex: Id={0}, Edge={1}]", mesh.IndexOf(this), mesh.IndexOf(Edge));
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
                    if (e.Previous == null) return count;
                    e = e.Previous.Opposite;
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
                    if (e.Previous == null) yield break;
                    e = e.Previous.Opposite;
                }
                else
                {
                    if (e.Opposite == null) yield break;
                    e = e.Opposite.Next;
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
        /// Create a vertex that is a interpolation 
        /// from this to the other vertex.
        /// </summary>
        /// <param name="to">other vertex</param>
        /// <returns>interpolation< between the two/returns>
        public virtual HBVertex Interpolate(HBVertex to, float t)
        {
            return new HBVertex();
        }

    }
}
