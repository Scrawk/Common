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

    /// <summary>
    /// A graph were the vertices make up a grid
    /// like the pixels in a image. Each vertex
    /// has a byte flag where the bits represent 
    /// if a edge is present to a neighbouring
    /// vertex.
    /// 
    /// Each edge has a capacity and a flow value
    /// and are used to perfrom the max flow / min cut algorithm.
    /// 
    /// The edge directions are in the folling order.
    /// See Common.Core.Directions.D8 script.
    /// 
    /// LEFT = 0;
    /// LEFT_TOP = 1;
    /// TOP = 2;
    /// RIGHT_TOP = 3;
    /// RIGHT = 4;
    /// RIGHT_BOTTOM = 5;
    /// BOTTOM = 6;
    /// LEFT_BOTTOM = 7;
    /// 
    /// </summary>
    public class GridFlowGraph
    {
        /// <summary>
        /// The max flow of the graph.
        /// Calculate must be called to find this.
        /// </summary>
        public float MaxFlow { get; private set; }

        public int VertexCount { get { return Width * Height; } }

        public int Width { get; private set; }

        public int Height { get; private set; }

        /// <summary>
        /// THe vertices edges capacity value.
        /// </summary>
        private float[,,] Capacity { get; set; }

        /// <summary>
        /// THe vertices edges flow value.
        /// </summary>
        private float[,,] Flow { get;  set; }

        /// <summary>
        /// THe vertices label.
        /// </summary>
        private byte[,] Label { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public GridFlowGraph(int width, int height)
        {
            Width = width;
            Height = height;

            Capacity = new float[width, height, 8];
            Flow = new float[width, height, 8];
            Label = new byte[width, height];
        }

        /// <summary>
        /// Create a new graph and set the vertices edge 
        /// capacities with the values in the araay.
        /// The graph will have the same dimensions as the array.
        /// </summary>
        /// <param name="capacities">The edges capacities.</param>
        public GridFlowGraph(float[,] capacities)
        {
            Width = capacities.GetLength(0);
            Height = capacities.GetLength(1);

            Capacity = new float[Width, Height, 8];
            Flow = new float[Width, Height, 8];
            Label = new byte[Width, Height];

            FillCapacity(capacities);
        }

        /// <summary>
        /// Clear the graph by setting all capacity, flow and labels to 0.
        /// </summary>
        public void Clear()
        {
            MaxFlow = 0;
            Array.Clear(Capacity, 0, Capacity.Length);
            Array.Clear(Flow, 0, Flow.Length);
            Array.Clear(Label, 0, Label.Length);
        }

        /// <summary>
        /// Are the indices within the bounds of the graph.
        /// </summary>
        /// <param name="x">The x axis index.</param>
        /// <param name="y">The y axis index</param>
        /// <returns></returns>
        public bool InBounds(int x, int y)
        {
            if (x < 0 || x >= Width) return false;
            if (y < 0 || y >= Height) return false;

            return true;
        }

        /// <summary>
        /// Iterate over all vertices in the graph
        /// and apply the function.
        /// </summary>
        /// <param name="func"></param>
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

        /// <summary>
        /// Iterate over all vertices edges in the graph
        /// and apply the function.
        /// </summary>
        /// <param name="func"></param>
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

        /// <summary>
        /// Fill the capacity off all edges with the values in the array.
        /// </summary>
        /// <param name="array"></param>
        public void FillCapacity(float[,] array)
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

        /// <summary>
        /// Get the capacity of the vertex edge at x,y 
        /// going to the neighbour vertex at i.
        /// </summary>
        /// <param name="x">The x axis index.</param>
        /// <param name="y">The y axis index</param>
        /// <param name="i">The  neigbour vertices index.</param>
        /// <returns></returns>
        public float GetCapacity(int x, int y, int i)
        {
            return Capacity[x, y, i];
        }

        /// <summary>
        /// Set the capacity of the vertex edge at x,y 
        /// going to the neighbour vertex at i.
        /// </summary>
        /// <param name="x">The x axis index.</param>
        /// <param name="y">The y axis index</param>
        /// <param name="i">The  neigbour vertices index.</param>
        /// <param name="capacity"></param>
        public void SetCapacity(int x, int y, int i, float capacity)
        {
            Capacity[x, y, i] = capacity;
        }

        /// <summary>
        /// Get the flow of the vertex edge at x,y 
        /// going to the neighbour vertex at i.
        /// </summary>
        /// <param name="x">The x axis index.</param>
        /// <param name="y">The y axis index</param>
        /// <param name="i">The  neigbour vertices index.</param>
        /// <returns></returns>
        public float GetFlow(int x, int y, int i)
        {
            return Flow[x, y, i];
        }

        /// <summary>
        /// Set the flow of the vertex edge at x,y 
        /// going to the neighbour vertex at i.
        /// </summary>
        /// <param name="x">The x axis index.</param>
        /// <param name="y">The y axis index</param>
        /// <param name="i">The  neigbour vertices index.</param>
        /// <param name="flow"></param>
        public void SetFlow(int x, int y, int i, float flow)
        {
            Flow[x, y, i] = flow;
        }

        /// <summary>
        /// Get the label of the vertex at x,y.
        /// </summary>
        /// <param name="x">The x axis index.</param>
        /// <param name="y">The y axis index</param>
        /// <returns></returns>
        public FLOW_GRAPH_LABEL GetLabel(int x, int y)
        {
            return (FLOW_GRAPH_LABEL)Label[x, y];
        }

        /// <summary>
        /// Set the label of the vertex at x,y.
        /// </summary>
        /// <param name="x">The x axis index.</param>
        /// <param name="y">The y axis index</param>
        /// <param name="label"></param>
        public void SetLabel(int x, int y, FLOW_GRAPH_LABEL label)
        {
            Label[x, y] = (byte)label;
        }

        /// <summary>
        /// Set the label of the vertex at x,y.
        /// Set all edges capacity going from this vertex.
        /// </summary>
        /// <param name="x">The x axis index.</param>
        /// <param name="y">The y axis index</param>
        /// <param name="label">The vertices label.</param>
        /// <param name="capacity">The vertices edges capacity.</param>
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

        /// <summary>
        /// Set the vertices label at x,y to source.
        /// Set all edges capacity going from this vertex.
        /// </summary>
        /// <param name="x">The x axis index.</param>
        /// <param name="y">The y axis index</param>
        /// <param name="capacity">The vertices edges capacity.</param>
        public void SetSource(int x, int y, int capacity)
        {
            SetLabel(x, y, FLOW_GRAPH_LABEL.SOURCE, capacity);
        }

        /// <summary>
        /// Set the vertices label at x,y to sink.
        /// Set all edges capacity going from this vertex.
        /// </summary>
        /// <param name="x">The x axis index.</param>
        /// <param name="y">The y axis index</param>
        /// <param name="capacity">The vertices edges capacity.</param>
        public void SetSink(int x, int y, int capacity)
        {
            SetLabel(x, y, FLOW_GRAPH_LABEL.SINK, capacity);
        }

        /// <summary>
        /// Is this vertex labeled as a source.
        /// </summary>
        /// <param name="x">The x axis index.</param>
        /// <param name="y">The y axis index</param>
        /// <returns></returns>
        public bool IsSource(int x, int y)
        {
            return GetLabel(x, y) == FLOW_GRAPH_LABEL.SOURCE;
        }

        /// <summary>
        /// Is this vertex labeled as a sink.
        /// </summary>
        /// <param name="x">The x axis index.</param>
        /// <param name="y">The y axis index</param>
        /// <returns></returns>
        public bool IsSink(int x, int y)
        {
            return GetLabel(x, y) == FLOW_GRAPH_LABEL.SINK;
        }

        /// <summary>
        /// Find the vertices that are labeled as source or sink and
        /// have at least one neighbour that has a different label.
        /// </summary>
        /// <param name="includeSource">Should source vertices be checked.</param>
        /// <param name="includeSink">Should sink vertices be checked.</param>
        /// <returns>A list of all the vertices.</returns>
        public List<Point2i> FindBoundaryPoints(bool includeSource, bool includeSink)
        {
            var points = new List<Point2i>();

            Iterate((x, y) =>
            {
                if (includeSource && IsSource(x, y))
                {
                    for (int i = 0; i < 8; i++)
                    {
                        int xi = x + D8.OFFSETS[i, 0];
                        int yi = y + D8.OFFSETS[i, 1];

                        if (xi < 0 || xi >= Width) continue;
                        if (yi < 0 || yi >= Height) continue;

                        if (!IsSource(xi, yi))
                        {
                            points.Add(new Point2i(x, y));
                            break;
                        }
                    }
                }
                else if (includeSink && IsSink(x, y))
                {
                    for (int i = 0; i < 8; i++)
                    {
                        int xi = x + D8.OFFSETS[i, 0];
                        int yi = y + D8.OFFSETS[i, 1];

                        if (xi < 0 || xi >= Width) continue;
                        if (yi < 0 || yi >= Height) continue;

                        if (!IsSink(xi, yi))
                        {
                            points.Add(new Point2i(x, y));
                            break;
                        }
                    }
                }

            });

            return points;
        }

        /// <summary>
        /// Calculate the max flow / min cut of the graph.
        /// </summary>
        /// <returns></returns>
        public float Calculate()
        {
            MaxFlow = FordFulkersonMaxFlow();
            CalculateMinCut();

            return MaxFlow;
        }

        /// <summary>
        /// Calculate the max flow of the graph using the FordFulkerson algorithm.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
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

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="sink"></param>
        /// <returns></returns>
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