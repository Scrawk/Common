using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.GraphTheory.AdjacencyGraphs
{

    /// <summary>
    /// A undirected adjacency graph made op of vertices and edges.
    /// </summary>
    public partial class UndirectedGraph : AdjacencyGraph
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public UndirectedGraph()
        {

        }

        /// <summary>
        /// Create a graph with a fixed number of vertices.
        /// </summary>
        /// <param name="size">The number of vertices.</param>
        public UndirectedGraph(int size) : base(size)
        {

        }

        /// <summary>
        /// Create a graph from a set of vertices.
        /// These vertices must have already had there
        /// index set correctly.
        /// </summary>
        /// <param name="vertices">The graphs vertices.</param>
        public UndirectedGraph(IEnumerable<GraphVertex> vertices) : base(vertices)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            return string.Format("[UndirectedGraph: VertexCount={0}, EdgeCount={1}]", VertexCount, EdgeCount);
        }

        /// <summary>
        /// Add a edge to the graph.
        /// </summary>
        public void AddUndirectedEdge(GraphEdge edge, GraphEdge opposite)
        {
            if (edge.From != opposite.To)
                throw new ArgumentException("Edge does not go to opposite.");

            if (opposite.From != edge.To)
                throw new ArgumentException("Opposite does not go to edge.");

            AddEdgeInternal(edge);
            AddEdgeInternal(opposite);
        }

        /// <summary>
        /// Add a undirected edge.
        /// A undirected edge has a edge going both ways
        /// </summary>
        /// <param name="from">The from vertex index</param>
        /// <param name="to">The to vertex index</param>
        public void AddUndirectedEdge(int from, int to)
        {
            AddUndirectedEdge(from, to, 0, 0);
        }

        /// <summary>
        /// Add a undirected edge.
        /// A undirected edge has a edge going both ways
        /// </summary>
        /// <param name="from">The from vertex index</param>
        /// <param name="to">The to vertex index</param>
        /// <param name="weight">The edges weight</param>
        public void AddUndirectedEdge(int from, int to, float weight)
        {
            AddUndirectedEdge(from, to, weight, weight);
        }

        /// <summary>
        /// Add a undirected edge.
        /// A undirected edge has a edge going both ways
        /// </summary>
        /// <param name="from">The from vertex index</param>
        /// <param name="to">The to vertex index</param>
        /// <param name="weight0">The edge going from-to weight</param>
        /// <param name="weight1">The edge going to-from weigh</param>
        public void AddUndirectedEdge(int from, int to, float weight0, float weight1)
        {
            var edge = new GraphEdge();
            edge.From = from;
            edge.To = to;
            edge.Weight = weight0;

            var opposite = new GraphEdge();
            opposite.From = to;
            opposite.To = from;
            opposite.Weight = weight1;

            AddEdgeInternal(edge);
            AddEdgeInternal(opposite);
        }

        /// <summary>
        /// Create a graph of vertices from the enumeration of data.
        /// A vertex for each data object will be created.
        /// </summary>
        public static UndirectedGraph FromData<T>(IEnumerable<T> data)
        {
            var graph = new UndirectedGraph();

            graph.Vertices = new List<GraphVertex>();
            graph.Edges = new List<List<GraphEdge>>();

            foreach (var datum in data)
            {
                int index = graph.Vertices.Count;
                var v = new GraphVertex(index, datum);
                graph.Vertices.Add(v);
                graph.Edges.Add(null);
            }

            return graph;
        }

    }
}
