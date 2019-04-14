using System;
using System.Linq;
using System.Collections.Generic;

using Common.Core.Directions;
using Common.Core.LinearAlgebra;

namespace Common.GraphTheory.Grids
{
    public static partial class GridGraphSearch
    {
        private struct Node
        {
            public int x, y, g, h, f;

            public Node(int x, int y)
            {
                this.x = x;
                this.y = y;
                g = h = f = 0;
            }
        }

        public static void AStar(GridGraph graph, GridSearch search, Vector2i start, Vector2i target)
        {
            search.Clear();
            int width = graph.Width;
            int height = graph.Height;

            search.Parent[start.x, start.y] = start;

            var open = new List<Node>();
            open.Add(new Node(start.x, start.y));

            int g = 0;

            while (open.Count > 0)
            {
                var lowest = open.Min(n => n.f);
                var u = open.First(n => n.f == lowest);

                open.Remove(u);
                search.IsVisited[u.x, u.y] = true;

                if (search.IsVisited[target.x, target.y]) break;

                int edge = graph.Edges[u.x, u.y];
                g++;

                for (int i = 0; i < 8; i++)
                {
                    int xi = u.x + D8.OFFSETS[i, 0];
                    int yi = u.y + D8.OFFSETS[i, 1];

                    if (xi < 0 || xi > width - 1) continue;
                    if (yi < 0 || yi > height - 1) continue;

                    if ((edge & 1 << i) == 0) continue;
                    if (search.IsVisited[xi, yi]) continue;

                    int idx = Contains(xi, yi, open);
                    if (idx == -1)
                    {
                        var n = new Node(xi, yi);
                        n.g = g;
                        n.h = Distance(target.x, target.y, xi, yi);
                        n.f = n.g + n.h;

                        search.Parent[n.x, n.y] = new Vector2i(u.x, u.y);
                        open.Add(n);
                    }
                    else
                    {
                        var n = open[idx];
                        if (g + n.h < n.f)
                        {
                            n.g = g;
                            n.f = n.g + n.h;
                            search.Parent[n.x, n.y] = new Vector2i(u.x, u.y);
                            open[idx] = n;
                        }
                    }
                }
            }
        }

        private static int Distance(int ax, int ay, int bx, int by)
        {
            return Math.Abs(ax - bx) + Math.Abs(ay - by);
        }

        private static int Contains(int x, int y, List<Node> open)
        {
            for (int i = 0; i < open.Count; i++)
                if (open[i].x == x && open[i].y == y) return i;

            return -1;
        }
    }
}
