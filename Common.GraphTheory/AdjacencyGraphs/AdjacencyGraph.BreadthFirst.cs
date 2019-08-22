using System;
using System.Collections.Generic;
using System.Text;

namespace Common.GraphTheory.AdjacencyGraphs
{

    public abstract partial class AdjacencyGraph
    {

        public GraphOrdering BreadthFirstOrder(int root)
        {
            TagVertices(NOT_VISITED_TAG);
            int count = VertexCount;

            var queue = new Queue<int>(count);
            queue.Enqueue(root);

            Vertices[root].Tag = IS_VISITED_TAG;

            var ordering = new GraphOrdering(count);

            while (queue.Count != 0)
            {
                int u = queue.Dequeue();
                ordering.Vertices.Add(u);

                var edges = Edges[u];
                if (edges == null) continue;

                for (int i = 0; i < edges.Count; i++)
                {
                    int to = edges[i].To;

                    if (Vertices[to].Tag == IS_VISITED_TAG) continue;

                    queue.Enqueue(to);
                    Vertices[to].Tag = IS_VISITED_TAG;
                }
            }

            return ordering;
        }

    }
}
