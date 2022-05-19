using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Directions;
using Common.Core.Numerics;
using Common.Core.Extensions;
using Common.GraphTheory.GridGraphs;

namespace Common.GraphTheory.Test.GridGraphs
{
    [TestClass]
    public class GridGraphTest
    {

        [TestMethod]
        public void DijkstrasShortestPathTree()
        {

            //var graph = RandomGraph(0);
            var graph = ConstGraph();

            var source = new Point2i(0, 0);
            var target = new Point2i(3, 3);

            var search = graph.DijkstrasShortestPathTree(source);

            var path = search.GetPath(target);

            //search.Print();

            //foreach (var p in path)
            //    Console.WriteLine(p);

        }

        private WeightedGridGraph RandomGraph(int seed)
        {
            var graph = new WeightedGridGraph(4, 4);

            var rnd = new Random(seed);

            graph.Iterate((x, y, i) =>
            {
                if (rnd.NextBool())
                {
                    float weight = rnd.NextFloat(0, 10);
                    graph.AddDirectedWeightedEdge(x, y, i, weight);
                }
            });

            return graph;
        }

        private WeightedGridGraph ConstGraph()
        {
            var graph = new WeightedGridGraph(4, 4);

            graph.Iterate((x, y, i) =>
            {
                int xi = x + D8.OFFSETS[i, 0];
                int yi = y + D8.OFFSETS[i, 1];

                if(graph.InBounds(xi, yi))
                {
                    graph.AddDirectedWeightedEdge(x, y, i, 1);
                }
                
            });

            return graph;
        }

    }


}
