using System;
using System.Collections.Generic;

using Common.Core.Directions;
using Common.Collections.Queues;
using Common.Core.Numerics;

namespace Common.Meshing.GridGraphs
{
    public static partial class GridGraphSearch
    {
        internal static void PrimsMinimumSpanningTree(GridGraph graph, GridSearch search, int x, int y, Func<Vector2i, Vector2i, float> GetWeight)
        {
            int width = graph.Width;
            int height = graph.Height;

            search.IsVisited[x, y] = true;
            search.Order.Add(new Vector2i(x, y));
            search.Parent[x, y] = new Vector2i(x, y);

            var queue = new BinaryHeap<GridEdge>(8);

            List<GridEdge> edges = new List<GridEdge>(8);
            graph.GetEdges(x, y, edges, GetWeight);

            if (edges.Count != 0)
            {
                foreach (GridEdge edge in edges)
                    queue.Add(edge);

                edges.Clear();
            }

            while (queue.Count != 0)
            {
                GridEdge edge = queue.RemoveFirst();

                Vector2i v = edge.To;
                if (search.IsVisited[v.x, v.y]) continue;

                search.Order.Add(v);
                search.IsVisited[v.x, v.y] = true;
                search.Parent[v.x, v.y] = edge.From;

                if (graph.Edges[v.x, v.y] == 0) continue;

                graph.GetEdges(v.x, v.y, edges, GetWeight);

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
