using System;
using System.Collections.Generic;

namespace Common.Meshing.AdjacencyGraphs
{
    /// <summary>
    /// The interface for adjacency edges.
    /// </summary>
    public interface IAdjacencyEdge : IComparable<IAdjacencyEdge>
    {
        /// <summary>
        /// The vertex index the edge starts at.
        /// </summary>
        int From { get; set; }

        /// <summary>
        /// The vertex index the edge ends at.
        /// </summary>
        int To { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int Tag { get; set; }

        /// <summary>
        /// The edges weight. Used in some seaarch algorithms.
        /// ie spanning trees.
        /// </summary>
        float Weight { get; set; }
    }

    /// <summary>
    /// A adjacency graphs edge.
    /// </summary>
    public class AdjacencyEdge : IAdjacencyEdge
	{

        public AdjacencyEdge()
        {

        }
		
		public AdjacencyEdge(int from, int to)
		{
			From = from;
			To = to;
		}

        public AdjacencyEdge(int from, int to, float weight)
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
        /// 
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// The edges weight. Used in some seaarch algorithms.
        /// ie spanning trees.
        /// </summary>
        public float Weight { get; set; }

        public override string ToString ()
		{
			return string.Format ("[AdjacencyEdge: From={0}, To={1}, Weight={2}]", From, To, Weight);
		}

        /// <summary>
        /// Used to sort edges by weight.
        /// </summary>
        public int CompareTo(IAdjacencyEdge other)
        {
            return Weight.CompareTo(other.Weight);
        }
    }

}













