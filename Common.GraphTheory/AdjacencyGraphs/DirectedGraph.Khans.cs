using System;
using System.Collections.Generic;
using System.Text;

namespace Common.GraphTheory.AdjacencyGraphs
{

    public partial class DirectedGraph<VERTEX, EDGE> : AdjacencyGraph<VERTEX, EDGE>
        where EDGE : class, IGraphEdge, new()
        where VERTEX : class, IGraphVertex, new()
    {

        public List<VERTEX> KhansTopologicalSort()
        {

            List<VERTEX> list = new List<VERTEX>();
            LinkedList<VERTEX> vertices = new LinkedList<VERTEX>();

            int edgeCount = Edges.Count;
            List<EDGE>[] edges = new List<EDGE>[edgeCount];

            for (int i = 0; i < edgeCount; i++)
            {
                if (Edges[i] == null) continue;
                edges[i] = new List<EDGE>(Edges[i]);
            }

            for (int i = 0; i < Vertices.Count; i++)
            {
                int idegree = Khans_GetInverseDegree(edges, i);

                if (idegree == 0)
                    vertices.AddLast(Vertices[i]);
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

                    int idegree = Khans_GetInverseDegree(edges, to);
                    if (idegree == 1)
                    {
                        vertices.AddLast(Vertices[to]);
                    }
                }

                edges[i].Clear();
            }

            if (Khans_CountEdges(edges) > 0)
                throw new InvalidOperationException("Can not find a topological sort on a cyclic graph");
            else
                return list;
        }

        private int Khans_GetInverseDegree(List<EDGE>[] Edges, int i)
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

        private int Khans_CountEdges(List<EDGE>[] Edges)
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
