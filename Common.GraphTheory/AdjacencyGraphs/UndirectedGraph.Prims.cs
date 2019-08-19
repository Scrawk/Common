using System;
using System.Collections.Generic;

using Common.Collections.Queues;

namespace Common.GraphTheory.AdjacencyGraphs
{
    public partial class UndirectedGraph<VERTEX, EDGE> : AdjacencyGraph<VERTEX, EDGE>
        where EDGE : class, IGraphEdge, new()
        where VERTEX : class, IGraphVertex, new()
    {
        public GraphTree PrimsMinimumSpanningTree(int root)
        {
            TagVertices(NOT_VISITED_TAG);
            int count = VertexCount;

            Vertices[root].Tag = IS_VISITED_TAG;

            var tree = new GraphTree(root, count);
            var queue = new BinaryHeap<IGraphEdge>();

            if (Edges[root] != null)
            {
                foreach (var edge in Edges[root])
                    queue.Add(edge);
            }

            while (queue.Count != 0)
            {
                var edge = queue.RemoveFirst();

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

            tree.CreateChildren();

            return tree;
        }
    }
}
