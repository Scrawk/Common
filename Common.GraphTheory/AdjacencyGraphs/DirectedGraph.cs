using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.GraphTheory.AdjacencyGraphs
{

    /// <summary>
    /// A directed adjacency graph made op of vertices and edges.
    /// </summary>
    public partial class DirectedGraph : AdjacencyGraph
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public DirectedGraph()
        {

        }

        /// <summary>
        /// Create a graph with a fixed number of vertices.
        /// </summary>
        /// <param name="size">The number of vertices.</param>
        public DirectedGraph(int size) : base(size)
        {

        }

        /// <summary>
        /// Create a graph from a set of vertices.
        /// These vertices must have already had there
        /// index set correctly.
        /// </summary>
        /// <param name="vertices">The graphs vertices.</param>
        public DirectedGraph(IEnumerable<GraphVertex> vertices) : base(vertices)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            return string.Format("[DirectedGraph: VertexCount={0}, EdgeCount={1}]", VertexCount, EdgeCount);
        }

        /// <summary>
        /// Add a edge to the graph.
        /// </summary>
        public void AddDirectedEdge(GraphEdge edge)
        {
            AddEdgeInternal(edge);
        }

        /// <summary>
        /// Add a directed edge.
        /// </summary>
        /// <param name="from">The from vertex index</param>
        /// <param name="to">The to vertex index</param>
        public void AddDirectedEdge(int from, int to)
        {
            AddDirectedEdge(from, to, 0);
        }

        /// <summary>
        /// Add a directed edge.
        /// </summary>
        /// <param name="from">The from vertex index</param>
        /// <param name="to">The to vertex index</param>
        /// <param name="weight">The edge weight</param>
        public void AddDirectedEdge(int from, int to, float weight)
        {
            var edge = new GraphEdge();
            edge.From = from;
            edge.To = to;
            edge.Weight = weight;

            AddEdgeInternal(edge);
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
        public static DirectedGraph FromData<T>(IEnumerable<T> data)
        {
            var graph = new DirectedGraph();

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

        /// <summary>
        /// Convert to a undirected graph by adding a 
        /// edge for any directed edge that does not 
        /// have a opposite edge.
        /// </summary>
        public UndirectedGraph ToUndirectedGraph()
        {
            var graph = new UndirectedGraph(Vertices);

            foreach (var neighbours in Edges)
            {
                if (neighbours == null) continue;

                foreach (var e in neighbours)
                {
                    if (graph.ContainsEdge(e.From, e.To)) continue;

                    var e0 = new GraphEdge(e.From, e.To, e.Weight);
                    var e1 = new GraphEdge(e.To, e.From, e.Weight);

                    graph.AddUndirectedEdge(e0, e1);
                }
            }

            return graph;
        }


    }
}
