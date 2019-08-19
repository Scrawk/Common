using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.GraphTheory.AdjacencyGraphs
{
    /// <summary>
    /// A undirected adjacency graph where the vertices data can be any object.
    /// </summary>
    /// <typeparam name="T">The type of object the data represents</typeparam>
    public class UndirectedGraph<T> : UndirectedGraph<GraphVertex<T>, GraphEdge>
    {

        public UndirectedGraph()
        {

        }

        public UndirectedGraph(int size) : base(size)
        {

        }

        public UndirectedGraph(IEnumerable<GraphVertex<T>> vertices) : base(vertices)
        {

        }

        public UndirectedGraph(IEnumerable<T> vertices)
        {
            Vertices = new List<GraphVertex<T>>();
            Edges = new List<List<GraphEdge>>(Vertices.Count);

            foreach (var data in vertices)
            {
                var v = new GraphVertex<T>();
                v.Index = Vertices.Count;
                v.Data = data;
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

    }

    /// <summary>
    /// A undirected adjacency graph made op of vertices and edges.
    /// </summary>
    public partial class UndirectedGraph<VERTEX, EDGE> : AdjacencyGraph<VERTEX, EDGE>
        where EDGE : class, IGraphEdge, new()
        where VERTEX : class, IGraphVertex, new()
    {

        public UndirectedGraph()
        {

        }

        public UndirectedGraph(int size) : base(size)
        {

        }

        public UndirectedGraph(IEnumerable<VERTEX> vertices) : base(vertices)
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
        public void AddEdge(EDGE edge, EDGE opposite)
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
            EDGE edge = new EDGE();
            edge.From = from;
            edge.To = to;
            edge.Weight = w0;

            EDGE opposite = new EDGE();
            opposite.From = to;
            opposite.To = from;
            opposite.Weight = w1;

            AddEdgeInternal(edge);
            AddEdgeInternal(opposite);
        }

    }
}
