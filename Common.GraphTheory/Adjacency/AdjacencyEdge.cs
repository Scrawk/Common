using System;
using System.Collections.Generic;

namespace Common.GraphTheory.Adjacency
{

    public interface IAdjacencyEdge : IComparable<IAdjacencyEdge>
    {
        int From { get; set; }

        int To { get; set; }

        float Weight { get; set; }
    }

	public class AdjacencyEdge : IAdjacencyEdge
	{

        public int From { get; set; }

        public int To { get; set; }

        public float Weight { get; set; }

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

        public override string ToString ()
		{
			return string.Format ("[AdjacencyEdge: From={0}, To={1}, Weight={2}]", From, To, Weight);
		}

        public int CompareTo(IAdjacencyEdge other)
        {
            return Weight.CompareTo(other.Weight);
        }
    }

}













