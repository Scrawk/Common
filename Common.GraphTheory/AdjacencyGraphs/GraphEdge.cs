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
        /// Create a edge going from and to these vertex indices
        /// with a edge weight and data.
        /// </summary>
        /// <param name="from">The from vertex index.</param>
        /// <param name="to">The to vertex index.</param>
        /// <param name="weight">The weight of the edge.</param>
        /// <param name="data">The edges data</param>
        public GraphEdge(int from, int to, float weight, object data)
        {
            From = from;
            To = to;
            Weight = weight;
            Data = data;
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
            return string.Format("[GraphEdge: From={0}, To={1}, Weight={2}, Data={3}]", 
                From, To, Weight, Data != null ? Data.ToString() : "Null");
        }

        /// <summary>
        /// Used to sort edges by weight.
        /// </summary>
        public int CompareTo(GraphEdge other)
        {
            return Weight.CompareTo(other.Weight);
        }


        /// <summary>
        /// Create deep copy of edge.
        /// </summary>
        /// <param name="dataCopy">Function to copy data. If null shallow copy will be used.</param>
        /// <returns>A copy of the edge.</returns>
        public GraphEdge Copy(Func<object, object> dataCopy = null)
        {
            GraphEdge copy = new GraphEdge();
            copy.From = From;
            copy.To = To;
            copy.Weight = Weight;

            if(dataCopy != null)
                copy.Data = dataCopy.Invoke(Data);
            else
                copy.Data = Data;

            return copy;
        }
    }

}













