using System;
using System.Collections.Generic;
using System.Text;

namespace Common.GraphTheory.AdjacencyGraphs
{

    public partial class DirectedGraph : AdjacencyGraph
    {
        /// <summary>
        /// Presuming the edges represent the order the 
        /// vertices must be iterated return a list of
        /// vertex indices that conform to this order.
        /// </summary>
        /// <returns></returns>
        public List<GraphVertex> KhansTopologicalSort()
        {
            var list = new List<GraphVertex>();
            var vertices = new LinkedList<GraphVertex>();

            int edgeCount = base.Edges.Count;
            var edges = new List<GraphEdge>[edgeCount];

            for (int i = 0; i < edgeCount; i++)
            {
                if (base.Edges[i] == null) continue;
                edges[i] = new List<GraphEdge>(base.Edges[i]);
            }

            for (int i = 0; i < base.Vertices.Count; i++)
            {
                int idegree = Khans_GetInverseDegree(edges, i);

                if (idegree == 0)
                    vertices.AddLast(base.Vertices[i]);
            }

            while (vertices.Count > 0)
            {
                GraphVertex v = vertices.Last.Value;
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
                        vertices.AddLast(base.Vertices[to]);
                    }
                }

                edges[i].Clear();
            }

            if (Khans_CountEdges(edges) > 0)
                throw new InvalidOperationException("Can not find a topological sort on a cyclic graph");
            else
                return list;
        }

        /// <summary>
        /// Find the number of vertices that go to this vertex.
        /// </summary>
        /// <param name="Edges">A list of the edges for each vertex.</param>
        /// <param name="i">The vertex index.</param>
        /// <returns></returns>
        private int Khans_GetInverseDegree(List<GraphEdge>[] Edges, int i)
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

        /// <summary>
        /// Count the number of edges in the list of lists.
        /// </summary>
        /// <param name="Edges">A list of the edges for each vertex.</param>
        /// <returns></returns>
        private int Khans_CountEdges(List<GraphEdge>[] Edges)
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
