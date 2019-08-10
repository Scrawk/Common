using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Meshing.AdjacencyGraphs
{
    public static partial class AdjacencyGraphSearch
    {
        public static AdjacencyFlowGraph<VERTEX> FordFulkersonMaxFlow<VERTEX, EDGE>(AdjacencyGraph<VERTEX, EDGE> graph, int source, int sink)
            where EDGE : class, IAdjacencyEdge, new()
            where VERTEX : class, IAdjacencyVertex, new()
        {
            int count = graph.VertexCount;
            List<AdjacencyFlowEdge>[] edges = CreateFlowEdges(graph);

            int[] parent = new int[count];
            for (int i = 0; i < count; i++)
                parent[i] = -1;

            float maxFlow = 0;

            while (BreadthFirstSearch(edges, parent, source, sink))
            {
                float flow = float.PositiveInfinity;
                for (int v = sink; v != source; v = parent[v])
                {
                    int u = parent[v];

                    if (u == v)
                        throw new InvalidOperationException("Did not stop at source.");

                    var edge = FindEdge(edges, u, v);
                    flow = Math.Min(flow, edge.Residual);
                }

                if (flow == float.PositiveInfinity)
                    throw new InvalidOperationException("Could not find path flow.");

                maxFlow += flow;

                for (int v = sink; v != source; v = parent[v])
                {
                    int u = parent[v];
                    var edge = FindEdge(edges, u, v);
                    var redge = FindEdge(edges, v, u);

                    edge.Flow += flow;
                    redge.Flow -= flow;
                }
            }

            var flowGraph = new AdjacencyFlowGraph<VERTEX>(graph.Vertices, source, sink, maxFlow);

            for (int i = 0; i < count; i++)
            {
                if (edges[i] == null || edges[i].Count == 0) continue;

                for (int j = 0; j < edges[i].Count; j++)
                {
                    if (edges[i][j].Flow > 0)
                        flowGraph.AddEdge(edges[i][j]);
                }
            }

            flowGraph.CalculateMinCut();

            return flowGraph;
        }

        private static AdjacencyFlowEdge FindEdge(List<AdjacencyFlowEdge>[] flow, int from, int to)
        {
            if (flow[from] == null) return null;
            var edges = flow[from];
            int count = edges.Count;

            for (int i = 0; i < count; i++)
                if (edges[i].To == to) return edges[i];

            return null;
        }

        private static bool BreadthFirstSearch(List<AdjacencyFlowEdge>[] flow, int[] parent, int source, int sink)
        {

            Queue<int> queue = new Queue<int>();
            queue.Enqueue(source);

            parent[source] = source;

            bool[] isVisited = new bool[parent.Length];
            isVisited[source] = true;

            while (queue.Count != 0)
            {
                int u = queue.Dequeue();

                var edges = flow[u];
                if (edges == null) continue;

                for (int i = 0; i < edges.Count; i++)
                {
                    int to = edges[i].To;

                    if (isVisited[to] || edges[i].Residual <= 0) continue;

                    queue.Enqueue(to);
                    parent[to] = u;
                    isVisited[to] = true;

                    if (to == sink) return true;
                }
            }

            return false;
        }

        private static List<AdjacencyFlowEdge>[] CreateFlowEdges<VERTEX, EDGE>(AdjacencyGraph<VERTEX, EDGE> graph)
            where EDGE : class, IAdjacencyEdge, new()
            where VERTEX : class, IAdjacencyVertex, new()
        {
            int count = graph.VertexCount;
            var flow = new List<AdjacencyFlowEdge>[count];

            for (int i = 0; i < count; i++)
            {
                var edges = graph.Edges[i];
                if (edges == null || edges.Count == 0) continue;

                flow[i] = new List<AdjacencyFlowEdge>(edges.Count);

                for (int j = 0; j < edges.Count; j++)
                {
                    int capacity = (int)edges[j].Weight;
                    int from = edges[j].From;
                    int to = edges[j].To;

                    if (capacity < 0)
                        throw new InvalidOperationException("Edge weight must not be negative.");

                    flow[i].Add(new AdjacencyFlowEdge(from, to, capacity));
                }
            }

            for (int i = 0; i < count; i++)
            {
                var edges = flow[i];
                if (edges == null || edges.Count == 0) continue;

                for (int j = 0; j < edges.Count; j++)
                {
                    int from = edges[j].From;
                    int to = edges[j].To;

                    var redge = FindEdge(flow, to, from);

                    if (flow[to] == null)
                        flow[to] = new List<AdjacencyFlowEdge>();

                    if (redge == null)
                        flow[to].Add(new AdjacencyFlowEdge(to, from, 0));
                }
            }

            return flow;
        }
    }
}
