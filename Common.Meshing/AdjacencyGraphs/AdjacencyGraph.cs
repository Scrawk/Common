using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Meshing.AdjacencyGraphs
{
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
            Vertices = new VERTEX[size];
            Edges = new List<EDGE>[size];

            for (int i = 0; i < size; i++)
            {
                Vertices[i] = new VERTEX();
                Vertices[i].Index = i;
            }
        }

        public AdjacencyGraph(IEnumerable<VERTEX> vertices)
        {
            Vertices = new List<VERTEX>(vertices);
            Edges = new List<EDGE>[Vertices.Count];
        }

        public int VertexCount { get { return Vertices.Count; } }

        public int EdgeCount { get; private set; }

        public IList<VERTEX> Vertices { get; set; }

        public IList<IList<EDGE>> Edges { get; set; }

        public override string ToString()
        {
            return string.Format("[AdjacencyGraph: VertexCount={0}, EdgeCount={1}]", VertexCount, EdgeCount);
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
        /// Applies the vertex index as a tag.
        /// </summary>
        public void TagVertices()
        {
            for (int i = 0; i < Vertices.Count; i++)
                Vertices[i].Tag = i;
        }

        /// <summary>
        /// Sets all vertex tags.
        /// </summary>
        public void TagVertices(int tag)
        {
            for (int i = 0; i < Vertices.Count; i++)
                Vertices[i].Tag = tag;
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
        /// Sets all vertex, edge and face tags.
        /// </summary>
        public void TagAll(int tag)
        {
            TagVertices(tag);
            TagEdges(tag);
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
