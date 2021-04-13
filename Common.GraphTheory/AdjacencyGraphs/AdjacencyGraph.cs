using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.GraphTheory.AdjacencyGraphs
{

    /// <summary>
    /// A adjacency graph made op of vertices and edges.
    /// </summary>
    public abstract partial class AdjacencyGraph
    {
        /// <summary>
        /// Use to tag if vertices have been visited or not.
        /// </summary>
        protected const int NOT_VISITED_TAG = 0;
        protected const int IS_VISITED_TAG = 1;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public AdjacencyGraph()
        {
            Vertices = new List<GraphVertex>();
            Edges = new List<List<GraphEdge>>();
        }

        /// <summary>
        /// Create a graph with a fixed number of vertices.
        /// </summary>
        /// <param name="size">The number of vertices.</param>
        public AdjacencyGraph(int size)
        {
            Vertices = new List<GraphVertex>(size);
            Edges = new List<List<GraphEdge>>(size);
            Fill(size);
        }

        /// <summary>
        /// Create a graph from a set of vertices.
        /// These vertices must have already had there
        /// index set correctly.
        /// </summary>
        /// <param name="vertices">The graphs vertices.</param>
        public AdjacencyGraph(IEnumerable<GraphVertex> vertices)
        {
            Vertices = new List<GraphVertex>(vertices);
            Edges = new List<List<GraphEdge>>(Vertices.Count);

            for (int i = 0; i < Vertices.Count; i++)
            {
                if (Vertices[i].Index != i)
                    throw new InvalidOperationException("Vertex index is not correct.");

                Edges.Add(null);
            }
        }

        /// <summary>
        /// The number of vertices in graph.
        /// </summary>
        public int VertexCount { get { return Vertices.Count; } }

        /// <summary>
        /// The number of edges in graph.
        /// </summary>
        public int EdgeCount { get; protected set; }

        /// <summary>
        /// The graph vertices.
        /// The vertex index must match its position in array.
        /// </summary>
        public List<GraphVertex> Vertices { get; set; }

        /// <summary>
        /// The graph edges.
        /// Each vertex index is used to look up
        /// all the edges going from that vertex.
        /// </summary>
        public List<List<GraphEdge>> Edges { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            return string.Format("[AdjacencyGraph: VertexCount={0}, EdgeCount={1}]", VertexCount, EdgeCount);
        }

        /// <summary>
        /// Clear the graph.
        /// </summary>
        public void Clear()
        {
            Vertices.Clear();
            ClearEdges();
        }

        /// <summary>
        /// Clear the edges and leave the vertices.
        /// </summary>
        public void ClearEdges()
        {
            EdgeCount = 0;
            for (int i = 0; i < VertexCount; i++)
            {
                if (Edges[i] == null) continue;
                Edges[i].Clear();
            }
        }

        /// <summary>
        /// Clear the edges for the vertex at the index.
        /// </summary>
        public void ClearEdges(int index)
        {
            if (Edges[index] == null) return;
            Edges[index].Clear();
        }

        /// <summary>
        /// Fill the graph.
        /// </summary>
        public void Fill(int size)
        {
            Clear();
            Vertices.Capacity = size;
            Edges.Capacity = size;

            for (int i = 0; i < size; i++)
            {
                var v = new GraphVertex();
                v.Index = i;
                Vertices.Add(v);
                Edges.Add(null);
            }
        }

        /// <summary>
        /// Set the vertices tag.
        /// </summary>
        public void TagVertices(int tag)
        {
            for (int i = 0; i < VertexCount; i++)
                Vertices[i].Tag = tag;
        }

        /// <summary>
        /// Get the data belonging to
        /// the vertex at index i.
        /// </summary>
        public T GetVertexData<T>(int i)
        {
            return (T)Vertices[i].Data;
        }

        /// <summary>
        /// Get the edge going from and 
        /// to vertices at the indexs.
        /// </summary>
        public T GetEdgeData<T>(int from, int to)
        {
            var edge = GetEdge(from, to);
            if (edge == null) return default(T);
            return (T)edge.Data;
        }

        /// <summary>
        /// Get the edge going from and 
        /// to vertices at the indexs.
        /// </summary>
        public GraphEdge GetEdge(int from, int to)
        {
            if (Edges[from] == null)
                return null;

            foreach (var e in Edges[from])
                if (e.To == to)
                    return e;

            return null;
        }

        /// <summary>
        /// Find the vertex index belonging to this data.
        /// </summary>
        public int IndexOf<T>(T data)
        {
            foreach (var v in Vertices)
            {
                if (ReferenceEquals(data, v.Data))
                    return v.Index;
            }

            return -1;
        }

        /// <summary>
        /// Does the graph contain a edge going
        /// from and to vertices at the indexs.
        /// </summary>
        public bool ContainsEdge(int from, int to)
        {
            return GetEdge(from, to) != null;
        }

        /// <summary>
        /// Get the number of edges that start from the 
        /// vertex at index.
        /// </summary>
        /// <param name="index">The vertices index.</param>
        public int GetDegree(int index)
        {
            if (Edges[index] == null)
                return 0;
            else
                return Edges[index].Count;
        }

        /// <summary>
        /// Get a flattened list of all edges in the graph.
        /// </summary>
        public void GetAllEdges(List<GraphEdge> edges)
        {
            for (int i = 0; i < VertexCount; i++)
            {
                if (Edges[i] == null || Edges[i].Count == 0) continue;
                edges.AddRange(Edges[i]);
            }
        }

        /// <summary>
        /// Add a edge to the graph.
        /// Used as a short cut when adding multiple 
        /// edges in derived classes.
        /// </summary>
        protected void AddEdgeInternal(GraphEdge edge)
        {
            int i = edge.From;

            if (Edges[i] == null)
                Edges[i] = new List<GraphEdge>();

            EdgeCount++;
            Edges[i].Add(edge);
        }

    }
}
