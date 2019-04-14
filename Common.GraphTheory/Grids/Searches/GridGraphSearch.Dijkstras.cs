using System;
using System.Collections.Generic;

using Common.Core.Directions;
using Common.Core.LinearAlgebra;

namespace Common.GraphTheory.Grids
{
    public static partial class GridGraphSearch
    {
        public static void DijkstrasShortestPathTree(GridGraph graph, GridSearch search, int x, int y)
        {
            search.Clear();
            int width = graph.Width;
            int height = graph.Height;

            var queue = new List<GridVertex>(width * height);
            graph.GetAllVertices(queue);
            var vertexGrid = new GridVertex[width, height];

            for (int i = 0; i < queue.Count; i++)
            {
                var v = queue[i];
                int xi = v.Index.x;
                int yi = v.Index.y;
                v.Cost = float.PositiveInfinity;

                vertexGrid[xi, yi] = v;
            }

            search.IsVisited[x, y] = true;
            search.Parent[x, y] = new Vector2i(x, y);
            vertexGrid[x, y].Cost = 0;

            while (queue.Count != 0)
            {
                queue.Sort();

                var vertex = queue[0];
                queue.RemoveAt(0);
                var u = vertex.Index;

                search.Order.Add(u);
                search.IsVisited[u.x, u.y] = true;

                int edge = graph.Edges[u.x, u.y];
                if (edge != 0)
                {
                    float cost = vertexGrid[u.x, u.y].Cost;

                    for (int i = 0; i < 8; i++)
                    {
                        int xi = u.x + D8.OFFSETS[i, 0];
                        int yi = u.y + D8.OFFSETS[i, 1];

                        if (xi < 0 || xi > width - 1) continue;
                        if (yi < 0 || yi > height - 1) continue;

                        if ((edge & 1 << i) == 0) continue;
                        if (search.IsVisited[xi, yi]) continue;

                        var v = vertexGrid[xi, yi];
                        float alt = cost + (float)Vector2i.Distance(u, v.Index);

                        if (alt < v.Cost)
                        {
                            v.Cost = alt;
                            search.Parent[xi, yi] = u;
                        }
                    }
                }
            }
        }
    }
}
