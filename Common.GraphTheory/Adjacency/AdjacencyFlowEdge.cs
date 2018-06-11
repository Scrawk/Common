using System;
using System.Collections.Generic;

namespace Common.GraphTheory.Adjacency
{
    public class AdjacencyFlowEdge : AdjacencyEdge
    {

        public float Flow { get; set; }

        public float Residual {  get { return Weight - Flow; } }

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

        public override string ToString()
        {
            return string.Format("[AdjacencyFlowEdge: From={0}, To={1}, Capacity={2}, Flow={3}]", From, To, Weight, Flow);
        }

    }
}
