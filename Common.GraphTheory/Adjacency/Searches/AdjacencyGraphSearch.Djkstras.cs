using System;
using System.Collections.Generic;
using System.Text;

namespace Common.GraphTheory.Adjacency
{
    public static partial class AdjacencyGraphSearch
    {
        public static void DijkstrasShortestPathTree<VERTEX, EDGE>(AdjacencyGraph<VERTEX, EDGE> graph, AdjacencySearch search, int root)
            where EDGE : class, IAdjacencyEdge, new()
            where VERTEX : class, IAdjacencyVertex, new()
        {

            search.Clear();
            int count = graph.VertexCount;

            for (int i = 0; i < count; i++)
                graph.Vertices[i].Cost = float.PositiveInfinity;

            search.IsVisited[root] = true;
            search.Parent[root] = root;
            graph.Vertices[root].Cost = 0;

            var queue = new List<VERTEX>(graph.Vertices);

            while (queue.Count != 0)
            {
                queue.Sort();

                var vertex = queue[0];
                queue.RemoveAt(0);
                int u = vertex.Index;

                search.Order.Add(u);
                search.IsVisited[u] = true;

                if (graph.Edges[u] != null)
                {
                    foreach (var e in graph.Edges[u])
                    {
                        int v = e.To;
                        if (search.IsVisited[v]) continue;

                        float alt = graph.Vertices[u].Cost + e.Weight;

                        if (alt < graph.Vertices[v].Cost)
                        {
                            graph.Vertices[v].Cost = alt;
                            search.Parent[v] = u;
                        }
                    }
                }

            }

        }
    }
}
