using System;
using System.Collections.Generic;

using Common.Core.Directions;
using Common.Collections.Queues;
using Common.Core.Numerics;

namespace Common.GraphTheory.GridGraphs
{
    public partial class GridGraph
    {
        public void PrimsMinimumSpanningTree(GridSearch search, int x, int y, Func<Point2i, Point2i, float> GetWeight = null)
        {
            int width = Width;
            int height = Height;

            search.SetIsVisited(x, y, true);;
            search.AddOrder(new Point2i(x, y));
            search.SetParent(x, y,  new Point2i(x, y));

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
                if (search.GetIsVisited(v)) continue;

                search.AddOrder(v);
                search.SetIsVisited(v, true);
                search.SetParent(v, edge.From);

                if (Edges[v.x, v.y] == 0) continue;

                GetEdges(v.x, v.y, edges, GetWeight);

                foreach (GridEdge e in edges)
                {
                    if (search.GetIsVisited(e.To)) continue;
                    queue.Add(e);
                }

                edges.Clear();
            }
        }
    }

}
