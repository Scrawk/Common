using System;
using System.Collections.Generic;

using Common.Core.Directions;
using Common.Collections.Queues;
using Common.Core.Numerics;

namespace Common.GraphTheory.GridGraphs
{
    public partial class WeightedGridGraph
    {

        public GridSearch PrimsMinimumSpanningTree(Point2i root)
        {
            var search = new GridSearch(Width, Height);
            PrimsMinimumSpanningTree(search, root.x, root.y);
            return search;
        }

        public GridSearch PrimsMinimumSpanningTree(int x, int y)
        {
            var search = new GridSearch(Width, Height);
            PrimsMinimumSpanningTree(search, x, y);
            return search;
        }

        public void PrimsMinimumSpanningTree(GridSearch search, int x, int y, Func<Point2i, Point2i, float> GetWeight = null)
        {
            int width = Width;
            int height = Height;

            search.SetIsVisited(x, y, true);;
            search.AddOrder(new Point2i(x, y));
            search.SetParent(x, y,  new Point2i(x, y));

            var queue = new BinaryHeap<WeighedGridEdge>(8);

            List<WeighedGridEdge> edges = new List<WeighedGridEdge>(8);
            GetEdges(x, y, edges, GetWeight);

            if (edges.Count != 0)
            {
                foreach (WeighedGridEdge edge in edges)
                    queue.Add(edge);

                edges.Clear();
            }

            while (queue.Count != 0)
            {
                WeighedGridEdge edge = queue.Pop();

                Point2i v = edge.To;
                if (search.GetIsVisited(v)) continue;

                search.AddOrder(v);
                search.SetIsVisited(v, true);
                search.SetParent(v, edge.From);

                if (Edges[v.x, v.y] == 0) continue;

                GetEdges(v.x, v.y, edges, GetWeight);

                foreach (WeighedGridEdge e in edges)
                {
                    if (search.GetIsVisited(e.To)) continue;
                    queue.Add(e);
                }

                edges.Clear();
            }
        }
    }

}
