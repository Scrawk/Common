using System;
using System.Collections.Generic;
using System.Text;

namespace Common.GraphTheory.AdjacencyGraphs
{

    public abstract partial class AdjacencyGraph<VERTEX, EDGE>
        where EDGE : class, IGraphEdge, new()
        where VERTEX : class, IGraphVertex, new()
    {

        public GraphOrdering BreadthFirstOrder(int root)
        {
            int count = VertexCount;

            var queue = new Queue<int>(count);
            queue.Enqueue(root);

            var isVisited = new bool[count];
            isVisited[root] = true;

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

                    if (isVisited[to]) continue;

                    queue.Enqueue(to);
                    isVisited[to] = true;
                }
            }

            return ordering;
        }

    }
}
