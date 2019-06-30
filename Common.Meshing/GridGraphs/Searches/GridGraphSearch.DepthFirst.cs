using System;
using System.Collections.Generic;

using Common.Core.Directions;
using Common.Core.Numerics;

namespace Common.Meshing.GridGraphs
{
    public static partial class GridGraphSearch
    {
        public static void DepthFirst(GridGraph graph, GridSearch search, int x, int y)
        {
            search.Clear();
            int width = graph.Width;
            int height = graph.Height;

            Stack<Vector2i> queue = new Stack<Vector2i>();
            queue.Push(new Vector2i(x, y));

            search.Parent[x, y] = new Vector2i(x, y);
            search.IsVisited[x, y] = true;

            while (queue.Count != 0)
            {
                Vector2i u = queue.Pop();
                search.Order.Add(u);

                int edge = graph.Edges[u.x, u.y];

                for (int i = 0; i < 8; i++)
                {
                    int xi = u.x + D8.OFFSETS[i, 0];
                    int yi = u.y + D8.OFFSETS[i, 1];

                    if (xi < 0 || xi > width - 1) continue;
                    if (yi < 0 || yi > height - 1) continue;

                    if ((edge & 1 << i) == 0) continue;
                    if (search.IsVisited[xi, yi]) continue;

                    queue.Push(new Vector2i(xi, yi));
                    search.IsVisited[xi, yi] = true;
                    search.Parent[xi, yi] = u;
                }
            }

        }
    }
}
