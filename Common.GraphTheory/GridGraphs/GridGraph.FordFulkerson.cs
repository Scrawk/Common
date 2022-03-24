using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Core.Directions;

using Common.GraphTheory.AdjacencyGraphs;

namespace Common.GraphTheory.GridGraphs
{
    /// <summary>
    /// https://tutorialspoint.dev/data-structure/graph-data-structure/ford-fulkerson-algorithm-for-maximum-flow-problem
    /// </summary>
    public partial class GridGraph
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public int MaxFlow(Point2i source, Point2i target)
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
            var search = new GridSearch(Width, Height);
            return MaxFlow(rGraph, search, source, target);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public int FordFulkersonMaxFlow(GridSearch search, Point2i source, Point2i target)
        {
            var rGraph = Copy();
            return MaxFlow(rGraph, search, source, target);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rGraph"></param>
        /// <param name="search"></param>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private int MaxFlow(GridGraph rGraph, GridSearch search, Point2i source, Point2i target)
        {

            int max_flow = 0; // There is no flow initially 

            // Augment the flow while tere is path from source 
            // to sink 
            while (bfs(rGraph, search, source, target))
            {
                // Find minimum residual capacity of the edhes 
                // along the path filled by BFS. Or we can say 
                // find the maximum flow through the path found. 
                int path_flow = int.MaxValue;
                for (Point2i v = target; v != source; v = search.GetParent(v))
                {
                    var u = search.GetParent(v);
                    path_flow = Math.Min(path_flow, (int)rGraph.GetWeight(u, v));
                }

                // update residual capacities of the edges and 
                // reverse edges along the path 
                for (Point2i v = target; v != source; v = search.GetParent(v))
                {
                    Point2i u = search.GetParent(v);

                    if (!rGraph.HasDirectedEdge(v, u))
                        rGraph.AddDirectedEdge(v, u);

                    float flowUV = rGraph.GetWeight(u, v);
                    float flowVU = rGraph.GetWeight(v, u);

                    flowUV -= path_flow;
                    flowVU += path_flow;

                    rGraph.SetWeight(u, v, flowUV);
                    rGraph.SetWeight(v, u, flowVU);
                }

                // Add path flow to overall flow 
                max_flow += path_flow;
            }

            // Return the overall flow 
            return max_flow;
        }

        public List<GridEdge> FordFulkersonMinCut(GridSearch search, Point2i source, Point2i target)
        {

            // Create a residual graph and fill the residual graph with 
            // given capacities in the original graph as residual capacities 
            // in residual graph 
            var rGraph = Copy();
            MaxFlow(rGraph, search, source, target);

            // Flow is maximum now, find vertices reachable from s 
            search.Clear();
            dfs(rGraph, search, source);

            //rGraph.Print();
            //search.Print();

            var cut = new List<GridEdge>();

            // Print all edges that are from a reachable vertex to 
            // non-reachable vertex in the original graph 

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (x == y) continue;

                    var u = new Point2i(x, y);

                    for (int i = 0; i < 8; i++)
                    {
                        int xi = x + D8.OFFSETS[i, 0];
                        int yi = y + D8.OFFSETS[i, 1];

                        if (xi < 0 || xi > Width - 1) continue;
                        if (yi < 0 || yi > Height - 1) continue;

                        var v = new Point2i(xi, yi);

                        if (search.GetIsVisited(u) && !search.GetIsVisited(v))
                        {
                            var edge = GetEdge(u, v);
                            if (edge == null || edge.Weight <= 0) continue;

                            cut.Add(edge);
                        }

                    }

                }
            }

            return cut;
            
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public List<GraphEdge> FordFulkersonMinCut2(Point2i source, Point2i target)
        {
            var search = new GridSearch(Width, Height);
            return FordFulkersonMinCut2(search, source, target);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public List<GraphEdge> FordFulkersonMinCut2(GridSearch search, Point2i source, Point2i target)
        {

            // Create a residual graph and fill the residual graph with 
            // given capacities in the original graph as residual capacities 
            // in residual graph 
            var rGraph = Copy();
            MaxFlow(rGraph, search, source, target);

            var graph = ToDirectedGraph();
            var rgraph = rGraph.ToDirectedGraph();

            bool[] visited = new bool[graph.VertexCount];
            int s = source.x + source.y * Width;
            DirectedGraph.dfs(rgraph, s, visited);

            var cut = new List<GraphEdge>();

            // Print all edges that are from a reachable vertex to 
            // non-reachable vertex in the original graph 
            for (int i = 0; i < graph.VertexCount; i++)
            {
                for (int j = 0; j < graph.VertexCount; j++)
                {
                    if (i == j) continue;

                    if (visited[i] && !visited[j])
                    {
                        var e = graph.GetEdge(i, j);
                        if (e == null || e.Weight <= 0) continue;

                        cut.Add(e);
                    }
                }
            }

            return cut;

        }

        private static void dfs(GridGraph graph, GridSearch search, Point2i u)
        {
            search.SetIsVisited(u, true);

            for (int x = 0; x < graph.Width; x++)
            {
                for (int y = 0; y < graph.Height; y++)
                {
                    var v = new Point2i(x, y);  

                    float flow = graph.GetWeight(u, v);

                    if (flow > 0 && !search.GetIsVisited(v))
                    {
                        dfs(graph, search, v);
                    }
                        
                }
            }

        }

        private static bool bfs(GridGraph rgraph, GridSearch search, Point2i source, Point2i target)
        {
            search.Clear();

            // Create a queue, enqueue source vertex and mark 
            // source vertex as visited 
            var queue = new List<Point2i>();
            queue.Add(source);

            // Standard BFS Loop 
            while (queue.Count != 0)
            {
                var u = queue[0];
                queue.RemoveAt(0);

                for (int x = 0; x < rgraph.Width; x++)
                {
                    for (int y = 0; y < rgraph.Height; y++)
                    {
                        var v = new Point2i(x, y);

                        var flow = rgraph.GetWeight(u, v);

                        if (!search.GetIsVisited(v) && flow > 0)
                        {
                            queue.Add(v);
                            search.SetParent(v, u);
                            search.SetIsVisited(v, true);
                        }
                    }
                }
            }

            // If we reached sink in BFS  
            // starting from source, then 
            // return true, else false 
            return search.GetIsVisited(target);
        }
        
    }
        
}
