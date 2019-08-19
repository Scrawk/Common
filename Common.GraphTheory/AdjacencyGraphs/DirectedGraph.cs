using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.GraphTheory.AdjacencyGraphs
{
    /// <summary>
    /// A directed adjacency graph where the vertices data can be any object.
    /// </summary>
    /// <typeparam name="T">The type of object the data represents</typeparam>
    public class DirectedGraph<T> : DirectedGraph<GraphVertex<T>, GraphEdge>
    {

        public DirectedGraph()
        {

        }

        public DirectedGraph(int size) : base(size)
        {

        }

        public DirectedGraph(IEnumerable<GraphVertex<T>> vertices) : base(vertices)
        {

        }

        public DirectedGraph(IEnumerable<T> vertices)
        {
            Vertices = new List<GraphVertex<T>>();
            Edges = new List<List<GraphEdge>>(Vertices.Count);

            foreach (var data in vertices)
            {
                int index = Vertices.Count;
                var v = new GraphVertex<T>(index, data);
                Vertices.Add(v);
                Edges.Add(null);
            }


        }

        /// <summary>
        /// Find the vertex index belonging to this data.
        /// </summary>
        public int IndexOf(T data)
        {
            foreach (var v in Vertices)
            {
                if (ReferenceEquals(data, v.Data))
                    return v.Index;
            }

            return -1;
        }

        public UndirectedGraph<T> ToUndirectedGraph()
        {
            var graph = new UndirectedGraph<T>(Vertices);

            foreach(var neighbours in Edges)
            {
                if (neighbours == null) continue;

                foreach (var e in neighbours)
                {
                    if (graph.ContainsEdge(e.From, e.To)) continue;

                    var e0 = new GraphEdge(e.From, e.To, e.Weight);
                    var e1 = new GraphEdge(e.To, e.From, e.Weight);

                    graph.AddEdge(e0, e1);
                }
            }

            return graph;
        }

    }

    /// <summary>
    /// A directed adjacency graph made op of vertices and edges.
    /// </summary>
    public partial class DirectedGraph<VERTEX, EDGE> : AdjacencyGraph<VERTEX, EDGE>
        where EDGE : class, IGraphEdge, new()
        where VERTEX : class, IGraphVertex, new()
    {

        public DirectedGraph()
        {

        }

        public DirectedGraph(int size) : base(size)
        {

        }

        public DirectedGraph(IEnumerable<VERTEX> vertices) : base(vertices)
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
        public void AddEdge(EDGE edge)
        {
            AddEdgeInternal(edge);
        }


        /// <summary>
        /// Add a edge to the graph.
        /// The edge starts at the from vertex 
        /// and ends at the to vertex.
        /// </summary>
        public void AddEdge(int from, int to, float weight = 0.0f)
        {
            EDGE edge = new EDGE();
            edge.From = from;
            edge.To = to;
            edge.Weight = weight;

            AddEdgeInternal(edge);
        }

    }
}
