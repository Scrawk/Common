using System;
using System.Collections.Generic;

using Common.Core.Numerics;

using Common.Console.Collections;
using Common.Console.Geometry;
using Common.Console.Core;

using Common.Core.Directions;
using Common.GraphTheory.GridGraphs;
using Common.GraphTheory.AdjacencyGraphs;
using Common.GraphTheory.MatrixGraphs;

using CONSOLE = System.Console;

namespace Common.Console
{
    class Program
    {

        static void Main(string[] args)
        {
            /*
            float[,] matrix =
                {{ 0, 16, 13, 0, 0, 0},
                { 0, 0, 10, 12, 0, 0},
                { 0, 4, 0, 0, 14, 0},
                { 0, 0, 9, 0, 0, 20},
                { 0, 0, 0, 7, 0, 4},
                { 0, 0, 0, 0, 0, 0}};

            var graph = new MatrixGraph(matrix);

            //graph.Print();

            var max_flow = graph.FordFulkersonMaxFlow(0, 5);

            CONSOLE.WriteLine("Max Flow = " + max_flow);

            CONSOLE.WriteLine("Min Cut");

            var cut = graph.FordFulkersonMinCut(0, 5);

            foreach (var edge in cut)
                CONSOLE.WriteLine(edge);
            */

            
            var graph = new DirectedGraph(6);

            for (int i = 0; i < graph.VertexCount; i++)
            {
                var vertex = graph.GetVertex(i);
                vertex.Data = new VertexFlowData(0, 0);
            }

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

            graph.Print();

            int s = 0, t = 5;

            CONSOLE.WriteLine("Maximum flow is " + graph.PushRelabelMaxFlow(s, t));

            //graph.Print();
            
            
        }
    }
}
