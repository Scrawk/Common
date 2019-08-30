using System;
using System.Collections.Generic;
using System.Text;

namespace Common.GraphTheory.AdjacencyGraphs
{

    public abstract partial class AdjacencyGraph
    {

        public GraphOrder DepthFirstOrder(int root)
        {
            TagVertices(NOT_VISITED_TAG);
            int count = VertexCount;

            var queue = new Stack<int>(count);
            queue.Push(root);

            Vertices[root].Tag = IS_VISITED_TAG;

            var ordering = new GraphOrder(count);

            while (queue.Count != 0)
            {
                int u = queue.Pop();
                ordering.Vertices.Add(u);

                var edges = Edges[u];
                if (edges == null) continue;

                for (int i = 0; i < edges.Count; i++)
                {
                    int to = edges[i].To;

                    if (Vertices[to].Tag == IS_VISITED_TAG) continue;

                    queue.Push(to);
                    Vertices[to].Tag = IS_VISITED_TAG;
                }
            }

            return ordering;
        }
    }

}
