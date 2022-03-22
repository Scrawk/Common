using System;
using System.Collections.Generic;

using Common.Core.Numerics;

using Common.Console.Collections;
using Common.Console.Geometry;
using Common.Console.Core;

using Common.Core.Directions;
using Common.GraphTheory.GridGraphs;
using Common.GraphTheory.AdjacencyGraphs;

using CONSOLE = System.Console;

namespace Common.Console
{
    class Program
    {
        static void Main(string[] args)
        {

            var graph = new DirectedGraph(6);

            for (int i = 0; i < graph.VertexCount; i++)
            {
                var vertex = graph.GetVertex(i);
                vertex.Data = new VertexFlowData(0,0);
            }

          
            // Creating above shown flow network 
            graph.AddDirectedEdge(0, 1, new EdgeFlowData(16));
            graph.AddDirectedEdge(0, 2, new EdgeFlowData(13));
            graph.AddDirectedEdge(1, 2, new EdgeFlowData(10));
            graph.AddDirectedEdge(2, 1, new EdgeFlowData(4));
            graph.AddDirectedEdge(1, 3, new EdgeFlowData(12));
            graph.AddDirectedEdge(2, 4, new EdgeFlowData(14));
            graph.AddDirectedEdge(3, 2, new EdgeFlowData(9));
            graph.AddDirectedEdge(3, 5, new EdgeFlowData(20));
            graph.AddDirectedEdge(4, 3, new EdgeFlowData(7));
            graph.AddDirectedEdge(4, 5, new EdgeFlowData(4));

            int s = 0, t = 5;

            CONSOLE.WriteLine("Maximum flow is " + graph.PushRelabelMaxFlow(s, t));


            /*
            CONSOLE.WriteLine("MaxFlow");

            var graph = new GridGraph(4, 4);
            var search = new GridSearch(4, 4);

            var source = new Point2i(0, 0);
            var target = new Point2i(3, 0);

            graph.AddDirectedWeightedEdge(source, D8.RIGHT, 13);
            graph.AddDirectedWeightedEdge(source, D8.RIGHT_TOP, 16);
            graph.AddDirectedWeightedEdge(1, 0, D8.TOP, 4);
            graph.AddDirectedWeightedEdge(1, 0, D8.RIGHT, 14);
            graph.AddDirectedWeightedEdge(1, 1, D8.BOTTOM, 10);
            graph.AddDirectedWeightedEdge(1, 1, D8.RIGHT, 12);
            graph.AddDirectedWeightedEdge(2, 1, D8.LEFT_BOTTOM, 9);
            graph.AddDirectedWeightedEdge(2, 1, D8.RIGHT_BOTTOM, 20);
            graph.AddDirectedWeightedEdge(2, 0, D8.TOP, 7);
            graph.AddDirectedWeightedEdge(2, 0, D8.RIGHT, 4);

            //Console.WriteLine("Graph");
            //graph.Print();

            int max_flow = graph.MaxFlow(search, source, target);

            var cut = graph.MinCut(search, source, target);

            CONSOLE.WriteLine(max_flow);

            foreach (var edge in cut)
                CONSOLE.WriteLine(edge);
            */
        }
    }
}
