using System;
using System.Collections.Generic;

namespace Common.GraphTheory.AdjacencyGraphs
{

    /// <summary>
    /// A adjacency graphs edge.
    /// </summary>
    public sealed class GraphEdge : IComparable<GraphEdge>
    {

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GraphEdge()
        {

        }

        /// <summary>
        /// Create a edge going from and to these vertex indices.
        /// </summary>
        /// <param name="from">The from vertex index.</param>
        /// <param name="to">The to vertex index.</param>
        public GraphEdge(int from, int to)
        {
            From = from;
            To = to;
        }

        /// <summary>
        /// Create a edge going from and to these vertex indices
        /// with a edge weight.
        /// </summary>
        /// <param name="from">The from vertex index.</param>
        /// <param name="to">The to vertex index.</param>
        /// <param name="weight">The weight of the edge.</param>
        public GraphEdge(int from, int to, float weight)
        {
            From = from;
            To = to;
            Weight = weight;
        }

        /// <summary>
        /// The vertex index the edge starts at.
        /// </summary>
        public int From { get; set; }

        /// <summary>
        /// The vertex index the edge ends at.
        /// </summary>
        public int To { get; set; }

        /// <summary>
        /// The edges weight. Used in some search algorithms.
        /// ie spanning trees.
        /// </summary>
        public float Weight { get; set; }

        /// <summary>
        /// The edges data.
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            return string.Format("[GraphEdge: From={0}, To={1}, Weight={2}]", From, To, Weight);
        }

        /// <summary>
        /// Used to sort edges by weight.
        /// </summary>
        public int CompareTo(GraphEdge other)
        {
            return Weight.CompareTo(other.Weight);
        }
    }

}













