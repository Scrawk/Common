using System;
using System.Collections.Generic;
using System.Text;

namespace Common.GraphTheory.AdjacencyGraphs
{

    public abstract partial class AdjacencyGraph
    {
        /// <summary>
        /// Return a list of vertex indices ordered breadth first.
        /// </summary>
        /// <param name="root">The vertex index to start at.</param>
        /// <returns>The vertices in breadth first order.</returns>
        public List<int> BreadthFirstOrder(int root)
        {
            TagVertices(NOT_VISITED_TAG);
            int count = VertexCount;

            var queue = new Queue<int>(count);
            queue.Enqueue(root);

            Vertices[root].Tag = IS_VISITED_TAG;

            var ordering = new List<int>(count);

            while (queue.Count != 0)
            {
                int u = queue.Dequeue();
                ordering.Add(u);

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

        /// <summary>
        /// Return a tree of vertex indices ordered breadth first.
        /// </summary>
        /// <param name="root">The vertex index to start at.</param>
        /// <returns>The vertices in breadth first order.</returns>
        public GraphTree BreadthFirstTree(int root)
        {
            TagVertices(NOT_VISITED_TAG);
            int count = VertexCount;

            var tree = new GraphTree(this, root, count);
            var queue = new Queue<int>(count);
            queue.Enqueue(root);

            Vertices[root].Tag = IS_VISITED_TAG;

            while (queue.Count != 0)
            {
                int u = queue.Dequeue();

                var edges = Edges[u];
                if (edges == null) continue;

                for (int i = 0; i < edges.Count; i++)
                {
                    int to = edges[i].To;

                    if (Vertices[to].Tag == IS_VISITED_TAG) continue;

                    queue.Enqueue(to);
                    Vertices[to].Tag = IS_VISITED_TAG;
                    tree.SetParent(to, u);
                }
            }

            return tree;
        }

    }
}
