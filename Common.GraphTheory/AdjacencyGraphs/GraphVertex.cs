using System;
using System.Collections.Generic;

namespace Common.GraphTheory.AdjacencyGraphs
{

    /// <summary>
    /// The interface for adjacency graph vertices.
    /// </summary>
    public interface IGraphVertex : IComparable<IGraphVertex>
    {
        /// <summary>
        /// The index of this vertex in the graph.
        /// </summary>
        int Index { get; set; }

        /// <summary>
        /// The cost of this vertex.
        /// Used in some search algorithms.
        /// ie shortest paths.
        /// </summary>
        float Cost { get; set; }
    }

    /// <summary>
    /// A adjacency graph vertex with a data object of any type.
    /// </summary>
    /// <typeparam name="T">The type of object the data represents</typeparam>
    public class GraphVertex<T> : GraphVertex
    {

        public GraphVertex()
        {

        }

        public GraphVertex(int index, T data)
        {
            Index = index;
            Data = data;
        }

        /// <summary>
        /// The vertices data.
        /// </summary>
        public T Data { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            return string.Format("[GraphVertex: Index={0}, Cost={1}, Data={2}]", Index, Cost, Data);
        }

    }

    public class GraphVertex : IGraphVertex
    {

        public GraphVertex()
        {
            Index = -1;
        }

        public GraphVertex(int index)
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
        public int CompareTo(IGraphVertex other)
        {
            return Cost.CompareTo(other.Cost);
        }
    }


}













