using System;
using System.Collections.Generic;

using Common.Core.Directions;
using Common.Core.LinearAlgebra;

namespace Common.Meshing.GridGraphs
{
    public static partial class GridGraphSearch
    {
        public static float FordFulkersonMaxFlow(GridFlowGraph graph)
        {
            int width = graph.Width;
            int height = graph.Height;

            Vector3i[,] parent = new Vector3i[width, height];

            float maxFlow = 0;

            Vector3i sink, v;
            while (BreadthFirstSearch(graph, parent, out sink))
            {
                float flow = float.PositiveInfinity;

                v = sink;
                while (true)
                {
                    Vector3i u = parent[v.x, v.y];
                    if (u.x == v.x && u.y == v.y)
                        throw new InvalidOperationException("Did not stop at source.");

                    float residual = graph.Capacity[u.x, u.y, u.z] - graph.Flow[u.x, u.y, u.z];
                    flow = Math.Min(flow, residual);

                    if (graph.Label[u.x, u.y] == GridFlowGraph.SOURCE)
                        break;
                    else
                        v = u;
                }

                if (flow == float.PositiveInfinity)
                    throw new InvalidOperationException("Could not find path flow.");

                maxFlow += flow;

                v = sink;
                while (true)
                {
                    Vector3i u = parent[v.x, v.y];

                    graph.Flow[u.x, u.y, u.z] += flow;
                    graph.Flow[v.x, v.y, D8.OPPOSITES[u.z]] -= flow;

                    if (graph.Label[u.x, u.y] == GridFlowGraph.SOURCE)
                        break;
                    else
                        v = u;
                }

            }

            return maxFlow;
        }

        private static bool BreadthFirstSearch(GridFlowGraph graph, Vector3i[,] parent, out Vector3i sink)
        {
            int width = graph.Width;
            int height = graph.Height;

            Queue<Vector2i> queue = new Queue<Vector2i>();
            bool[,] isVisited = new bool[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (graph.Label[x, y] != GridFlowGraph.SOURCE) continue;
                    queue.Enqueue(new Vector2i(x, y));
                    parent[x, y] = new Vector3i(x, y, -1);
                    isVisited[x, y] = true;
                }
            }

            while (queue.Count != 0)
            {
                Vector2i u = queue.Dequeue();

                for (int i = 0; i < 8; i++)
                {
                    float residual = graph.Capacity[u.x, u.y, i] - graph.Flow[u.x, u.y, i];
                    if (residual <= 0) continue;

                    int xi = u.x + D8.OFFSETS[i, 0];
                    int yi = u.y + D8.OFFSETS[i, 1];

                    if (xi < 0 || xi >= width) continue;
                    if (yi < 0 || yi >= height) continue;
                    if (isVisited[xi, yi]) continue;

                    queue.Enqueue(new Vector2i(xi, yi));
                    parent[xi, yi] = new Vector3i(u.x, u.y, i);
                    isVisited[xi, yi] = true;

                    if (graph.Label[xi, yi] == GridFlowGraph.SINK)
                    {
                        sink = new Vector3i(xi, yi, -1);
                        return true;
                    }
                }
            }

            sink = new Vector3i(-1, -1, -1);
            return false;
        }
    }
}
