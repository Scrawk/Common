using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Meshing.AdjacencyGraphs
{
    public static partial class AdjacencyGraphSearch
    {
        public static List<VERTEX> KhansTopologicalSort<VERTEX, EDGE>(AdjacencyGraph<VERTEX, EDGE> graph)
            where EDGE : class, IAdjacencyEdge, new()
            where VERTEX : class, IAdjacencyVertex, new()
        {

            List<VERTEX> list = new List<VERTEX>();
            LinkedList<VERTEX> vertices = new LinkedList<VERTEX>();

            int edgeCount = graph.Edges.Count;
            List<EDGE>[] edges = new List<EDGE>[edgeCount];

            for (int i = 0; i < edgeCount; i++)
            {
                if (graph.Edges[i] == null) continue;
                edges[i] = new List<EDGE>(graph.Edges[i]);
            }

            for (int i = 0; i < graph.Vertices.Count; i++)
            {
                int idegree = GetInverseDegree(edges, i);

                if (idegree == 0)
                    vertices.AddLast(graph.Vertices[i]);
            }

            while (vertices.Count > 0)
            {
                VERTEX v = vertices.Last.Value;
                vertices.RemoveLast();

                list.Add(v);
                int i = v.Index;

                if (edges[i] == null || edges[i].Count == 0) continue;

                for (int j = 0; j < edges[i].Count; j++)
                {
                    int to = edges[i][j].To;

                    int idegree = GetInverseDegree(edges, to);
                    if (idegree == 1)
                    {
                        vertices.AddLast(graph.Vertices[to]);
                    }
                }

                edges[i].Clear();

            }

            if (CountEdges(edges) > 0)
                throw new InvalidOperationException("Can not find a topological sort on a cyclic graph");
            else
                return list;

        }

        private static int GetInverseDegree<EDGE>(List<EDGE>[] Edges, int i)
            where EDGE : class, IAdjacencyEdge, new()
        {
            int degree = 0;

            foreach (var edges in Edges)
            {
                if (edges == null || edges.Count == 0) continue;

                foreach (var edge in edges)
                {
                    if (edge.To == i) degree++;
                }
            }

            return degree;
        }

        private static int CountEdges<EDGE>(List<EDGE>[] Edges)
            where EDGE : class, IAdjacencyEdge, new()
        {
            int count = 0;

            foreach (var edges in Edges)
            {
                if (edges == null) continue;
                count += edges.Count;
            }

            return count;
        }
    }
}
