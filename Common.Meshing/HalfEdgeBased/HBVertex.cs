using System;
using System.Xml;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Meshing.HalfEdgeBased
{
    /// <summary>
    /// A half edge vertex. Presumes edges are connected in CCW order.
    /// </summary>
    public abstract class HBVertex
    {
        /// <summary>
        /// The dimension of the vertex, ie 2D, 3D.
        /// </summary>
        public abstract int Dimension { get; }

        /// <summary>
        /// 
        /// </summary>
        public int Tag;

        /// <summary>
        /// The vertex edge.
        /// </summary>
        public HBEdge Edge;

        public HBVertex()
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
        /// A vertex is closed if all its edges have a face
        /// </summary>
        public bool IsClosed
        {
            get
            {
                HBEdge start = Edge;
                HBEdge e = start;

                do
                {
                    if (e == null) return false;
                    if (e.Previous == null) return false;
                    if (e.Previous.Opposite == null) return false;
                    if (e.Previous.Opposite.Face == null) return false;
                    e = e.Previous.Opposite;
                }
                while (!ReferenceEquals(start, e));

                return true;
            }
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
        public IEnumerable<HBEdge> EnumerateEdges(bool forwards = true)
        {
            HBEdge start = Edge;
            HBEdge e = start;

            do
            {
                if (e == null) yield break;
                yield return e;

                if(forwards)
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

        public abstract void SetPosition(HBVertex vertex);

        public abstract void SetPosition(Vector3d pos);

        public abstract Vector3d GetPosition();

        /// <summary>
        /// Compute the vertices centroid from the 
        /// edges to vertex surrounding it.
        /// </summary>
        public Vector3d GetCentriod()
        {
            int count = 0;
            Vector3d centroid = Vector3d.Zero;
            foreach (var edge in EnumerateEdges())
            {
                centroid += edge.To.GetPosition();
                count++;
            }

            if (count != 0)
                centroid /= count;

            return centroid;
        }

        /// <summary>
        /// Compute the vertices area weighted normal. 
        /// </summary>
        public Vector3d GetNormal()
        {
            var n = Vector3d.Zero;

            foreach (var e in EnumerateEdges())
            {
                var p0 = e.From.GetPosition();
                var p1 = e.To.GetPosition();
                var p2 = e.Opposite.Next.To.GetPosition();
                n = Vector3d.Cross(p2 - p0, p1 - p0);
            }

            return n.Normalized;
        }

    }
}
