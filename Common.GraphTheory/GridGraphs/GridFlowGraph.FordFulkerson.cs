using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Core.Directions;
using Common.Core.Extensions;

namespace Common.GraphTheory.GridGraphs
{
    public partial class GridFlowGraph
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <param name="seed"></param>
        /// <exception cref="InvalidOperationException"></exception>
        private void FordFulkersonMaxFlow(GridFlowSearch search, int seed)
        {

            float maxFlow = 0;
            int step = 1;
            var rnd = new Random(seed);
            var directions = new List<int>(Directions);
            directions.Shuffle(rnd);

            Point3i sink, v;
            while (BreadthFirstSearch(search, step, directions, out sink))
            {
                step++;
                float flow = float.PositiveInfinity;
                directions.Shuffle(rnd);

                v = sink;
                while (true)
                {
                    Point3i u = search.GetParent(v.x, v.y);
                    if (u.x == v.x && u.y == v.y)
                        throw new InvalidOperationException("Did not stop at source.");

                    float residual = Capacity[u.x, u.y, u.z] - Flow[u.x, u.y, u.z];
                    flow = Math.Min(flow, residual);

                    if (Label[u.x, u.y] == (byte)FLOW_GRAPH_LABEL.SOURCE)
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
                    Point3i u = search.GetParent(v.x, v.y);

                    Flow[u.x, u.y, u.z] += flow;
                    Flow[v.x, v.y, D8.OPPOSITES[u.z]] -= flow;

                    if (Label[u.x, u.y] == (byte)FLOW_GRAPH_LABEL.SOURCE)
                        break;
                    else
                        v = u;
                }

            }

            search.MaxFlow = maxFlow;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        private void CalculateMinCut(GridFlowSearch search)
        {
            search.ClearQueue();

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (Label[x, y] == (byte)FLOW_GRAPH_LABEL.SOURCE)
                        search.Enqueue(new Point2i(x, y));
                }
            }

            while (search.QueueCount > 0)
            {
                Point2i u = search.Dequeue();

                for (int j = 0; j < Directions.Count; j++)
                {
                    int i = Directions[j];

                    float residual = Capacity[u.x, u.y, i] - Flow[u.x, u.y, i];
                    if (residual <= 0) continue;

                    int xi = u.x + D8.OFFSETS[i, 0];
                    int yi = u.y + D8.OFFSETS[i, 1];

                    if (xi < 0 || xi >= Width) continue;
                    if (yi < 0 || yi >= Height) continue;
                    if (Label[xi, yi] == (byte)FLOW_GRAPH_LABEL.SOURCE) continue;

                    Label[xi, yi] = (byte)FLOW_GRAPH_LABEL.SOURCE;

                    search.Enqueue(new Point2i(xi, yi));
                }

            }

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (Label[x, y] != (byte)FLOW_GRAPH_LABEL.SOURCE)
                        Label[x, y] = (byte)FLOW_GRAPH_LABEL.SINK;

                    for (int j = 0; j < Directions.Count; j++)
                    {
                        int i = Directions[j];

                        if (Flow[x, y, i] < 0) Flow[x, y, i] = 0;
                    }

                }
            }

        }

        /// <summary>
        /// Find if there exists a path to the sink.
        /// </summary>
        /// <param name="search">The helper search data structure.</param>
        /// <param name="step">Used to determine if vertex has been visited.</param>
        /// <param name="sink">The index of the sink point.</param>
        /// <returns></returns>
        private bool BreadthFirstSearch(GridFlowSearch search, int step, IList<int> directions, out Point3i sink)
        {
            search.ClearQueue();

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (Label[x, y] != (byte)FLOW_GRAPH_LABEL.SOURCE) continue;

                    search.Enqueue(new Point2i(x, y));
                    search.SetParent(x, y, new Point3i(x, y, -1));
                    search.SetIsVisited(x, y, step);
                }
            }

            while (search.QueueCount != 0)
            {
                Point2i u = search.Dequeue();

                for (int j = 0; j < directions.Count; j++)
                {
                    int i = directions[j];

                    float residual = Capacity[u.x, u.y, i] - Flow[u.x, u.y, i];
                    if (residual <= 0) continue;

                    int xi = u.x + D8.OFFSETS[i, 0];
                    int yi = u.y + D8.OFFSETS[i, 1];

                    if (xi < 0 || xi >= Width) continue;
                    if (yi < 0 || yi >= Height) continue;
                    if (search.GetIsVisited(xi, yi) >= step) continue;

                    search.Enqueue(new Point2i(xi, yi));
                    search.SetParent(xi, yi, new Point3i(u.x, u.y, i));
                    search.SetIsVisited(xi, yi, step);

                    if (Label[xi, yi] == (byte)FLOW_GRAPH_LABEL.SINK)
                    {
                        sink = new Point3i(xi, yi, -1);
                        return true;
                    }
                }

            }

            sink = new Point3i(-1, -1, -1);
            return false;
        }
    }
}
