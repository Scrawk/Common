using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Core.Directions;

namespace Common.GraphTheory.GridGraphs
{
    public class GridFlowGraph
    {
        public const byte UNLABELED = 0;

        public const byte SOURCE = 1;

        public const byte SINK = 2;

        public float MaxFlow { get; private set; }

        public int VertexCount { get { return Width * Height; } }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public float[,,] Capacity { get; private set; }

        public float[,,] Flow { get; private set; }

        public byte[,] Label { get; private set; }

        public GridFlowGraph(int width, int height)
        {
            Width = width;
            Height = height;

            Capacity = new float[width, height, 8];
            Flow = new float[width, height, 8];
            Label = new byte[width, height];
        }

        public void Clear()
        {
            Array.Clear(Capacity, 0, Capacity.Length);
            Array.Clear(Flow, 0, Flow.Length);
            Array.Clear(Label, 0, Label.Length);
        }

        public float Calculate()
        {
            MaxFlow = FordFulkersonMaxFlow();
            CalculateMinCut();

            return MaxFlow;
        }

        private float FordFulkersonMaxFlow()
        {

            Point3i[,] parent = new Point3i[Width, Height];

            float maxFlow = 0;

            Point3i sink, v;
            while (BreadthFirstSearch(parent, out sink))
            {
                float flow = float.PositiveInfinity;

                v = sink;
                while (true)
                {
                    Point3i u = parent[v.x, v.y];
                    if (u.x == v.x && u.y == v.y)
                        throw new InvalidOperationException("Did not stop at source.");

                    float residual = Capacity[u.x, u.y, u.z] - Flow[u.x, u.y, u.z];
                    flow = Math.Min(flow, residual);

                    if (Label[u.x, u.y] == GridFlowGraph.SOURCE)
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
                    Point3i u = parent[v.x, v.y];

                    Flow[u.x, u.y, u.z] += flow;
                    Flow[v.x, v.y, D8.OPPOSITES[u.z]] -= flow;

                    if (Label[u.x, u.y] == GridFlowGraph.SOURCE)
                        break;
                    else
                        v = u;
                }

            }

            return maxFlow;

        }

        private void CalculateMinCut()
        {
            Queue<Point2i> queue = new Queue<Point2i>(VertexCount * 8);

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (Label[x, y] == SOURCE)
                        queue.Enqueue(new Point2i(x, y));
                }
            }

            while (queue.Count > 0)
            {
                Point2i u = queue.Dequeue();

                for (int i = 0; i < 8; i++)
                {
                    float residual = Capacity[u.x, u.y, i] - Flow[u.x, u.y, i];
                    if (residual <= 0) continue;

                    int xi = u.x + D8.OFFSETS[i, 0];
                    int yi = u.y + D8.OFFSETS[i, 1];

                    if (xi < 0 || xi >= Width) continue;
                    if (yi < 0 || yi >= Height) continue;
                    if (Label[xi, yi] == SOURCE) continue;

                    Label[xi, yi] = SOURCE;

                    queue.Enqueue(new Point2i(xi, yi));
                }
            }

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (Label[x, y] != SOURCE)
                        Label[x, y] = SINK;

                    for (int i = 0; i < 8; i++)
                        if (Flow[x, y, i] < 0) Flow[x, y, i] = 0;
                }
            }

        }

        private bool BreadthFirstSearch(Point3i[,] parent, out Point3i sink)
        {

            Queue<Point2i> queue = new Queue<Point2i>();
            bool[,] isVisited = new bool[Width, Height];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (Label[x, y] != GridFlowGraph.SOURCE) continue;
                    queue.Enqueue(new Point2i(x, y));
                    parent[x, y] = new Point3i(x, y, -1);
                    isVisited[x, y] = true;
                }
            }

            while (queue.Count != 0)
            {
                Point2i u = queue.Dequeue();

                for (int i = 0; i < 8; i++)
                {
                    float residual = Capacity[u.x, u.y, i] - Flow[u.x, u.y, i];
                    if (residual <= 0) continue;

                    int xi = u.x + D8.OFFSETS[i, 0];
                    int yi = u.y + D8.OFFSETS[i, 1];

                    if (xi < 0 || xi >= Width) continue;
                    if (yi < 0 || yi >= Height) continue;
                    if (isVisited[xi, yi]) continue;

                    queue.Enqueue(new Point2i(xi, yi));
                    parent[xi, yi] = new Point3i(u.x, u.y, i);
                    isVisited[xi, yi] = true;

                    if (Label[xi, yi] == GridFlowGraph.SINK)
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