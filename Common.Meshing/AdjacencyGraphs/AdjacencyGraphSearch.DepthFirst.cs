using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Meshing.AdjacencyGraphs
{
    public static partial class AdjacencyGraphSearch
    {
        public static void DepthFirst<VERTEX, EDGE>(AdjacencyGraph<VERTEX, EDGE> graph, AdjacencySearch search, int root)
            where EDGE : class, IAdjacencyEdge, new()
            where VERTEX : class, IAdjacencyVertex, new()
        {
            Stack<int> queue = new Stack<int>();
            queue.Push(root);

            search.Parent[root] = root;
            search.IsVisited[root] = true;

            while (queue.Count != 0)
            {
                int u = queue.Pop();
                search.Order.Add(u);

                IList<EDGE> edges = graph.Edges[u];
                if (edges == null) continue;

                for (int i = 0; i < edges.Count; i++)
                {
                    int to = edges[i].To;

                    if (search.IsVisited[to]) continue;

                    queue.Push(to);
                    search.Parent[to] = u;
                    search.IsVisited[to] = true;
                }
            }

        }
    }
}
