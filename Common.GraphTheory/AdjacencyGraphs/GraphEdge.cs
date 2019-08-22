using System;
using System.Collections.Generic;

namespace Common.GraphTheory.AdjacencyGraphs
{

    /// <summary>
    /// A adjacency graphs edge.
    /// </summary>
    public class GraphEdge : IComparable<GraphEdge>
    {

        public GraphEdge()
        {

        }

        public GraphEdge(int from, int to)
        {
            From = from;
            To = to;
        }

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
        /// The edges weight. Used in some seaarch algorithms.
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













