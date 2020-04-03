using System;
using System.Linq;
using System.Collections.Generic;

using Common.Core.Directions;
using Common.Core.Numerics;

namespace Common.GraphTheory.GridGraphs
{
    public static partial class GridGraphSearch
    {
        private struct AStarNode
        {
            public int x, y;
            public float g, h, f;

            public AStarNode(int x, int y)
            {
                this.x = x;
                this.y = y;
                g = h = f = 0;
            }
        }

        public static void AStar(GridGraph graph, GridSearch search, Vector2i start, Vector2i target, Func<Vector2i, Vector2i, float> GetWeight = null)
        {
            int width = graph.Width;
            int height = graph.Height;

            if (GetWeight == null)
                GetWeight = ManhattanDistance;

            search.Parent[start.x, start.y] = start;

            var open = new List<AStarNode>();
            open.Add(new AStarNode(start.x, start.y));

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
                        var n = new AStarNode(xi, yi);
                        n.g = g;
                        n.h = GetWeight(target, new Vector2i(xi, yi));
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

        public static float EuclideanDistance(Vector2i a, Vector2i b)
        {
            return (float)Vector2i.Distance(a, b);
        }

        public static float ManhattanDistance(Vector2i a, Vector2i b)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
        }

        public static float ChebyshevDistance(Vector2i a, Vector2i b)
        {
            return Math.Max(Math.Abs(a.x - b.x), Math.Abs(a.y - b.y));
        }

        private static int Contains(int x, int y, List<AStarNode> open)
        {
            for (int i = 0; i < open.Count; i++)
                if (open[i].x == x && open[i].y == y) return i;

            return -1;
        }
    }
}
