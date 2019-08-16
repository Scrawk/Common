using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.GraphTheory.AdjacencyGraphs
{
    /// <summary>
    /// A adjacency graph where the vertices data can be any object.
    /// </summary>
    /// <typeparam name="T">The type of object the data represents</typeparam>
    public class AdjacencyGraph<T> : AdjacencyGraph<AdjacencyVertex<T>, AdjacencyEdge>
    {
        public AdjacencyGraph(int size) : base(size)
        {
        }

        public AdjacencyGraph(IEnumerable<AdjacencyVertex<T>> vertices) : base(vertices)
        {
        }

        public AdjacencyGraph(IEnumerable<T> vertices)
        {
            Vertices = new List<AdjacencyVertex<T>>();
            Edges = new List<List<AdjacencyEdge>>(Vertices.Count);

            foreach (var data in vertices)
            {
                var v = new AdjacencyVertex<T>();
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
    /// A adjacency graph made op of vertices and edges.
    /// </summary>
    public class AdjacencyGraph<VERTEX, EDGE> 
        where EDGE : class, IAdjacencyEdge, new()
        where VERTEX : class, IAdjacencyVertex, new()
    {

        public AdjacencyGraph()
        {

        }

        public AdjacencyGraph(int size)
        {
            Vertices = new List<VERTEX>(size);
            Edges = new List<List<EDGE>>(size);
            Fill(size);
        }

        public AdjacencyGraph(IEnumerable<VERTEX> vertices)
        {
            Vertices = new List<VERTEX>(vertices);
            Edges = new List<List<EDGE>>(Vertices.Count);

            for (int i = 0; i < Vertices.Count; i++)
                Edges.Add(null);
        }

        public int VertexCount { get { return Vertices.Count; } }

        public int EdgeCount { get; private set; }

        public List<VERTEX> Vertices { get; set; }

        public List<List<EDGE>> Edges { get; set; }

        public override string ToString()
        {
            return string.Format("[AdjacencyGraph: VertexCount={0}, EdgeCount={1}]", VertexCount, EdgeCount);
        }

        /// <summary>
        /// Clear the graph.
        /// </summary>
        public virtual void Clear()
        {
            EdgeCount = 0;
            Vertices.Clear();
            Edges.Clear();
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
                var v = new VERTEX();
                v.Index = i;
                Vertices.Add(v);
                Edges.Add(null);
            }
        }

        /// <summary>
        /// Applies the vertex index.
        /// </summary>
        public void SetVertexIndices()
        {
            for (int i = 0; i < VertexCount; i++)
                Vertices[i].Index = i;
        }

        /// <summary>
        /// Sets all edge tags.
        /// </summary>
        public void TagEdges(int tag)
        {
            for (int i = 0; i < Edges.Count; i++)
                for (int j = 0; j < Edges[i].Count; j++)
                    Edges[i][j].Tag = tag;
        }


        /// <summary>
        /// Add a edge to the graph.
        /// </summary>
        public void AddEdge(EDGE edge)
        {
            int i = edge.From;

            if (Edges[i] == null)
                Edges[i] = new List<EDGE>();

            EdgeCount++;
            Edges[i].Add(edge);
        }

        /// <summary>
        /// Does the graph contain a edge going
        /// from and to vertices at the indexs.
        /// </summary>
        public bool ContainsEdge(int from, int to)
        {
            if (Edges[from] == null)
                return false;

            foreach (var e in Edges[from])
                if (e.To == to)
                    return true;

            return false;
        }

        /// <summary>
        /// Add a edge to the graph.
        /// The edge starts at the from vertex 
        /// and ends at the to vertex.
        /// </summary>
        public void AddEdge(VERTEX from, VERTEX to, float weight = 0.0f)
        {
            int i = from.Index;
            int j = to.Index;

            if (Edges[i] == null)
                Edges[i] = new List<EDGE>();

            EDGE edge = new EDGE();
            edge.From = i;
            edge.To = j;
            edge.Weight = weight;

            EdgeCount++;
            Edges[i].Add(edge);
        }

        /// <summary>
        /// Add a edge to the graph.
        /// The edge starts at the from vertex 
        /// and ends at the to vertex.
        /// </summary>
        public void AddEdge(int from, int to, float weight = 0.0f)
        {
            int i = from;
            int j = to;

            if (Edges[i] == null)
                Edges[i] = new List<EDGE>();

            EDGE edge = new EDGE();
            edge.From = i;
            edge.To = j;
            edge.Weight = weight;

            EdgeCount++;
            Edges[i].Add(edge);
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
        public void GetAllEdges(List<EDGE> edges)
        {
            for (int i = 0; i < VertexCount; i++)
            {
                if (Edges[i] == null || Edges[i].Count == 0) continue;
                edges.AddRange(Edges[i]);
            }
        }

    }
}
