using System;
using System.Collections.Generic;
using System.Text;

namespace Common.GraphTheory.AdjacencyGraphs
{

    public class MaxFlow
    {

        /* Returns true if there is a path 
        from source 's' to sink 't' in residual 
        graph. Also fills parent[] to store the 
        path */
        bool bfs(int[,] rGraph, int s, int t, int[] parent)
        {
            int V = parent.Length;

            // Create a visited array and mark  
            // all vertices as not visited 
            bool[] visited = new bool[V];
            for (int i = 0; i < V; ++i)
                visited[i] = false;

            // Create a queue, enqueue source vertex and mark 
            // source vertex as visited 
            List<int> queue = new List<int>();
            queue.Add(s);
            visited[s] = true;
            parent[s] = -1;

            // Standard BFS Loop 
            while (queue.Count != 0)
            {
                int u = queue[0];
                queue.RemoveAt(0);

                for (int v = 0; v < V; v++)
                {
                    if (visited[v] == false && rGraph[u, v] > 0)
                    {
                        queue.Add(v);
                        parent[v] = u;
                        visited[v] = true;
                    }
                }
            }

            // If we reached sink in BFS  
            // starting from source, then 
            // return true, else false 
            return (visited[t] == true);
        }

        // Returns tne maximum flow 
        // from s to t in the given graph 
        public int fordFulkerson(int[,] graph, int s, int t)
        {
            int V = graph.GetLength(0);
            int u, v;

            // Create a residual graph and fill  
            // the residual graph with given  
            // capacities in the original graph as 
            // residual capacities in residual graph 

            // Residual graph where rGraph[i,j]  
            // indicates residual capacity of  
            // edge from i to j (if there is an  
            // edge. If rGraph[i,j] is 0, then  
            // there is not) 
            int[,] rGraph = new int[V, V];

            for (u = 0; u < V; u++)
                for (v = 0; v < V; v++)
                    rGraph[u, v] = graph[u, v];

            // This array is filled by BFS and to store path 
            int[] parent = new int[V];

            int max_flow = 0; // There is no flow initially 

            // Augment the flow while tere is path from source 
            // to sink 
            while (bfs(rGraph, s, t, parent))
            {
                // Find minimum residual capacity of the edhes 
                // along the path filled by BFS. Or we can say 
                // find the maximum flow through the path found. 
                int path_flow = int.MaxValue;
                for (v = t; v != s; v = parent[v])
                {
                    u = parent[v];
                    path_flow = Math.Min(path_flow, rGraph[u, v]);
                }

                // update residual capacities of the edges and 
                // reverse edges along the path 
                for (v = t; v != s; v = parent[v])
                {
                    u = parent[v];
                    rGraph[u, v] -= path_flow;
                    rGraph[v, u] += path_flow;
                }

                // Add path flow to overall flow 
                max_flow += path_flow;
            }

            // Return the overall flow 
            return max_flow;
        }

    }

    public class MinCut
    {

        /* Returns true if there is a path from source 's' to sink 't' in 
          residual graph. Also fills parent[] to store the path */
        bool bfs(int[,] rGraph, int s, int t, int[] parent)
        {
            int V = parent.Length;
            // Create a visited array and mark all vertices as not visited 
            bool[] visited = new bool[V];

            // Create a queue, enqueue source vertex and mark source vertex 
            // as visited 
            Queue<int> q = new Queue<int>();
            q.Enqueue(s);
            visited[s] = true;
            parent[s] = -1;

            // Standard BFS Loop 
            while (q.Count > 0)
            {
                int u = q.Dequeue();

                for (int v = 0; v < V; v++)
                {
                    if (visited[v] == false && rGraph[u, v] > 0)
                    {
                        q.Enqueue(v);
                        parent[v] = u;
                        visited[v] = true;
                    }
                }
            }

            // If we reached sink in BFS starting from source, then return 
            // true, else false 
            return (visited[t] == true);
        }

        // A DFS based function to find all reachable vertices from s.  The function 
        // marks visited[i] as true if i is reachable from s.  The initial values in 
        // visited[] must be false. We can also use BFS to find reachable vertices 
        void dfs(int[,] rGraph, int s, bool[] visited)
        {
            visited[s] = true;
            for (int i = 0; i < visited.Length; i++)
                if (rGraph[s, i] > 0 && !visited[i])
                    dfs(rGraph, i, visited);
        }

        // Prints the minimum s-t cut 
        public void minCut(int[,] graph, int s, int t)
        {
            int V = graph.GetLength(0);
            int u, v;

            // Create a residual graph and fill the residual graph with 
            // given capacities in the original graph as residual capacities 
            // in residual graph 
            int[,] rGraph = new int[V, V];

            for (u = 0; u < V; u++)
                for (v = 0; v < V; v++)
                    rGraph[u, v] = graph[u, v];

            int[] parent = new int[V];  // This array is filled by BFS and to store path 

            // Augment the flow while there is a path from source to sink 
            while (bfs(rGraph, s, t, parent))
            {
                // Find minimum residual capacity of the edhes along the 
                // path filled by BFS. Or we can say find the maximum flow 
                // through the path found. 
                int path_flow = int.MaxValue;
                for (v = t; v != s; v = parent[v])
                {
                    u = parent[v];
                    path_flow = Math.Min(path_flow, rGraph[u, v]);
                }

                // update residual capacities of the edges and reverse edges 
                // along the path 
                for (v = t; v != s; v = parent[v])
                {
                    u = parent[v];
                    rGraph[u, v] -= path_flow;
                    rGraph[v, u] += path_flow;
                }
            }

            // Flow is maximum now, find vertices reachable from s 
            bool[] visited = new bool[V];

            dfs(rGraph, s, visited);

            // Print all edges that are from a reachable vertex to 
            // non-reachable vertex in the original graph 
            for (int i = 0; i < V; i++)
                for (int j = 0; j < V; j++)
                {
                    if (visited[i] && !visited[j] && graph[i, j] > 0)
                        Console.WriteLine(i + " " + j);
                }


            return;
        }
    }
}
