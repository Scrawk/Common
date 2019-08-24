using System;
using System.Collections.Generic;

namespace Common.GraphTheory.AdjacencyGraphs
{

    public sealed class GraphVertex : IComparable<GraphVertex>
    {

        public GraphVertex()
        {
            Index = -1;
        }

        public GraphVertex(int index)
        {
            Index = index;
        }

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
        ///
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
            return string.Format("[GraphVertex: Index={0}, Cost={1}]", Index, Cost);
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













