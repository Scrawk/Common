using System;
using System.Linq;
using System.Collections.Generic;

using Common.Core.Directions;
using Common.Core.Numerics;

namespace Common.GraphTheory.GridGraphs
{
    public partial class GridGraph
    {
        private struct AStarNode
        {
            public Point2i point;
            public float g, h, f;

            public AStarNode(int x, int y)
            {
                point = new Point2i(x, y);
                g = h = f = 0;
            }
        }

        public GridSearch AStar(Point2i start, Point2i target, Func<Point2i, Point2i, float> Heuristic = null)
        {
            var search = new GridSearch(Width, Height);
            AStar(search, start, target, Heuristic);
            return search;
        }

        public void AStar(GridSearch search, Point2i start, Point2i target, Func<Point2i, Point2i, float> Heuristic = null)
        {
            int width = Width;
            int height = Height;

            if (Heuristic == null)
                Heuristic = ManhattanDistance;

            search.SetParent(start, start);

            var open = new List<AStarNode>();
            open.Add(new AStarNode(start.x, start.y));

            int g = 0;

            while (open.Count > 0)
            {
                var lowest = open.Min(n => n.f);
                var u = open.First(n => n.f == lowest);

                open.Remove(u);
                search.SetIsVisited(u.point, true);

                if (search.GetIsVisited(target)) break;

                int edge = GetEdges(u.point.x, u.point.y);
                g++;

                for (int i = 0; i < 8; i++)
                {
                    int xi = u.point.x + D8.OFFSETS[i, 0];
                    int yi = u.point.y + D8.OFFSETS[i, 1];

                    if (xi < 0 || xi > width - 1) continue;
                    if (yi < 0 || yi > height - 1) continue;

                    if ((edge & 1 << i) == 0) continue;
                    if (search.GetIsVisited(xi, yi)) continue;

                    int idx = Contains(open, xi, yi);
                    if (idx == -1)
                    {
                        var n = new AStarNode(xi, yi);
                        n.g = g;
                        n.h = Heuristic(target, new Point2i(xi, yi));
                        n.f = n.g + n.h;

                        search.SetParent(n.point, u.point);
                        open.Add(n);
                    }
                    else
                    {
                        var n = open[idx];
                        if (g + n.h < n.f)
                        {
                            n.g = g;
                            n.f = n.g + n.h;
                            search.SetParent(n.point, u.point);
                            open[idx] = n;
                        }
                    }
                }
            }
        }

        public static float EuclideanDistance(Point2i a, Point2i b)
        {
            return (float)Point2i.Distance(a, b);
        }

        public static float ManhattanDistance(Point2i a, Point2i b)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
        }

        public static float ChebyshevDistance(Point2i a, Point2i b)
        {
            return Math.Max(Math.Abs(a.x - b.x), Math.Abs(a.y - b.y));
        }

        private static int Contains(List<AStarNode> open, int x, int y)
        {
            for (int i = 0; i < open.Count; i++)
                if (open[i].point.x == x && open[i].point.y == y) return i;

            return -1;
        }
    }
}
