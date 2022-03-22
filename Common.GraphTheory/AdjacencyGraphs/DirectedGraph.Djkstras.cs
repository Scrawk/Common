using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.GraphTheory.AdjacencyGraphs
{
    public partial class DirectedGraph : AdjacencyGraph
    {
        /// <summary>
        /// Create a graph tree representing the shortest paths
        /// between the vertices in the graph.
        /// </summary>
        /// <param name="root">The vertex index to start at.</param>
        /// <returns>The shortest path tree.</returns>
        public GraphTree DijkstrasShortestPathTree(int root)
        {
            TagVertices(NOT_VISITED_TAG);
            int count = VertexCount;

            for (int i = 0; i < count; i++)
                base.Vertices[i].Cost = float.PositiveInfinity;

            base.Vertices[root].Cost = 0;
            base.Vertices[root].Tag = IS_VISITED_TAG;

            var tree = new GraphTree(this, root);
            var queue = new List<GraphVertex>(base.Vertices);

            while (queue.Count != 0)
            {
                queue.Sort();

                var vertex = queue[0];
                queue.RemoveAt(0);
                int u = vertex.Index;

                base.Vertices[u].Tag = IS_VISITED_TAG;

                if (base.Edges[u] != null)
                {
                    foreach (var e in base.Edges[u])
                    {
                        int v = e.To;
                        if (base.Vertices[v].Tag == IS_VISITED_TAG) continue;

                        float alt = base.Vertices[u].Cost + e.Weight;

                        if (alt < base.Vertices[v].Cost)
                        {
                            if (!MathUtil.IsFinite(alt))
                                throw new ArithmeticException("Cost is not finite.");

                            base.Vertices[v].Cost = alt;
                            tree.Parent[v] = u;
                        }
                    }
                }
            }

            return tree;
        }
    }
}
