using System;
using System.Collections.Generic;
using System.Text;

using Common.Core.Extensions;

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
        protected List<GraphVertex> Vertices { get; set; }

        /// <summary>
        /// The graph edges.
        /// Each vertex index is used to look up
        /// all the edges going from that vertex.
        /// </summary>
        protected List<List<GraphEdge>> Edges { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            return string.Format("[AdjacencyGraph: VertexCount={0}, EdgeCount={1}]", VertexCount, EdgeCount);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Print()
        {
            var builder = new StringBuilder();
            Print(builder);
            Console.WriteLine(builder.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Print(StringBuilder builder)
        {
            builder.AppendLine(this.ToString());

            builder.AppendLine("Vertices");
            foreach(var v in Vertices)
                builder.AppendLine(v.ToString());

            builder.AppendLine("Edges");
            foreach (var edges in Edges)
            {
                if (edges == null) continue;

                foreach (var e in edges)
                    builder.AppendLine(e.ToString());
            }
                
        }

        /// <summary>
        /// Enumerate through all the vertices in th graph.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GraphVertex> EnumerateVertices()
        {
            foreach(var v in Vertices)
                yield return v;
        }

        /// <summary>
        /// Enumerate through all the edges in th graph.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GraphEdge> EnumerateEdges()
        {
            foreach (var edges in Edges)
            {
                if(edges == null || edges.Count == 0) 
                    continue;

                foreach (var edge in edges)
                    yield return edge;  
            }  
        }

        /// <summary>
        /// Is the index in the graph vertices bounds.
        /// </summary>
        /// <param name="i">The index</param>
        /// <returns>Is the index in the graph vertices bounds.</returns>
        public bool InBounds(int i)
        {
            return (i >= 0 && i < VertexCount);
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
        /// Get the vertex at index i.
        /// </summary>
        /// <param name="i">The vertices index.</param>
        /// <returns>The vertex</returns>
        public GraphVertex GetVertex(int i)
        {
            return Vertices[i]; 
        }

        /// <summary>
        /// Set the vertex at index i.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="vert"></param>
        public void SetVertex(int i, GraphVertex vert)
        {
            Vertices[i] = vert;
        }

        /// <summary>
        /// Get a list of all the vertices.
        /// </summary>
        /// <returns></returns>
        public List<GraphVertex> GetVertices()
        {
            return new List<GraphVertex>(Vertices);
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
        /// Get The edges weight or 0 if edge not found.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public float GetEdgeWeight(int from, int to)
        {
            var edge = GetEdge(from, to);
            if (edge == null) return 0;
            return edge.Weight;
        }

        /// <summary>
        /// Get the edge and if edge
        /// does not already exist create a new one.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public GraphEdge GetEdgeOrCreateEdge(int from, int to)
        {
            var edge = GetEdge(from, to);

            if (edge == null)
            {
                edge = new GraphEdge(from, to);
                AddEdge(edge);
            }

             return edge;
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
        /// Get the first edge fo the vertex.
        /// </summary>
        /// <param name="v">The verts index</param>
        /// <returns>THe first edge.</returns>
        public GraphEdge GetFirstEdge(int v)
        {
            var edges = GetEdges(v);
            if(edges == null) return null;

            return edges[0];
        }

        /// <summary>
        /// Randomize all the edges order in graph
        /// </summary>
        /// <param name="seed">The random generators seed</param>
        public void RandomizeEdges(int seed)
        {
            var rnd = new Random(seed);
            foreach(var edges in Edges)
            {
                if(edges == null)
                    continue;

                edges.Shuffle(rnd);
            }
        }

        /// <summary>
        /// Randomize all the edges order for a vertex.
        /// </summary>
        /// <param name="v">The vertices index.</param>
        /// <param name="rnd">The random generator.</param>
        public void RandomizeEdges(int v, Random rnd)
        {
            var edges = GetEdges(v);
            if (edges == null) return ;

            edges.Shuffle(rnd);
        }

        /// <summary>
        /// Get all the edges of vertex.
        /// </summary>
        /// <param name="i">The vertex index.</param>
        /// <returns>A list of all the edges. Maybe null.</returns>
        public List<GraphEdge> GetEdges(int i)
        {
            return Edges[i];
        }

        /// <summary>
        /// Set the edges for a vertex.
        /// </summary>
        /// <param name="i">The vertices index.</param>
        /// <param name="edges">The vertices edges</param>
        public void SetEdges(int i, List<GraphEdge> edges)
        {
             Edges[i] = edges;
        }

        /// <summary>
        /// Find the vertex index belonging to this data.
        /// </summary>
        public int IndexOfVertexData<T>(T data)
        {
            foreach (var v in Vertices)
            {
                if (ReferenceEquals(data, v.Data))
                    return v.Index;
            }

            return -1;
        }

        /// <summary>
        /// Does the graph contain a vertex at the index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Does the graph contain a vertex at the index.</returns>
        public bool HasVertex(int index)
        {
            for(int i = 0; i < VertexCount; i++)
                if(Vertices[i].Index == i) return true;

            return false;
        }

        /// <summary>
        /// Does the graph contain a edge going
        /// from and to vertices at the indexs.
        /// </summary>
        public bool HasEdge(int from, int to)
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
        /// Get a flattened list of all edges in the graph.
        /// </summary>
        /// <returns>The edges</returns>
        public List<GraphEdge> GetAllEdges()
        {
            var edges = new List<GraphEdge>();
            GetAllEdges(edges);
            return edges;
        }

        /// <summary>
        /// Add a edge to the graph.
        /// Used as a short cut when adding multiple 
        /// edges in derived classes.
        /// </summary>
        public void AddEdge(GraphEdge edge)
        {
            if (HasEdge(edge.From, edge.To))
                throw new InvalidOperationException("Edge already exists.");
;
            int i = edge.From;

            if (Edges[i] == null)
                Edges[i] = new List<GraphEdge>();

            EdgeCount++;
            Edges[i].Add(edge);
        }

        /// <summary>
        /// Add a vertex to graph
        /// </summary>
        /// <param name="vert"></param>
        public void AddVertex(GraphVertex vert)
        {
            if (HasVertex(vert.Index))
                throw new InvalidOperationException("Vertex already exists.");

            vert.Index = Vertices.Count;
            Vertices.Add(vert);
            Edges.Add(null);
        }

        /// <summary>
        /// Create and add a vertex to graph.
        /// </summary>
        /// <returns>The new vertex.</returns>
        public GraphVertex AddVertex(object data = null)
        {
            var vert = new GraphVertex();
            vert.Index = Vertices.Count;
            vert.Data = data;
            Vertices.Add(vert);
            Edges.Add(null);

            return vert;
        }

    }
}
