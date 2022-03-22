﻿using System;
using System.Collections.Generic;

using Common.Core.Directions;
using Common.Core.Numerics;

namespace Common.GraphTheory.GridGraphs
{
    public partial class GridGraph
    {
        public void BreadthFirst(GridSearch search, int x, int y)
        {
            int width = Width;
            int height = Height;

            Queue<Point2i> queue = new Queue<Point2i>();
            queue.Enqueue(new Point2i(x, y));

            search.Parent[x, y] = new Point2i(x, y);
            search.IsVisited[x, y] = true;

            while (queue.Count != 0)
            {
                Point2i u = queue.Dequeue();
                search.Order.Add(u);

                int edge = Edges[u.x, u.y];

                for (int i = 0; i < 8; i++)
                {
                    int xi = u.x + D8.OFFSETS[i, 0];
                    int yi = u.y + D8.OFFSETS[i, 1];

                    if (xi < 0 || xi > width - 1) continue;
                    if (yi < 0 || yi > height - 1) continue;

                    if ((edge & 1 << i) == 0) continue;
                    if (search.IsVisited[xi, yi]) continue;

                    queue.Enqueue(new Point2i(xi, yi));
                    search.IsVisited[xi, yi] = true;
                    search.Parent[xi, yi] = u;
                }
            }

        }
    }
}