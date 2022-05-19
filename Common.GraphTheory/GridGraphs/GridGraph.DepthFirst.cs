using System;
using System.Collections.Generic;

using Common.Core.Directions;
using Common.Core.Numerics;

namespace Common.GraphTheory.GridGraphs
{
    public partial class GridGraph
    {
        public GridSearch DepthFirst(Point2i root)
        {
            var search = new GridSearch(Width, Height);
            DepthFirst(search, root.x, root.y);
            return search;
        }

        public GridSearch DepthFirst(int x, int y)
        {
            var search = new GridSearch(Width, Height);
            DepthFirst(search, x, y);
            return search;
        }

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

                for (int j = 0; j < Directions.Count; j++)
                {
                    int i = Directions[j];
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
