using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Collections.Sets;
using Common.GraphTheory.Adjacency;
using Common.GraphTheory.Grids;

namespace Common.GraphTheory.Searches
{
    internal static class KruskalsMinimumSpanningForest
    {
        internal static Dictionary<int, List<EDGE>> Search<VERTEX, EDGE>(AdjacencyGraph<VERTEX, EDGE> graph)
            where EDGE : class, IAdjacencyEdge, new()
            where VERTEX : class, IAdjacencyVertex, new()
        {

            DisjointSet set = new DisjointSet(graph.VertexCount);

            for(int i = 0; i < graph.VertexCount; i++)
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

        internal static Dictionary<Vector2i, List<GridEdge>> Search(GridGraph graph, float[,] weights)
        {
            int width = graph.Width;
            int height = graph.Height;

            DisjointGridSet2 set = new DisjointGridSet2(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    set.Add(x, y, x, y);
                }
            }

            var sorted = new List<GridEdge>(width * height);
            graph.GetAllEdges(sorted, weights);
            sorted.Sort();

            int edgeCount = sorted.Count;
            List<GridEdge> edges = new List<GridEdge>();

            for (int i = 0; i < edgeCount; i++)
            {
                Vector2i from = sorted[i].From;
                Vector2i to = sorted[i].To;

                if (set.Union(to.x, to.y, from.x, from.y))
                    edges.Add(sorted[i]);
            }

            Dictionary<Vector2i, List<GridEdge>> forest = new Dictionary<Vector2i, List<GridEdge>>();

            edgeCount = edges.Count;
            for (int i = 0; i < edgeCount; i++)
            {
                Vector2i root = set.FindParent(edges[i].From.x, edges[i].From.y);

                if (!forest.ContainsKey(root))
                    forest.Add(root, new List<GridEdge>());

                forest[root].Add(edges[i]);
            }

            return forest;
        }
    }
}
