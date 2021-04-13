using System;
using System.Collections.Generic;

namespace Common.GraphTheory.AdjacencyGraphs
{

    public sealed class GraphVertex : IComparable<GraphVertex>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public GraphVertex()
        {
            Index = -1;
        }

        /// <summary>
        /// Create a vertex with a index.
        /// </summary>
        /// <param name="index">The index of the vertex in its graph.</param>
        public GraphVertex(int index)
        {
            Index = index;
        }

        /// <summary>
        /// Create a vertex with a index and data.
        /// </summary>
        /// <param name="index">The index of the vertex in its graph.</param>
        /// <param name="data">The vertices data.</param>
        public GraphVertex(int index, object data)
        {
            Index = index;
            Data = data;
        }

        /// <summary>
        /// The index of this vertex in the graph.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        ///Use to tempory mark the vertex.
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// The cost of this vertex.
        /// Used in some search algorithms.
        /// ie shortest paths.
        /// </summary>
        public float Cost { get; set; }

        /// <summary>
        /// The vertices data.
        /// </summary>
        public object Data { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            return string.Format("[GraphVertex: Index={0}, Cost={1}, Data={2}]", 
                Index, Cost, Data != null ? Data.ToString() : "Null");
        }

        /// <summary>
        /// Used to sort vertices by cost.
        /// </summary>
        public int CompareTo(GraphVertex other)
        {
            return Cost.CompareTo(other.Cost);
        }
    }


}













