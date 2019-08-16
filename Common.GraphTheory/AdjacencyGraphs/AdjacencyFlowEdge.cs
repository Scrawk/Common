using System;
using System.Collections.Generic;

namespace Common.GraphTheory.AdjacencyGraphs
{
    /// <summary>
    /// A edge used in flow graphs.
    /// Required by algorithms such as min cut 
    /// and max flow. The edges weight is used
    /// to hold the graphs capacity.
    /// </summary>
    public class AdjacencyFlowEdge : AdjacencyEdge
    {

        public AdjacencyFlowEdge()
        {

        }

        public AdjacencyFlowEdge(int from, int to, int capacity)
        {
            Weight = capacity;
            Flow = 0;
            From = from;
            To = to;
        }

        /// <summary>
        /// The current flow along this edge.
        /// </summary>
        public float Flow { get; set; }

        /// <summary>
        /// The remaining capacity at the current flow.
        /// </summary>
        public float Residual { get { return Weight - Flow; } }

        public override string ToString()
        {
            return string.Format("[AdjacencyFlowEdge: From={0}, To={1}, Capacity={2}, Flow={3}]", From, To, Weight, Flow);
        }

    }
}
