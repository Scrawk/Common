using System;
using System.Collections.Generic;
using System.Text;

namespace Common.GraphTheory.AdjacencyGraphs
{
    public partial class DirectedGraph<VERTEX, EDGE> : AdjacencyGraph<VERTEX, EDGE>
        where EDGE : class, IGraphEdge, new()
        where VERTEX : class, IGraphVertex, new()
    {
        public GraphTree DijkstrasShortestPathTree(int root)
        {
            int count = VertexCount;

            for (int i = 0; i < count; i++)
                Vertices[i].Cost = float.PositiveInfinity;

            Vertices[root].Cost = 0;

            var isVisited = new bool[count];
            isVisited[root] = true;

            var tree = new GraphTree(root, count);
            var queue = new List<VERTEX>(Vertices);

            while (queue.Count != 0)
            {
                queue.Sort();

                var vertex = queue[0];
                queue.RemoveAt(0);
                int u = vertex.Index;

                isVisited[u] = true;

                if (Edges[u] != null)
                {
                    foreach (var e in Edges[u])
                    {
                        int v = e.To;
                        if (isVisited[v]) continue;

                        float alt = Vertices[u].Cost + e.Weight;

                        if (alt < Vertices[v].Cost)
                        {
                            Vertices[v].Cost = alt;
                            tree.Parent[v] = u;
                        }
                    }
                }
            }

            return tree;
        }
    }
}
