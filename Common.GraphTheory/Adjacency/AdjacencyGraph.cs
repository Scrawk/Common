using System;
using System.Collections.Generic;

namespace Common.GraphTheory.Adjacency
{

    public class AdjacencyGraph<T> : AdjacencyGraph<AdjacencyVertex<T>, AdjacencyEdge>
    {
        public AdjacencyGraph(int size) : base(size) { }

        public AdjacencyGraph(IEnumerable<AdjacencyVertex<T>> vertices) : base(vertices) { }

        public AdjacencyGraph(IEnumerable<T> vertices)
        {
            Vertices = new List<AdjacencyVertex<T>>();
            foreach(var data in vertices)
            {
                var v = new AdjacencyVertex<T>();
                v.Index = Vertices.Count;
                v.Data = data;
                Vertices.Add(v);
            }

            Edges = new List<AdjacencyEdge>[Vertices.Count];
        }

        public int IndexOf(T data)
        {
            foreach(var v in Vertices)
            {
                if (ReferenceEquals(data, v.Data))
                    return v.Index;
            }

            return -1;
        }

    }

    public class AdjacencyGraph<VERTEX, EDGE> 
        where EDGE : class, IAdjacencyEdge, new()
        where VERTEX : class, IAdjacencyVertex, new()
    {

        public int VertexCount { get { return Vertices.Count; } }

        public int EdgeCount { get; private set; }

        public IList<VERTEX> Vertices { get; protected set; }

        public IList<IList<EDGE>> Edges { get; protected set; }

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

        public override string ToString()
        {
            return string.Format("[AdjacencyGraph: VertexCount={0}, EdgeCount={1}]", VertexCount, EdgeCount);
        }

        public void SetVertexIndices()
        {
            for (int i = 0; i < VertexCount; i++)
                Vertices[i].Index = i;
        }

        public void AddEdge(EDGE edge)
        {
            int i = edge.From;

            if (Edges[i] == null)
                Edges[i] = new List<EDGE>();

            EdgeCount++;
            Edges[i].Add(edge);
        }

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

        public int GetDegree(int i)
        {
            if (Edges[i] == null)
                return 0;
            else
                return Edges[i].Count;
        }

        public void GetAllEdges(List<EDGE> edges)
        {
            for (int i = 0; i < VertexCount; i++)
            {
                if (Edges[i] == null || Edges[i].Count == 0) continue;
                edges.AddRange(Edges[i]);
            }
        }

        public void DepthFirstSearch(AdjacencySearch search, int root)
        {
            Searches.DepthFirstSearch.Search(this, search, root);
        }

        public void BreadthFirstSearch(AdjacencySearch search, int root)
        {
            Searches.BreadthFirstSearch.Search(this, search, root);
        }

        public List<VERTEX> KhansTopologicalSort()
        {
            return Searches.KhansTopologicalSort.Sort(this);
        }

        public void PrimsMinimumSpanningTree(AdjacencySearch search, int root)
        {
            Searches.PrimsMinimumSpanningTree.Search(this, search, root);
        }

        public void DijkstrasShortestPathTree(AdjacencySearch search, int root)
        {
            Searches.DijkstrasShortestPathTree.Search(this, search, root);
        }

        public Dictionary<int, List<EDGE>> KruskalsMinimumSpanningForest()
        {
            return Searches.KruskalsMinimumSpanningForest.Search(this);
        }

        public AdjacencyFlowGraph<VERTEX> FoldFulkersonMaxFlow(int source, int sink)
        {
            return Searches.FordFulkersonAdjacency.MaxFlow(this, source, sink);
        }

    }
}
