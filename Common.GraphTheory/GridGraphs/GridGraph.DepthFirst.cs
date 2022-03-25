using System;
using System.Collections.Generic;

using Common.Core.Directions;
using Common.Core.Numerics;

namespace Common.GraphTheory.GridGraphs
{
    public partial class GridGraph
    {
        public void DepthFirst(GridSearch search, int x, int y)
        {
            int width = Width;
            int height = Height;

            Stack<Point2i> queue = new Stack<Point2i>();
            queue.Push(new Point2i(x, y));

            search.SetParent(x, y, new Point2i(x, y));
            search.SetIsVisited(x, y, true);;

            while (queue.Count != 0)
            {
                Point2i u = queue.Pop();
                search.AddOrder(u);

                int edge = Edges[u.x, u.y];

                for (int i = 0; i < 8; i++)
                {
                    int xi = u.x + D8.OFFSETS[i, 0];
                    int yi = u.y + D8.OFFSETS[i, 1];

                    if (xi < 0 || xi > width - 1) continue;
                    if (yi < 0 || yi > height - 1) continue;

                    if ((edge & 1 << i) == 0) continue;
                    if (search.GetIsVisited(xi, yi)) continue;

                    queue.Push(new Point2i(xi, yi));
                    search.SetIsVisited(xi, yi, true);
                    search.SetParent(xi, yi, u);
                }
            }

        }
    }
}
