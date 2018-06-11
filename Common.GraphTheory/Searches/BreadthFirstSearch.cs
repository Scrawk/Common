using System;
using System.Collections.Generic;

using Common.Core.Directions;
using Common.Core.LinearAlgebra;
using Common.GraphTheory.Adjacency;
using Common.GraphTheory.Grids;

namespace Common.GraphTheory.Searches
{
    internal static class BreadthFirstSearch
    {

        internal static void Search<VERTEX, EDGE>(AdjacencyGraph<VERTEX, EDGE> graph, AdjacencySearch search, int root)
            where EDGE : class, IAdjacencyEdge, new()
        {
            int count = graph.VertexCount;

            Queue<int> queue = new Queue<int>();
            queue.Enqueue(root);

            search.Parent[root] = root;

            bool[] isVisited = new bool[count];
            isVisited[root] = true;

            while (queue.Count != 0)
            {
                int u = queue.Dequeue();
                search.Order.Add(u);

                IList<EDGE> edges = graph.Edges[u];
                if (edges == null) continue;

                for (int i = 0; i < edges.Count; i++)
                {
                    int to = edges[i].To;

                    if (isVisited[to]) continue;

                    queue.Enqueue(to);
                    search.Parent[to] = u;
                    isVisited[to] = true;
                }
            }

        }

        internal static void Search(GridGraph graph, GridSearch search, int x, int y)
        {
            int width = graph.Width;
            int height = graph.Height;

            Queue<Vector2i> queue = new Queue<Vector2i>();
            queue.Enqueue(new Vector2i(x, y));

            if(search.Parent != null)
                search.Parent[x, y] = new Vector2i(x, y);

            bool[,] isVisited = new bool[width, height];
            isVisited[x, y] = true;

            while (queue.Count != 0)
            {
                Vector2i u = queue.Dequeue();
                search.Order.Add(u);

                int edge = graph.Edges[u.x, u.y];

                for (int i = 0; i < 8; i++)
                {
                    int xi = u.x + D8.OFFSETS[i, 0];
                    int yi = u.y + D8.OFFSETS[i, 1];

                    if (xi < 0 || xi > width - 1) continue;
                    if (yi < 0 || yi > height - 1) continue;

                    if ((edge & 1 << i) == 0) continue;
                    if (isVisited[xi, yi]) continue;

                    queue.Enqueue(new Vector2i(xi, yi));
                    isVisited[xi, yi] = true;

                    if (search.Parent != null)
                        search.Parent[xi, yi] = u;
                }
            }

        }

    }
}
