using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Directions;
using Common.Core.Numerics;
using Common.GraphTheory.GridGraphs;

namespace Common.GraphTheory.Test.GridGraphs
{
    [TestClass]
    public class GridGraphTest
    {
        [TestMethod]
        public void MaxFlow()
        {
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

            var cut2 = graph.MinCut2(search, source, target);

            Console.WriteLine(max_flow);

            Console.WriteLine("Cut 1");

            foreach (var edge in cut)
                Console.WriteLine(edge);

            Console.WriteLine("Cut 2");

            foreach (var edge in cut2)
                Console.WriteLine(edge);

        }
    }
}
