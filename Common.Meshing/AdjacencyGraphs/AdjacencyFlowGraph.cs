using System;
using System.Collections.Generic;

namespace Common.Meshing.AdjacencyGraphs
{
    /// <summary>
    /// A flow graph created by min cut and max flow 
    /// algorithms such as FordFulk.
    /// </summary>
    public class AdjacencyFlowGraph<VERTEX> : AdjacencyGraph<VERTEX, AdjacencyFlowEdge>
        where VERTEX : class, IAdjacencyVertex, new()
    {

        private bool[] m_minCut;

        internal AdjacencyFlowGraph(IEnumerable<VERTEX> vertices, int source, int sink, float maxFlow) : base(vertices)
        {
            Source = source;
            Sink = sink;
            MaxFlow = maxFlow;

            m_minCut = new bool[VertexCount];
        }

        public float MaxFlow { get; private set; }

        public int Source { get; private set; }

        public int Sink { get; private set; }

        public override string ToString()
        {
            return string.Format("[AdjacencyFlowGraph: VertexCount={0}, EdgeCount={1}, MaxFlow={2}, Source={3}, Sink={4}]",
                VertexCount, EdgeCount, MaxFlow, Source, Sink);
        }

        public bool InSourceCut(int i)
        {
            return m_minCut[i];
        }

        public bool InSinkCut(int i)
        {
            return !m_minCut[i];
        }

        internal void CalculateMinCut()
        {
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(Source);

            Array.Clear(m_minCut, 0, VertexCount);

            m_minCut[Source] = true;

            while (queue.Count > 0)
            {
                int u = queue.Dequeue();

                var edges = Edges[u];
                if (edges == null) continue;

                for (int i = 0; i < edges.Count; i++)
                {
                    int to = edges[i].To;
                    if (edges[i].Residual <= 0 || m_minCut[to]) continue;

                    m_minCut[to] = true;
                    queue.Enqueue(to);
                }
            }

        }
    }
}
