using System;
using System.Collections.Generic;

using Common.Collections.Queues;

namespace Common.GraphTheory.AdjacencyGraphs
{
    public partial class UndirectedGraph : AdjacencyGraph
    {
        /// <summary>
        /// Create a graph tree representing the minimum 
        /// spanning tree between the vertices in the graph.
        /// </summary>
        /// <param name="root">The vertex index to start at.</param>
        /// <returns>The minimum  spanning tree.</returns>
        public GraphTree PrimsMinimumSpanningTree(int root)
        {
            TagVertices(NOT_VISITED_TAG);
            int count = VertexCount;

            Vertices[root].Tag = IS_VISITED_TAG;

            var tree = new GraphTree(this, root, count);
            var queue = new BinaryHeap<GraphEdge>();

            if (Edges[root] != null)
            {
                foreach (var edge in Edges[root])
                    queue.Add(edge);
            }

            while (queue.Count != 0)
            {
                var edge = queue.Pop();

                int v = edge.To;
                if (Vertices[v].Tag == IS_VISITED_TAG) continue;

                Vertices[v].Tag = IS_VISITED_TAG;
                tree.Parent[v] = edge.From;

                if (Edges[v] != null)
                {
                    foreach (var e in Edges[v])
                    {
                        if (Vertices[e.To].Tag == IS_VISITED_TAG) continue;
                        queue.Add(e);
                    }
                }
            }

            return tree;
        }
    }
}
