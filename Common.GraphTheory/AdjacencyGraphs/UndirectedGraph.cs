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
        /// Add a edge to the graph.
        /// The edge starts at the from vertex 
        /// and ends at the to vertex.
        /// </summary>
        public void AddEdge(int from, int to, float w0 = 0.0f, float w1 = 0.0f)
        {
            var edge = new GraphEdge();
            edge.From = from;
            edge.To = to;
            edge.Weight = w0;

            var opposite = new GraphEdge();
            opposite.From = to;
            opposite.To = from;
            opposite.Weight = w1;

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
