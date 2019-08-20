using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.GraphTheory.AdjacencyGraphs
{

    /// <summary>
    /// A adjacency graph made op of vertices and edges.
    /// </summary>
    public abstract partial class AdjacencyGraph<VERTEX, EDGE>
        where EDGE : class, IGraphEdge, new()
        where VERTEX : class, IGraphVertex, new()
    {
        protected const int NOT_VISITED_TAG = 0;
        protected const int IS_VISITED_TAG = 1;

        public AdjacencyGraph()
        {
            Vertices = new List<VERTEX>();
            Edges = new List<List<EDGE>>();
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
        public List<VERTEX> Vertices { get; set; }

        /// <summary>
        /// The graph edges.
        /// Each vertex index is used to look up
        /// all the edges going from that vertex.
        /// </summary>
        public List<List<EDGE>> Edges { get; set; }

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
        /// Clear the graph edges and leave the vertices.
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
        /// Set the vertices tag.
        /// </summary>
        public void TagVertices(int tag)
        {
            for (int i = 0; i < VertexCount; i++)
                Vertices[i].Tag = tag;
        }

        /// <summary>
        /// Find the edge going
        /// from and to vertices at the indexs.
        /// </summary>
        public EDGE FindEdge(int from, int to)
        {
            if (Edges[from] == null)
                return null;

            foreach (var e in Edges[from])
                if (e.To == to)
                    return e;

            return null;
        }


        /// <summary>
        /// Does the graph contain a edge going
        /// from and to vertices at the indexs.
        /// </summary>
        public bool ContainsEdge(int from, int to)
        {
            return FindEdge(from, to) != null;
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
        public void GetAllEdges<T>(List<T> edges)
            where T : class, IGraphEdge, new()
        {
            for (int i = 0; i < VertexCount; i++)
            {
                if (Edges[i] == null || Edges[i].Count == 0) continue;

                for (int j = 0; j < Edges[i].Count; j++)
                    edges.Add(Edges[i][j] as T);
            }
        }

        /// <summary>
        /// Get a flattened list of all edges in the tree.
        /// </summary>
        public void GetAllEdges<T>(List<T> edges, GraphTree tree)
            where T : class, IGraphEdge, new()
        {
            for (int i = 0; i < tree.Count; i++)
            {
                var children = tree.Children[i];
                if (children == null) continue;

                for (int j = 0; j < children.Count; j++)
                {
                    var c = children[j];
                    edges.Add(FindEdge(i, c) as T);
                }
            }
        }

        /// <summary>
        /// Find the sum of the weights from this tree.
        /// </summary>
        public float FindWeightSum(GraphTree tree)
        {
            float sum = 0;
            for (int i = 0; i < tree.Count; i++)
            {
                var children = tree.Children[i];
                if (children == null) continue;

                for (int j = 0; j < children.Count; j++)
                {
                    var c = children[j];
                    var edge = FindEdge(i, c);

                    sum += edge.Weight;
                }
            }

            return sum;
        }

        /// <summary>
        /// Find the sum of the weights from this path.
        /// </summary>
        public float FindWeightSum(IList<int> path)
        {
            float sum = 0;
            for (int i = 0; i < path.Count-1; i++)
            {
                int i0 = path[i + 0];
                int i1 = path[i + 1];
                sum += FindEdge(i0, i1).Weight;
            }
                
            return sum;
        }

        /// <summary>
        /// Add a edge to the graph.
        /// Used as a short cut when adding multiple 
        /// edges in derived classes.
        /// </summary>
        protected void AddEdgeInternal(EDGE edge)
        {
            int i = edge.From;

            if (Edges[i] == null)
                Edges[i] = new List<EDGE>();

            EdgeCount++;
            Edges[i].Add(edge);
        }

    }
}
