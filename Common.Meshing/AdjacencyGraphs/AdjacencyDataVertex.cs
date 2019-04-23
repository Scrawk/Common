using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Meshing.AdjacencyGraphs
{
    /// <summary>
    /// A adjacency vertex with a data object of any type.
    /// </summary>
    /// <typeparam name="T">The type of object the data represents</typeparam>
    public class AdjacencyDataVertex<T> : IAdjacencyVertex
    {

        public AdjacencyDataVertex()
        {
            Index = -1;
        }

        public AdjacencyDataVertex(int index, T data)
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
        public T Data { get; set; }

        public override string ToString()
        {
            return string.Format("[AdjacencyDataVertex: Index={0}, Cost={1}]", Index, Cost);
        }

        /// <summary>
        /// Used to sort vertices by cost.
        /// </summary>
        public int CompareTo(IAdjacencyVertex other)
        {
            return Cost.CompareTo(other.Cost);
        }
    }
}
