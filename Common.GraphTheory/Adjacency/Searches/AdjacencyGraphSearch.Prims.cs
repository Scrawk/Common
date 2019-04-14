using System;
using System.Collections.Generic;

using Common.Collections.Queues;

namespace Common.GraphTheory.Adjacency
{
    public static partial class AdjacencyGraphSearch
    {

        public static void PrimsMinimumSpanningTree<VERTEX, EDGE>(AdjacencyGraph<VERTEX, EDGE> graph, AdjacencySearch search, int root)
            where EDGE : class, IAdjacencyEdge, new()
            where VERTEX : class, IAdjacencyVertex, new()
        {
            search.Clear();
            int count = graph.VertexCount;

            search.IsVisited[root] = true;
            search.Parent[root] = root;
            search.Order.Add(root);

            var queue = new BinaryHeap<IAdjacencyEdge>();

            if (graph.Edges[root] != null)
            {
                foreach (var edge in graph.Edges[root])
                    queue.Add(edge);
            }

            while (queue.Count != 0)
            {
                var edge = queue.RemoveFirst();

                int v = edge.To;
                if (search.IsVisited[v]) continue;

                search.Order.Add(v);
                search.IsVisited[v] = true;
                search.Parent[v] = edge.From;

                if (graph.Edges[v] != null)
                {
                    foreach (var e in graph.Edges[v])
                    {
                        if (search.IsVisited[e.To]) continue;
                        queue.Add(e);
                    }
                }

            }

        }
    }
}
