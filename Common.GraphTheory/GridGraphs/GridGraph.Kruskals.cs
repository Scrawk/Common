using System;
using System.Collections.Generic;

using Common.Core.Directions;
using Common.Collections.Sets;
using Common.Core.Numerics;

namespace Common.GraphTheory.GridGraphs
{
    public partial class GridGraph
    {
        public Dictionary<Point2i, List<GridEdge>> KruskalsMinimumSpanningForest(Func<Point2i, Point2i, float> GetWeight)
        {
            int width = Width;
            int height = Height;

            DisjointGridSet2 set = new DisjointGridSet2(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    set.Add(x, y, x, y);
                }
            }

            var sorted = new List<GridEdge>(width * height);
            GetAllEdges(sorted, GetWeight);
            sorted.Sort();

            int edgeCount = sorted.Count;
            List<GridEdge> edges = new List<GridEdge>();

            for (int i = 0; i < edgeCount; i++)
            {
                Point2i from = sorted[i].From;
                Point2i to = sorted[i].To;

                if (set.Union(to.x, to.y, from.x, from.y))
                    edges.Add(sorted[i]);
            }

            Dictionary<Point2i, List<GridEdge>> forest = new Dictionary<Point2i, List<GridEdge>>();

            edgeCount = edges.Count;
            for (int i = 0; i < edgeCount; i++)
            {
                Point2i root = set.FindParent(edges[i].From.x, edges[i].From.y);

                if (!forest.ContainsKey(root))
                    forest.Add(root, new List<GridEdge>());

                forest[root].Add(edges[i]);
            }

            return forest;
        }
    }
}
