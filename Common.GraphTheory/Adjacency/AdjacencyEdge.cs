using System;
using System.Collections.Generic;

namespace Common.GraphTheory.Adjacency
{

    public interface IAdjacencyEdge
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
	}

    public class AdjacencyEdgeComparer<EDGE> : IComparer<EDGE>
        where EDGE : class, IAdjacencyEdge, new()
    {
        public static AdjacencyEdgeComparer<EDGE> Instance { get; private set; }

        static AdjacencyEdgeComparer()
        {
            Instance = new AdjacencyEdgeComparer<EDGE>();
        }

        public int Compare(EDGE e0, EDGE e1)
        {
            return e0.Weight.CompareTo(e1.Weight);
        }
    }

}













