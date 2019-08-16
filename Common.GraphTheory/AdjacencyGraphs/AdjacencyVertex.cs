using System;
using System.Collections.Generic;

namespace Common.GraphTheory.AdjacencyGraphs
{

    /// <summary>
    /// The interface for adjacency vertices.
    /// </summary>
    public interface IAdjacencyVertex : IComparable<IAdjacencyVertex>
    {
        /// <summary>
        /// The index of this vertex in the graph.
        /// </summary>
        int Index { get; set; }

        float Cost { get; set; }
    }

    /// <summary>
    /// A adjacency vertex with a data object of any type.
    /// </summary>
    /// <typeparam name="T">The type of object the data represents</typeparam>
    public class AdjacencyVertex<T> : AdjacencyVertex
    {

        public AdjacencyVertex()
        {

        }

        public AdjacencyVertex(int index, T data)
        {
            Index = index;
            Data = data;
        }

        /// <summary>
        /// The vertices data.
        /// </summary>
        public T Data { get; set; }

    }

    public class AdjacencyVertex : IAdjacencyVertex
    {

        public AdjacencyVertex()
        {
            Index = -1;
        }

        public AdjacencyVertex(int index)
        {
            Index = index;
        }

        /// <summary>
        /// The index of this vertex in the graph.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// The cost of this vertex.
        /// Used in some search algorithms.
        /// ie shortest paths.
        /// </summary>
        public float Cost { get; set; }

        public override string ToString()
        {
            return string.Format("[AdjacencyVertex: Index={0}, Cost={1}]", Index, Cost);
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













