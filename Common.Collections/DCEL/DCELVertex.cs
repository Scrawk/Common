using System;
using System.Text;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Collections.DCEL
{
    /// <summary>
    /// A half edge vertex. Presumes edges are connected in CCW order.
    /// </summary>
    public partial class DCELVertex
    {

        internal DCELVertex(int index, Vector2d point)
        {
            Index = index;
            Point = point;
        }

        /// <summary>
        /// The vertex position.
        /// </summary>
        public Vector2d Point;

        /// <summary>
        /// Used for temporary making the vertex.
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// The vertices index in the mesh.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// The vertex edge.
        /// </summary>
        public DCELHalfedge Edge { get; internal set; }

        /// <summary>
        /// Th vertices optional data.
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Convert vertex to string.
        /// </summary>
        /// <returns>Vertex as string</returns>
        public override string ToString()
        {
            return string.Format("[DCELVertex: Id={0}, Edge={1}, Point={2}]",
                Tag, DCELMesh.IndexOrDefault(Edge), Point);
        }

        /// <summary>
        /// The number of edges connecting to this vertex.
        /// Edges must have a opposite member.
        /// </summary>
        public int Degree
        {
            get
            {
                int count = 0;
                foreach (var edge in EnumerateEdges())
                    count++;

                return count;
            }
        }

        /// <summary>
        /// Clear vertex of all connections.
        /// </summary>
        internal void Clear()
        {
            Data = null;
            Edge = null;
            Index = -1;
        }

        /// <summary>
        /// Enumerate all edges connected to this vertex.
        /// Edges must have a opposite member.
        /// </summary>
        public IEnumerable<DCELHalfedge> EnumerateEdges()
        {
            DCELHalfedge start = Edge;
            DCELHalfedge e = start;

            do
            {
                if (e == null) yield break;
                yield return e;

                if (e.Previous == null) yield break;
                e = e.Previous.Opposite;
            }
            while (!ReferenceEquals(start, e));
        }


        /// <summary>
        /// Enumerate all vertices surrounding this vertex.
        /// Same as enumeration the edges and return the to vertex.
        /// Edges must have a opposite member.
        /// </summary>
        public IEnumerable<DCELVertex> EnumerateVertices()
        {
            DCELHalfedge start = Edge;
            DCELHalfedge e = start;

            do
            {
                if (e == null) yield break;
                yield return e.To;

                if (e.Previous == null) yield break;
                e = e.Previous.Opposite;
            }
            while (!ReferenceEquals(start, e));
        }

        /// <summary>
        /// Find which two edges belonging to this vertex
        /// the point b is inbetween.
        /// </summary>
        /// <param name="b">Any position.</param>
        /// <returns>The edge where b is between and previous.</returns>
        internal DCELHalfedge FindInBetweenEdges(Vector2d b)
        {
            Vector2d zero = Vector2d.Zero;

            foreach (var e in EnumerateEdges())
            {
                if (e.Previous == null)
                    throw new BetweenEdgeNotFoundException("e.Previous == null");

                if (e.Opposite == null)
                    throw new BetweenEdgeNotFoundException("e.Opposite == null");

                var a = e.From.Point;

                var ab = a - b;
                var a0 = a - e.Previous.From.Point;
                var a1 = a - e.Opposite.From.Point;

                ab.Normalize();
                a0.Normalize();
                a1.Normalize();

                if (DCELGeometry.Collinear(a0, zero, a1) && DCELGeometry.Collinear(a0, zero, ab))
                    throw new BetweenEdgeNotFoundException("Collinear");

                if (DCELGeometry.InCone(a0, zero, a1, ab))
                    return e;
            }

            //Will happen if b collinear to a edge around the vertex
            // or if b is same point as vertex.
            throw new BetweenEdgeNotFoundException("Not found");
        }

        /// <summary>
        /// Finds the edge that goes from this vert to the 
        /// other vert or null if there is no such edge.
        /// </summary>
        /// <param name="v">The other vert.</param>
        /// <returns>The connecting edge if found.</returns>
        public DCELHalfedge FindConnectingEdge(DCELVertex v)
        {
            if (Edge == null) return null;
            if (v.Edge == null) return null;

            foreach (var e in EnumerateEdges())
                if (e.To == v) return e;

            return null;
        }

        /// <summary>
        /// Are these two vertices connected by a edge.
        /// </summary>
        /// <param name="v">The other vert.</param>
        /// <returns>True is are connected by a edge</returns>
        public bool AreConnected(DCELVertex v)
        {
            return FindConnectingEdge(v) != null;
        }
    }
}
