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
        /// Create a deep copy of the graph.
        /// </summary>
        /// <param name="edgeDataCopy">Optonal func to copy edge data.</param>
        /// <param name="vertDataCopy">ptonal func to copy vertex data.</param>
        /// <returns></returns>
        public DirectedGraph Copy(Func<object, object> edgeDataCopy = null, Func<object, object> vertDataCopy = null)
        {
            var copy = new DirectedGraph(VertexCount);

            for (int i = 0; i < VertexCount; i++)
            {
                copy.SetVertex(i, GetVertex(i).Copy(vertDataCopy));
            }

            for (int i = 0; i < VertexCount; i++)
            {
                var edges = GetEdges(i);
                if (edges == null) continue;

                var list = new List<GraphEdge>(edges.Count);

                for (int j = 0; j < edges.Count; j++)
                {
                    list.Add(edges[j].Copy(edgeDataCopy));
                }

                copy.SetEdges(i, list);
            }

            return copy;
        }

        /// <summary>
        /// Add a edge to the graph.
        /// </summary>
        public void AddDirectedEdge(GraphEdge edge)
        {
            AddEdge(edge);
        }

        /// <summary>
        /// Add a directed edge.
        /// </summary>
        /// <param name="from">The from vertex index</param>
        /// <param name="to">The to vertex index</param>
        public GraphEdge AddDirectedEdge(int from, int to)
        {
            return AddDirectedWeightedEdge(from, to, 0, null);
        }

        /// <summary>
        /// Add a directed edge.
        /// </summary>
        /// <param name="from">The from vertex index</param>
        /// <param name="to">The to vertex index</param>
        public GraphEdge AddDirectedWeightedEdge(int from, int to, float weight)
        {
            return AddDirectedWeightedEdge(from, to, weight, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public GraphEdge AddDirectedEdge(int from, int to, object data)
        {
            return AddDirectedWeightedEdge(from, to, 0, data);
        }

        /// <summary>
        /// Add a directed edge.
        /// </summary>
        /// <param name="from">The from vertex index</param>
        /// <param name="to">The to vertex index</param>
        /// <param name="weight">The edge weight</param>
        public GraphEdge AddDirectedWeightedEdge(int from, int to, float weight, object data)
        {
            var edge = new GraphEdge();
            edge.From = from;
            edge.To = to;
            edge.Weight = weight;
            edge.Data = data;

            AddEdge(edge);

            return edge;
        }

        /// <summary>
        /// Add a undirected edge.
        /// A undirected edge has a edge going both ways
        /// </summary>
        /// <param name="from">The from vertex index</param>
        /// <param name="to">The to vertex index</param>
        public GraphEdge AddUndirectedEdge(int from, int to)
        {
            return AddUndirectedWeightedEdge(from, to, 0, 0, null, null);
        }

        /// <summary>
        /// Add a undirected edge.
        /// A undirected edge has a edge going both ways
        /// </summary>
        /// <param name="from">The from vertex index</param>
        /// <param name="to">The to vertex index</param>
        /// <param name="weight">The edges weight</param>
        public GraphEdge AddUndirectedWeightedEdge(int from, int to, float weight)
        {
            return AddUndirectedWeightedEdge(from, to, weight, weight, null, null);
        }

        /// <summary>
        /// Add a undirected edge.
        /// A undirected edge has a edge going both ways
        /// </summary>
        /// <param name="from">The from vertex index</param>
        /// <param name="to">The to vertex index</param>
        /// <param name="weight">The edges weight</param>
        public GraphEdge AddUndirectedWeightedEdge(int from, int to, float weight, object data)
        {
            return AddUndirectedWeightedEdge(from, to, weight, weight, data, data);
        }

        /// <summary>
        /// Add a undirected edge.
        /// A undirected edge has a edge going both ways
        /// </summary>
        /// <param name="from">The from vertex index</param>
        /// <param name="to">The to vertex index</param>
        /// <param name="weight0">The edge going from-to weight</param>
        /// <param name="weight1">The edge going to-from weigh</param>
        public GraphEdge AddUndirectedWeightedEdge(int from, int to, float weight0, float weight1, object data1, object data2)
        {
            var edge = new GraphEdge();
            edge.From = from;
            edge.To = to;
            edge.Weight = weight0;
            edge.Data = data1;

            var opposite = new GraphEdge();
            opposite.From = to;
            opposite.To = from;
            opposite.Weight = weight1;
            opposite.Data = data2;

            AddEdge(edge);
            AddEdge(opposite);

            return edge;
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
            var graph = new UndirectedGraph(base.Vertices);

            foreach (var neighbours in base.Edges)
            {
                if (neighbours == null) continue;

                foreach (var e in neighbours)
                {
                    if (graph.HasEdge(e.From, e.To)) continue;

                    var e0 = new GraphEdge(e.From, e.To, e.Weight);
                    var e1 = new GraphEdge(e.To, e.From, e.Weight);

                    graph.AddUndirectedEdge(e0, e1);
                }
            }

            return graph;
        }

        /// <summary>
        /// Create a graph from a matrix.
        /// Non zero entries represent a edge with the value being the edges weight.
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static DirectedGraph FromMatrix(int[,] matrix)
        {
            if (matrix.GetLength(0) != matrix.GetLength(1))
                throw new ArgumentException("Matrix must be square");

            int vertices = matrix.GetLength(0);

            var graph = new DirectedGraph(vertices);

            for(int x = 0; x < vertices; x++)
            {
                for (int y = 0; y < vertices; y++)
                {
                    int weight = matrix[x, y];  

                    if(weight != 0)
                    {
                        graph.AddDirectedWeightedEdge(x, y, weight);
                    }
                }
            }

            return graph;
        }


    }
}
