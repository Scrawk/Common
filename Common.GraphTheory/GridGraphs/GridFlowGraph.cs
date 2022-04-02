using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Core.Directions;
using Common.GraphTheory.AdjacencyGraphs;

namespace Common.GraphTheory.GridGraphs
{

    public enum FLOW_GRAPH_LABEL
    {
        NONE, SOURCE, SINK
    }

    public class GridFlowGraph
    {

        public float MaxFlow { get; private set; }

        public int VertexCount { get { return Width * Height; } }

        public int Width { get; private set; }

        public int Height { get; private set; }

        private float[,,] Capacity { get; set; }

        private float[,,] Flow { get;  set; }

        private byte[,] Label { get; set; }

        public GridFlowGraph(int width, int height)
        {
            Width = width;
            Height = height;

            Capacity = new float[width, height, 8];
            Flow = new float[width, height, 8];
            Label = new byte[width, height];
        }

        public GridFlowGraph(float[,] array)
        {
            Width = array.GetLength(0);
            Height = array.GetLength(1);

            Capacity = new float[Width, Height, 8];
            Flow = new float[Width, Height, 8];
            Label = new byte[Width, Height];

            Fill(array);
        }

        public void Clear()
        {
            Array.Clear(Capacity, 0, Capacity.Length);
            Array.Clear(Flow, 0, Flow.Length);
            Array.Clear(Label, 0, Label.Length);
        }

        public bool InBounds(int x, int y)
        {
            if (x < 0 || x >= Width) return false;
            if (y < 0 || y >= Height) return false;

            return true;
        }

        public bool InBounds(Point2i p)
        {
            if (p.x < 0 || p.x >= Width) return false;
            if (p.y < 0 || p.y >= Height) return false;

            return true;
        }

        public void Iterate(Action<int, int> func)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    func(x, y);
                }
            }
        }

        public void Iterate(Action<int, int, int> func)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        func(x, y, i);
                    }
                }
            }
        }

        public void Fill(float[,] array)
        {
            for(int x = 0; x < Width; x++)
            {
                for(int y = 0; y < Height; y++)
                {
                    var c1 = array[x, y];

                    for (int i = 0; i < 8; i++)
                    {
                        int xi = x + D8.OFFSETS[i, 0];
                        int yi = y + D8.OFFSETS[i, 1];

                        if (xi < 0 || xi > Width - 1) continue;
                        if (yi < 0 || yi > Height - 1) continue;

                        var c2 = array[xi, yi];

                        Capacity[x, y, i] = (c1 + c2) * 0.5f;
                    }
                }
            }
        }

        public float GetCapacity(int x, int y, int i)
        {
            return Capacity[x, y, i];
        }

        public float GetCapacity(Point2i p, int i)
        {
            return Capacity[p.x, p.y, i];
        }

        public void SetCapacity(int x, int y, int i, float capacity)
        {
            Capacity[x, y, i] = capacity;
        }

        public void SetCapacity(Point2i p, int i, float capacity)
        {
            Capacity[p.x, p.y, i] = capacity;
        }

        public float GetFlow(int x, int y, int i)
        {
            return Flow[x, y, i];
        }

        public float GetFlow(Point2i p, int i)
        {
            return Flow[p.x, p.y, i];
        }

        public void SetFlow(int x, int y, int i, float flow)
        {
            Flow[x, y, i] = flow;
        }

        public void SetFlow(Point2i p, int i, float flow)
        {
            Flow[p.x, p.y, i] = flow;
        }

        public FLOW_GRAPH_LABEL GetLabel(int x, int y)
        {
            return (FLOW_GRAPH_LABEL)Label[x, y];
        }

        public FLOW_GRAPH_LABEL GetLabel(Point2i p)
        {
            return (FLOW_GRAPH_LABEL)Label[p.x, p.y];
        }

        public void SetLabel(int x, int y, FLOW_GRAPH_LABEL label)
        {
            Label[x, y] = (byte)label;
        }

        public void SetLabel(Point2i p, FLOW_GRAPH_LABEL label)
        {
            Label[p.x, p.y] = (byte)label;
        }

        public void SetLabel(int x, int y, FLOW_GRAPH_LABEL label, int capacity)
        {
            SetLabel(x, y, label);

            for (int i = 0; i < 8; i++)
            {
                int xi = x + D8.OFFSETS[i, 0];
                int yi = y + D8.OFFSETS[i, 1];

                if (xi < 0 || xi >= Width) continue;
                if (yi < 0 || yi >= Height) continue;

                SetCapacity(x, y, i, capacity);
            }
        }

        public void SetSource(int x, int y, int capacity)
        {
            SetLabel(x, y, FLOW_GRAPH_LABEL.SOURCE, capacity);
        }

        public void SetSink(int x, int y, int capacity)
        {
            SetLabel(x, y, FLOW_GRAPH_LABEL.SINK, capacity);
        }

        public bool IsSource(int x, int y)
        {
            return GetLabel(x, y) == FLOW_GRAPH_LABEL.SOURCE;
        }

        public bool IsSink(int x, int y)
        {
            return GetLabel(x, y) == FLOW_GRAPH_LABEL.SINK;
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
                    Point3i u = parent[v.x, v.y];

                    Flow[u.x, u.y, u.z] += flow;
                    Flow[v.x, v.y, D8.OPPOSITES[u.z]] -= flow;

                    if (Label[u.x, u.y] == (byte)FLOW_GRAPH_LABEL.SOURCE)
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
                    if (Label[x, y] == (byte)FLOW_GRAPH_LABEL.SOURCE)
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
                    if (Label[xi, yi] == (byte)FLOW_GRAPH_LABEL.SOURCE) continue;

                    Label[xi, yi] = (byte)FLOW_GRAPH_LABEL.SOURCE;

                    queue.Enqueue(new Point2i(xi, yi));
                }
            }

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (Label[x, y] != (byte)FLOW_GRAPH_LABEL.SOURCE)
                        Label[x, y] = (byte)FLOW_GRAPH_LABEL.SINK;

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
                    if (Label[x, y] != (byte)FLOW_GRAPH_LABEL.SOURCE) continue;

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