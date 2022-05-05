using System;
using System.Collections;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Core.Directions;
using Common.Core.Shapes;
using Common.Core.Extensions;

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
        /// Create a new graph.
        /// </summary>
        /// <param name="width">The graphs size on the x axis.</param>
        /// <param name="height">The graphs size on the y axis.</param>
        /// <param name="isOrthogonal">Is the graph orthogonal.</param>
        public GridFlowGraph(int width, int height, bool isOrthogonal = false)
        {
            Width = width;
            Height = height;

            Capacity = new float[width, height, 8];
            Flow = new float[width, height, 8];
            Label = new byte[width, height];

            Directions = new List<int>(8);
            SetIsOrthogonal(isOrthogonal);
        }

        /// <summary>
        /// Create a new graph and set the vertices edge 
        /// capacities with the values in the araay.
        /// The graph will have the same dimensions as the array. 
        /// </summary>
        /// <param name="capacities">The array of capacities.</param>
        /// <param name="isOrthogonal">Is the graph orthogonal.</param>
        public GridFlowGraph(float[,] capacities, bool isOrthogonal = false)
        {
            Width = capacities.GetLength(0);
            Height = capacities.GetLength(1);

            Capacity = new float[Width, Height, 8];
            Flow = new float[Width, Height, 8];
            Label = new byte[Width, Height];

            Directions = new List<int>(8);
            SetIsOrthogonal(isOrthogonal);

            FillCapacity(capacities);
        }

        /// <summary>
        /// THe number of vertices in the graph 
        /// which is the width times the height.
        /// </summary>
        public int VertexCount => Width * Height;

        /// <summary>
        /// The width of the graph on the x axis.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// The height of the graph on the y axis.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Does the graph use only orthogonal and not diagonal directions.
        /// </summary>
        public bool IsOrthogonal { get; private set; }

        /// <summary>
        /// THe vertices edges capacity value.
        /// </summary>
        private float[,,] Capacity { get; set; }

        /// <summary>
        /// THe vertices edges flow value.
        /// </summary>
        private float[,,] Flow { get; set; }

        /// <summary>
        /// THe vertices label.
        /// </summary>
        private byte[,] Label { get; set; }

        /// <summary>
        /// The neigbour directions each pixel is connect to.
        /// If orthogonal there will be 4 neighbours (left, bottom, right and top).
        /// If not orthogonal there will be 8 neighbours which includes the diagonals.
        /// </summary>
        private List<int> Directions { get; set; }

        public override string ToString()
        {
            return string.Format("[GridGraph: IsOrthogonal={0}, Width={1}, Height={2}]",
                IsOrthogonal, Width, Height);
        }

        /// <summary>
        /// The neigbour directions each pixel is connect to.
        /// If orthogonal there will be 4 neighbours (left, bottom, right and top).
        /// If not orthogonal there will be 8 neighbours which includes the diagonals.
        /// </summary>
        /// <param name="isOrthogonal">Is the grapgh orthogonal</param>
        public void SetIsOrthogonal(bool isOrthogonal)
        {
            IsOrthogonal = isOrthogonal;
            Directions.Clear();

            if (isOrthogonal)
                Directions.AddRange(D8.ORTHOGONAL);
            else
                Directions.AddRange(D8.ALL);
        }

        /// <summary>
        /// Clear the graph by setting all capacity, flow and labels to 0.
        /// </summary>
        public void Clear()
        {
            Array.Clear(Capacity, 0, Capacity.Length);
            Array.Clear(Flow, 0, Flow.Length);
            Array.Clear(Label, 0, Label.Length);
        }

        /// <summary>
        /// Creates a new helper search data structure.
        /// </summary>
        /// <returns></returns>
        public GridFlowSearch CreateSearch()
        {
            var search = new GridFlowSearch(Width, Height);
            return search;
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
                    for(int j = 0; j < Directions.Count; j++)
                    {
                        int i = Directions[j];
                        func(x, y, i);
                    }
                }
            }
        }

        /// <summary>
        /// Iterate over the graphs edge directions.
        /// </summary>
        public IEnumerable<int> EnumerateDirections()
        {
            for (int j = 0; j < Directions.Count; j++)
            {
                int i = Directions[j];
                yield return i;
            }
        }

        /// <summary>
        /// Iterate over the graphs edge directions.
        /// </summary>
        public IEnumerable<Point3i> EnumerateInBoundsDirections(int x, int y)
        {
            for (int j = 0; j < Directions.Count; j++)
            {
                int i = Directions[j];

                int xi = x + D8.OFFSETS[i, 0];
                int yi = y + D8.OFFSETS[i, 1];

                if (xi < 0 || xi > Width - 1) continue;
                if (yi < 0 || yi > Height - 1) continue;

                yield return new Point3i(xi, yi, i);
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

                    foreach(var i in EnumerateInBoundsDirections(x,y))
                    {
                        var c2 = array[i.x, i.y];
                        Capacity[x, y, i.z] = (c1 + c2) * 0.5f;
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
        /// Set the capacity of the vertex edge at x,y 
        /// going to the neighbour vertex.
        /// </summary>
        /// <param name="x">The x axis index.</param>
        /// <param name="y">The y axis index</param>
        /// <param name="capacity"></param>
        public void SetCapacity(int x, int y, float capacity)
        {
            foreach (var i in EnumerateInBoundsDirections(x, y))
            {
                SetCapacity(x, y, i.z, capacity);
            }
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
        /// Set the flow of the vertex edge at x,y 
        /// going to the neighbour vertex.
        /// </summary>
        /// <param name="x">The x axis index.</param>
        /// <param name="y">The y axis index</param>
        /// <param name="flow"></param>
        public void SetFlow(int x, int y, float flow)
        {
            foreach (var i in EnumerateInBoundsDirections(x, y))
                Flow[x, y, i.z] = flow;
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
        public void SetLabelAndCapacity(int x, int y, FLOW_GRAPH_LABEL label, int capacity)
        {
            SetLabel(x, y, label);
            SetCapacity(x, y, capacity);
        }

        /// <summary>
        /// Sets the labels of the vertices in the bounds of the box.
        /// </summary>
        /// <param name="bounds">The box.</param>
        /// <param name="label">The label.</param>
        /// <param name="capacity">The vertices edges capacity.</param>
        public void SetLabelAndCapacityInBounds(Box2i bounds, FLOW_GRAPH_LABEL label, int capacity)
        {
            foreach (var p in bounds.EnumerateBounds())
            {
                SetLabel(p.x, p.y, label);
                SetCapacity(p.x, p.y, capacity);
            }
        }

        /// <summary>
        /// Sets the labels of the vertices in the perimeter of the box.
        /// </summary>
        /// <param name="width">The width of the perimeters border.</param>
        /// <param name="label">The label.</param>
        /// <param name="capacity">The vertices edges capacity.</param>
        public void SetLabelAndCapacityInPerimeter(int width, FLOW_GRAPH_LABEL label, int capacity)
        {
            var bounds = new Box2i(0, 0, Width-1, Height-1);

            foreach (var p in bounds.EnumeratePerimeter(width))
            {
                SetLabel(p.x, p.y, label);
                SetCapacity(p.x, p.y, capacity);
            }
        }

        /// <summary>
        /// Is this vertex labeled as a source.
        /// </summary>
        /// <param name="x">The x axis index.</param>
        /// <param name="y">The y axis index</param>
        /// <returns>True is this vertex is labeled as a source.</returns>
        public bool IsSource(int x, int y)
        {
            return GetLabel(x, y) == FLOW_GRAPH_LABEL.SOURCE;
        }

        /// <summary>
        /// Is this vertex labeled as a sink.
        /// </summary>
        /// <param name="x">The x axis index.</param>
        /// <param name="y">The y axis index</param>
        /// <returns>True this vertex is labeled as a sink.</returns>
        public bool IsSink(int x, int y)
        {
            return GetLabel(x, y) == FLOW_GRAPH_LABEL.SINK;
        }

        /// <summary>
        /// Does the vertex at x,y have a neighbour with a different label.
        /// </summary>
        /// <param name="x">The x axis index.</param>
        /// <param name="y">The y axis index</param>
        /// <returns>True if the vertex at x,y have a neighbour with a different label.</returns>
        public bool IsBoundaryPoint(int x, int y)
        {
            var label = Label[x, y];

            foreach (var i in EnumerateInBoundsDirections(x, y))
            {
                if (Label[i.x, i.y] != label)
                {
                    return true;
                }
            }

            return false;
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
                    if(IsBoundaryPoint(x, y))
                        points.Add(new Point2i(x, y));
                }
                else if (includeSink && IsSink(x, y))
                {
                    if (IsBoundaryPoint(x, y))
                        points.Add(new Point2i(x, y));
                }

            });

            return points;
        }

        /// <summary>
        /// Calculate the max flow / min cut of the graph.
        /// </summary>
        /// <returns>The max flow.</returns>
        public float Calculate()
        {
            var search = new GridFlowSearch(Width, Height);
            return Calculate(search);
        }

        /// <summary>
        /// Calculate the max flow / min cut of the graph.
        /// </summary>
        /// <param name="search">The helper search data structure.</param>
        /// <param name="seed">The random generators seed.</param>
        /// <returns>The max flow.</returns>
        public float Calculate(GridFlowSearch search, int seed = 0)
        {
            FordFulkersonMaxFlow(search, seed);
            CalculateMinCut(search);

            return search.MaxFlow;
        }

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