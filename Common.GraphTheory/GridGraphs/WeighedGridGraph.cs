using System;
using System.Collections.Generic;
using System.Text;

using Common.Core.Directions;
using Common.Core.Numerics;
using Common.Core.Bits;

using Common.GraphTheory.AdjacencyGraphs;

namespace Common.GraphTheory.GridGraphs
{
    /// <summary>
    /// A graph were the vertices make up a grid
    /// like the pixels in a image. Each vertex
    /// has a byte flag where the bits represent 
    /// if a edge is present to a neighbouring
    /// vertex.
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
    public partial class WeightedGridGraph : GridGraph
    {

        /// <summary>
        /// Create a new graph.
        /// </summary>
        /// <param name="width">The graphs size on the x axis.</param>
        /// <param name="height">The graphs size on the y axis.</param>
        /// <param name="isOrthogonal">Is the graph orthogonal.</param>
        public WeightedGridGraph(int width, int height, bool isOrthogonal = false) 
            : base(width, height, isOrthogonal)
        {

        }

        /// <summary>
        /// Does the graph have weights for the edges.
        /// The weights are optional and are only created if needed.
        /// </summary>
        public bool HasWeights => m_weights != null;

        /// <summary>
        /// The edges weights.
        /// The weights are optional and are only created if needed.
        /// </summary>
        private float[,,] Weights
        {
            get
            {
                if(m_weights == null)
                    m_weights = new  float[Width, Height, 8];

                return m_weights;
            }
        }

        private float[,,] m_weights;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[WeightedGridGraph: VertexCount={0}, EdgeCount={1}, Width={2}, Height={3}]", 
                VertexCount, EdgeCount, Width, Height);
        }

        /// <summary>
        /// Prints each vertices edge connections.
        /// </summary>
        public void Print(bool printEdges)
        {
            var builder = new StringBuilder();
            Print(builder, printEdges);
            Console.WriteLine(builder.ToString());
        }

        /// <summary>
        /// Prints each vertices edge connections.
        /// </summary>
        public void Print(StringBuilder builder, bool printEdges)
        {
            base.Print(builder);

            if (printEdges)
            {
                var edges = new List<WeighedGridEdge>();
                GetAllEdges(edges);

                foreach (var edge in edges)
                    builder.AppendLine(edge.ToString());
            }

        }

        /// <summary>
        /// Create a deep copy of the graph.
        /// </summary>
        /// <returns></returns>
        public WeightedGridGraph Copy()
        {
            var copy = new WeightedGridGraph(Width, Height, IsOrthogonal);

            if (m_weights != null)
                copy.m_weights = new float[Width, Height, 8];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    copy.Edges[x, y] = Edges[x, y];

                    if(HasWeights)
                    {
                        for (int j = 0; j < Directions.Count; j++)
                        {
                            int i = Directions[j];
                            copy.m_weights[x, y, i] = m_weights[x, y, i];
                        }
                            
                    }

                }
            }

            return copy;
        }

        /// <summary>
        /// Clears the graph.
        /// All vertices connections and weights are removed.
        /// 
        /// </summary>
        public override void Clear()
        {
            m_weights = null;
            base.Clear();
        }

        /// <summary>
        /// Set a edges weight.
        /// </summary>
        /// <param name="x">The vertices x index.</param>
        /// <param name="y">The vertices y index.</param>
        /// <param name="i">The bit position in the edge connections byte value.</param>
        /// <param name="w">The edges weight.</param>
        public void SetWeight(int x, int y, int i, float w)
        {
            Weights[x, y, i] = w;
        }

        /// <summary>
        /// Set a edges weight.
        /// </summary>
        /// <param name="fx">The x index the edge is from.</param>
        /// <param name="fy">The y index the edge is from.</param>
        /// <param name="tx">The x index the edge goes to.</param>
        /// <param name="ty">The y index the edge goes to.</param>
        /// <param name="w">The edges weight.</param>
        public void SetWeight(int fx, int fy, int tx, int ty, float w)
        {
            int i = GetEdgeDirection(fx, fy, tx, ty);
            if (i == -1) return;

            Weights[fx, fy, i] = w;
        }

        /// <summary>
        /// Get a edges weight.
        /// </summary>
        /// <param name="x">The vertices x index.</param>
        /// <param name="y">The vertices y index.</param>
        /// <param name="i">The bit position in the edge connections byte value.</param>
        /// <returns></returns>
        public float GetWeight(int x, int y, int i)
        {
            return Weights[x, y, i];
        }

        /// <summary>
        /// Get a edges weight.
        /// </summary>
        /// <param name="fx">The x index the edge is from.</param>
        /// <param name="fy">The y index the edge is from.</param>
        /// <param name="tx">The x index the edge goes to.</param>
        /// <param name="ty">The y index the edge goes to.</param>
        /// <returns></returns>
        public float GetWeight(int fx, int fy, int tx, int ty)
        {
            int i = GetEdgeDirection(fx, fy, tx, ty);
            if (i == -1) return 0;

            return Weights[fx, fy, i];
        }

        /// <summary>
        /// Add a edge from the vertex at x,y to the vertices
        /// neighbour i.
        /// </summary>
        /// <param name="x">The vertices x index.</param>
        /// <param name="y">The vertices y index.</param>
        /// <param name="i">The bit position in the edge connections byte value.</param>
        /// <param name="w">The edges weight.</param>
        public void AddDirectedWeightedEdge(int x, int y, int i, float w)
        {
            AddDirectedEdge(x, y, i);
            SetWeight(x, y, i, w);
        }

        /// <summary>
        /// Add a edge to and from the vertex at x,y to the vertices
        /// neighbour i.
        /// </summary>
        /// <param name="x">The vertices x index.</param>
        /// <param name="y">The vertices y index.</param>
        /// <param name="i">The bit position in the edge connections byte value.</param>
        /// <param name="w">The edges weight.</param>
        public void AddUndirectedWeightedEdge(int x, int y, int i, float w)
        {
            AddUndirectedEdge(x, y, i);
            SetWeight(x, y, i, w);
        }

        /// <summary>
        /// Create a GridEdge object for the edge
        /// from the vertex at x,y to the neighbour
        /// vertex at i.
        /// </summary>
        /// <param name="x">The vertices x index.</param>
        /// <param name="y">The vertices y index.</param>
        /// <param name="i">The bit position in the edge connections byte value.</param>
        /// <exception cref="ArgumentException">If i less than 0 or greater than 7.</exception>
        /// <returns>The edge object or null if there is no edge between the vertices.</returns>
        public WeighedGridEdge GetEdge(int x, int y, int i)
        {
            if (i < 0 || i > 7)
                throw new ArgumentException("To must represent a bit and have a range of 0-7.");

            if (!Bit.IsSet(Edges[x, y], i)) return null;

            var edge = new WeighedGridEdge(x, y, x, y);

            if (HasWeights)
                edge.Weight = GetWeight(x, y, i);

            return edge;
        }

        /// <summary>
        /// Create a GridEdge object for the edge
        /// from the vertex at fx,fy to the neighbour
        /// vertex at tx,ty.
        /// </summary>
        /// <param name="fx">The x index the edge is from.</param>
        /// <param name="fy">The y index the edge is from.</param>
        /// <param name="tx">The x index the edge goes to.</param>
        /// <param name="ty">The y index the edge goes to.</param>
        /// <returns>The edge object or null if there is no edge between the vertices.</returns>
        public WeighedGridEdge GetEdge(int fx, int fy, int tx, int ty)
        {
            int i = GetEdgeDirection(fx, fy, tx, ty);
            if (i == -1) return null;

            if (!Bit.IsSet(Edges[fx, fy], i)) return null;

            var edge = new WeighedGridEdge(fx, fy, tx, ty);

            if(HasWeights)
                edge.Weight = GetWeight(fx, fy, tx, ty);

            return edge;
        }

        /// <summary>
        /// For each edge create a GridEdge object
        /// and add it to the provided list.
        /// </summary>
        /// <param name="edges">The list of edges to add to.</param>
        /// <param name="GetWeightFunc">A optional function to provide the edges weight. 
        /// If null the edges weight in the graph is used.</param>
        public void GetAllEdges(List<WeighedGridEdge> edges, Func<Point2i, Point2i, float> GetWeightFunc = null)
        {

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int edge = Edges[x, y];
                    if (edge == 0) continue;

                    for (int j = 0; j < Directions.Count; j++)
                    {
                        int i = Directions[j];
                        int xi = x + D8.OFFSETS[i, 0];
                        int yi = y + D8.OFFSETS[i, 1];

                        if (xi < 0 || xi > Width - 1) continue;
                        if (yi < 0 || yi > Height - 1) continue;

                        if ((edge & 1 << i) == 0) continue;

                        var e = new WeighedGridEdge(x, y, xi, yi);

                        if(GetWeightFunc != null)
                            e.Weight = GetWeightFunc(e.From, e.To);
                        else if (HasWeights)
                            e.Weight = GetWeight(e.From.x, e.From.y, e.To.x, e.To.y);

                        edges.Add(e);
                    }
                }
            }

        }

        /// <summary>
        /// Foreach edge the vertex at x,y has create a GridEdge object
        /// and add it to the provided list.
        /// </summary>
        /// <param name="x">The vertices x index.</param>
        /// <param name="y">The vertices y index.</param>
        /// <param name="edges">The list of edges to add to.</param>
        /// <param name="GetWeightFunc">A optional function to provide the edges weight. 
        /// If null the edges weight in the graph is used.</param>
        public void GetEdges(int x, int y, List<WeighedGridEdge> edges, Func<Point2i, Point2i, float> GetWeightFunc  = null)
        {

            int edge = Edges[x, y];
            if (edge == 0) return;

            for (int j = 0; j < Directions.Count; j++)
            {
                int i = Directions[j];
                int xi = x + D8.OFFSETS[i, 0];
                int yi = y + D8.OFFSETS[i, 1];

                if (xi < 0 || xi > Width - 1) continue;
                if (yi < 0 || yi > Height - 1) continue;

                if ((edge & 1 << i) == 0) continue;

                var e = new WeighedGridEdge(x, y, xi, yi);

                if (GetWeightFunc != null)
                    e.Weight = GetWeightFunc(e.From, e.To);
                else if (HasWeights)
                    e.Weight = GetWeight(e.From.x, e.From.y, e.To.x, e.To.y);

                edges.Add(e);
                edges.Add(new WeighedGridEdge(x, y, xi, yi));
            }

        }

        /// <summary>
        /// Convert the GridGraph to a adjacency DirectedGraph.
        /// </summary>
        /// <returns></returns>
        public DirectedGraph ToDirectedGraph()
        {
            var graph = new DirectedGraph(Width * Height);

            var edges = new List<WeighedGridEdge>();
            GetAllEdges(edges);

            foreach (var edge in edges)
            {
                int from = edge.From.x + edge.From.y * Width;
                int to = edge.To.x + edge.To.y * Width;
                float weight = edge.Weight;

                var e = new GraphEdge(from, to, weight, edge);
   
                graph.AddEdge(e);
            }

            return graph;
        }

        /// <summary>
        /// Convert the GridGraph to a adjacency UndirectedGraph.
        /// </summary>
        /// <returns></returns>
        public UndirectedGraph ToUndirectedGraph()
        {
            var graph = new UndirectedGraph(Width * Height);

            var edges = new List<WeighedGridEdge>();
            GetAllEdges(edges);

            foreach (var edge in edges)
            {
                int from = edge.From.x + edge.From.y * Width;
                int to = edge.To.x + edge.To.y * Width;
                float weight = edge.Weight;

                var e = new GraphEdge(from, to, weight, edge);

                graph.AddEdge(e);
            }

            return graph;
        }

    }
}
