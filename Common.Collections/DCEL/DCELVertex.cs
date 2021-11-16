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

        internal DCELVertex()
        {

        }

        internal DCELVertex(Vector3d point)
        {
            Point = point;
        }

        /// <summary>
        /// The vertex position.
        /// </summary>
        public Vector3d Point;

        /// <summary>
        /// Used for temporary making the vertex.
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// The vertices index in the mesh.
        /// </summary>
        public int Index { get; internal set; }

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
        public int EdgeCount
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


    }
}
