using System;
using System.Collections.Generic;

using Common.GraphTheory.Adjacency;
using Common.GraphTheory.Grids;
using Common.Core.LinearAlgebra;
using Common.Core.Directions;

namespace Common.GraphTheory.Searches
{

    internal static class DijkstrasShortestPathTree
    {

        internal static void Search<VERTEX, EDGE>(AdjacencyGraph<VERTEX, EDGE> graph, AdjacencySearch search, int root, IComparer<VERTEX> comparer)
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
                queue.Sort(comparer);

                var vertex = queue[0];
                queue.RemoveAt(0);
                int u = vertex.Index;

                search.Order.Add(u);
                search.IsVisited[u] = true;

                if (graph.Edges[u] != null)
                {
                    foreach (EDGE e in graph.Edges[u])
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

        internal static void Search(GridGraph graph, GridSearch search, int x, int y, IComparer<GridVertex> comparer)
        {
            search.Clear();
            int width = graph.Width;
            int height = graph.Height;

            search.IsVisited[x, y] = true;
            search.Parent[x, y] = new Vector2i(x, y);

            var queue = graph.GetAllVertices();
            var vertexGrid = new GridVertex[width, height];

            for (int i = 0; i < queue.Count; i++)
            {
                var v = queue[i];
                int xi = v.Index.x;
                int yi = v.Index.y;

                if (xi == x && yi == y)
                    v.Cost = 0;
                else
                    v.Cost = float.PositiveInfinity;

                vertexGrid[xi, yi] = v;
            }
                
            while (queue.Count != 0)
            {
                queue.Sort(comparer);

                var vertex = queue[0];
                queue.RemoveAt(0);
                var u = vertex.Index;

                search.Order.Add(u);
                search.IsVisited[u.x,u.y] = true;

                int edge = graph.Edges[u.x, u.y];

                if (edge != 0)
                {
                    float cost = vertexGrid[u.x, u.y].Cost;

                    for (int i = 0; i < 8; i++)
                    {
                        int xi = x + D8.OFFSETS[i, 0];
                        int yi = y + D8.OFFSETS[i, 1];

                        if (xi < 0 || xi > width - 1) continue;
                        if (yi < 0 || yi > height - 1) continue;

                        if ((edge & 1 << i) == 0) continue;
                        if (search.IsVisited[xi, yi]) continue;

                        float alt = cost + (float)Vector2i.Distance(u, new Vector2i(xi,yi));

                        if (alt < vertexGrid[xi, yi].Cost)
                        {
                            vertexGrid[xi, yi].Cost = alt;
                            search.Parent[xi, yi] = u;
                        }
                    }
                }
            }
        }

    }
}
