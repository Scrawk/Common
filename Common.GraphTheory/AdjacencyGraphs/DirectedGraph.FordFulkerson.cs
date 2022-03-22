using System;
using System.Collections.Generic;
using System.Text;

namespace Common.GraphTheory.AdjacencyGraphs
{

    public partial class DirectedGraph : AdjacencyGraph
    {

        private static bool[] visited;

        private static Queue<int> queue;

        private static int[] parent;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public int MaxFlow(int source, int target)
        {
            // Create a residual graph and fill  
            // the residual graph with given  
            // capacities in the original graph as 
            // residual capacities in residual graph 

            // Residual graph where rGraph[i,j]  
            // indicates residual capacity of  
            // edge from i to j (if there is an  
            // edge. If rGraph[i,j] is 0, then  
            // there is not) 
            var rGraph = Copy();

            return MaxFlow(rGraph, source, target);
        }

        /// <summary>
        /// https://tutorialspoint.dev/data-structure/graph-data-structure/ford-fulkerson-algorithm-for-maximum-flow-problem
        /// </summary>
        /// <param name="rGraph"></param>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private int MaxFlow(DirectedGraph rGraph, int source, int target)
        {

            parent = new int[VertexCount];
            queue = new Queue<int>();
            visited = new bool[VertexCount];

            int max_flow = 0; // There is no flow initially 

            // Augment the flow while tere is path from source to sink 

            while (bfs(rGraph, source, target, parent))
            {

                // Find minimum residual capacity of the edhes 
                // along the path filled by BFS. Or we can say 
                // find the maximum flow through the path found. 
                int path_flow = int.MaxValue;
                for (int v = target; v != source; v = parent[v])
                {
                    int u = parent[v];
                    path_flow = Math.Min(path_flow, (int)rGraph.GetEdgeWeight(u,v));
                }

                // update residual capacities of the edges and 
                // reverse edges along the path 
                for (int v = target; v != source; v = parent[v])
                {
                    int u = parent[v];

                    var edgeUV = rGraph.GetEdgeOrCreateEdge(u, v);
                    var edgeVU = rGraph.GetEdgeOrCreateEdge(v, u);

                    edgeUV.Weight -= path_flow;
                    edgeVU.Weight += path_flow;
                }

                // Add path flow to overall flow 
                max_flow += path_flow;
            }

            // Return the overall flow 
            return max_flow;
        }

        public List<GraphEdge> MinCut(int source, int target)
        {
            var rGraph = Copy();
            MaxFlow(rGraph, source, target);

            // Flow is maximum now, find vertices reachable from s 
            visited = new bool[VertexCount];
            dfs(rGraph, source, visited);

            var cut = new List<GraphEdge>();

            // Print all edges that are from a reachable vertex to 
            // non-reachable vertex in the original graph 
            for (int i = 0; i < VertexCount; i++)
            {
                for (int j = 0; j < VertexCount; j++)
                {
                    if (i == j) continue;

                    if (visited[i] && !visited[j])
                    {
                        var e = GetEdge(i, j);
                        if (e == null || e.Weight <= 0) continue;

                        cut.Add(e);
                    }
                }
            }

            return cut;
        }

        public static void dfs(DirectedGraph graph, int s, bool[] visited)
        {
            visited[s] = true;
            for (int i = 0; i < graph.VertexCount; i++)
            {
                float flow = graph.GetEdgeWeight(s, i);

                if (flow > 0 && !visited[i])
                    dfs(graph, i, visited);
            }
          
        }

        private static bool bfs(DirectedGraph graph, int s, int t, int[] parent)
        {
            int V = parent.Length;

            // Create a visited array and mark  
            // all vertices as not visited 
            for (int i = 0; i < V; ++i)
                visited[i] = false;

            // Create a queue, enqueue source vertex and mark 
            // source vertex as visited 
            queue.Clear();
            queue.Enqueue(s);
            visited[s] = true;
            parent[s] = -1;

            // Standard BFS Loop 
            while (queue.Count != 0)
            {
                int u = queue.Dequeue();
 
                for (int v = 0; v < V; v++)
                {
                    var flow = graph.GetEdgeWeight(u, v);

                    if (visited[v] == false && flow > 0)
                    {
                        queue.Enqueue(v);
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

   
    }
}
