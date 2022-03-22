using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.GraphTheory.AdjacencyGraphs
{

    public class EdgeFlowData
    { 
        public int flow, capacity;
 
        public EdgeFlowData(int capacity)
        {
            this.flow = 0;
            this.capacity = capacity;
        }

        public EdgeFlowData(int flow, int capacity)
        {
            this.flow = flow;
            this.capacity = capacity;
        }
    }

    public class VertexFlowData
    {
        public int h, e_flow;

        public VertexFlowData(int h, int e_flow)
        {
            this.h = h;
            this.e_flow = e_flow;
        }
    }

    public partial class DirectedGraph : AdjacencyGraph
    {
   
        public int PushRelabelMaxFlow(int source, int target)
        {
            preflow(source);

            // loop untill none of the Vertex is in overflow 
            while (overFlowVertex() != -1)
            {
                int u = overFlowVertex();

                if (!push(u))
                    relabel(u);
            }

            // ver.back() returns last Vertex, whose 
            // e_flow will be final maximum flow 
            var vdata = Vertices.Last().Data as VertexFlowData;

            return vdata.e_flow;
        }

        void updateReverseEdgeFlow(int i, int j, int flow)
        {
            var edges = Edges[i];
            if (edges == null) return;

            int u = edges[j].From; 
            int v = edges[j].To;

            for (int x = 0; x < Edges.Count; x++)
            {
                edges = Edges[x];
                if (edges == null) return;

                for (int y = 0; y < edges.Count; y++)
                {
                    var edge = edges[y];

                    if (edge.To == v && edge.From == u)
                    {
                        var edata = edge.Data as EdgeFlowData;
                        edata.flow -= flow;
                        return;
                    }
                }
            }

            // adding reverse Edge in residual graph 
            var e = new GraphEdge(u, v, 0, new EdgeFlowData(0, flow));
            AddEdge(e);
        }

        // To push flow from overflowing vertex u 
        bool push(int u)
        {
            // Traverse through all edges to find an adjacent (of u) 
            // to which flow can be pushed 
            for (int i = 0; i < Edges.Count; i++)
            {
                var edges = Edges[i];
                if (edges == null) continue;

                for (int j = 0; j < edges.Count; j++)
                {
                    var edge = edges[j];
                    int from = edge.From;
                    int to = edge.To;

                    // Checks u of current edge is same as given 
                    // overflowing vertex 
                    if (from == u)
                    {
                        var edata = edge.Data as EdgeFlowData;

                        // if flow is equal to capacity then no push 
                        // is possible 
                        if (edata.flow == edata.capacity)
                            continue;

                        var from_vdata = Vertices[from].Data as VertexFlowData;
                        var to_vdata = Vertices[to].Data as VertexFlowData;

                        // Push is only possible if height of adjacent 
                        // is smaller than height of overflowing vertex 
                        if (from_vdata.h > to_vdata.h)
                        {
                            // Flow to be pushed is equal to minimum of 
                            // remaining flow on edge and excess flow. 
                            int flow = Math.Min(edata.capacity - edata.flow, from_vdata.e_flow);

                            // Reduce excess flow for overflowing vertex 
                            from_vdata.e_flow -= flow;
                            // Increase excess flow for adjacent 
                            to_vdata.e_flow += flow;

                            // Add residual flow (With capacity 0 and negative 
                            // flow) 
                            edata.flow += flow;

                            updateReverseEdgeFlow(i, j, flow);

                            return true;
                        }
                    }
                }
            }
            return false;
        }

        // main function for printing maximum flow of graph 
        int getMaxFlow(int s, int t)
        {
            preflow(s);

            // loop untill none of the Vertex is in overflow 
            while (overFlowVertex() != -1)
            {
                int u = overFlowVertex();
                if (!push(u))
                    relabel(u);
            }

            // ver.back() returns last Vertex, whose 
            // e_flow will be final maximum flow 
            return (Vertices.Last().Data as VertexFlowData).e_flow;
        }

        void relabel(int u)
        {
            // Initialize minimum height of an adjacent 
            int mh = int.MaxValue;

            // Find the adjacent with minimum height 
            for (int i = 0; i < Edges.Count; i++)
            {
                var edges = Edges[i];
                if (edges == null) continue;

                for (int j = 0; j < edges.Count; j++)
                {
                    var edge = edges[j];
                    int from = edge.From;
                    int to = edge.To;

                    if (from == u)
                    {
                        var edata = edge.Data as EdgeFlowData;

                        // if flow is equal to capacity then no 
                        // relabeling 
                        if (edata.flow == edata.capacity)
                            continue;

                        var from_vdata = Vertices[from].Data as VertexFlowData;
                        var to_vdata = Vertices[to].Data as VertexFlowData;

                        // Update minimum height 
                        if (to_vdata.h < mh)
                        {
                            mh = to_vdata.h;
                            from_vdata.h = mh + 1;
                        }
                    }
                }
            }
        }

        void preflow(int s)
        {
            // Making h of source Vertex equal to no. of vertices 
            // Height of other vertices is 0. 
            var s_vdata = Vertices[s].Data as VertexFlowData;
            s_vdata.h = Vertices.Count;

            int count1 = Edges.Count;
            for (int i = 0; i < count1; i++)
            {
                var edges = Edges[i];
                if (edges == null) continue;

                int count2 = edges.Count;
                for (int j = 0; j < count2; j++)
                {
                    var edge = edges[j];
                    int from = edge.From;
                    int to = edge.To;

                    // If current edge goes from source 
                    if (from == s)
                    {
                        var edata = edge.Data as EdgeFlowData;

                        // if flow is equal to capacity then no 
                        // relabeling 
                        edata.flow = edata.capacity;

                        var vdata = Vertices[to].Data as VertexFlowData;

                        // Initialize excess flow for adjacent v 
                        vdata.e_flow += edata.flow;

                        // Add an edge from v to s in residual graph with 
                        // capacity equal to 0 
                        var data = new EdgeFlowData(-edata.flow, 0);
                        AddEdge(new GraphEdge(from, s, 0, data));
                    }
                }
            }
        }

        // returns index of overflowing Vertex 
        int overFlowVertex()
        {
            for (int i = 1; i < Vertices.Count - 1; i++)
            {
                var vdata = Vertices[i].Data as VertexFlowData;

                if (vdata.e_flow > 0)
                    return i;
            }
        
            // -1 if no overflowing Vertex 
            return -1;
        }

    }
}
