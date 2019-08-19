using System;
using System.Collections.Generic;

namespace Common.GraphTheory.AdjacencyGraphs
{
    /// <summary>
    /// 
    /// </summary>
    public class GraphOrdering
    {

        public GraphOrdering()
        {
            Vertices = new List<int>();
        }

        public GraphOrdering(int size)
        {
            Vertices = new List<int>(size);
        }

    	/// <summary>
    	/// 
    	/// </summary>
        public int Count { get { return Vertices.Count; } }

        /// <summary>
        /// 
        /// </summary>
        public int First => (Count > 0) ? Vertices[0] : -1;

        /// <summary>
        /// 
        /// </summary>
        public int Last => (Count > 0) ? Vertices[Count - 1] : -1;

    	/// <summary>
    	/// 
    	/// </summary>
        public List<int> Vertices { get; private set; }

    	/// <summary>
    	/// 
    	/// </summary>
        public override string ToString()
        {
            return string.Format("[GraphOrdering: Count={0}, First={1}]", Count, First);
        }

    }
}
