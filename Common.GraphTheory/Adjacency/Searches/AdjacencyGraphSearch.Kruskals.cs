using System;
using System.Collections.Generic;


using Common.Collections.Sets;

namespace Common.GraphTheory.Adjacency
{
    public static partial class AdjacencyGraphSearch
    {
        public static Dictionary<int, List<EDGE>> KruskalsMinimumSpanningForest<VERTEX, EDGE>(AdjacencyGraph<VERTEX, EDGE> graph)
            where EDGE : class, IAdjacencyEdge, new()
            where VERTEX : class, IAdjacencyVertex, new()
        {

            DisjointSet set = new DisjointSet(graph.VertexCount);

            for (int i = 0; i < graph.VertexCount; i++)
                set.Add(i, i);

            var sorted = new List<EDGE>();
            graph.GetAllEdges(sorted);
            sorted.Sort();

            int edgeCount = sorted.Count;
            List<EDGE> edges = new List<EDGE>(edgeCount);

            for (int i = 0; i < edgeCount; i++)
            {
                int u = sorted[i].From;
                int v = sorted[i].To;

                if (set.Union(v, u))
                    edges.Add(sorted[i]);
            }

            Dictionary<int, List<EDGE>> forest = new Dictionary<int, List<EDGE>>();

            edgeCount = edges.Count;
            for (int i = 0; i < edgeCount; i++)
            {
                int root = set.FindParent(edges[i].From);

                if (!forest.ContainsKey(root))
                    forest.Add(root, new List<EDGE>());

                forest[root].Add(edges[i]);
            }

            return forest;
        }
    }
}
