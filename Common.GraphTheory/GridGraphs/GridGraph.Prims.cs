using System;
using System.Collections.Generic;

using Common.Core.Directions;
using Common.Collections.Queues;
using Common.Core.Numerics;

namespace Common.GraphTheory.GridGraphs
{
    public partial class GridGraph
    {
        public void PrimsMinimumSpanningTree(GridSearch search, int x, int y, Func<Point2i, Point2i, float> GetWeight)
        {
            int width = Width;
            int height = Height;

            search.IsVisited[x, y] = true;
            search.Order.Add(new Point2i(x, y));
            search.Parent[x, y] = new Point2i(x, y);

            var queue = new BinaryHeap<GridEdge>(8);

            List<GridEdge> edges = new List<GridEdge>(8);
            GetEdges(x, y, edges, GetWeight);

            if (edges.Count != 0)
            {
                foreach (GridEdge edge in edges)
                    queue.Add(edge);

                edges.Clear();
            }

            while (queue.Count != 0)
            {
                GridEdge edge = queue.Pop();

                Point2i v = edge.To;
                if (search.IsVisited[v.x, v.y]) continue;

                search.Order.Add(v);
                search.IsVisited[v.x, v.y] = true;
                search.Parent[v.x, v.y] = edge.From;

                if (Edges[v.x, v.y] == 0) continue;

                GetEdges(v.x, v.y, edges, GetWeight);

                foreach (GridEdge e in edges)
                {
                    if (search.IsVisited[e.To.x, e.To.y]) continue;
                    queue.Add(e);
                }

                edges.Clear();
            }
        }
    }

}
