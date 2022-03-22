using System;
using System.Linq;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.GraphTheory.AdjacencyGraphs
{
    public partial class DirectedGraph
    {
        private struct AStarNode
        {
            public int idx;
            public float g, h, f;

            public AStarNode(int i)
            {
                idx = i;
                g = h = f = 0;
            }
        }

        public List<int> AStar(int start, int end, Func<GraphVertex, GraphVertex, float> Heuristic)
        {

            TagVertices(NOT_VISITED_TAG);

            var path = new List<int>();
            var open = new List<AStarNode>();
            open.Add(new AStarNode(start));

            bool found = false;

            int g = 0;

            while (open.Count > 0)
            {
                var lowest = open.Min(n => n.f);
                var u = open.First(n => n.f == lowest);

                open.Remove(u);
                path.Add(u.idx);
                base.Vertices[u.idx].Tag = IS_VISITED_TAG;

                if (base.Vertices[end].Tag == IS_VISITED_TAG)
                {
                    found = true;
                    break;
                }

                var edges = base.Edges[u.idx];
                if (edges == null) continue;

                for (int i = 0; i < edges.Count; i++)
                {
                    int to = edges[i].To;
                    if (base.Vertices[to].Tag == IS_VISITED_TAG) continue;

                    int idx = Contains(open, to);
                    if (idx == -1)
                    {
                        var n = new AStarNode(to);
                        n.g = g;
                        n.h = Heuristic(base.Vertices[end], base.Vertices[to]);
                        n.f = n.g + n.h;

                        open.Add(n);
                    }
                    else
                    {
                        var n = open[idx];
                        if (g + n.h < n.f)
                        {
                            n.g = g;
                            n.f = n.g + n.h;

                            open[idx] = n;
                        }
                    }
                }

            }

            if (!found)
                path.Clear();

            return path;
        }

        public static float EuclideanDistance(Vector3f a, Vector3f b)
        {
            return (a - b).Magnitude;
        }

        private static int Contains(List<AStarNode> open, int idx)
        {
            for (int i = 0; i < open.Count; i++)
                if (open[i].idx == idx) return i;

            return -1;
        }
    }
}
