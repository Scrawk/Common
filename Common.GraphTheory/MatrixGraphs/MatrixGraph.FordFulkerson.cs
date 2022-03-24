using System;
using System.Collections.Generic;

namespace Common.GraphTheory.MatrixGraphs
{
    public partial class MatrixGraph
    {
        public float FordFulkersonMaxFlow(int source, int target)
        {
            var rGraph = Copy();
            return MaxFlow(rGraph, source, target);
        }

        private float MaxFlow(MatrixGraph rGraph, int source, int target)
        {
            int[] parent = new int[Size];  

            float max_flow = 0;  

            while (bfs(rGraph, source, target, parent))
            {
                float path_flow = int.MaxValue;

                for (int v = target; v != source; v = parent[v])
                {
                    int u = parent[v];
                    path_flow = Math.Min(path_flow, rGraph[u,v]);
                }

                for (int v = target; v != source; v = parent[v])
                {
                    int u = parent[v];
                    rGraph[u,v] -= path_flow;
                    rGraph[v,u] += path_flow;
                }

                max_flow += path_flow;
            }

            return max_flow;
        }

        public List<MatrixEdge> FordFulkersonMinCut(int source, int target)
        {
            var rGraph = Copy();
            MaxFlow(rGraph, source, target);

            int size = rGraph.Size;

            bool[] visited = new bool[size];

            dfs(rGraph, source, visited);

            var cut = new List<MatrixEdge>();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (visited[i] && !visited[j] && this[i, j] != 0)
                        cut.Add(new MatrixEdge(i, j, this[i, j]));
                }
            }
   
            return cut;
        }

        private static void dfs(MatrixGraph rGraph, int s, bool[] visited)
        {
            visited[s] = true;
            for (int i = 0; i < rGraph.Size; i++)
                if (rGraph[s,i]!= 0 && !visited[i])
                    dfs(rGraph, i, visited);
        }

        private static bool bfs(MatrixGraph rGraph, int s, int t, int[] parent)
        {
            int size = rGraph.Size;
 
            bool[] visited = new bool[size];

            Queue<int> q = new Queue<int>();
            q.Enqueue(s);
            visited[s] = true;
            parent[s] = -1;

            while (q.Count > 0)
            {
                int u = q.Dequeue();
      
                for (int v = 0; v < size; v++)
                {
                    if (visited[v] == false && rGraph[u,v] > 0)
                    {
                        q.Enqueue(v);
                        parent[v] = u;
                        visited[v] = true;
                    }
                }
            }

            return visited[t];
        }
    }
}
