using System;
using System.Collections.Generic;

using Common.Collections.Sets;

namespace Common.GraphTheory.AdjacencyGraphs
{

    public partial class UndirectedGraph : AdjacencyGraph
    {

        public GraphForest KruskalsMinimumSpanningForest()
        {
            var set = new DisjointSet(VertexCount);

            for (int i = 0; i < VertexCount; i++)
                set.Add(i, i);

            var sorted = new List<GraphEdge>();
            GetAllEdges(sorted);
            sorted.Sort();

            int edgeCount = sorted.Count;
            var edges = new List<GraphEdge>(edgeCount);

            for (int i = 0; i < edgeCount; i++)
            {
                int u = sorted[i].From;
                int v = sorted[i].To;

                if (set.Union(v, u))
                    edges.Add(sorted[i]);
            }

            var table = new Dictionary<int, List<GraphEdge>>();

            edgeCount = edges.Count;
            for (int i = 0; i < edgeCount; i++)
            {
                int root = set.FindParent(edges[i].From);

                if (!table.ContainsKey(root))
                    table.Add(root, new List<GraphEdge>());

                table[root].Add(edges[i]);
            }

            var forest = new GraphForest();
            var graph = new UndirectedGraph(VertexCount);

            foreach (var kvp in table)
            {
                int root = kvp.Key;
                var list = kvp.Value;

                graph.ClearEdges();
                foreach(var edge in list)
                {
                    int from = edge.From;
                    int to = edge.To;
                    graph.AddEdge(from, to);
                }

                var tree = Kruskals_BuildTree(root, graph);
                forest.AddTree(tree);
            }

            return forest;
        }

        private GraphTree Kruskals_BuildTree(int root, UndirectedGraph graph)
        {
            int count = graph.VertexCount;

            var queue = new Stack<int>(count);
            queue.Push(root);

            var isVisited = new bool[count];
            isVisited[root] = true;

            var tree = new GraphTree(root, count);
            tree.Parent[root] = root;

            while (queue.Count != 0)
            {
                int u = queue.Pop();

                var edges = graph.Edges[u];
                if (edges == null) continue;

                for (int i = 0; i < edges.Count; i++)
                {
                    int to = edges[i].To;
                    if (isVisited[to]) continue;

                    tree.Parent[to] = u;
                    queue.Push(to);
                    isVisited[to] = true;
                }
            }

            tree.CreateChildren();
          
            return tree;
        }
    }
}
