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

        public UndirectedGraph()
        {

        }

        public UndirectedGraph(int size) : base(size)
        {

        }

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
        public void AddEdge(GraphEdge edge, GraphEdge opposite)
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
        /// <param name="w">The edges weight</param>
        public void AddEdge(int from, int to)
        {
            AddEdge(from, to, 0, 0);
        }

        /// <summary>
        /// Add a undirected edge.
        /// A undirected edge has a edge going both ways
        /// </summary>
        /// <param name="from">The from vertex index</param>
        /// <param name="to">The to vertex index</param>
        /// <param name="weight">The edges weight</param>
        public void AddEdge(int from, int to, float weight)
        {
            AddEdge(from, to, weight, weight);
        }

        /// <summary>
        /// Add a undirected edge.
        /// A undirected edge has a edge going both ways
        /// </summary>
        /// <param name="from">The from vertex index</param>
        /// <param name="to">The to vertex index</param>
        /// <param name="w0">The edge going from-to weight</param>
        /// <param name="w1">The edge going to-from weigh</param>
        public void AddEdge(int from, int to, float weight0, float weight1)
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
